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
    public class DocumentSourceService : IGenericService<DocumentSourceEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public DocumentSourceService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<DocumentSource>("0");
        }

        public DocumentSourceEntity GetById(int id)
        {
            var product = _unitOfWork.DocumentSourceRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<DocumentSourceEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DocumentSourceEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<DocumentSource>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<DocumentSource> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<DocumentSourceEntity> PrepareResult(List<DocumentSource> entities)
        {
            return (from e in entities
                                                 select new DocumentSourceEntity
                                                 {
                                                     DocumentSourceId = e.DocumentSourceId,
                                                     Name = e.Name,
                                                     Description = e.Description,
                                                     Organization = e.Organization,
                                                     Priority = e.Priority,
                                                     DocumentUriPath = e.DocumentUriPath,
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
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<DocumentSource>(args, _unitOfWork);
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

        public int Create(DocumentSourceEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<DocumentSource>(entity);
                _unitOfWork.DocumentSourceRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.DocumentSourceId;
            }
        }

        public bool Update(int entityId, DocumentSourceEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.DocumentSourceRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<DocumentSource>(entity);
                        _unitOfWork.DocumentSourceRepository.Update(item);
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
                    var item = _unitOfWork.DocumentSourceRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.DocumentSourceRepository.Delete(item);
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
