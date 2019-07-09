using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sjg.IdentityCore.Models
{
    public partial class AccAuthInviteRole
    {
        public Guid AccAuthInviteId { get; set; }

        [ForeignKey("AccAuthInviteId")]
        public virtual AccAuthInvite AccAuthInvite { get; set; }

        public Guid AccessRoleId { get; set; }

        [ForeignKey("AccessRoleId")]
        public virtual AccAuthRole AccessRole { get; set; }
    }
}