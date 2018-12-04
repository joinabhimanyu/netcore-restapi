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
    public class ProcessSchemaService : IGenericService<ProcessSchemaEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public ProcessSchemaService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<ProcessSchema>("0");
        }
        public ProcessSchemaEntity GetById(int id)
        {
            var product = _unitOfWork.ProcessSchemaRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<ProcessSchemaEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProcessSchemaEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<ProcessSchema>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<ProcessSchema> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<ProcessSchemaEntity> PrepareResult(List<ProcessSchema> entities)
        {
            return (entities.Select(e => new ProcessSchemaEntity
            {
                ProcessSchemaId = e.ProcessSchemaId,
                ProcessSchemaGuid = e.ProcessSchemaGuid,
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
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<ProcessSchema>(args, _unitOfWork);
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

        public int Create(ProcessSchemaEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<ProcessSchema>(entity);
                _unitOfWork.ProcessSchemaRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.ProcessSchemaId;
            }
        }

        public bool Update(int entityId, ProcessSchemaEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.ProcessSchemaRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<ProcessSchema>(entity);
                        _unitOfWork.ProcessSchemaRepository.Update(item);
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
                    var item = _unitOfWork.ProcessSchemaRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.ProcessSchemaRepository.Delete(item);
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
