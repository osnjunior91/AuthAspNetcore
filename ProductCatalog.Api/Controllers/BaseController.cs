using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductCatalog.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult GetErros()
        {
            var errors = ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).ToList();
            var result = new
            {
                Message = "Json Invalido",
                Errors = errors
            };
            return new BadRequestObjectResult(result);
        }
    }
}