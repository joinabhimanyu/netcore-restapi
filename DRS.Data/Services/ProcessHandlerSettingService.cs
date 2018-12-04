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
    public class ProcessHandlerSettingService : IGenericService<ProcessHandlerSettingEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public ProcessHandlerSettingService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<ProcessHandlerSetting>("0");
        }
        public ProcessHandlerSettingEntity GetById(int id)
        {
            var product = _unitOfWork.ProcessHandlerSettingRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<ProcessHandlerSettingEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProcessHandlerSettingEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<ProcessHandlerSetting>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<ProcessHandlerSetting> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<ProcessHandlerSettingEntity> PrepareResult(List<ProcessHandlerSetting> entities)
        {
            return (entities.Select(e => new ProcessHandlerSettingEntity
            {
                ProcessHandlerSettingId = e.ProcessHandlerSettingId,
                ProcessHandlerId = e.ProcessHandlerId,
                Name = e.Name,
                Value = e.Value,
                Description = e.Description,
                Visible = e.Visible,
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
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<ProcessHandlerSetting>(args, _unitOfWork);
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

        public int Create(ProcessHandlerSettingEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<ProcessHandlerSetting>(entity);
                _unitOfWork.ProcessHandlerSettingRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.ProcessHandlerSettingId;
            }
        }

        public bool Update(int entityId, ProcessHandlerSettingEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.ProcessHandlerSettingRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<ProcessHandlerSetting>(entity);
                        _unitOfWork.ProcessHandlerSettingRepository.Update(item);
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
                    var item = _unitOfWork.ProcessHandlerSettingRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.ProcessHandlerSettingRepository.Delete(item);
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
