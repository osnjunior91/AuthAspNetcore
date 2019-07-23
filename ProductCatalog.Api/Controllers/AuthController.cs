using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Mock.User;
using ProductCatalog.Api.Security;
using ProductCatalog.Api.Security.Jwt;
using ProductCatalog.Api.ViewModels.Auth;

namespace ProductCatalog.Api.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        //Implementar validações, se necessário
        //Padronizar retorno dos Enpoints

        [Route("v1/auth/login")]
        [HttpPost]
        public object Login([FromServices] JwtConfiguration tokenConfigurations, [FromBody] LoginViewModel model)
        {

            if (!ModelState.IsValid)
                return GetErros();

            string token = string.Empty;

            var user = UserRepositoryMock.GetUser(model.Username, model.Password);

            if (user != null)
            {
                token = new JwtTokenBuilder()
                       .AddSecurityKey(JwtSecurityKey.Create(tokenConfigurations.JwtKey))
                       .AddSubject(user.Name)
                       .AddIssuer(tokenConfigurations.Issuer)
                       .AddAudience(tokenConfigurations.Audience)
                       .AddNameId(user.Username)
                       .AddExpiryDays(tokenConfigurations.Days)
                       //Adicionado um claim com os perfis de uso.
                       .AddClaimsPermission(user.Permissions)
                       .Build();

                return new { token };
            }
            else
            {
                return BadRequest(
                    new { message = "Usuário ou senha inválidos!" }
                    );
            }
        }
    }
}