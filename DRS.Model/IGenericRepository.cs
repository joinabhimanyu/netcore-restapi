using DRS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DRS.Model
{
    public interface IGenericRepository<TEntity>:IDisposable where TEntity : class
    {
        DRSDBContext Context { get; set; }
        TEntity GetByID(object id);
        bool Exists(object item);
        TEntity Get(object id);
        TEntity GetByEntity(object entity);
        IEnumerable<TEntity> GetAll();
        IQueryable<TEntity> GetAllQueryable();
        IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> predicate, bool IsNavigationEnabled = false, params string[] include);
        IQueryable<TEntity> GetManyQueryable(Expression<Func<TEntity, bool>> predicate, string[] include, bool IsNavigationEnabled = false, string FilterClause = "where");
        TEntity GetSingle(Expression<Func<TEntity, bool>> predicate, bool IsNavigationEnabled = false, params string[] include);
        void Create(TEntity entity);
        void Create(ICollection<TEntity> entities);
        void Update(TEntity entity);
        void Update(ICollection<TEntity> entities);
        void Delete(Int64[] ids);
        void Delete(Int64 id);
        void Delete(ICollection<TEntity> entities);
        void Delete(TEntity entityToDelete);
        void Delete(Func<TEntity, bool> where);
        void SaveChanges();
        
    }
}
