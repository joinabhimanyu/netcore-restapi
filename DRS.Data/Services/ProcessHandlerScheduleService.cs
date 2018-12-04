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
    public class ProcessHandlerScheduleService : IGenericService<ProcessHandlerScheduleEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public ProcessHandlerScheduleService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<ProcessHandlerSchedule>("0");
        }
        public ProcessHandlerScheduleEntity GetById(int id)
        {
            var product = _unitOfWork.ProcessHandlerScheduleRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<ProcessHandlerScheduleEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProcessHandlerScheduleEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<ProcessHandlerSchedule>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<ProcessHandlerSchedule> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<ProcessHandlerScheduleEntity> PrepareResult(List<ProcessHandlerSchedule> entities)
        {
            return (from e in entities
                                                         select new ProcessHandlerScheduleEntity
                                                         {
                                                             ProcessHandlerScheduleId = e.ProcessHandlerScheduleId,
                                                             ProcessHandlerId = e.ProcessHandlerId,
                                                             Name = e.Name,
                                                             Description = e.Description,
                                                             IsActive = e.IsActive,
                                                             FrequencyTypeId = e.FrequencyTypeId,
                                                             FrequencyFlags = e.FrequencyFlags,
                                                             DailyFrequencyOccursAt = e.DailyFrequencyOccursAt,
                                                             DailyFrequencyInterval = e.DailyFrequencyInterval,
                                                             DailyFrequencyIntervalType = e.DailyFrequencyIntervalType,
                                                             DailyFrequencyStartAt = e.DailyFrequencyStartAt,
                                                             DailyFrequencyEndAt = e.DailyFrequencyEndAt,
                                                             DurationStartDate = e.DurationStartDate,
                                                             DurationEndDate = e.DurationEndDate,
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
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<ProcessHandlerSchedule>(args, _unitOfWork);
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

        public int Create(ProcessHandlerScheduleEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<ProcessHandlerSchedule>(entity);
                _unitOfWork.ProcessHandlerScheduleRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.ProcessHandlerScheduleId;
            }
        }

        public bool Update(int entityId, ProcessHandlerScheduleEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.ProcessHandlerScheduleRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<ProcessHandlerSchedule>(entity);
                        _unitOfWork.ProcessHandlerScheduleRepository.Update(item);
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
                    var item = _unitOfWork.ProcessHandlerScheduleRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.ProcessHandlerScheduleRepository.Delete(item);
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
