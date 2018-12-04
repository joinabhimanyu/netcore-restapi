using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DRS.Data.BusinessEntities;
using DRS.Data.ServiceInterfaces;
using DRS.Model.Models;
using System;
using DRS.Model;
using Newtonsoft.Json.Linq;

namespace DRS.Data.Services
{
    public class ProcessQueueService : IGenericService<ProcessQueueEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public ProcessQueueService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<ProcessQueue>("0");
        }

        public ProcessQueueEntity GetById(int id)
        {
            var product = _unitOfWork.ProcessQueueRepository.Get(id);
            if (product != null)
            {
                var document = _unitOfWork.DocumentRepository.GetMany(x => x.DocumentId == product.DocumentId).FirstOrDefault();
                product.Document = document;
                var processState = _unitOfWork.ProcessQueueStateRepository.GetMany(x => x.ProcessQueueStateId == product.ProcessQueueStateId).FirstOrDefault();
                product.ProcessQueueState = processState;
                var processSchema = _unitOfWork.ProcessSchemaRepository.GetMany(x => x.ProcessSchemaId == product.ProcessSchemaId).FirstOrDefault();
                product.ProcessSchema = processSchema;
                var importHandler = _unitOfWork.ProcessHandlerRepository.GetMany(x => x.ProcessHandlerId == product.ImportHandlerId).FirstOrDefault();
                product.ImportHandler = importHandler;
                var processLogs = _unitOfWork.ProcessLogRepository.GetMany(x => x.ProcessQueueId == product.ProcessQueueId).ToList();
                product.ProcessLog = processLogs;
                var nextProcessSchemaStep = _unitOfWork.ProcessSchemaStepRepository.GetMany(x => x.ProcessSchemaStepId == product.NextProcessSchemaStepId).FirstOrDefault();
                product.NextProcessSchemaStep = nextProcessSchemaStep;
                var productModel = Mapper.Map<ProcessQueueEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProcessQueueEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<ProcessQueue>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<ProcessQueue> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<ProcessQueueEntity> PrepareResult(List<ProcessQueue> entities)
        {
            return (entities.Select(e => new ProcessQueueEntity
            {
                ProcessQueueId = e.ProcessQueueId,
                ImportHandlerId = e.ImportHandlerId,
                ProcessSchemaId = e.ProcessSchemaId,
                DocumentRuleId = e.DocumentRuleId,
                NextProcessSchemaStepId = e.NextProcessSchemaStepId,
                ProcessQueueStateId = e.ProcessQueueStateId,
                ProcessErrorCount = e.ProcessErrorCount,
                ProcessTotaltime = e.ProcessTotaltime,
                LastProcessDate = e.LastProcessDate,
                DocumentId = e.DocumentId,
                DocumentKey = e.DocumentKey,
                DocumentIdentity = e.DocumentIdentity,
                DocumentName = e.DocumentName,
                DocumentCategory = e.DocumentCategory,
                DocumentStamp = e.DocumentStamp,
                DocumentPages = e.DocumentPages,
                DocumentArchiveKey = e.DocumentArchiveKey,
                Created = e.Created,
                Stamp = e.Stamp,
                Document = e.Document != null ? new DocumentEntity
                {
                    DocumentId = e.Document != null ? e.Document.DocumentId : 0,
                    DocumentIdentity = e.Document?.DocumentIdentity,
                    Number = e.Document?.Number,
                    Version = e.Document?.Version,
                    Name = e.Document?.Name,
                    Description = e.Document?.Description,
                    Pages = e.Document?.Pages,
                    CompanyCode = e.Document?.CompanyCode,
                    BatchClass = e.Document?.BatchClass,
                    ExportFileType = e.Document?.ExportFileType,
                    Link = e.Document?.Link,
                    DocumentTemplateId = e.Document?.DocumentTemplateId,
                    DocumentCategoryId = e.Document != null ? e.Document.DocumentCategoryId : 0,
                    DocumentSourceId = e.Document != null ? e.Document.DocumentSourceId : 0,
                    IsActive = e.Document != null ? e.Document.IsActive : false,
                    Created = e.Document?.Created,
                    CreatedBy = e.Document?.CreatedBy,
                    Updated = e.Document?.Updated,
                    UpdatedBy = e.Document?.UpdatedBy
                } : null,
                ImportHandler = e.ImportHandler != null? new ProcessHandlerEntity
                {
                    ProcessHandlerId=e.ImportHandler!=null?e.ImportHandler.ProcessHandlerId:0,
                    ProcessHandlerGuid=e.ImportHandler.ProcessHandlerGuid,
                    Name=e.ImportHandler.Name,
                    Description=e.ImportHandler.Description,
                    AssemblyName=e.ImportHandler.AssemblyName,
                    ClassName=e.ImportHandler.ClassName,
                    IsActive=e.ImportHandler.IsActive,
                    ProcessHandlerTypeId=e.ImportHandler!=null?e.ImportHandler.ProcessHandlerTypeId:0,
                    ProcessSchemaId=e.ImportHandler!=null?e.ImportHandler.ProcessSchemaId:0,
                    Priority=e.ImportHandler!=null?e.ImportHandler.Priority:0,
                    LastExecuteDate=e.ImportHandler.LastExecuteDate,
                    LastProcessDate=e.ImportHandler.LastProcessDate,
                    NextProcessDate=e.ImportHandler.NextProcessDate,
                    NumberOfItemsToProcess=e.ImportHandler!=null?e.ImportHandler.NumberOfItemsToProcess:0,
                    NumberOfRetries=e.ImportHandler!=null?e.ImportHandler.NumberOfRetries:0,
                    WaitIntervalBetweenRetries=e.ImportHandler!=null?e.ImportHandler.WaitIntervalBetweenRetries:0,
                    NumberOfErrors=e.ImportHandler!=null?e.ImportHandler.NumberOfErrors:0,
                    WaitIntervalOnErrors=e.ImportHandler!=null?e.ImportHandler.WaitIntervalOnErrors:0,
                    ProcessErrorCount=e.ImportHandler!=null?e.ImportHandler.ProcessErrorCount:0,
                    ProcessServers=e.ImportHandler.ProcessServers,
                    Created=e.ImportHandler.Created,
                    CreatedBy=e.ImportHandler.CreatedBy,
                    Updated=e.ImportHandler.Updated,
                    UpdatedBy=e.ImportHandler.UpdatedBy,
                    Stamp=e.ImportHandler.Stamp
                }:null,
                NextProcessSchemaStep= e.NextProcessSchemaStep!=null? new ProcessSchemaStepEntity
                {
                    ProcessSchemaStepId=e.NextProcessSchemaStep!=null?e.NextProcessSchemaStep.ProcessSchemaStepId:0,
                    ProcessSchemaStepGuid= e.NextProcessSchemaStep.ProcessSchemaStepGuid,
                    ProcessSchemaId=e.NextProcessSchemaStep!=null?e.NextProcessSchemaStep.ProcessSchemaId:0,
                    ProcessHandlerId=e.NextProcessSchemaStep!=null?e.NextProcessSchemaStep.ProcessHandlerId:0,
                    StepOrder=e.NextProcessSchemaStep!=null?e.NextProcessSchemaStep.StepOrder:0,
                    Name=e.NextProcessSchemaStep.Name,
                    Description=e.NextProcessSchemaStep.Description,
                    OnSuccessStepId=e.NextProcessSchemaStep.OnSuccessStepId,
                    OnSuccessStepGuid=e.NextProcessSchemaStep.OnSuccessStepGuid,
                    OnErrorStepId=e.NextProcessSchemaStep.OnErrorStepId,
                    OnErrorStepGuid=e.NextProcessSchemaStep.OnErrorStepGuid,
                    FieldQueryRule=e.NextProcessSchemaStep.FieldQueryRule,
                    Created=e.NextProcessSchemaStep.Created,
                    CreatedBy=e.NextProcessSchemaStep.CreatedBy,
                    Updated=e.NextProcessSchemaStep.Updated,
                    UpdatedBy=e.NextProcessSchemaStep.UpdatedBy,
                    Stamp=e.NextProcessSchemaStep.Stamp
                }:null,
                ProcessQueueState = e.ProcessQueueState!=null? new ProcessQueueStateEntity
                {
                    ProcessQueueStateId = e.ProcessQueueState != null ? e.ProcessQueueState.ProcessQueueStateId : 0,
                    Name = e.ProcessQueueState?.Name,
                    Description = e.ProcessQueueState?.Description,
                    StateImage = e.ProcessQueueState?.StateImage,
                    Created = e.ProcessQueueState?.Created,
                    CreatedBy = e.ProcessQueueState?.CreatedBy,
                    Updated = e.ProcessQueueState?.Updated,
                    UpdatedBy = e.ProcessQueueState?.UpdatedBy
                }:null,
                ProcessSchema = e.ProcessSchema!=null? new ProcessSchemaEntity
                {
                    ProcessSchemaId = e.ProcessSchema != null ? e.ProcessSchema.ProcessSchemaId : 0,
                    ProcessSchemaGuid = e.ProcessSchema != null ? e.ProcessSchema.ProcessSchemaGuid : new Guid(),
                    Name = e.ProcessSchema?.Name,
                    Description = e.ProcessSchema?.Description,
                    Created = e.ProcessSchema?.Created,
                    CreatedBy = e.ProcessSchema?.CreatedBy,
                    Updated = e.ProcessSchema?.Updated,
                    UpdatedBy = e.ProcessSchema?.UpdatedBy
                }:null,
                ProcessLog = e.ProcessLog!=null && e.ProcessLog.Count>0? e.ProcessLog.Select(x => new ProcessLogEntity
                {
                    ProcessLogId = x != null ? x.ProcessLogId : 0,
                    ProcessHandlerId = x?.ProcessHandlerId,
                    ProcessQueueId = x?.ProcessQueueId,
                    ProcessSchemaStepId = x?.ProcessSchemaStepId,
                    TraceEventTypeId = x?.TraceEventTypeId,
                    EventId = x != null ? x.EventId : 0,
                    Severity = x?.Severity,
                    Message = x?.Message,
                    Timestamp = x != null ? x.Timestamp : DateTime.Now
                }).ToList():null
            })).ToList();
        }
        public PagedResult GetAll(PaginationArgs args)
        {
            var r = new PagedResult();
            try
            {
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<ProcessQueue>(args, _unitOfWork);
                if (args.IsNavigationEnabled)
                {
                    foreach (var item in entities)
                    {
                        var processLogs = _unitOfWork.ProcessLogRepository.GetMany(x => x.ProcessQueueId == item.ProcessQueueId).ToList();
                        item.ProcessLog = processLogs;
                    }
                }
                r.Total = Total;
                if (entities.Any())
                    r.Result = PrepareResult(entities).ToList<Object>();
            }
            catch (Exception e)
            {
                r.Error.IsError = true;
                r.Error.Stacktrace = e.StackTrace;
                r.Error.Message = e.Message;
            }
            return r;
        }

        public int Create(ProcessQueueEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<ProcessQueue>(entity);
                _unitOfWork.ProcessQueueRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.ProcessQueueId;
            }
        }

        public bool Update(int entityId, ProcessQueueEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.ProcessQueueRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<ProcessQueue>(entity);
                        _unitOfWork.ProcessQueueRepository.Update(item);
                        _unitOfWork.Save();
                        scope.Commit();
                        success = true;
                    }
                }
            }
            return success;
        }

        public bool Delete(int entityId)
        {
            var success = false;
            if (entityId > 0)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var item = _unitOfWork.ProcessQueueRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.ProcessQueueRepository.Delete(item);
                        _unitOfWork.Save();
                        scope.Commit();
                        success = true;
                    }
                }
            }
            return success;
        }
    }
}
