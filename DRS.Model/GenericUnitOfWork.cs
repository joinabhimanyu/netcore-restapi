using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DRS.Model.Models;

namespace DRS.Model
{
    public class GenericUnitOfWork: IDisposable
    {
        #region Private member variables...

        private DRSDBContext _context = null;
        private Dictionary<Type,IGenericRepository<object>> _repositoryPool;

        #endregion

        public GenericUnitOfWork(DRSDBContext context)
        {
            _context = context;
            _repositoryPool = new Dictionary<Type, IGenericRepository<object>>();
        }

        #region Public Repository Creation properties...

        /// <summary>
        /// Get/Set Property for Archive repository.
        /// </summary>
        public IGenericRepository<TEntity> Repository<TBusinessEntity,TEntity>(TBusinessEntity bussinesEntity = null, TEntity entity = null) 
            where TBusinessEntity : class
            where TEntity : class
        {
            var type = typeof (TBusinessEntity);
                if (this._repositoryPool[type] == null)
                    this._repositoryPool[type] = (IGenericRepository<object>) new GenericRepository<TEntity>(_context);
                return (IGenericRepository<TEntity>) _repositoryPool[type];
        }
        #endregion

        #region Public member methods...
        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
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

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

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
                    _context.Dispose();
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
        #endregion
        
    }
}
