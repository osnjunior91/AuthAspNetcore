using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Api.Security.Permission
{
    /// <summary>
    /// Classe responsavel pela verificação do Method
    /// </summary>
    public class PermissionByClaim : AuthorizationHandler<Method>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Method requirement)
        {
            //Buscando os dados das requests para comparação
            var mvcContext = context.Resource as AuthorizationFilterContext;
            var descriptor = mvcContext?.ActionDescriptor as ControllerActionDescriptor;
            ////Verifica por meio do claim se pessoa possui permissão
            if (context.User.HasClaim(c => c.Type.Equals(descriptor.ControllerName) && requirement.MethodType.Any(x => x.Equals(c.Value))))
            {
                //Retorna Ok
                context.Succeed(requirement);
            }
            //Retorna erro 403
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Classe que contem o parametro de qual tipo de verificação e implementa a interface IAuthorizationRequirement
    /// </summary>
    public class Method : IAuthorizationRequirement
    {
        public List<string> MethodType { get; set; }
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="MethodType">Operações a serem verificada</param>
        public Method(List<string> MethodType)
        {
            this.MethodType = MethodType;
        }
    }
}
