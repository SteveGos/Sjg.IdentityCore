using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Sjg.IdentityCore;
using Sjg.IdentityCore.Providers;

// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/platform-specific-configuration?view=aspnetcore-2.2&tabs=windows

[assembly: HostingStartup(typeof(SampleWebApp_1_x_x.IdentityHostingStartup))]

namespace SampleWebApp_1_x_x
{
    /// <summary>
    /// Identity Hosting Startup for Sjg.IdentityCore
    /// </summary>
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            AccAuthConfigure(builder);
        }

        private static void AccAuthConfigure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                // To Use a Custom Authentication Provider - Add Class that implements ICustomUserAuthenticatorProvider 
                //services.AddScoped<ICustomUserAuthenticatorProvider, {yourCustomUserAuthenticatorProvider}>();

                // ----------------------------------
                // CUSTOM AUTHENTICATION PROVIDER
                // ----------------------------------

                //var ldapConfigType = typeof(Providers.LdapCustomUserAuthConfig);
                //var ldapConfig = context.Configuration.GetSection(ldapConfigType.FullName).Get<Providers.LdapCustomUserAuthConfig>();
                //services.TryAddSingleton<Providers.ILdapCustomUserAuthConfig>(ldapConfig);

                //services.TryAddScoped<ICustomUserAuthenticatorProvider, Providers.LdapCustomUserAuth>();

                // ----------------------------------
                // ----------------------------------

                // Sjg.IdentityCore - Set Up - Razor Class Libraries Extensions (Sets up Authentication, Static Files, etc...)
                services.AddIdentityCoreExtensions(context.Configuration); // Set Up Sjg.IdentityCore
            });
        }
    }
}