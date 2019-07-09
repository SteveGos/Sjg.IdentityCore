using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace Sjg.IdentityCore.Models
{
    public partial class AccAuthRoleStore : RoleStore<AccAuthRole, AccAuthContext, Guid, IdentityUserRole<Guid>, IdentityRoleClaim<Guid>>
    {
        public AccAuthRoleStore(AccAuthContext context)
            : base(context)
        {
        }
    }
}