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
    public class SystemLogService : IGenericService<SystemLogEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public SystemLogService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<SystemLog>("0");
        }
        public SystemLogEntity GetById(int id)
        {
            var product = _unitOfWork.SystemLogRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<SystemLogEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public SystemLogEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<SystemLog>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<SystemLog> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<SystemLogEntity> PrepareResult(List<SystemLog> entities)
        {
            return (entities.Select(e => new SystemLogEntity
            {
                LogId = e.LogId,
                EventId = e.EventId,
                Priority = e.Priority,
                Severity = e.Severity,
                Title = e.Title,
                Timestamp = e.Timestamp,
                MachineName = e.MachineName,
                AppDomainName = e.AppDomainName,
                ProcessName = e.ProcessName,
                Message = e.Message
            })).ToList();
        }
        public PagedResult GetAll(PaginationArgs args)
        {
            var r = new PagedResult();
            try
            {
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<SystemLog>(args, _unitOfWork);
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

        public int Create(SystemLogEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<SystemLog>(entity);
                _unitOfWork.SystemLogRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.LogId;
            }
        }

        public bool Update(int entityId, SystemLogEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.SystemLogRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<SystemLog>(entity);
                        _unitOfWork.SystemLogRepository.Update(item);
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
                    var item = _unitOfWork.SystemLogRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.SystemLogRepository.Delete(item);
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
