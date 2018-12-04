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
    public class ProcessSchemaStepSettingController : Controller
    {
        private IGenericService<ProcessSchemaStepSettingEntity> _service;

        public ProcessSchemaStepSettingController(IGenericService<ProcessSchemaStepSettingEntity> unit)
        {
            _service = unit;
        }
        //[HttpPost("~/api/getallProcessSchemaStepSettingsFromRule")]
        //public IActionResult GetAllFromDocumentRule([FromBody] PaginationArgs args)
        //{
        //    var items = _service.GetAllFromDocumentRule(args ?? new PaginationArgs());
        //    if (items != null)
        //    {
        //        return Ok(items);
        //    }
        //    else
        //        return NotFound();
        //}
        [HttpGet("~/api/ProcessSchemaStepSetting/GetTypeHierarchy")]
        public IActionResult GetTypeHierarchy()
        {
            var items = _service.GetTypeHierarchy();
            if (items != null)
            {
                return Ok(items);
            }
            else
                return null;
        }
        // GET: api/values
        [HttpPost("~/api/getallProcessSchemaStepSettings")]
        public IActionResult Get([FromBody] PaginationArgs args)
        {
            var items = _service.GetAll(args ?? new PaginationArgs());
            if (items != null)
            {
                return Ok(items);
            }
            else
                return NotFound();
        }
        /// <summary>
        /// get single process schema step setting by props
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("~/api/getSingleProcessSchemaStepSetting")]
        public IActionResult GetSingle([FromBody]GetSingleArgs args)
        {
            var item = _service.GetSingleByProp(args ?? new GetSingleArgs());
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
            var item = _service.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/values
        [HttpPost]
        public int Post([FromBody]ProcessSchemaStepSettingEntity value)
        {
            return _service.Create(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody]ProcessSchemaStepSettingEntity value)
        {
            if (id > 0)
            {
                return _service.Update(id, value);
            }
            return false;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return _service.Delete(id);
        }
    }
}
