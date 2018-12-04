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
    public class ProcessHandlerFieldService : IGenericService<ProcessHandlerFieldEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public ProcessHandlerFieldService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<ProcessHandlerField>("0");
        }
        public ProcessHandlerFieldEntity GetById(int id)
        {
            var product = _unitOfWork.ProcessHandlerFieldRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<ProcessHandlerFieldEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProcessHandlerFieldEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<ProcessHandlerField>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<ProcessHandlerField> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<ProcessHandlerFieldEntity> PrepareResult(List<ProcessHandlerField> entities)
        {
            return (from e in entities
                                                      select new ProcessHandlerFieldEntity
                                                      {
                                                          ProcessHandlerFieldId = e.ProcessHandlerFieldId,
                                                          ProcessHandlerId = e.ProcessHandlerId,
                                                          FieldId = e.FieldId,
                                                          FieldName = e.FieldName,
                                                          FieldValueFormat = e.FieldValueFormat,
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
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<ProcessHandlerField>(args, _unitOfWork);
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

        public int Create(ProcessHandlerFieldEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<ProcessHandlerField>(entity);
                _unitOfWork.ProcessHandlerFieldRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.ProcessHandlerFieldId;
            }
        }

        public bool Update(int entityId, ProcessHandlerFieldEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.ProcessHandlerFieldRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<ProcessHandlerField>(entity);
                        _unitOfWork.ProcessHandlerFieldRepository.Update(item);
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
                    var item = _unitOfWork.ProcessHandlerFieldRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.ProcessHandlerFieldRepository.Delete(item);
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
