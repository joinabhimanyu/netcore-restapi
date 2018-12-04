using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRS.Data;
using DRS.Data.BusinessEntities;
using DRS.Data.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DRS.Model.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DRS.API.Controllers
{
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        private readonly IGenericListService<DocumentEntity> _documentService;
        public DocumentController(IGenericListService<DocumentEntity> service)
        {
            _documentService = service;
        }
        [HttpGet("~/api/Document/GetTypeHierarchy")]
        public IActionResult GetTypeHierarchy()
        {
            var items = _documentService.GetTypeHierarchy();
            if (items != null)
            {
                return Ok(items);
            }
            else
                return null;
        }
        /// <summary>
        /// get all documents
        /// </summary>
        /// <returns></returns>
        [HttpPost("~/api/getallDocuments")]
        public IActionResult Get([FromBody]PaginationArgs args)
        {
            var u = User;
            var items = _documentService.GetAll(args ?? new PaginationArgs());
            if (items != null)
            {
                return Ok(items);
            }
            return NotFound();

        }
        /// <summary>
        /// get single document by props
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("~/api/getSingleDocument")]
        public IActionResult GetSingle([FromBody]GetSingleArgs args)
        {
            var item = _documentService.GetSingleByProp(args ?? new GetSingleArgs());
            if (item!=null)
            {
                return Ok(item);
            }
            return NotFound();
        }
        /// <summary>
        /// get document by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = _documentService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        /// <summary>
        /// create document
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public int Post([FromBody]DocumentEntity value)
        {
            return _documentService.Create(value);
        }
        /// <summary>
        /// create list of documents
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        [HttpPost]
        public object[] Post([FromBody] ICollection<DocumentEntity> entities)
        {
            return _documentService.Create(entities);
        }
        /// <summary>
        /// update document by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody]DocumentEntity value)
        {
            if (id > 0)
            {
                return _documentService.Update(id, value);
            }
            return false;
        }
        /// <summary>
        /// update list of documents
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        [HttpPut]
        public bool Put([FromBody] ICollection<DocumentEntity> entities)
        {
            return _documentService.Update(entities);
        }
        /// <summary>
        /// delete document by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return _documentService.Delete(id);
        }
        /// <summary>
        /// delete list of documents by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public bool Delete([FromBody]ICollection<DocumentEntity> entities)
        {
            return _documentService.Delete(entities);
        }
    }
}
