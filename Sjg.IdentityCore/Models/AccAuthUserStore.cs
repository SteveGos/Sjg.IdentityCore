using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace Sjg.IdentityCore.Models
{
    public partial class AccAuthUserStore : UserStore<
        AccAuthUser,
        AccAuthRole,
        AccAuthContext,
        Guid
        //// SJG - NOT NEEDED ??
        ////IdentityUserClaim<Guid>,
        ////IdentityUserRole<Guid>,
        ////IdentityUserLogin<Guid>,
        ////IdentityUserToken<Guid>,
        ////IdentityRoleClaim<Guid>
        >
    {
        public AccAuthUserStore(AccAuthContext context)
            : base(context)
        {
        }
    }
}