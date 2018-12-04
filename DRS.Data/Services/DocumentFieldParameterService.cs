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
    public class DocumentFieldParameterService : IGenericService<DocumentFieldParameterEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public DocumentFieldParameterService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<DocumentFieldParameter>("0");
        }
        public DocumentFieldParameterEntity GetById(int id)
        {
            var product = _unitOfWork.DocumentFieldParameterRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<DocumentFieldParameterEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DocumentFieldParameterEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<DocumentFieldParameter>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<DocumentFieldParameter> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<DocumentFieldParameterEntity> PrepareResult(List<DocumentFieldParameter> entities)
        {
            return (
                        from e in entities
                        select new DocumentFieldParameterEntity
                        {
                            DocumentFieldParameterId = e.DocumentFieldParameterId,
                            Parameter = e.Parameter,
                            Value = e.Value,
                            Description = e.Description,
                            Created = e.Created,
                            CreatedBy = e.CreatedBy,
                            Updated = e.Updated,
                            UpdatedBy = e.UpdatedBy,
                            Stamp=e.Stamp
                        }
                        ).ToList();
        }
        public PagedResult GetAll(PaginationArgs args)
        {
            var r = new PagedResult();
            try
            {
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<DocumentFieldParameter>(args, _unitOfWork);
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

        public int Create(DocumentFieldParameterEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<DocumentFieldParameter>(entity);
                _unitOfWork.DocumentFieldParameterRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.DocumentFieldParameterId;
            }
        }

        public bool Update(int entityId, DocumentFieldParameterEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.DocumentFieldParameterRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<DocumentFieldParameter>(entity);
                        _unitOfWork.DocumentFieldParameterRepository.Update(item);
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
                    var item = _unitOfWork.DocumentFieldParameterRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.DocumentFieldParameterRepository.Delete(item);
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
