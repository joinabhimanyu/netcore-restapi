﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRS.Data;
using DRS.Data.BusinessEntities;
using DRS.Data.ServiceInterfaces;
using DRS.Data.Services;
using DRS.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DRS.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class DocumentFieldParameterController : Controller
    {
        private IGenericService<DocumentFieldParameterEntity> _service;

        public DocumentFieldParameterController(IGenericService<DocumentFieldParameterEntity> unit)
        {
            _service = unit;
        }
        [HttpGet("~/api/DocumentFieldParameter/GetTypeHierarchy")]
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
        [HttpPost("~/api/getallDocumentFieldParameters")]
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
        /// get single document field parameter by props
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("~/api/getSingleDocumentFieldParameter")]
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
        public int Post([FromBody]DocumentFieldParameterEntity value)
        {
            return _service.Create(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody]DocumentFieldParameterEntity value)
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
