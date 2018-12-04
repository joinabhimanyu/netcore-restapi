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
    public class ProcessHandlerService : IGenericService<ProcessHandlerEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public ProcessHandlerService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<ProcessHandler>("0");
        }

        public ProcessHandlerEntity GetById(int id)
        {
            var product = _unitOfWork.ProcessHandlerRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<ProcessHandlerEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProcessHandlerEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<ProcessHandler>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<ProcessHandler> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<ProcessHandlerEntity> PrepareResult(List<ProcessHandler> entities)
        {
            return (entities.Select(e => new ProcessHandlerEntity
            {
                ProcessHandlerId = e.ProcessHandlerId,
                ProcessHandlerGuid = e.ProcessHandlerGuid,
                Name = e.Name,
                Description = e.Description,
                AssemblyName = e.AssemblyName,
                ClassName = e.ClassName,
                IsActive = e.IsActive,
                ProcessHandlerTypeId = e.ProcessHandlerTypeId,
                ProcessSchemaId = e.ProcessSchemaId,
                Priority = e.Priority,
                LastExecuteDate = e.LastExecuteDate,
                LastProcessDate = e.LastProcessDate,
                NextProcessDate = e.NextProcessDate,
                NumberOfItemsToProcess = e.NumberOfItemsToProcess,
                NumberOfRetries = e.NumberOfRetries,
                WaitIntervalBetweenRetries = e.WaitIntervalBetweenRetries,
                NumberOfErrors = e.NumberOfErrors,
                WaitIntervalOnErrors = e.WaitIntervalOnErrors,
                ProcessErrorCount = e.ProcessErrorCount,
                ProcessServers = e.ProcessServers,
                Created = e.Created,
                CreatedBy = e.CreatedBy,
                Updated = e.Updated,
                UpdatedBy = e.UpdatedBy,
                Stamp = e.Stamp
            })).ToList();
        }
        public PagedResult GetAll(PaginationArgs args)
        {
            var r = new PagedResult();
            try
            {
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<ProcessHandler>(args, _unitOfWork);
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

        public int Create(ProcessHandlerEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<ProcessHandler>(entity);
                _unitOfWork.ProcessHandlerRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.ProcessHandlerId;
            }
        }

        public bool Update(int entityId, ProcessHandlerEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.ProcessHandlerRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<ProcessHandler>(entity);
                        _unitOfWork.ProcessHandlerRepository.Update(item);
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
                    var item = _unitOfWork.ProcessHandlerRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.ProcessHandlerRepository.Delete(item);
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
