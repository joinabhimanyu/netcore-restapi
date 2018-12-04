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
    public class DocumentRuleSettingService : IGenericListService<DocumentRuleSettingEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public DocumentRuleSettingService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<DocumentRuleSetting>("0");
        }

        public DocumentRuleSettingEntity GetById(int id)
        {
            var product = _unitOfWork.DocumentRuleSettingRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<DocumentRuleSettingEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DocumentRuleSettingEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<DocumentRuleSetting>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<DocumentRuleSetting> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<DocumentRuleSettingEntity> PrepareResult(List<DocumentRuleSetting> entities)
        {
            return (from e in entities
                    select new DocumentRuleSettingEntity
                    {
                        DocumentRuleSettingId = e.DocumentRuleSettingId,
                        DocumentRuleId = e.DocumentRuleId,
                        Name = e.Name,
                        Value = e.Value,
                        Description = e.Description,
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
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<DocumentRuleSetting>(args, _unitOfWork);
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

        public int Create(DocumentRuleSettingEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<DocumentRuleSetting>(entity);
                _unitOfWork.DocumentRuleSettingRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.DocumentRuleSettingId;
            }
        }
        public object[] Create(ICollection<DocumentRuleSettingEntity> entities)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var listInput = new List<DocumentRuleSetting>();
                foreach (var item in entities)
                {
                    listInput.Add(Mapper.Map<DocumentRuleSetting>(item));
                }
                _unitOfWork.DocumentRuleSettingRepository.Create(listInput);
                _unitOfWork.Save();
                scope.Commit();
                return listInput.Select(x => new
                {
                    DocumentRuleSettingId = x.DocumentRuleSettingId
                }).ToArray();
            }
        }
        public bool Update(int entityId, DocumentRuleSettingEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.DocumentRuleSettingRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<DocumentRuleSetting>(entity);
                        _unitOfWork.DocumentRuleSettingRepository.Update(item);
                        _unitOfWork.Save();
                        scope.Commit();
                        success = true;
                    }
                }
            }
            return success;
        }
        public bool Update(ICollection<DocumentRuleSettingEntity> entities)
        {
            var success = true;
            if (entities != null)
            {
                try
                {
                    List<DocumentRuleSetting> inputList = new List<DocumentRuleSetting>();
                    foreach (var item in entities)
                    {
                        var existing = _unitOfWork.DocumentRuleSettingRepository.GetSingle(x => x.DocumentRuleSettingId == item.DocumentRuleSettingId);
                        if (existing != null)
                        {
                            var inputItem = Mapper.Map<DocumentRuleSetting>(item);
                            inputList.Add(inputItem);
                        }
                    }
                    if (inputList != null && inputList.Count > 0)
                    {
                        using (var scope = _unitOfWork.ContextTransaction)
                        {
                            _unitOfWork.DocumentRuleSettingRepository.Update(inputList);
                            _unitOfWork.Save();
                            scope.Commit();
                        }
                    }
                }
                catch (System.Exception)
                {
                    success = false;
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
                    var item = _unitOfWork.DocumentRuleSettingRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.DocumentRuleSettingRepository.Delete(item);
                        _unitOfWork.Save();
                        scope.Commit();
                        success = true;
                    }
                }
            }
            return success;
        }
        public bool Delete(ICollection<DocumentRuleSettingEntity> entities)
        {
            var success = true;
            var ids = new List<Int64>();
            try
            {
                foreach (var entity in entities)
                {
                    if (entity.DocumentRuleSettingId > 0)
                    {
                        var item = _unitOfWork.DocumentRuleSettingRepository.GetSingle(x => x.DocumentRuleSettingId == entity.DocumentRuleSettingId);
                        if (item != null)
                        {
                            ids.Add(entity.DocumentRuleSettingId);
                        }
                    }
                }
                if (ids.Count > 0)
                {
                    using (var scope = _unitOfWork.ContextTransaction)
                    {
                        _unitOfWork.DocumentRuleSettingRepository.Delete(ids.ToArray());
                        _unitOfWork.Save();
                        scope.Commit();
                    }
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }
    }
}
