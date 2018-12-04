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
    public class DocumentRuleController : Controller
    {
        private IGenericListService<DocumentRuleEntity> _documentRuleService;

        public DocumentRuleController(IGenericListService<DocumentRuleEntity> unit)
        {
            _documentRuleService = unit;
        }
        [HttpGet("~/api/DocumentRule/GetTypeHierarchy")]
        public IActionResult GetTypeHierarchy()
        {
            var items = _documentRuleService.GetTypeHierarchy();
            if (items != null)
            {
                return Ok(items);
            }
            else
                return null;
        }
        // GET: api/values
        [HttpPost("~/api/getallDocumentRules")]
        public IActionResult Get([FromBody] PaginationArgs args)
        {
            var items = _documentRuleService.GetAll(args ?? new PaginationArgs());
            if (items != null)
            {
                return Ok(items);
            }
            else
                return NotFound();
        }
        /// <summary>
        /// get single document rule by props
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("~/api/getSingleDocumentRule")]
        public IActionResult GetSingle([FromBody]GetSingleArgs args)
        {
            var item = _documentRuleService.GetSingleByProp(args ?? new GetSingleArgs());
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
            var item = _documentRuleService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/values
        [HttpPost]
        public int Post([FromBody]DocumentRuleEntity value)
        {
            return _documentRuleService.Create(value);
        }
        [HttpPost]
        public object[] Post([FromBody]ICollection<DocumentRuleEntity> entities)
        {
            return _documentRuleService.Create(entities);
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody]DocumentRuleEntity value)
        {
            if (id > 0)
            {
                return _documentRuleService.Update(id, value);
            }
            return false;
        }
        [HttpPut]
        public bool Put([FromBody]ICollection<DocumentRuleEntity> entities)
        {
            return _documentRuleService.Update(entities);
        }
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return _documentRuleService.Delete(id);
        }
        [HttpDelete]
        public bool Delete([FromBody]ICollection<DocumentRuleEntity> entities)
        {
            return _documentRuleService.Delete(entities);
        }
    }
}
