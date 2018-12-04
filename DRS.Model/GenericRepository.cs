using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DRS.Model.Models;
using DRS.Model.Utils;
using Microsoft.EntityFrameworkCore;

namespace DRS.Model
{
    public static class RepoUtils
    {
        /// <summary>
        /// used for paging of queryable
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
        /// <summary>
        /// used for paging of enumerable
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// field for dbcontext
        /// </summary>
        public DRSDBContext Context { get; set; }
        /// <summary>
        /// field for specific dbset
        /// </summary>
        internal DbSet<TEntity> DbSet;
        /// <summary>
        /// constructor initializing dbcontext
        /// </summary>
        /// <param name="context"></param>
        public GenericRepository(DRSDBContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>(); ;
        }
        /// <summary>
        /// Generic get method on the basis of id for Entities.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetByID(object id)
        {
            return DbSet.Find(id);
        }
        /// <summary>
        /// Generic method to check if entity exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Exists(object id)
        {
            return DbSet.Find(id) != null;
        }
        public static dynamic ConvertToType(dynamic source, Type dest, bool nullable = false)
        {
            if (!nullable)
            {
                return Convert.ChangeType(source, dest);
            }
            return Convert.ChangeType(source, Nullable.GetUnderlyingType(dest));
        }
        /// <summary>
        /// get entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity Get(object id)
        {
            var (keyname, keytype) = GetKey<TEntity>();
            var _type = typeof(TEntity);
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            Expression expression = null;
            if (!String.IsNullOrEmpty(keyname))
            {
                if (_type.GetProperties().Where(x => x.Name == keyname).Count() > 0)
                {
                    if (keytype.GetTypeInfo().IsValueType)
                    {
                        if (Nullable.GetUnderlyingType(keytype) != null)
                        {
                            expression = (Expression)Expression.Equal(
                                        Expression.Convert(Expression.Property(parameter, keyname), Nullable.GetUnderlyingType(keytype)),
                                        Expression.Constant(ConvertToType(id, keytype, nullable: true)));
                        }
                        expression = (Expression)Expression.Equal(
                                        Expression.Property(parameter, keyname),
                                        Expression.Constant(ConvertToType(id, keytype)));
                    }
                }
            }
            if (expression != null)
            {
                var fexpression = Expression.Lambda<Func<TEntity, bool>>(
                              expression, parameter);
                var result = DbSet.AsNoTracking().Where(fexpression).FirstOrDefault();
                return result;
            }
            return null;
        }
        public virtual (string, Type) GetKey<T>()
        {
            var keyName = Context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();
            Type keyType = null;
            if (typeof(T).GetProperties().Where(x => x.Name == keyName).Count() > 0)
            {
                keyType = typeof(T).GetProperties().Where(x => x.Name == keyName).First().PropertyType;
            }
            return (keyName, keyType);
        }
        /// <summary>
        /// get by entire entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity GetByEntity(object entity)
        {
            return DbSet.AsNoTracking().FirstOrDefault(x => x.Equals((TEntity)entity));
        }
        /// <summary>
        /// generic method to fetch all the records from db
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            return DbSet.AsNoTracking().ToList();
        }
        /// <summary>
        /// generic method to fetch all records as queryable object
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetAllQueryable()
        {
            return DbSet.AsNoTracking();
        }
        /// <summary>
        /// generic method to get many record on the basis of a condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> predicate, bool IsNavigationEnabled = false, params string[] include)
        {
            if (IsNavigationEnabled)
            {
                IQueryable<TEntity> query = this.DbSet;
                query = include.Aggregate(query, (current, inc) => current.Include(inc));
                return query.AsNoTracking().Where(predicate).Take(10).ToList();
            }
            else
                return DbSet.AsNoTracking().Where(predicate).ToList();
        }
        /// <summary>
        /// generic method to get many record on the basis of a condition but query able.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetManyQueryable(Expression<Func<TEntity, bool>> predicate, string[] include, bool IsNavigationEnabled = false, string FilterClause = "where")
        {
            IQueryable<TEntity> query = this.DbSet;
            IQueryable<TEntity> result = null;
            
            switch (FilterClause)
            {
                case "where":
                    result = query.AsNoTracking().Where(predicate).AsQueryable();
                    break;
                case "where-not":
                    result = query.AsNoTracking().Except(query.AsNoTracking().Where(predicate).AsQueryable());
                    break;
            }
            if (IsNavigationEnabled)
            {
                result = include.Aggregate(result, (current, inc) => current.Include(inc));
            }
            return result;
        }
        /// <summary>
        /// Gets a single record by the specified criteria (usually the unique identifier)
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record that matches the specified criteria</returns>
        public virtual TEntity GetSingle(Expression<Func<TEntity, bool>> predicate, bool IsNavigationEnabled = false, params string[] include)
        {
            if (IsNavigationEnabled)
            {
                IQueryable<TEntity> query = this.DbSet;
                query = include.Aggregate(query, (current, inc) => current.Include(inc));
                return query.AsNoTracking().Single<TEntity>(predicate);
            }
            else
                return DbSet.AsNoTracking().Single<TEntity>(predicate);
        }
        /// <summary>
        /// create new entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Create(TEntity entity)
        {
            DbSet.Add(entity);
        }
        /// <summary>
        /// create list of entities
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Create(ICollection<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }
        /// <summary>
        /// Generic update method for the entities
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }
        /// <summary>
        /// update list of entities
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Update(ICollection<TEntity> entities)
        {
            foreach (var item in entities)
            {
                Update(item);
            }
        }
        /// <summary>
        /// delete by multiple ids
        /// </summary>
        /// <param name="ids"></param>
        public virtual void Delete(Int64[] ids)
        {
            foreach (var item in ids)
            {
                Delete(item);
            }
        }
        /// <summary>
        /// delete by single id
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(Int64 id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }
        /// <summary>
        /// delete entity list
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Delete(ICollection<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }
        /// <summary>
        /// Generic Delete method for the entities
        /// </summary>
        /// <param name="entityToDelete"></param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }
        /// <summary>
        /// generic delete method , deletes data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual void Delete(Func<TEntity, bool> where)
        {
            IQueryable<TEntity> objects = DbSet.Where<TEntity>(where).AsQueryable();
            foreach (TEntity obj in objects)
                DbSet.Remove(obj);
        }
        /// <summary>
        /// save changes of dbcontext
        /// </summary>
        public virtual void SaveChanges()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (Exception e)
            {

                var outputLines = new List<string>();
                outputLines.Add(e.Message);
                //                foreach (var eve in e.EntityValidationErrors)
                //                {
                //                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                //                    foreach (var ve in eve.ValidationErrors)
                //                    {
                //                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                //                    }
                //                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }
        }
        /// <summary>
        /// disposed field to detect if disposed or not
        /// </summary>
        private bool disposed = false;
        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }
        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Inclue multiple
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        //        public IQueryable<TEntity> GetWithInclude(
        //            System.Linq.Expressions.Expression<Func<TEntity,
        //            bool>> predicate, params string[] include)
        //        {
        //            IQueryable<TEntity> query = this.DbSet;
        //            query = include.Aggregate(query, (current, inc) => current.Include(inc));
        //            return query.Where(predicate);
        //        }
    }
}
