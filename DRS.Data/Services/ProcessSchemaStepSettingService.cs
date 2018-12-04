using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DRS.Data.BusinessEntities;
using DRS.Data.ServiceInterfaces;
using DRS.Model.Models;
using System;
using DRS.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace DRS.Data.Services
{
    public class ProcessSchemaStepSettingService : IGenericService<ProcessSchemaStepSettingEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public ProcessSchemaStepSettingService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<ProcessSchemaStepSetting>("0");
        }

        public ProcessSchemaStepSettingEntity GetById(int id)
        {
            var product = _unitOfWork.ProcessSchemaStepSettingRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<ProcessSchemaStepSettingEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProcessSchemaStepSettingEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<ProcessSchemaStepSetting>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<ProcessSchemaStepSetting> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<ProcessSchemaStepSettingEntity> PrepareResult(List<ProcessSchemaStepSetting> entities)
        {
            return (entities.Select(e => new ProcessSchemaStepSettingEntity
            {
                ProcessSchemaStepSettingId = e.ProcessSchemaStepSettingId,
                ProcessSchemaStepId = e.ProcessSchemaStepId,
                Name = e.Name,
                Value = e.Value,
                Description = e.Description,
                Created = e.Created,
                CreatedBy = e.CreatedBy,
                Updated = e.Updated,
                UpdatedBy = e.UpdatedBy,
                Stamp = e.Stamp
            })).ToList();
        }
        /// <summary>
        /// get all with navigation param as DocumentRuleId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        //public PagedResult GetAllFromDocumentRule(PaginationArgs args)
        //{
        //    var r = new PagedResult();
        //    List<ProcessSchemaStepSetting> entities = null;
        //    try
        //    {
        //        if (args.NavigationProperty.Props.First().PropName == "DocumentRuleId")
        //            entities = _unitOfWork.ProcessSchemaStepSettingRepository.GetAllFromDocumentRule(Convert.ToInt32(args.NavigationProperty.Props.FirstOrDefault().PropValue)).ToList();
        //        else
        //            entities = EfUtilities.PerformFilterOnNavigationProp<ProcessSchemaStepSetting>(args, _unitOfWork);

        //        entities = EfUtilities.PerformFilterOnProps<ProcessSchemaStepSetting>(args, entities, _unitOfWork);
        //        r.Total = EfUtilities.PerformPaging<ProcessSchemaStepSetting>(ref entities, args, _unitOfWork);
        //        if (entities.Any())
        //            r.Result = PrepareResult(entities).ToList<Object>();
        //    }
        //    catch (Exception e)
        //    {
        //        r.Error.IsError = true;
        //        r.Error.Stacktrace = e.StackTrace;
        //        r.Error.Message = e.Message;
        //    }
        //    return r;
        //}
        /// <summary>
        /// get all with any navigation args of self entity
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public PagedResult GetAll(PaginationArgs args)
        {
            var r = new PagedResult();
            try
            {
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<ProcessSchemaStepSetting>(args, _unitOfWork);
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

        public int Create(ProcessSchemaStepSettingEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<ProcessSchemaStepSetting>(entity);
                _unitOfWork.ProcessSchemaStepSettingRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.ProcessSchemaStepSettingId;
            }
        }

        public bool Update(int entityId, ProcessSchemaStepSettingEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.ProcessSchemaStepSettingRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<ProcessSchemaStepSetting>(entity);
                        _unitOfWork.ProcessSchemaStepSettingRepository.Update(item);
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
                    var item = _unitOfWork.ProcessSchemaStepSettingRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.ProcessSchemaStepSettingRepository.Delete(item);
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
