using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DRS.Data.BusinessEntities;
using DRS.Data.ServiceInterfaces;
using DRS.Model.Models;
using DRS.Model;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace DRS.Data.Services
{
    public class DocumentCategoryService : IGenericListService<DocumentCategoryEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public DocumentCategoryService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<DocumentCategory>("0");
        }
        /// <summary>
        /// get category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DocumentCategoryEntity GetById(int id)
        {
            var entity = _unitOfWork.DocumentCategoryRepository.Get(id);
            if (entity != null)
            {
                var documents = this._unitOfWork.DocumentRepository.GetMany(x => x.DocumentCategoryId == id);
                foreach (var item in documents)
                {
                    var source = this._unitOfWork.DocumentSourceRepository.GetMany(x => x.DocumentSourceId == item.DocumentSourceId);
                    item.DocumentSource = source.ToList().FirstOrDefault();
                }
                entity.Document = documents.ToList();
                var archive = this._unitOfWork.ArchiveRepository.GetMany(x => x.ArchiveId == entity.ArchiveId).ToList().FirstOrDefault();
                entity.Archive = archive;
                var categories = Mapper.Map<DocumentCategoryEntity>(entity);
                return categories;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DocumentCategoryEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<DocumentCategory>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<DocumentCategory> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<DocumentCategoryEntity> PrepareResult(List<DocumentCategory> entities)
        {
            return (from e in entities
                                                   select new DocumentCategoryEntity
                                                   {
                                                       DocumentCategoryId = e.DocumentCategoryId,
                                                       ArchiveId = e.ArchiveId,
                                                       Category = e.Category,
                                                       Name = e.Name,
                                                       Description = e.Description,
                                                       IsActive = e.IsActive,
                                                       Created = e.Created,
                                                       CreatedBy = e.CreatedBy,
                                                       Updated = e.Updated,
                                                       UpdatedBy = e.UpdatedBy,
                                                       Archive = e.Archive!=null? new ArchiveEntity
                                                       {
                                                           ArchiveId = e.Archive != null ? e.Archive.ArchiveId : 0,
                                                           Name = e.Archive?.Name,
                                                           Description = e.Archive?.Description,
                                                           Organization = e.Archive?.Organization,
                                                           Created = e.Archive?.Created,
                                                           CreatedBy = e.Archive?.CreatedBy,
                                                           Updated = e.Archive?.Updated,
                                                           UpdatedBy = e.Archive?.UpdatedBy
                                                       }:null,
                                                       Document = e.Document!=null && e.Document.Count>0? (from d in e.Document
                                                                   select new DocumentEntity
                                                                   {
                                                                       DocumentId = d != null ? d.DocumentId : 0,
                                                                       DocumentIdentity = d?.DocumentIdentity,
                                                                       Number = d?.Number,
                                                                       Version = d?.Version,
                                                                       Name = d?.Name,
                                                                       Description = d?.Description,
                                                                       Pages = d?.Pages,
                                                                       CompanyCode = d?.CompanyCode,
                                                                       BatchClass = d?.BatchClass,
                                                                       ExportFileType = d?.ExportFileType,
                                                                       Link = d?.Link,
                                                                       DocumentTemplateId = d != null ? d.DocumentTemplateId : 0,
                                                                       DocumentCategoryId = d != null ? d.DocumentCategoryId : 0,
                                                                       DocumentSourceId = d != null ? d.DocumentSourceId : 0,
                                                                       IsActive = d != null ? d.IsActive : false,
                                                                       Created = d?.Created,
                                                                       CreatedBy = d?.CreatedBy,
                                                                       Updated = d?.Updated,
                                                                       UpdatedBy = d?.UpdatedBy,
                                                                       DocumentSource = d.DocumentSource!=null? new DocumentSourceEntity
                                                                       {
                                                                           DocumentSourceId = d.DocumentSource != null ? d.DocumentSource.DocumentSourceId : 0,
                                                                           Name = d.DocumentSource?.Name,
                                                                           Description = d.DocumentSource?.Description,
                                                                           Organization = d.DocumentSource?.Organization,
                                                                           Priority = d.DocumentSource != null ? d.DocumentSource.Priority : 0,
                                                                           DocumentUriPath = d.DocumentSource?.DocumentUriPath,
                                                                           Created = d.DocumentSource?.Created,
                                                                           CreatedBy = d.DocumentSource?.CreatedBy,
                                                                           Updated = d.DocumentSource?.Updated,
                                                                           UpdatedBy = d.DocumentSource?.UpdatedBy,
                                                                           Stamp = d.DocumentSource?.Stamp
                                                                       }:null
                                                                   }).ToList():null
                                                   }).ToList();
        }
        /// <summary>
        /// get all categories
        /// </summary>
        /// <returns></returns>
        public PagedResult GetAll(PaginationArgs args)
        {
            var r = new PagedResult();
            try
            {
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<DocumentCategory>(args, _unitOfWork);
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
        /// create single category
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Create(DocumentCategoryEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<DocumentCategory>(entity);
                _unitOfWork.DocumentCategoryRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.DocumentCategoryId;
            }
        }
        /// <summary>
        /// create list of document category entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public object[] Create(ICollection<DocumentCategoryEntity> entities)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var listInput = new List<DocumentCategory>();
                foreach (var item in entities)
                {
                    listInput.Add(Mapper.Map<DocumentCategory>(item));
                }
                _unitOfWork.DocumentCategoryRepository.Create(listInput);
                _unitOfWork.Save();
                scope.Commit();
                return listInput.Select(x => new {
                    DocumentCategoryId=x.DocumentCategoryId
                }).ToArray();
            }
        }
        /// <summary>
        /// update document category entity
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(int entityId, DocumentCategoryEntity entity)
        {
            var success = true;
            try
            {
                if (entity != null)
                {
                    using (var scope = _unitOfWork.ContextTransaction)
                    {
                        var existing = _unitOfWork.DocumentCategoryRepository.GetSingle(x => x.DocumentCategoryId == entityId);
                        if (existing != null)
                        {
                            var item = Mapper.Map<DocumentCategory>(entity);
                            _unitOfWork.DocumentCategoryRepository.Update(item);
                            _unitOfWork.Save();
                            scope.Commit();
                        }
                    }
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }
        /// <summary>
        /// update list of document category entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool Update(ICollection<DocumentCategoryEntity> entities)
        {
            var success = true;
            if (entities != null)
            {
                try
                {
                    List<DocumentCategory> inputList = new List<DocumentCategory>();
                    foreach (var item in entities)
                    {
                        //var existing = _unitOfWork.DocumentCategoryRepository.GetByID(item.DocumentCategoryId);
                        var existing = _unitOfWork.DocumentCategoryRepository.GetSingle(x => x.DocumentCategoryId == item.DocumentCategoryId);
                        if (existing != null)
                        {
                            var inputItem = Mapper.Map<DocumentCategory>(item);
                            inputList.Add(inputItem);
                        }
                    }
                    if (inputList != null && inputList.Count > 0)
                    {
                        using (var scope = _unitOfWork.ContextTransaction)
                        {
                            _unitOfWork.DocumentCategoryRepository.Update(inputList);
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
        /// delete document category by id
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
                        var item = _unitOfWork.DocumentCategoryRepository.GetSingle(x => x.DocumentCategoryId == entityId);
                        if (item != null)
                        {
                            _unitOfWork.DocumentCategoryRepository.Delete(item);
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
        /// delete list of categories
        /// </summary>
        /// <param name="entityids"></param>
        /// <returns></returns>
        public bool Delete(ICollection<DocumentCategoryEntity> entities)
        {
            var success = true;
            var ids = new List<Int64>();
            try
            {
                foreach (var entity in entities)
                {
                    if (entity.DocumentCategoryId > 0)
                    {
                        var item = _unitOfWork.DocumentCategoryRepository.GetSingle(x => x.DocumentCategoryId == entity.DocumentCategoryId);
                        if (item != null)
                        {
                            ids.Add(entity.DocumentCategoryId);
                        }
                    }
                }
                if (ids.Count>0)
                {
                    using (var scope = _unitOfWork.ContextTransaction)
                    {
                        _unitOfWork.DocumentCategoryRepository.Delete(ids.ToArray());
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
