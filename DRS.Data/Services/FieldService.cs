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
    public class FieldService : IGenericService<FieldEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public FieldService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<Field>("0");
        }

        public FieldEntity GetById(int id)
        {
            var product = _unitOfWork.FieldRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<FieldEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public FieldEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<Field>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<Field> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<FieldEntity> PrepareResult(List<Field> entities)
        {
            return (
                        from e in entities
                        select new FieldEntity
                        {
                            FieldId = e.FieldId,
                            Name = e.Name,
                            Description = e.Description,
                            FieldDataTypeNo = e.FieldDataTypeNo,
                            IsActive = e.IsActive,
                            Created = e.Created,
                            CreatedBy = e.CreatedBy,
                            Updated = e.Updated,
                            UpdatedBy = e.UpdatedBy,
                            Stamp = e.Stamp
                        }
                        ).ToList();
        }
        public PagedResult GetAll(PaginationArgs args)
        {
            var r = new PagedResult();
            try
            {
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<Field>(args, _unitOfWork);
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

        public int Create(FieldEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<Field>(entity);
                _unitOfWork.FieldRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.FieldId;
            }
        }

        public bool Update(int entityId, FieldEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.FieldRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<Field>(entity);
                        _unitOfWork.FieldRepository.Update(item);
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
                    var item = _unitOfWork.FieldRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.FieldRepository.Delete(item);
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
