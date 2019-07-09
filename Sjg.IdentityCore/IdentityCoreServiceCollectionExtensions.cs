using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Sjg.IdentityCore
{
    public static class IdentityCoreServiceCollectionExtensions
    {
        public static void AddIdentityCoreExtensions(this IServiceCollection services)
        {
            // Add Configuration (if needed)
            services.ConfigureOptions(typeof(IdentityCoreConfigureOptions));

            // Add Services for RCL (if needed)

            // Use services.Try* - for avoiding duplicate services...
            // Example:
            // Use services.TryAddSingleton -  checks for the existence of the service descriptor in the
            //                                 service collection and any only adds it if it's not already there

            // AccAuthGridPagerTagHelper - Services Needed -
            //    IHttpContextAccessor, IActionContextAccessor, and IUrlHelperFactory

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddSingleton<IUrlHelperFactory, UrlHelperFactory>();
        }
    }
}