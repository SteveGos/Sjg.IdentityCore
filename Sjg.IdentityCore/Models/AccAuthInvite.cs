using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sjg.IdentityCore.Models
{
    /// <summary>
    /// Invitation to join application.
    /// </summary>
    public partial class AccAuthInvite  // : IValidatableObject
    {
        [Key]
        public Guid AccAuthInviteId { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        //[Index(IsUnique = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required.")]
        [StringLength(256, ErrorMessage = "The {0} must not exceed {1} characters in length.")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Expiration Data
        /// </summary>
        //[Index]
        [Display(Name = "Expiration")]
        [DataType(DataType.DateTime)]
        public DateTime ExpirationDateUtc { get; set; }

        /// <summary>
        /// Display Name - Example: John Smith
        /// </summary>
        //[Index]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required.")]
        [StringLength(100, ErrorMessage = "The {0} must not exceed {1} characters in length.")]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Confirmation Code - Needed with Email to accept invitation.
        /// </summary>
        //[Index]
        [StringLength(128)]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Service Acct")]
        public bool IsServiceAccount { get; set; }

        public virtual ICollection<AccAuthInviteRole> AccAuthInviteRoles { get; set; }
        ////////public virtual ICollection<AccAuthGroup> AccAuthGroups { get; set; }

        //public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    // Duplicate Logic not needed as an Invite will always be added.
        //    // - They will be deleted when expired or the user registers.

        //    //if (validationContext.Items.ContainsKey("Context"))
        //    //{
        //    //    var context = (AccessDbContext)validationContext.Items["Context"];

        //    //    var entityState = context.Entry(this).State;

        //    //    //if (entityState == EntityState.Deleted)
        //    //    //{
        //    //    //}

        //    //    if (entityState == EntityState.Added || entityState == EntityState.Modified)
        //    //    {
        //    //        //Duplicate Logic
        //    //        if (context.AccAuthInvites.Any(o => o.Email == Email && o.AccAuthInviteId != AccAuthInviteId))
        //    //        {
        //    //                yield return new ValidationResult("Invite already exists.");
        //    //        }
        //    //    }
        //    //}
        //}
    }
}