using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Data.Repository;
using ProductCatalog.Api.Models;
using System;
using System.Collections.Generic;

namespace ProductCatalog.Api.Controllers
{
    public class ProductController : BaseController
    {
        //Implementar restrições de acesso
        //Implementar Repository Pattern
        //Implementar Validações, se necessário
        //Padronizar retorno dos Endpoints

        private IRepository<Product> _repository;

        public ProductController(IRepository<Product> repository)
        {
            _repository = repository;
        }

        [Route("v1/products")]
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

        [Route("v1/products/{id}")]
        [HttpGet]
        [Authorize("Find")]
        public IActionResult GetById(Guid id)
        {

            try
            {
                var result = _repository.FindById(id.ToString());
                if (result == null)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Route("v1/products")]
        [HttpPost]
        [Authorize("Add")]
        public IActionResult Post([FromBody] Product product)
        {

            if (!ModelState.IsValid)
                return GetErros();

            try
            {
                var result = _repository.Create(product);
                return CreatedAtAction(nameof(GetById),
                new { id = result.Id }, result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Route("v1/products/{id}")]
        [HttpDelete]
        [Authorize("Add")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var result = _repository.FindById(id.ToString());
                if (result == null)
                    return NotFound();
                _repository.Delete(id.ToString());
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}