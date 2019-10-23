using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Sjg.IdentityCore;
using Sjg.IdentityCore.Models;
using System;
using System.Threading.Tasks;

namespace SampleWebApp_1_x_x.Api
{
    //public class ApiBasicAuthorization : AuthorizeAttribute, IAsyncAuthorizationFilter
    public class ApiBasicAuthorization : IAsyncAuthorizationFilter
    {
        private readonly SignInManager<AccAuthUser> _signInManager;

        private readonly IAccAuthConfiguration _accAuthConfiguration;

        public ApiBasicAuthorization(
            SignInManager<AccAuthUser> signInManager,
            IAccAuthConfiguration accAuthConfiguration)
        {
            _signInManager = signInManager;
            _accAuthConfiguration = accAuthConfiguration;
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}