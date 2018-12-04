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
    public class ProcessSchemaStepService : IGenericService<ProcessSchemaStepEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public ProcessSchemaStepService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<ProcessSchemaStep>("0");
        }
        public ProcessSchemaStepEntity GetById(int id)
        {
            var product = _unitOfWork.ProcessSchemaStepRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<ProcessSchemaStepEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProcessSchemaStepEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<ProcessSchemaStep>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<ProcessSchemaStep> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<ProcessSchemaStepEntity> PrepareResult(List<ProcessSchemaStep> entities)
        {
            return (entities.Select(e => new ProcessSchemaStepEntity
            {
                ProcessSchemaStepId = e.ProcessSchemaStepId,
                ProcessSchemaStepGuid = e.ProcessSchemaStepGuid,
                ProcessSchemaId = e.ProcessSchemaId,
                ProcessHandlerId = e.ProcessHandlerId,
                StepOrder = e.StepOrder,
                Name = e.Name,
                Description = e.Description,
                OnSuccessStepId = e.OnSuccessStepId,
                OnSuccessStepGuid = e.OnSuccessStepGuid,
                OnErrorStepId = e.OnErrorStepId,
                OnErrorStepGuid = e.OnErrorStepGuid,
                FieldQueryRule = e.FieldQueryRule,
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
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<ProcessSchemaStep>(args, _unitOfWork);
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

        public int Create(ProcessSchemaStepEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<ProcessSchemaStep>(entity);
                _unitOfWork.ProcessSchemaStepRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.ProcessSchemaStepId;
            }
        }

        public bool Update(int entityId, ProcessSchemaStepEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.ProcessSchemaStepRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<ProcessSchemaStep>(entity);
                        _unitOfWork.ProcessSchemaStepRepository.Update(item);
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
                    var item = _unitOfWork.ProcessSchemaStepRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.ProcessSchemaStepRepository.Delete(item);
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
