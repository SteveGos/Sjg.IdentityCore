using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sjg.IdentityCore;

namespace WebAppTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Sjg.IdentityCore 
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;

                // If the LoginPath isn't set, ASP.NET Core defaults
                // the path to /Account/Login.
                //options.LoginPath = "/Account/Login";
                options.LoginPath = "/Identity/Account/Login";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults
                // the path to /Account/AccessDenied.
                //options.AccessDeniedPath = "/Account/AccessDenied";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";

                // OBSOLETE options.Cookie.HttpOnly = true;
            });

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true; // support areas in a Razor Pages application
                    options.Conventions.AuthorizeFolder("/"); // Authorize All Non-Area Razor Pages and Folders => /Pages
                    options.Conventions.AllowAnonymousToFolder("/Terms"); // Allow Anonymous on terms
                });

            // IHttpContextAccessor is no longer wired up by default, you have to register it yourself
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // Sjg.IdentityCore - Razor Class Libraries Extensions (Static Files, etc...)
            services.AddIdentityCoreExtensions(); // Adding Static Resources for Sjg.IdentityCore

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // -----------------------
            // Security Stuff - Start
            // -----------------------

            // https://www.codeproject.com/Articles/1259066/10-Points-to-Secure-Your-ASP-NET-Core-MVC-Applic-2

            //// NWebSec - Registered before static files to always set header
            ////   https://docs.nwebsec.com/en/latest/
            ////   https://docs.nwebsec.com/en/latest/nwebsec/Configuring-csp.html
            ////
            ////   https://damienbod.com/2018/03/14/securing-the-cdn-links-in-the-asp-net-core-2-1-templates/
            ////   https://books.google.com/books?id=_95YDwAAQBAJ&pg=PA192&lpg=PA192&dq=NWebsec+ScriptSources+CustomSources&source=bl&ots=Mb0rxfllE4&sig=HV3d7UHLvBhQMFGW3CsCapNeNAQ&hl=en&sa=X&ved=2ahUKEwiGuYCsrrTeAhWpHTQIHRqBDlEQ6AEwBXoECAcQAQ#v=onepage&q=NWebsec%20ScriptSources%20CustomSources&f=false

            ////   https://development.robinwinslow.uk/2013/06/20/loading-fonts-as-data-urls/

            app.UseHsts(hsts => hsts.MaxAge(365).IncludeSubdomains());
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXfo(options => options.Deny());

            app.UseCsp(opts => opts
                //.BlockAllMixedContent()

                .StyleSources(s => s.Self().CustomSources("cdnjs.cloudflare.com", "stackpath.bootstrapcdn.com", "use.fontawesome.com"))
                .StyleSources(s => s.UnsafeInline())

                .FontSources(s => s.Self().CustomSources("use.fontawesome.com", "data:"))

                .FormActions(s => s.Self())
                .FrameAncestors(s => s.Self())
                //.ImageSources(s => s.Self())
                .ImageSources(s => s.Self().CustomSources("data:"))
                //.ImageSources(s => s.Self())  // SJG - Research - Commented out - embedded --> bootstrap.css --> data:image/svg+xml;charset=utf8,%3Csvg xmlns='http://www.w3.org/2000/svg'

                .ScriptSources(s => s.Self().CustomSources("cdnjs.cloudflare.com", "stackpath.bootstrapcdn.com"))
                .ScriptSources(s => s.UnsafeInline()) //SJG - Allows OnClick=

                );

            //////app.UseCsp(options => options
            //////    .DefaultSources(s => s.Self())
            //////    .ConnectSources(s => s.Self())
            //////    .StyleSources(s => s.Self().CustomSources("ajax.aspnetcdn.com"))
            //////    .ScriptSources(s => s.Self().CustomSources("localhost", "ajax.aspnetcdn.com", "ajax.googleapis.com"))
            //////);

            // -----------------------
            // Security Stuff - Stop
            // -----------------------

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Sjg.IdentityCore 
            app.UseAuthentication();


            app.UseMvc();


            // --------------------------------------------------------
            // Application's Initialization Processing....
            // --------------------------------------------------------

            // Sjg.IdentityCore - Establish Access/Authorization Roles for User Managament
            Task.Run(() => Sjg.IdentityCore.Areas.UserMgmt.Roles.SetUpRolesAsync(app.ApplicationServices));  // Process Asynchronously - no need to wait.
        }
    }
}
