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
    public class ProcessQueueFileService : IGenericService<ProcessQueueFileEntity>
    {
        private readonly UnitOfWork _unitOfWork;

        public ProcessQueueFileService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<ProcessQueueFile>("0");
        }
        public ProcessQueueFileEntity GetById(int id)
        {
            var product = _unitOfWork.ProcessQueueFileRepository.Get(id);
            if (product != null)
            {
                var productModel = Mapper.Map<ProcessQueueFileEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProcessQueueFileEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<ProcessQueueFile>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<ProcessQueueFile> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<ProcessQueueFileEntity> PrepareResult(List<ProcessQueueFile> entities)
        {
            return (entities.Select(e => new ProcessQueueFileEntity
            {
                ProcessQueueFileId = e.ProcessQueueFileId,
                ProcessQueueId = e.ProcessQueueId,
                Path = e.Path,
                Filename = e.Filename,
                FileType = e.FileType,
                ProcessState = e.ProcessState,
                Created = e.Created,
                Stamp = e.Stamp
            })).ToList();
        }
        public PagedResult GetAll(PaginationArgs args)
        {
            var r = new PagedResult();
            try
            {
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<ProcessQueueFile>(args, _unitOfWork);
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

        public int Create(ProcessQueueFileEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<ProcessQueueFile>(entity);
                _unitOfWork.ProcessQueueFileRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.ProcessQueueFileId;
            }
        }

        public bool Update(int entityId, ProcessQueueFileEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.ProcessQueueFileRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<ProcessQueueFile>(entity);
                        _unitOfWork.ProcessQueueFileRepository.Update(item);
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
                    var item = _unitOfWork.ProcessQueueFileRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.ProcessQueueFileRepository.Delete(item);
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
