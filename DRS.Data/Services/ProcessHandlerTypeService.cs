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
    public class ProcessHandlerTypeService : IGenericService<ProcessHandlerTypeEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public ProcessHandlerTypeService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<ProcessHandlerType>("0");
        }
        public ProcessHandlerTypeEntity GetById(int id)
        {
            var product = _unitOfWork.ProcessHandlerTypeRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<ProcessHandlerTypeEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProcessHandlerTypeEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<ProcessHandlerType>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<ProcessHandlerType> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<ProcessHandlerTypeEntity> PrepareResult(List<ProcessHandlerType> entities)
        {
            return (entities.Select(e => new ProcessHandlerTypeEntity
            {
                ProcessHandlerTypeId = e.ProcessHandlerTypeId,
                Name = e.Name,
                Description = e.Description,
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
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<ProcessHandlerType>(args, _unitOfWork);
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

        public int Create(ProcessHandlerTypeEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<ProcessHandlerType>(entity);
                _unitOfWork.ProcessHandlerTypeRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.ProcessHandlerTypeId;
            }
        }

        public bool Update(int entityId, ProcessHandlerTypeEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.ProcessHandlerTypeRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<ProcessHandlerType>(entity);
                        _unitOfWork.ProcessHandlerTypeRepository.Update(item);
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
                    var item = _unitOfWork.ProcessHandlerTypeRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.ProcessHandlerTypeRepository.Delete(item);
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
