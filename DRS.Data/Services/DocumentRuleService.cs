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
    public class DocumentRuleService : IGenericListService<DocumentRuleEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public DocumentRuleService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }

        public DocumentRuleEntity GetById(int id)
        {
            var product = _unitOfWork.DocumentRuleRepository.Get(id);
            if (product != null)
            {
                var processSchema = this._unitOfWork.ProcessSchemaRepository.GetMany(x => x.ProcessSchemaId == product.ProcessSchemaId).ToList().FirstOrDefault();
                product.ProcessSchema = processSchema;
                var productModel = Mapper.Map<DocumentRuleEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DocumentRuleEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<DocumentRule>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<DocumentRule> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<DocumentRuleEntity> PrepareResult(List<DocumentRule> entities)
        {
            return (from e in entities
                                               select new DocumentRuleEntity
                                               {
                                                   DocumentRuleId = e.DocumentRuleId,
                                                   DocumentRuleGuid = e.DocumentRuleGuid,
                                                   Name = e.Name,
                                                   Description = e.Description,
                                                   DocumentId = e.DocumentId,
                                                   ProcessSchemaId = e.ProcessSchemaId,
                                                   RuleOrder = e.RuleOrder,
                                                   FieldQueryRule = e.FieldQueryRule,
                                                   IsActive = e.IsActive,
                                                   Created = e.Created,
                                                   CreatedBy = e.CreatedBy,
                                                   Updated = e.Updated,
                                                   UpdatedBy = e.UpdatedBy,
                                                   Stamp = e.Stamp,
                                                   ProcessSchema = e.ProcessSchema!=null? new ProcessSchemaEntity
                                                   {
                                                       ProcessSchemaId = e.ProcessSchema != null ? e.ProcessSchema.ProcessSchemaId : 0,
                                                       ProcessSchemaGuid = e.ProcessSchema != null ? e.ProcessSchema.ProcessSchemaGuid : new Guid(),
                                                       Name = e.ProcessSchema?.Name,
                                                       Description = e.ProcessSchema?.Description,
                                                       Created = e.ProcessSchema?.Created,
                                                       CreatedBy = e.ProcessSchema?.CreatedBy,
                                                       Updated = e.ProcessSchema?.Updated,
                                                       UpdatedBy = e.ProcessSchema?.UpdatedBy,
                                                       Stamp = e.ProcessSchema?.Stamp
                                                   }:null
                                               }).ToList();
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<DocumentRule>("0");
        }
        public PagedResult GetAll(PaginationArgs args)
        {
            var r = new PagedResult();
            try
            {
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<DocumentRule>(args, _unitOfWork);
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

        public int Create(DocumentRuleEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<DocumentRule>(entity);
                _unitOfWork.DocumentRuleRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.DocumentRuleId;
            }
        }
        public object[] Create(ICollection<DocumentRuleEntity> entities) {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var listInput = new List<DocumentRule>();
                foreach (var item in entities)
                {
                    listInput.Add(Mapper.Map<DocumentRule>(item));
                }
                _unitOfWork.DocumentRuleRepository.Create(listInput);
                _unitOfWork.Save();
                scope.Commit();
                return listInput.Select(x => new {
                    DocumentRuleId=x.DocumentRuleId
                }).ToArray();
            }
        }
        public bool Update(int entityId, DocumentRuleEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.DocumentRuleRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<DocumentRule>(entity);
                        _unitOfWork.DocumentRuleRepository.Update(item);
                        _unitOfWork.Save();
                        scope.Commit();
                        success = true;
                    }
                }
            }
            return success;
        }
        public bool Update(ICollection<DocumentRuleEntity> entities) {
            var success = true;
            if (entities != null)
            {
                try
                {
                    List<DocumentRule> inputList = new List<DocumentRule>();
                    foreach (var item in entities)
                    {
                        var existing = _unitOfWork.DocumentRuleRepository.GetSingle(x => x.DocumentRuleId == item.DocumentRuleId);
                        if (existing != null)
                        {
                            var inputItem = Mapper.Map<DocumentRule>(item);
                            inputList.Add(inputItem);
                        }
                    }
                    if (inputList != null && inputList.Count > 0)
                    {
                        using (var scope = _unitOfWork.ContextTransaction)
                        {
                            _unitOfWork.DocumentRuleRepository.Update(inputList);
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
                    var item = _unitOfWork.DocumentRuleRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.DocumentRuleRepository.Delete(item);
                        _unitOfWork.Save();
                        scope.Commit();
                        success = true;
                    }
                }
            }
            return success;
        }
        public bool Delete(ICollection<DocumentRuleEntity> entities)
        {
            var success = true;
            var ids = new List<Int64>();
            try
            {
                foreach (var entity in entities)
                {
                    if (entity.DocumentRuleId > 0)
                    {
                        var item = _unitOfWork.DocumentRuleRepository.GetSingle(x => x.DocumentRuleId == entity.DocumentRuleId);
                        if (item != null)
                        {
                            ids.Add(entity.DocumentRuleId);
                        }
                    }
                }
                if (ids.Count > 0)
                {
                    using (var scope = _unitOfWork.ContextTransaction)
                    {
                        _unitOfWork.DocumentRuleRepository.Delete(ids.ToArray());
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
