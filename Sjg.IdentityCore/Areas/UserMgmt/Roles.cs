using Sjg.IdentityCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sjg.IdentityCore.Areas.UserMgmt
{
    public class Roles
    {
        public const string Category = "Identity";

        public const string UserAdministrator = "Identity - User Administrator";

        public static AccAuthRole AccessAdministratorRole
        {
            get
            {
                return new AccAuthRole
                {
                    Category = Category,
                    Name = UserAdministrator,
                    Description = "Administrate Access Authorization.",
                };
            }
        }

        public static async Task SetUpRolesAsync(IServiceProvider services)
        {
            var accessRoles = new List<AccAuthRole>
            {
                AccessAdministratorRole,
            };

            await AccAuthAppRoles.SetUpAsync(services, accessRoles);
        }
    }
}