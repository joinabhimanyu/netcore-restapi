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
    public class DocumentFieldSettingController : Controller
    {
        private IGenericService<DocumentFieldSettingEntity> _documentFieldSettingService;

        public DocumentFieldSettingController(IGenericService<DocumentFieldSettingEntity> unit)
        {
            _documentFieldSettingService = unit;
        }
        [HttpGet("~/api/DocumentFieldSetting/GetTypeHierarchy")]
        public IActionResult GetTypeHierarchy()
        {
            var items = _documentFieldSettingService.GetTypeHierarchy();
            if (items != null)
            {
                return Ok(items);
            }
            else
                return null;
        }
        // GET: api/values
        [HttpPost("~/api/getallDocumentFieldSettings")]
        public IActionResult Get([FromBody] PaginationArgs args)
        {
            var items = _documentFieldSettingService.GetAll(args ?? new PaginationArgs());
            if (items != null)
            {
                return Ok(items);
            }
            else
                return NotFound();
        }
        /// <summary>
        /// get single document field setting by props
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("~/api/getSingleDocumentFieldSetting")]
        public IActionResult GetSingle([FromBody]GetSingleArgs args)
        {
            var item = _documentFieldSettingService.GetSingleByProp(args ?? new GetSingleArgs());
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
            var item = _documentFieldSettingService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/values
        [HttpPost]
        public int Post([FromBody]DocumentFieldSettingEntity value)
        {
            return _documentFieldSettingService.Create(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody]DocumentFieldSettingEntity value)
        {
            if (id > 0)
            {
                return _documentFieldSettingService.Update(id, value);
            }
            return false;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return _documentFieldSettingService.Delete(id);
        }
    }
}
