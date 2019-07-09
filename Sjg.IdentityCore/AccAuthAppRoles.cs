using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Sjg.IdentityCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sjg.IdentityCore
{
    public class AccAuthAppRoles
    {
        public static async Task SetUpAsync(IServiceProvider services, List<AccAuthRole> AccessRoles)
        {
            try
            {
                using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    //var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AccAuthUser>>();
                    //Obtain reference to RoleManager

                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AccAuthRole>>();

                    foreach (var item in AccessRoles)
                    {
                        if (!roleManager.RoleExistsAsync(item.Name).Result)
                        {
                            var identityResult = await roleManager.CreateAsync(new AccAuthRole
                            {
                                Name = item.Name,
                                Category = item.Category,
                                Description = item.Description,
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                // TODO: Add Logging
            }
        }

        public static async Task SetUpAsync(RoleManager<AccAuthRole> roleManager, List<AccAuthRole> AccessRoles)
        {
            try
            {
                foreach (var item in AccessRoles)
                {
                    if (!roleManager.RoleExistsAsync(item.Name).Result)
                    {
                        var identityResult = await roleManager.CreateAsync(new AccAuthRole
                        {
                            Name = item.Name,
                            Category = item.Category,
                            Description = item.Description,
                        });
                    }
                }
            }
            catch (Exception)
            {
                // TODO: Add Logging
            }
        }
    }
}