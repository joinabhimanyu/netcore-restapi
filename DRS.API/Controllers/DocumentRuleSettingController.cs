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
    public class DocumentRuleSettingController : Controller
    {
        private IGenericListService<DocumentRuleSettingEntity> _documentRuleSettingService;

        public DocumentRuleSettingController(IGenericListService<DocumentRuleSettingEntity> unit)
        {
            _documentRuleSettingService = unit;
        }
        [HttpGet("~/api/DocumentRuleSetting/GetTypeHierarchy")]
        public IActionResult GetTypeHierarchy()
        {
            var items = _documentRuleSettingService.GetTypeHierarchy();
            if (items != null)
            {
                return Ok(items);
            }
            else
                return null;
        }
        // GET: api/values
        [HttpPost("~/api/getallDocumentRuleSettings")]
        public IActionResult Get([FromBody] PaginationArgs args)
        {
            var items = _documentRuleSettingService.GetAll(args ?? new PaginationArgs());
            if (items != null)
            {
                return Ok(items);
            }
            else
                return NotFound();
        }
        /// <summary>
        /// get single document rule setting by props
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("~/api/getSingleDocumentRuleSetting")]
        public IActionResult GetSingle([FromBody]GetSingleArgs args)
        {
            var item = _documentRuleSettingService.GetSingleByProp(args ?? new GetSingleArgs());
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
            var item = _documentRuleSettingService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/values
        [HttpPost]
        public int Post([FromBody]DocumentRuleSettingEntity value)
        {
            return _documentRuleSettingService.Create(value);
        }
        [HttpPost]
        public object[] Post([FromBody]ICollection<DocumentRuleSettingEntity> entities)
        {
            return _documentRuleSettingService.Create(entities);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody]DocumentRuleSettingEntity value)
        {
            if (id > 0)
            {
                return _documentRuleSettingService.Update(id, value);
            }
            return false;
        }
        [HttpPut]
        public bool Put([FromBody]ICollection<DocumentRuleSettingEntity> entities)
        {
            return _documentRuleSettingService.Update(entities);
        }
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return _documentRuleSettingService.Delete(id);
        }
        [HttpDelete]
        public bool Delete([FromBody]ICollection<DocumentRuleSettingEntity> entities)
        {
            return _documentRuleSettingService.Delete(entities);
        }
    }
}
