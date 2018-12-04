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
    public class ProcessQueueFieldService : IGenericService<ProcessQueueFieldEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public ProcessQueueFieldService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<ProcessQueueField>("0");
        }
        public ProcessQueueFieldEntity GetById(int id)
        {
            var product = _unitOfWork.ProcessQueueFieldRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<ProcessQueueFieldEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProcessQueueFieldEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<ProcessQueueField>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<ProcessQueueField> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<ProcessQueueFieldEntity> PrepareResult(List<ProcessQueueField> entities)
        {
            return (entities.Select(e => new ProcessQueueFieldEntity
            {
                ProcessQueueFieldId = e.ProcessQueueFieldId,
                ProcessQueueId = e.ProcessQueueId,
                FieldId = e.FieldId,
                FieldValue = e.FieldValue,
                Created = e.Created,
                Stamp = e.Stamp
            })).ToList();
        }
        public PagedResult GetAll(PaginationArgs args)
        {
            var r = new PagedResult();
            try
            {
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<ProcessQueueField>(args, _unitOfWork);
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

        public int Create(ProcessQueueFieldEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<ProcessQueueField>(entity);
                _unitOfWork.ProcessQueueFieldRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.ProcessQueueFieldId;
            }
        }

        public bool Update(int entityId, ProcessQueueFieldEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.ProcessQueueFieldRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<ProcessQueueField>(entity);
                        _unitOfWork.ProcessQueueFieldRepository.Update(item);
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
                    var item = _unitOfWork.ProcessQueueFieldRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.ProcessQueueFieldRepository.Delete(item);
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
