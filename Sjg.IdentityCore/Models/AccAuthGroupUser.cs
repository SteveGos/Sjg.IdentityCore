using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sjg.IdentityCore.Models
{
    public partial class AccAuthGroupUser
    {
        public Guid AccAuthGroupId { get; set; }

        [ForeignKey("AccAuthGroupId")]
        public virtual AccAuthGroup AccAuthGroup { get; set; }

        public Guid AccAuthUserId { get; set; }

        [ForeignKey("AccAuthUserId")]
        public virtual AccAuthUser AccAuthUser { get; set; }
    }
}