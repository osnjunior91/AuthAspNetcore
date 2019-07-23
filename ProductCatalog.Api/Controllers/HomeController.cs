using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ProductCatalog.Api.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {        
        [HttpGet]
        [Route("")]
        public object Get()
        {
            return new { message = "Api is running..."};
        }
    }
}