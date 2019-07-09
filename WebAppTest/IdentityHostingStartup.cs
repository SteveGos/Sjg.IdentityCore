using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sjg.IdentityCore;
using Sjg.IdentityCore.ActiveDirectory;
using Sjg.IdentityCore.Models;
using Sjg.IdentityCore.Services;
using Sjg.IdentityCore.Utilities;

// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/platform-specific-configuration?view=aspnetcore-2.1&tabs=windows
[assembly: HostingStartup(typeof(WebAppTest.IdentityHostingStartup))]

namespace WebAppTest
{
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
                // Sjg.IdentityCore - Integration Start
                var accAuthConfig = context.Configuration.GetSection("AccAuthConfiguration").Get<AccAuthConfiguration>();

                services.AddSingleton<IAccAuthConfiguration>(accAuthConfig);

                services.AddDbContext<AccAuthContext>(options =>
                       options.UseSqlServer(context.Configuration.GetConnectionString("Identity"),
                        x =>
                        {
                            // -----------------------------------------------------------
                            // The MigrationsAssembly must have your Assembly Name..... In this Case "WebAppTest"
                            // -----------------------------------------------------------
                            x.MigrationsAssembly("WebAppTest");
                            x.MigrationsHistoryTable("__IdentityMigrationsHistory", AccAuthContext.DefaultSchema);
                        })
                   );

                services.AddIdentity<AccAuthUser, AccAuthRole>(options =>
                {
                    options.Password = accAuthConfig.PasswordOptions;
                    options.Lockout = accAuthConfig.LockoutOptions;
                    options.SignIn = accAuthConfig.SignInOptions;
                    options.User = accAuthConfig.UserOptions;
                })

                    .AddEntityFrameworkStores<AccAuthContext>()
                    .AddDefaultTokenProviders()
                    // .AddDefaultUI() // SJG ????

                    .AddUserManager<UserManager<AccAuthUser>>()
                    .AddRoleManager<RoleManager<AccAuthRole>>()

                    //.AddUserStore<UserStore<AccAuthUser, AccAuthRole, AccAuthContext, Guid>>()
                    .AddUserStore<AccAuthUserStore>()

                    //.AddRoleStore<RoleStore<AccAuthRole, AccAuthContext, Guid>>()
                    .AddRoleStore<AccAuthRoleStore>();

                services.AddSingleton<IAccAuthEmailSender, AccAuthEmailSender>();

                services.AddScoped<IAccAuthViewRenderService, AccAuthViewRenderService>();

                services.AddScoped<IActiveDirApi, ActDirValidationApi>();

                //// Must Be Before AddMvc
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Areas.UserMgmt.Roles.UserAdministrator",
                        policy => policy.RequireRole(Sjg.IdentityCore.Areas.UserMgmt.Roles.UserAdministrator));
                });

                services.AddMvc().AddRazorPagesOptions(options =>
                {
                    // Area -- Identity - Set Aurthorization.
                    //options.Conventions.AllowAnonymousToAreaFolder("Identity", "/");
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");

                    // Area -- UserMgmt => based on Policy
                    options.Conventions.AuthorizeAreaFolder("UserMgmt", "/", "Areas.UserMgmt.Roles.UserAdministrator"); // All of Area
                });

                // Sjg.IdentityCore - Integration Stop

            });
        }
    }
}