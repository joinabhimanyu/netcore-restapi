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
    public class DocumentFieldSettingsService : IGenericService<DocumentFieldSettingEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public DocumentFieldSettingsService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<DocumentFieldSetting>("0");
        }
        public DocumentFieldSettingEntity GetById(int id)
        {
            var product = _unitOfWork.DocumentFieldSettingRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<DocumentFieldSettingEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DocumentFieldSettingEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<DocumentFieldSetting>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<DocumentFieldSetting> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<DocumentFieldSettingEntity> PrepareResult(List<DocumentFieldSetting> entities)
        {
            return (from e in entities
                                                       select new DocumentFieldSettingEntity
                                                       {
                                                           DocumentFieldSettingId = e.DocumentFieldSettingId,
                                                           DocumentId = e.DocumentId,
                                                           FieldId = e.FieldId,
                                                           Name = e.Name,
                                                           Value = e.Value,
                                                           Description = e.Description,
                                                           IsActive = e.IsActive,
                                                           Created = e.Created,
                                                           CreatedBy = e.CreatedBy,
                                                           Updated = e.Updated,
                                                           UpdatedBy = e.UpdatedBy,
                                                           Stamp = e.Stamp
                                                       }).ToList();
        }
        public PagedResult GetAll(PaginationArgs args)
        {
            var r = new PagedResult();
            try
            {
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<DocumentFieldSetting>(args, _unitOfWork);
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

        public int Create(DocumentFieldSettingEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<DocumentFieldSetting>(entity);
                _unitOfWork.DocumentFieldSettingRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.DocumentFieldSettingId;
            }
        }

        public bool Update(int entityId, DocumentFieldSettingEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.DocumentFieldSettingRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<DocumentFieldSetting>(entity);
                        _unitOfWork.DocumentFieldSettingRepository.Update(item);
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
                    var item = _unitOfWork.DocumentFieldSettingRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.DocumentFieldSettingRepository.Delete(item);
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
