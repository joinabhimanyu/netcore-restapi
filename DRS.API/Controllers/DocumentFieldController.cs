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
    public class DocumentFieldController : Controller
    {
        private IGenericListService<DocumentFieldEntity> _documentFieldService;
        
        public DocumentFieldController(IGenericListService<DocumentFieldEntity> unit)
        {
            _documentFieldService = unit;
        }
        [HttpGet("~/api/DocumentField/GetTypeHierarchy")]
        public IActionResult GetTypeHierarchy()
        {
            var items = _documentFieldService.GetTypeHierarchy();
            if (items != null)
            {
                return Ok(items);
            }
            else
                return null;
        }
        // GET: api/values
        [HttpPost("~/api/getallDocumentFields")]
        public IActionResult Get([FromBody] PaginationArgs args)
        {
            var items = _documentFieldService.GetAll(args ?? new PaginationArgs());
            if (items != null)
            {
                return Ok(items);
            }
            else
                return NotFound();
        }
        /// <summary>
        /// get single document field by props
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("~/api/getSingleDocumentField")]
        public IActionResult GetSingle([FromBody]GetSingleArgs args)
        {
            var item = _documentFieldService.GetSingleByProp(args ?? new GetSingleArgs());
            if (item != null)
            {
                return Ok(item);
            }
            return NotFound();
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = _documentFieldService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/values
        [HttpPost]
        public int Post([FromBody]DocumentFieldEntity value)
        {
            return _documentFieldService.Create(value);
        }
        [HttpPost]
        public object[] Post([FromBody]ICollection<DocumentFieldEntity> entities)
        {
            return _documentFieldService.Create(entities);
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody]DocumentFieldEntity value)
        {
            if (id > 0)
            {
                return _documentFieldService.Update(id, value);
            }
            return false;
        }
        [HttpPut]
        public bool Put([FromBody]ICollection<DocumentFieldEntity> entities)
        {
            return _documentFieldService.Update(entities);
        }
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return _documentFieldService.Delete(id);
        }
        [HttpDelete]
        public bool Delete([FromBody]ICollection<DocumentFieldEntity> entities)
        {
            return _documentFieldService.Delete(entities);
        }
    }
}
