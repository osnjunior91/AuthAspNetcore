using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Api.Data.Repository;
using ProductCatalog.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalog.Api.Controllers
{
    public class CategoryController : BaseController
    {
        //Implementar restrições de acesso
        //Refatorar Contorller para implementação do Repository Pattern
        //Implementar validações, se necessário
        //Padronizar retorno dos Enpoints

        private IRepository<Category> _repository;

        public CategoryController(IRepository<Category> repository)
        {
            _repository = repository;
        }

        [Route("v1/categories")]
        [HttpGet]
        [Authorize("Find")]
        public IActionResult Get()
        {
            
            try
            {
                return Ok(_repository.FindAll());
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Route("v1/categories/{id}")]
        [HttpGet]
        [Authorize("Find")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var result =  _repository.FindById(id.ToString());
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }

        [Route("v1/categories")]
        [HttpPost]
        [Authorize("Add")]
        public IActionResult Post([FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _repository.Create(category);
                    return CreatedAtAction(nameof(GetById),
                    new { id = result.Id }, result);
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }
            }

            return GetErros();
           
        }
    }
}