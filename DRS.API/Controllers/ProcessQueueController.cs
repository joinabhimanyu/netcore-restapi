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
    public class ProcessQueueController : Controller
    {
        private IGenericService<ProcessQueueEntity> _processQueueService;

        public ProcessQueueController(IGenericService<ProcessQueueEntity> unit)
        {
            _processQueueService = unit;
        }
        [HttpGet("~/api/ProcessQueue/GetTypeHierarchy")]
        public IActionResult GetTypeHierarchy()
        {
            var items = _processQueueService.GetTypeHierarchy();
            if (items != null)
            {
                return Ok(items);
            }
            else
                return null;
        }
        // GET: api/values
        [HttpPost("~/api/getallProcessQueues")]
        public IActionResult Get([FromBody] PaginationArgs args)
        {
            var items = _processQueueService.GetAll(args ?? new PaginationArgs());
            if (items != null)
            {
                return Ok(items);
            }
            else
                return NotFound();
        }
        /// <summary>
        /// get single process queue by props
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("~/api/getSingleProcessQueue")]
        public IActionResult GetSingle([FromBody]GetSingleArgs args)
        {
            var item = _processQueueService.GetSingleByProp(args ?? new GetSingleArgs());
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
            var item = _processQueueService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/values
        [HttpPost]
        public int Post([FromBody]ProcessQueueEntity value)
        {
            return _processQueueService.Create(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody]ProcessQueueEntity value)
        {
            if (id > 0)
            {
                return _processQueueService.Update(id, value);
            }
            return false;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return _processQueueService.Delete(id);
        }
    }
}
