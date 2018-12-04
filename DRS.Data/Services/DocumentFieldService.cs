using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DRS.Data.BusinessEntities;
using DRS.Data.ServiceInterfaces;
using DRS.Model.Models;
using DRS.Model;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace DRS.Data.Services
{
    public class DocumentFieldService : IGenericListService<DocumentFieldEntity>
    {

        private readonly UnitOfWork _unitOfWork;

        public DocumentFieldService(UnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public JArray GetTypeHierarchy()
        {
            return EfUtilities.GetEntityPropertyHierarchy<DocumentField>("0");
        }
        public DocumentFieldEntity GetById(int id)
        {
            var product = _unitOfWork.DocumentFieldRepository.Get(id);
            if (product != null)
            {
                var field = _unitOfWork.FieldRepository.GetMany(x => x.FieldId == product.FieldId).FirstOrDefault();
                product.Field = field;
                var productModel = Mapper.Map<DocumentFieldEntity>(product);
                return productModel;
            }
            return null;
        }
        /// <summary>
        /// get single with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DocumentFieldEntity GetSingleByProp(GetSingleArgs args)
        {
            var entity = EfUtilities.GetSingleWithProps<DocumentField>(args, _unitOfWork);
            if (entity != null)
            {
                return PrepareResult(new List<DocumentField> { entity }).FirstOrDefault();
            }
            return null;
        }
        private List<DocumentFieldEntity> PrepareResult(List<DocumentField> entities)
        {
            return (
                        from e in entities
                        select new DocumentFieldEntity
                        {
                            DocumentId = e.DocumentId,
                            FieldId = e.FieldId,
                            Name = e.Name,
                            Value = e.Value,
                            Parameter = e.Parameter,
                            Created = e.Created,
                            CreatedBy = e.CreatedBy,
                            Updated = e.Updated,
                            UpdatedBy = e.UpdatedBy,
                            Stamp = e.Stamp,
                            Field = e.Field!=null? new FieldEntity
                            {
                                FieldId = e.Field != null ? e.Field.FieldId : 0,
                                Name = e.Field?.Name,
                                Description = e.Field?.Description,
                                FieldDataTypeNo = e.Field?.FieldDataTypeNo,
                                IsActive = e.Field != null ? e.Field.IsActive : false,
                                Created = e.Field?.Created,
                                CreatedBy = e.Field?.CreatedBy,
                                Updated = e.Field?.Updated,
                                UpdatedBy = e.Field?.UpdatedBy,
                                Stamp = e.Field?.Stamp
                            }:null
                        }
                        ).ToList();
        }
        public PagedResult GetAll(PaginationArgs args)
        {
            var r = new PagedResult();
            try
            {
                var (entities, Total) = EfUtilities.PerformFilterationAndPaging<DocumentField>(args, _unitOfWork);
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

        public int Create(DocumentFieldEntity entity)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var item = Mapper.Map<DocumentField>(entity);
                _unitOfWork.DocumentFieldRepository.Create(item);
                _unitOfWork.Save();
                scope.Commit();
                return item.DocumentId;
            }
        }
        public object[] Create(ICollection<DocumentFieldEntity> entities)
        {
            using (var scope = _unitOfWork.ContextTransaction)
            {
                var listInput = new List<DocumentField>();
                foreach (var item in entities)
                {
                    listInput.Add(Mapper.Map<DocumentField>(item));
                }
                _unitOfWork.DocumentFieldRepository.Create(listInput);
                _unitOfWork.Save();
                scope.Commit();
                return listInput.Select(x => new
                {
                    DocumentId = x.DocumentId,
                    FieldId = x.FieldId
                }).ToArray();
            }
        }
        private IEnumerable<DocumentField> GetDocumentFields(ICollection<DocumentFieldEntity> entities)
        {
            IEnumerable<DocumentField> builtUpQuery = null;

            foreach (var item in entities)
            {
                // first time through only.
                if (builtUpQuery == null)
                {
                    builtUpQuery = _unitOfWork.DocumentFieldRepository.GetMany(p => p.DocumentId == item.DocumentId && p.FieldId == item.FieldId);
                }
                // all subsequent times
                else
                {
                    builtUpQuery = builtUpQuery.Union(_unitOfWork.DocumentFieldRepository.GetMany(p => p.DocumentId == item.DocumentId && p.FieldId == item.FieldId));
                }
            }

            return builtUpQuery;
        }
        public bool Update(int entityId, DocumentFieldEntity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = _unitOfWork.ContextTransaction)
                {
                    var product = _unitOfWork.DocumentFieldRepository.Get(entityId);
                    if (product != null)
                    {
                        var item = Mapper.Map<DocumentField>(entity);
                        _unitOfWork.DocumentFieldRepository.Update(item);
                        _unitOfWork.Save();
                        scope.Commit();
                        success = true;
                    }
                }
            }
            return success;
        }
        public bool Update(ICollection<DocumentFieldEntity> entities)
        {
            var success = true;
            if (entities != null)
            {
                try
                {
                    List<DocumentField> inputList = new List<DocumentField>();

                    foreach (var item in entities)
                    {
                        var existing = _unitOfWork.DocumentFieldRepository.GetSingle(x => x.DocumentId == item.DocumentId && x.FieldId == item.FieldId);
                        if (existing != null)
                        {
                            var inputItem = Mapper.Map<DocumentField>(item);
                            inputList.Add(inputItem);
                        }
                    }
                    if (inputList != null && inputList.Count > 0)
                    {
                        using (var scope = _unitOfWork.ContextTransaction)
                        {
                            _unitOfWork.DocumentFieldRepository.Update(inputList);
                            _unitOfWork.Save();
                            scope.Commit();
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                    success = false;
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
                    var item = _unitOfWork.DocumentFieldRepository.Get(entityId);
                    if (item != null)
                    {
                        _unitOfWork.DocumentFieldRepository.Delete(item);
                        _unitOfWork.Save();
                        scope.Commit();
                        success = true;
                    }
                }
            }
            return success;
        }
        public bool Delete(ICollection<DocumentFieldEntity> entities)
        {
            var success = true;
            try
            {
                var inputList = GetDocumentFields(entities);
                if (inputList != null && inputList.Count() > 0)
                {
                    using (var scope = _unitOfWork.ContextTransaction)
                    {
                        _unitOfWork.DocumentFieldRepository.Delete(inputList.ToList());
                        _unitOfWork.Save();
                        scope.Commit();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success = false;
            }
            return success;
        }
    }
}
