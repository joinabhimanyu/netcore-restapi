using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRS.Data;
using DRS.Data.BusinessEntities;
using DRS.Data.ServiceInterfaces;
using DRS.Model.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DRS.API.Controllers
{
    [Route("api/[controller]")]
    public class DocumentCategoryController : Controller
    {
        private IGenericListService<DocumentCategoryEntity> _documentCategoryService;
        public DocumentCategoryController(IGenericListService<DocumentCategoryEntity> service)
        {
            _documentCategoryService = service;
        }
        [HttpGet("~/api/DocumentCategory/GetTypeHierarchy")]
        public IActionResult GetTypeHierarchy()
        {
            var items = _documentCategoryService.GetTypeHierarchy();
            if (items != null)
            {
                return Ok(items);
            }
            else
                return null;
        }
        /// <summary>
        /// get all document categories
        /// </summary>
        /// <returns></returns>
        [HttpPost("~/api/getallDocumentCategories")]
        public IActionResult Get([FromBody] PaginationArgs args)
        {
            var items = _documentCategoryService.GetAll(args ?? new PaginationArgs());
            if (items != null)
            {
                return Ok(items);
            }
            else
                return NotFound();
        }
        /// <summary>
        /// get single document category by props
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("~/api/getSingleDocumentCategory")]
        public IActionResult GetSingle([FromBody]GetSingleArgs args)
        {
            var item = _documentCategoryService.GetSingleByProp(args ?? new GetSingleArgs());
            if (item != null)
            {
                return Ok(item);
            }
            return NotFound();
        }
        /// <summary>
        /// get document category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = _documentCategoryService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        /// <summary>
        /// create document category
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public int Post([FromBody]DocumentCategoryEntity value)
        {
            return _documentCategoryService.Create(value);
        }
        /// <summary>
        /// create list of document categories
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        [HttpPost]
        public object[] Post([FromBody] ICollection<DocumentCategoryEntity> entities)
        {
            return _documentCategoryService.Create(entities);
        }
        /// <summary>
        /// update document category by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody]DocumentCategoryEntity value)
        {
            if (id > 0)
            {
                return _documentCategoryService.Update(id, value);
            }
            return false;
        }
        /// <summary>
        /// update list of document categories
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        [HttpPut]
        public bool Put([FromBody] ICollection<DocumentCategoryEntity> entities)
        {
            return _documentCategoryService.Update(entities);
        }
        /// <summary>
        /// delete document category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return _documentCategoryService.Delete(id);
        }
        /// <summary>
        /// delete list of document category entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        [HttpDelete]
        public bool Delete([FromBody]ICollection<DocumentCategoryEntity> entities)
        {
            return _documentCategoryService.Delete(entities);
        }
    }
}
