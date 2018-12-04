using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DRS.Data.BusinessEntities;
using DRS.Data.ServiceInterfaces;
using DRS.Model.Models;
using DRS.Model;
using Newtonsoft.Json.Linq;

namespace DRS.Data.Services
{
    public class DocumentService : IGenericListService<DocumentEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public DocumentService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<Document>("0");
        }
        /// <summary>
        /// get document by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DocumentEntity GetById(int id)
        {
            var entity = _unitOfWork.DocumentRepository.Get(id);
            if (entity != null)
            {
                var documentsource = this._unitOfWork.DocumentSourceRepository.GetMany(x => x.DocumentSourceId == entity.DocumentSourceId).ToList().FirstOrDefault();
                entity.DocumentSource = documentsource;
                var documentcategory = this._unitOfWork.DocumentCategoryRepository.GetMany(x => x.DocumentCategoryId == entity.DocumentCategoryId).ToList().FirstOrDefault();
                entity.DocumentCategory = documentcategory;
                var documentrules = this._unitOfWork.DocumentRuleRepository.GetMany(x => x.DocumentId == entity.DocumentId).ToList();
                entity.DocumentRule = documentrules;
                var document = Mapper.Map<DocumentEntity>(entity);
                return document;
            }
            return null;
        }
        private List<DocumentEntity> PrepareResult(List<Document> entities)
        {
            return (from e in entities
                    select new DocumentEntity
                    {
                        DocumentId = e.DocumentId,
                        DocumentIdentity = e.DocumentIdentity,
                        Number = e.Number,
                        Version = e.Version,
                        Name = e.Name,
                        Description = e.Description,
                        Pages = e.Pages,
                        CompanyCode = e.CompanyCode,
                        BatchClass = e.BatchClass,
                        ExportFileType = e.ExportFileType,
                        Link = e.Link,
                        DocumentTemplateId = e.DocumentTemplateId,
                        DocumentCategoryId = e.DocumentCategoryId,
                        DocumentSourceId = e.DocumentSourceId,
                        IsActive = e.IsActive,
                        Created = e.Created,
                        CreatedBy = e.CreatedBy,
                        Updated = e.Updated,
                        UpdatedBy = e.UpdatedBy,
                        Stamp = e.Stamp,
                        DocumentSource = e.DocumentSource != null ? new DocumentSourceEntity
                        {
                            DocumentSourceId = e.DocumentSource != null ? e.DocumentSource.DocumentSourceId : 0,
                            Name = e.DocumentSource?.Name,
                            Description = e.DocumentSource?.Description,
                            Organization = e.DocumentSource?.Organization,
                            Priority = e.DocumentSource != null ? e.DocumentSource.Priority : 0,
                            DocumentUriPath = e.DocumentSource?.DocumentUriPath,
                            Created = e.DocumentSource?.Created,
                            CreatedBy = e.DocumentSource?.CreatedBy,
                            Updated = e.DocumentSource?.Updated,
                            UpdatedBy = e.DocumentSource?.UpdatedBy,
                            Stamp = e.DocumentSource?.Stamp
                        } :null,
                        DocumentCategory = e.DocumentCategory != null ? new DocumentCategoryEntity
                        {
                            DocumentCategoryId = e.DocumentCategory != null ? e.DocumentCategory.DocumentCategoryId : 0,
                            ArchiveId = e.DocumentCategory != null ? e.DocumentCategory.ArchiveId : 0,
                            Category = e.DocumentCategory?.Category,
                            Name = e.DocumentCategory?.Name,
                            Description = e.DocumentCategory?.Description,
                            IsActive = e.DocumentCategory != null ? e.DocumentCategory.IsActive : false,
                            Created = e.DocumentCategory?.Created,
                            CreatedBy = e.DocumentCategory?.CreatedBy,
                            Updated = e.DocumentCategory?.Updated,
                            UpdatedBy = e.DocumentCategory?.UpdatedBy
                        } : null,
                        DocumentRule = e.DocumentRule != null && e.DocumentRule.Count>0 ? (from dr in e.DocumentRule
                                                                 select new DocumentRuleEntity
                                                                 {
                                                                     DocumentRuleId = dr != null ? dr.DocumentRuleId : 0,
                                                                     DocumentRuleGuid = dr != null ? dr.DocumentRuleGuid : new Guid(),
                                                                     Name = dr?.Name,
                                                                     Description = dr?.Description,
                                                                     DocumentId = dr != null ? dr.DocumentId : 0,
                                                                     ProcessSchemaId = dr != null ? dr.ProcessSchemaId : 0,
                                                                     RuleOrder = dr != null ? dr.RuleOrder : 0,
                                                                     FieldQueryRule = dr?.FieldQueryRule,
                                                                     IsActive = dr != null ? dr.IsActive : false,
                                                                     Created = dr?.Created,
                                                                     CreatedBy = dr?.CreatedBy,
                                                                     Updated = dr?.Updated,
                                                                     UpdatedBy = dr?.UpdatedBy,
                                                                     Stamp = dr?.Stamp
                                                                 }).ToList() : null
                    }).ToList();
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DocumentEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<Document>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<Document> { entity }).FirstOrDefault();
            }
            return null;
        }
        /// <summary>
        /// get all documents
        /// </summary>
        /// <returns></returns>
        public PagedResult GetAll(PaginationArgs args)
        {
            var r = new PagedResult();
            try
            {
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<Document>(args, _unitOfWork);
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
        /// <summary>
        /// create single document
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Create(DocumentEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<Document>(entity);
                _unitOfWork.DocumentRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.DocumentId;
            }
        }
        /// <summary>
        /// create list of documents
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public object[] Create(ICollection<DocumentEntity> entities)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var listInput = new List<Document>();
                foreach (var item in entities)
                {
                    listInput.Add(Mapper.Map<Document>(item));
                }
                _unitOfWork.DocumentRepository.Create(listInput);
                _unitOfWork.Save();
                scope.Commit();
                return listInput.Select(x => new
                {
                    DocumentId = x.DocumentId
                }).ToArray();
            }
        }
        /// <summary>
        /// update single document
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(int entityId, DocumentEntity entity)
        {
            var success = true;
            try
            {
                if (entity != null)
                {
                    using (var scope = _unitOfWork.ContextTransaction)
                    {
                        var existing = _unitOfWork.DocumentRepository.GetSingle(x => x.DocumentId == entityId);
                        if (existing != null)
                        {
                            var item = Mapper.Map<Document>(entity);
                            _unitOfWork.DocumentRepository.Update(item);
                            _unitOfWork.Save();
                            scope.Commit();
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                success = false;
            }
            return success;
        }
        /// <summary>
        /// update list of entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool Update(ICollection<DocumentEntity> entities)
        {
            var success = true;
            if (entities != null)
            {
                try
                {
                    List<Document> inputList = new List<Document>();
                    foreach (var item in entities)
                    {
                        var existing = _unitOfWork.DocumentRepository.GetSingle(x => x.DocumentId == item.DocumentId);
                        if (existing != null)
                        {
                            var inputItem = Mapper.Map<Document>(item);
                            inputList.Add(inputItem);
                        }
                    }
                    if (inputList != null && inputList.Count > 0)
                    {
                        using (var scope = _unitOfWork.ContextTransaction)
                        {
                            _unitOfWork.DocumentRepository.Update(inputList);
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
        /// <summary>
        /// delete single document
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public bool Delete(int entityId)
        {
            var success = true;
            try
            {
                if (entityId > 0)
                {
                    using (var scope = _unitOfWork.ContextTransaction)
                    {
                        var item = _unitOfWork.DocumentRepository.GetSingle(x => x.DocumentId == entityId);
                        if (item != null)
                        {
                            _unitOfWork.DocumentRepository.Delete(item);
                            _unitOfWork.Save();
                            scope.Commit();
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                success = false;
            }
            return success;
        }
        /// <summary>
        /// delete list of documents
        /// </summary>
        /// <param name="entityids"></param>
        /// <returns></returns>
        public bool Delete(ICollection<DocumentEntity> entities)
        {
            var success = true;
            var ids = new List<Int64>();
            try
            {
                foreach (var entity in entities)
                {
                    if (entity.DocumentId > 0)
                    {
                        var item = _unitOfWork.DocumentRepository.GetSingle(x => x.DocumentId == entity.DocumentId);
                        if (item != null)
                        {
                            ids.Add(entity.DocumentId);
                        }
                    }
                }
                if (ids.Count > 0)
                {
                    using (var scope = _unitOfWork.ContextTransaction)
                    {
                        _unitOfWork.DocumentRepository.Delete(ids.ToArray());
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
