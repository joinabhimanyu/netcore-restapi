using System;
using System.Collections.Generic;
using System.Diagnostics;
using DRS.Model;
using DRS.Model.Models;
using DRS.Model.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace DRS.Data
{
    /// <summary>
    /// Unit of Work class responsible for DB transactions
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        #region Private member variables...

        public IDbContextTransaction ContextTransaction => _context?.Database.BeginTransaction();
        private DRSDBContext _context = null;
        private IGenericRepository<Archive> _archiveRepository;
        private IGenericRepository<Document> _documentRepository;
        private IGenericRepository<DocumentCategory> _documentCategoryRepository;
        private IGenericRepository<DocumentField> _documentFieldRepository;
        private IGenericRepository<DocumentFieldSetting> _documentFieldSettingRepository;
        private IGenericRepository<DocumentRule> _documentRuleRepository;
        private IGenericRepository<ProcessSchemaStepSetting> _processSchemaStepSettingRepository;
        private IGenericRepository<SystemLog> _systemLogRepository;
        private IGenericRepository<ProcessSchemaStep> _processSchemaStepRepository;
        private IGenericRepository<SystemSetting> _systemSettingRepository;
        private IGenericRepository<ProcessSchema> _processSchemaRepository;
        private IGenericRepository<ProcessQueueState> _processQueueStateRepository;
        private IGenericRepository<ProcessHandlerSchedule> _processHandlerScheduleRepository;
        private IGenericRepository<ProcessHandlerField> _processHandlerFieldRepository;
        private IGenericRepository<ProcessHandlerType> _processHandlerTypeRepository;
        private IGenericRepository<ProcessHandler> _processHandlerRepository;
        private IGenericRepository<Field> _fieldRepository;
        private IGenericRepository<ImportMapping> _importMappingRepository;
        private IGenericRepository<DocumentSource> _documentSourceRepository;
        private IGenericRepository<DocumentFieldParameter> _documentFieldParameterRepository;
        private IGenericRepository<DocumentRuleSetting> _documentRuleSettingRepository;
        private IGenericRepository<ProcessQueueFile> _processQueueFileRepository;
        private IGenericRepository<ProcessQueueField> _processQueueFieldRepository;
        private IGenericRepository<ProcessQueue> _processQueueRepository;
        private IGenericRepository<ProcessLog> _processLogRepository;
        private IGenericRepository<ProcessHandlerSetting> _processHandlerSettingRepository;

        #endregion

        public UnitOfWork(DRSDBContext context)
        {
            _context = context;
        }

        #region Public Repository Creation properties...
        /// <summary>
        /// Get/Set Property for Archive repository.
        /// </summary>
        [RegisterRepository(TargetType =typeof(Archive))]
        public IGenericRepository<Archive> ArchiveRepository
        {
            get
            {
                if (this._archiveRepository == null)
                    this._archiveRepository = new ArchiveRepository(_context);
                return _archiveRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for DocumentField repository.
        /// </summary>
        /// 
        [RegisterRepository(TargetType = typeof(DocumentField))]
        public IGenericRepository<DocumentField> DocumentFieldRepository
        {
            get
            {
                if (this._documentFieldRepository == null)
                    this._documentFieldRepository = new GenericRepository<DocumentField>(_context);
                return _documentFieldRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for Document repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(Document))]
        public IGenericRepository<Document> DocumentRepository
        {
            get
            {
                if (this._documentRepository == null)
                    this._documentRepository = new GenericRepository<Document>(_context);
                return _documentRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for DocumentCategory repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(DocumentCategory))]
        public IGenericRepository<DocumentCategory> DocumentCategoryRepository
        {
            get
            {
                if (this._documentCategoryRepository == null)
                    this._documentCategoryRepository = new GenericRepository<DocumentCategory>(_context);
                return _documentCategoryRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for DocumentFieldSetting repository.
        /// </summary>
        [RegisterRepository(TargetType =typeof(DocumentFieldSetting))]
        public IGenericRepository<DocumentFieldSetting> DocumentFieldSettingRepository
        {
            get
            {
                if (this._documentFieldSettingRepository == null)
                    this._documentFieldSettingRepository = new GenericRepository<DocumentFieldSetting>(_context);
                return _documentFieldSettingRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for DocumentRule repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(DocumentRule))]
        public IGenericRepository<DocumentRule> DocumentRuleRepository
        {
            get
            {
                if (this._documentRuleRepository == null)
                    this._documentRuleRepository = new GenericRepository<DocumentRule>(_context);
                return _documentRuleRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for DocumentRuleSetting repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(DocumentRuleSetting))]
        public IGenericRepository<DocumentRuleSetting> DocumentRuleSettingRepository
        {
            get
            {
                if (this._documentRuleSettingRepository == null)
                    this._documentRuleSettingRepository = new GenericRepository<DocumentRuleSetting>(_context);
                return _documentRuleSettingRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for DocumentSource repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(DocumentSource))]
        public IGenericRepository<DocumentSource> DocumentSourceRepository
        {
            get
            {
                if (this._documentSourceRepository == null)
                    this._documentSourceRepository = new GenericRepository<DocumentSource>(_context);
                return _documentSourceRepository;
            }
        }
        /// <summary>
        /// document field parameter repository
        /// </summary>
        [RegisterRepository(TargetType = typeof(DocumentFieldParameter))]
        public IGenericRepository<DocumentFieldParameter> DocumentFieldParameterRepository
        {
            get
            {
                if (this._documentFieldParameterRepository == null)
                    this._documentFieldParameterRepository = new GenericRepository<DocumentFieldParameter>(_context);
                return _documentFieldParameterRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for ImportMapping repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(ImportMapping))]
        public IGenericRepository<ImportMapping> ImportMappingRepository
        {
            get
            {
                if (this._importMappingRepository == null)
                    this._importMappingRepository = new GenericRepository<ImportMapping>(_context);
                return _importMappingRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for Field repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(Field))]
        public IGenericRepository<Field> FieldRepository
        {
            get
            {
                if (this._fieldRepository == null)
                    this._fieldRepository = new GenericRepository<Field>(_context);
                return _fieldRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for ProcessHandler repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(ProcessHandler))]
        public IGenericRepository<ProcessHandler> ProcessHandlerRepository
        {
            get
            {
                if (this._processHandlerRepository == null)
                    this._processHandlerRepository = new GenericRepository<ProcessHandler>(_context);
                return _processHandlerRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for ProcessHandlerType repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(ProcessHandlerType))]
        public IGenericRepository<ProcessHandlerType> ProcessHandlerTypeRepository
        {
            get
            {
                if (this._processHandlerTypeRepository == null)
                    this._processHandlerTypeRepository = new GenericRepository<ProcessHandlerType>(_context);
                return _processHandlerTypeRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for ProcessHandlerField repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(ProcessHandlerField))]
        public IGenericRepository<ProcessHandlerField> ProcessHandlerFieldRepository
        {
            get
            {
                if (this._processHandlerFieldRepository == null)
                    this._processHandlerFieldRepository = new GenericRepository<ProcessHandlerField>(_context);
                return _processHandlerFieldRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for ProcessHandlerSchedule repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(ProcessHandlerSchedule))]
        public IGenericRepository<ProcessHandlerSchedule> ProcessHandlerScheduleRepository
        {
            get
            {
                if (this._processHandlerScheduleRepository == null)
                    this._processHandlerScheduleRepository = new GenericRepository<ProcessHandlerSchedule>(_context);
                return _processHandlerScheduleRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for ProcessHandlerSetting repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(ProcessHandlerSetting))]
        public IGenericRepository<ProcessHandlerSetting> ProcessHandlerSettingRepository
        {
            get
            {
                if (this._processHandlerSettingRepository == null)
                    this._processHandlerSettingRepository = new GenericRepository<ProcessHandlerSetting>(_context);
                return _processHandlerSettingRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for ProcessLog repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(ProcessLog))]
        public IGenericRepository<ProcessLog> ProcessLogRepository
        {
            get
            {
                if (this._processLogRepository == null)
                    this._processLogRepository = new GenericRepository<ProcessLog>(_context);
                return _processLogRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for ProcessQueue repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(ProcessQueue))]
        public IGenericRepository<ProcessQueue> ProcessQueueRepository
        {
            get
            {
                if (this._processQueueRepository == null)
                    this._processQueueRepository = new GenericRepository<ProcessQueue>(_context);
                return _processQueueRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for ProcessQueueField repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(ProcessQueueField))]
        public IGenericRepository<ProcessQueueField> ProcessQueueFieldRepository
        {
            get
            {
                if (this._processQueueFieldRepository == null)
                    this._processQueueFieldRepository = new GenericRepository<ProcessQueueField>(_context);
                return _processQueueFieldRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for ProcessQueueFile repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(ProcessQueueFile))]
        public IGenericRepository<ProcessQueueFile> ProcessQueueFileRepository
        {
            get
            {
                if (this._processQueueFileRepository == null)
                    this._processQueueFileRepository = new GenericRepository<ProcessQueueFile>(_context);
                return _processQueueFileRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for ProcessQueueState repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(ProcessQueueState))]
        public IGenericRepository<ProcessQueueState> ProcessQueueStateRepository
        {
            get
            {
                if (this._processQueueStateRepository == null)
                    this._processQueueStateRepository = new GenericRepository<ProcessQueueState>(_context);
                return _processQueueStateRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for ProcessSchema repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(ProcessSchema))]
        public IGenericRepository<ProcessSchema> ProcessSchemaRepository
        {
            get
            {
                if (this._processSchemaRepository == null)
                    this._processSchemaRepository = new GenericRepository<ProcessSchema>(_context);
                return _processSchemaRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for ProcessSchemaStep repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(ProcessSchemaStep))]
        public IGenericRepository<ProcessSchemaStep> ProcessSchemaStepRepository
        {
            get
            {
                if (this._processSchemaStepRepository == null)
                    this._processSchemaStepRepository = new GenericRepository<ProcessSchemaStep>(_context);
                return _processSchemaStepRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for ProcessSchemaStepSetting repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(ProcessSchemaStepSetting))]
        public IGenericRepository<ProcessSchemaStepSetting> ProcessSchemaStepSettingRepository
        {
            get
            {
                if (this._processSchemaStepSettingRepository == null)
                    this._processSchemaStepSettingRepository = new GenericRepository<ProcessSchemaStepSetting>(_context);
                return _processSchemaStepSettingRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for SystemSetting repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(SystemSetting))]
        public IGenericRepository<SystemSetting> SystemSettingRepository
        {
            get
            {
                if (this._systemSettingRepository == null)
                    this._systemSettingRepository = new GenericRepository<SystemSetting>(_context);
                return _systemSettingRepository;
            }
        }
        /// <summary>
        /// Get/Set Property for SystemLog repository.
        /// </summary>
        [RegisterRepository(TargetType = typeof(SystemLog))]
        public IGenericRepository<SystemLog> SystemLogRepository
        {
            get
            {
                if (this._systemLogRepository == null)
                    this._systemLogRepository = new GenericRepository<SystemLog>(_context);
                return _systemLogRepository;
            }
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
