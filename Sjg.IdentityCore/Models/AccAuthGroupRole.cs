using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sjg.IdentityCore.Models
{
    public partial class AccAuthGroupRole
    {
        public Guid AccAuthGroupId { get; set; }

        [ForeignKey("AccAuthGroupId")]
        public virtual AccAuthGroup AccAuthGroup { get; set; }

        public Guid AccessRoleId { get; set; }

        [ForeignKey("AccessRoleId")]
        public virtual AccAuthRole AccessRole { get; set; }
    }
}