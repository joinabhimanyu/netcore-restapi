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
    public class ProcessLogService : IGenericService<ProcessLogEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public ProcessLogService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<ProcessLog>("0");
        }

        public ProcessLogEntity GetById(int id)
        {
            var product = _unitOfWork.ProcessLogRepository.Get(id);
            if (product != null)
            {
                var processhandler = _unitOfWork.ProcessHandlerRepository.GetMany(x => x.ProcessHandlerId == product.ProcessHandlerId).FirstOrDefault();
                product.ProcessHandler = processhandler;
                var processqueue = _unitOfWork.ProcessQueueRepository.GetMany(x => x.ProcessQueueId == product.ProcessQueueId).FirstOrDefault();
                product.ProcessQueue = processqueue;
                var processschemastep = _unitOfWork.ProcessSchemaStepRepository.GetMany(x => x.ProcessSchemaStepId == product.ProcessSchemaStepId).FirstOrDefault();
                product.ProcessSchemaStep = processschemastep;

                var productModel = Mapper.Map<ProcessLogEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProcessLogEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<ProcessLog>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<ProcessLog> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<ProcessLogEntity> PrepareResult(List<ProcessLog> entities)
        {
            return (entities.Select(e => new ProcessLogEntity
            {
                ProcessLogId = e.ProcessLogId,
                ProcessHandlerId = e.ProcessHandlerId,
                ProcessQueueId = e.ProcessQueueId,
                ProcessSchemaStepId = e.ProcessSchemaStepId,
                TraceEventTypeId = e.TraceEventTypeId,
                EventId = e.EventId,
                Severity = e.Severity,
                Message = e.Message,
                Timestamp = e.Timestamp,
                ProcessHandler = e.ProcessHandler!=null? new ProcessHandlerEntity
                {
                    ProcessHandlerId = e.ProcessHandler != null ? e.ProcessHandler.ProcessHandlerId : 0,
                    ProcessHandlerGuid = e.ProcessHandler != null ? e.ProcessHandler.ProcessHandlerGuid : new Guid(),
                    Name = e.ProcessHandler?.Name,
                    Description = e.ProcessHandler?.Description,
                    AssemblyName = e.ProcessHandler?.AssemblyName,
                    ClassName = e.ProcessHandler?.ClassName,
                    IsActive = e.ProcessHandler != null ? e.ProcessHandler.IsActive : false,
                    ProcessHandlerTypeId = e.ProcessHandler != null ? e.ProcessHandler.ProcessHandlerTypeId : 0,
                    ProcessSchemaId = e.ProcessHandler != null ? e.ProcessHandler.ProcessSchemaId : 0,
                    Priority = e.ProcessHandler != null ? e.ProcessHandler.Priority : 0,
                    LastExecuteDate = e.ProcessHandler?.LastExecuteDate,
                    LastProcessDate = e.ProcessHandler?.LastProcessDate,
                    NextProcessDate = e.ProcessHandler?.NextProcessDate,
                    NumberOfItemsToProcess = e.ProcessHandler != null ? e.ProcessHandler.NumberOfItemsToProcess : 0,
                    NumberOfRetries = e.ProcessHandler != null ? e.ProcessHandler.NumberOfRetries : 0,
                    WaitIntervalBetweenRetries = e.ProcessHandler != null ? e.ProcessHandler.WaitIntervalBetweenRetries : 0,
                    NumberOfErrors = e.ProcessHandler != null ? e.ProcessHandler.NumberOfErrors : 0,
                    WaitIntervalOnErrors = e.ProcessHandler != null ? e.ProcessHandler.WaitIntervalOnErrors : 0,
                    ProcessErrorCount = e.ProcessHandler != null ? e.ProcessHandler.ProcessErrorCount : 0,
                    ProcessServers = e.ProcessHandler?.ProcessServers,
                    Created = e.ProcessHandler?.Created,
                    CreatedBy = e.ProcessHandler?.CreatedBy,
                    Updated = e.ProcessHandler?.Updated,
                    UpdatedBy = e.ProcessHandler?.UpdatedBy,
                    Stamp = e.ProcessHandler?.Stamp
                }:null,
                ProcessQueue = e.ProcessQueue!=null? new ProcessQueueEntity
                {
                    ProcessQueueId = e.ProcessQueue != null ? e.ProcessQueue.ProcessQueueId : 0,
                    ImportHandlerId = e.ProcessQueue != null ? e.ProcessQueue.ImportHandlerId : 0,
                    ProcessSchemaId = e.ProcessQueue != null ? e.ProcessQueue.ProcessSchemaId : 0,
                    DocumentRuleId = e.ProcessQueue?.DocumentRuleId,
                    NextProcessSchemaStepId = e.ProcessQueue?.NextProcessSchemaStepId,
                    ProcessQueueStateId = e.ProcessQueue != null ? e.ProcessQueue.ProcessQueueStateId : 0,
                    ProcessErrorCount = e.ProcessQueue != null ? e.ProcessQueue.ProcessErrorCount : 0,
                    ProcessTotaltime = e.ProcessQueue?.ProcessTotaltime,
                    LastProcessDate = e.ProcessQueue?.LastProcessDate,
                    DocumentId = e.ProcessQueue?.DocumentId,
                    DocumentKey = e.ProcessQueue?.DocumentKey,
                    DocumentIdentity = e.ProcessQueue?.DocumentIdentity,
                    DocumentName = e.ProcessQueue?.DocumentName,
                    DocumentCategory = e.ProcessQueue?.DocumentCategory,
                    DocumentStamp = e.ProcessQueue?.DocumentStamp,
                    DocumentPages = e.ProcessQueue?.DocumentPages,
                    DocumentArchiveKey = e.ProcessQueue?.DocumentArchiveKey,
                    Created = e.ProcessQueue?.Created,
                    Stamp = e.ProcessQueue?.Stamp
                }:null,
                ProcessSchemaStep = e.ProcessSchemaStep!=null? new ProcessSchemaStepEntity
                {
                    ProcessSchemaStepId = e.ProcessSchemaStep != null ? e.ProcessSchemaStep.ProcessSchemaStepId : 0,
                    ProcessSchemaStepGuid = e.ProcessSchemaStep != null ? e.ProcessSchemaStep.ProcessSchemaStepGuid : new Guid(),
                    ProcessSchemaId = e.ProcessSchemaStep != null ? e.ProcessSchemaStep.ProcessSchemaId : 0,
                    ProcessHandlerId = e.ProcessSchemaStep != null ? e.ProcessSchemaStep.ProcessHandlerId : 0,
                    StepOrder = e.ProcessSchemaStep != null ? e.ProcessSchemaStep.StepOrder : 0,
                    Name = e.ProcessSchemaStep?.Name,
                    Description = e.ProcessSchemaStep?.Description,
                    OnSuccessStepId = e.ProcessSchemaStep?.OnSuccessStepId,
                    OnSuccessStepGuid = e.ProcessSchemaStep?.OnSuccessStepGuid,
                    OnErrorStepId = e.ProcessSchemaStep?.OnErrorStepId,
                    OnErrorStepGuid = e.ProcessSchemaStep?.OnErrorStepGuid,
                    FieldQueryRule = e.ProcessSchemaStep?.FieldQueryRule,
                    Created = e.ProcessSchemaStep?.Created,
                    CreatedBy = e.ProcessSchemaStep?.CreatedBy,
                    Updated = e.ProcessSchemaStep?.Updated,
                    UpdatedBy = e.ProcessSchemaStep?.UpdatedBy,
                    Stamp = e.ProcessSchemaStep?.Stamp
                }:null
            })).ToList();
        }
        public PagedResult GetAll(PaginationArgs args)
        {
            var r = new PagedResult();
            try
            {
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<ProcessLog>(args, _unitOfWork);
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

        public int Create(ProcessLogEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<ProcessLog>(entity);
                _unitOfWork.ProcessLogRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.ProcessLogId;
            }
        }

        public bool Update(int entityId, ProcessLogEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.ProcessLogRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<ProcessLog>(entity);
                        _unitOfWork.ProcessLogRepository.Update(item);
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
                    var item = _unitOfWork.ProcessLogRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.ProcessLogRepository.Delete(item);
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
