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
    public class ProcessQueueStateService: IGenericService<ProcessQueueStateEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public ProcessQueueStateService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<ProcessQueueState>("0");
        }
        public ProcessQueueStateEntity GetById(int id)
        {
            var product = _unitOfWork.ProcessQueueStateRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<ProcessQueueStateEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProcessQueueStateEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<ProcessQueueState>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<ProcessQueueState> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<ProcessQueueStateEntity> PrepareResult(List<ProcessQueueState> entities)
        {
            return (entities.Select(e => new ProcessQueueStateEntity
            {
                ProcessQueueStateId = e.ProcessQueueStateId,
                Name = e.Name,
                Description = e.Description,
                StateImage = e.StateImage,
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
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<ProcessQueueState>(args, _unitOfWork);
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

        public int Create(ProcessQueueStateEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<ProcessQueueState>(entity);
                _unitOfWork.ProcessQueueStateRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.ProcessQueueStateId;
            }
        }

        public bool Update(int entityId, ProcessQueueStateEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.ProcessQueueStateRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<ProcessQueueState>(entity);
                        _unitOfWork.ProcessQueueStateRepository.Update(item);
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
                    var item = _unitOfWork.ProcessQueueStateRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.ProcessQueueStateRepository.Delete(item);
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
