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
    public class SystemSettingService : IGenericService<SystemSettingEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public SystemSettingService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<SystemSetting>("0");
        }

        public SystemSettingEntity GetById(int id)
        {
            var product = _unitOfWork.SystemSettingRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<SystemSettingEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public SystemSettingEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<SystemSetting>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<SystemSetting> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<SystemSettingEntity> PrepareResult(List<SystemSetting> entities)
        {
            return (entities.Select(e => new SystemSettingEntity
            {
                SystemSettingId = e.SystemSettingId,
                Name = e.Name,
                Value = e.Value,
                Description = e.Description,
                SystemSettingTypeId = e.SystemSettingTypeId,
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
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<SystemSetting>(args, _unitOfWork);
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

        public int Create(SystemSettingEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<SystemSetting>(entity);
                _unitOfWork.SystemSettingRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.SystemSettingId;
            }
        }

        public bool Update(int entityId, SystemSettingEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.SystemSettingRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<SystemSetting>(entity);
                        _unitOfWork.SystemSettingRepository.Update(item);
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
                    var item = _unitOfWork.SystemSettingRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.SystemSettingRepository.Delete(item);
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
