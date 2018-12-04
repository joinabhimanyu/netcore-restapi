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
    public class DocumentSourceController : Controller
    {
        private IGenericService<DocumentSourceEntity> _documentSourceService;

        public DocumentSourceController(IGenericService<DocumentSourceEntity> unit)
        {
            _documentSourceService = unit;
        }
        [HttpGet("~/api/DocumentSource/GetTypeHierarchy")]
        public IActionResult GetTypeHierarchy()
        {
            var items = _documentSourceService.GetTypeHierarchy();
            if (items != null)
            {
                return Ok(items);
            }
            else
                return null;
        }
        // GET: api/values
        [HttpPost("~/api/getallDocumentSources")]
        public IActionResult Get([FromBody] PaginationArgs args)
        {
            var items = _documentSourceService.GetAll(args ?? new PaginationArgs());
            if (items != null)
            {
                return Ok(items);
            }
            else
                return NotFound();
        }
        /// <summary>
        /// get single document source by props
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("~/api/getSingleDocumentSource")]
        public IActionResult GetSingle([FromBody]GetSingleArgs args)
        {
            var item = _documentSourceService.GetSingleByProp(args ?? new GetSingleArgs());
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
            var item = _documentSourceService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/values
        [HttpPost]
        public int Post([FromBody]DocumentSourceEntity value)
        {
            return _documentSourceService.Create(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody]DocumentSourceEntity value)
        {
            if (id > 0)
            {
                return _documentSourceService.Update(id, value);
            }
            return false;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return _documentSourceService.Delete(id);
        }
    }
}
