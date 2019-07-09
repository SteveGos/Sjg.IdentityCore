using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sjg.IdentityCore.Models
{
    //[ModelMetadataType(typeof(AccAuthGroupMetaData))]
    [Display(Name = "Access Group")]
    public partial class AccAuthGroup
    {
        /// <summary>
        /// PK
        /// </summary>
        [Key]
        public Guid AccAuthGroupId { get; set; }

        /// <summary>
        /// Group Description
        /// </summary>
        //[Index(IsUnique = true)]
        [Required(ErrorMessage = "Group is required.", AllowEmptyStrings = false)]
        [StringLength(256, ErrorMessage = "Group must not exceed {1} characters.")]
        [Display(Name = "Group")]
        public string Group { get; set; }

        /// <summary>
        /// Group Description
        /// </summary>
        //[Index]
        [Required(ErrorMessage = "Group Description is required.", AllowEmptyStrings = false)]
        [StringLength(256, ErrorMessage = "Group Description must not exceed {1} characters.")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Group Category.  User defined category of Access Group
        /// </summary>
        //[Index]
        [StringLength(256, ErrorMessage = "Group Category must not exceed {1} characters.")]
        [Display(Name = "Category")]
        public string Category { get; set; }

        public virtual ICollection<AccAuthGroupUser> AccAuthGroupUsers { get; set; }

        public virtual ICollection<AccAuthGroupRole> AccAuthGroupRoles { get; set; }
    }

    //public class AccAuthGroupMetaData
    //{
    //    [Display(Name = "Normalized Name")]
    //    public virtual string NormalizedName { get; set; }

    //    [Display(Name = "Concurrency Stamp")]
    //    public virtual string ConcurrencyStamp { get; set; }
    //}
}