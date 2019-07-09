using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sjg.IdentityCore.Models
{
    [ModelMetadataType(typeof(AccessRoleMetaData))]
    [Display(Name = "Access Role")]
    public partial class AccAuthRole : IdentityRole<Guid>
    {
        public AccAuthRole()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Role Category.  User defined category of Access Roll
        /// </summary>
        [StringLength(256, ErrorMessage = "Role Category must not exceed {1} characters.")]
        //[Index]
        [Display(Name = "Category")]
        public string Category { get; set; }

        /// <summary>
        /// Role Description
        /// </summary>
        [StringLength(256, ErrorMessage = "Role Description must not exceed {1} characters.")]
        //[Index]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public virtual ICollection<AccAuthGroupRole> AccessRoleGroups { get; set; }
    }

    public class AccessRoleMetaData
    {
        [Display(Name = "Role")]
        public virtual string Name { get; set; }

        [Display(Name = "Normalized Name")]
        public virtual string NormalizedName { get; set; }

        [Display(Name = "Concurrency Stamp")]
        public virtual string ConcurrencyStamp { get; set; }
    }
}