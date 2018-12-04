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
    public class ArchiveService : IGenericService<ArchiveEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public ArchiveService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<Archive>("0");
        }
        public ArchiveEntity GetById(int id)
        {
            var product = _unitOfWork.ArchiveRepository.Get(id);
            if (product != null)
            {
                var category = _unitOfWork.DocumentCategoryRepository.GetMany(x => x.ArchiveId == id);
                product.DocumentCategory = category.ToList();
                var productModel = Mapper.Map<ArchiveEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ArchiveEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<Archive>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<Archive> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<ArchiveEntity> PrepareResult(List<Archive> entities)
        {
            return (
                        from e in entities
                        select new ArchiveEntity
                        {
                            ArchiveId = e.ArchiveId,
                            Name = e.Name,
                            Description = e.Description,
                            Organization = e.Organization,
                            Created = e.Created,
                            CreatedBy = e.CreatedBy,
                            Updated = e.Updated,
                            UpdatedBy = e.UpdatedBy,
                            DocumentCategory = e.DocumentCategory!=null && e.DocumentCategory.Count>0? (from d in e.DocumentCategory
                                                select new DocumentCategoryEntity
                                                {
                                                    DocumentCategoryId = d != null ? d.DocumentCategoryId : 0,
                                                    ArchiveId = d != null ? d.ArchiveId : 0,
                                                    Category = d?.Category,
                                                    Name = d?.Name,
                                                    Description = d?.Description,
                                                    IsActive = d != null ? d.IsActive : false,
                                                    Created = d?.Created,
                                                    CreatedBy = d?.CreatedBy,
                                                    Updated = d?.Updated,
                                                    UpdatedBy = d?.UpdatedBy
                                                }).ToList():null
                        }
                        ).ToList();
        }
        public PagedResult GetAll(PaginationArgs args)
        {
            var r = new PagedResult();
            try
            {
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<Archive>(args, _unitOfWork);
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

        public int Create(ArchiveEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<Archive>(entity);
                _unitOfWork.ArchiveRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.ArchiveId;
            }
        }

        public bool Update(int entityId, ArchiveEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.ArchiveRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<Archive>(entity);
                        _unitOfWork.ArchiveRepository.Update(item);
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
                    var item = _unitOfWork.ArchiveRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.ArchiveRepository.Delete(item);
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
