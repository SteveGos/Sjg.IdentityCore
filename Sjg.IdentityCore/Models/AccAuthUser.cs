using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// https://www.learnentityframeworkcore.com/configuration/data-annotation-attributes
// https://docs.microsoft.com/en-us/ef/core/modeling/relational/indexes

namespace Sjg.IdentityCore.Models
{
    // Add profile data for application users by adding properties to the AccAuthUser class
    //public class AccAuthUser : IdentityUser<Guid, AccAuthUserLogin, AccAuthUserRole, AccAuthUserClaim>

    // [PersonalData]
    // [ProtectedPersonalData]

    [ModelMetadataType(typeof(AccAuthUserMetaData))]
    public partial class AccAuthUser : IdentityUser<Guid>
    {
        // Custom Identity User Data

        /// <summary>
        /// User's Last Name
        /// </summary>
        //[Index]
        [StringLength(75, ErrorMessage = "{0} must not exceed {1} characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        /// User's First Name
        /// </summary>
        //[Index]
        [StringLength(75, ErrorMessage = "{0} must not exceed {1} characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Service Acct")]
        public bool IsServiceAccount { get; set; }

        [Display(Name = "Internal Service Acct")]
        public bool IsInternalServiceAccount { get; set; }

        [Display(Name = "Frozen")]
        public bool IsFrozen { get; set; }

        [Display(Name = "Password Never Expires")]
        public bool PasswordNeverExpires { get; set; }

        [Display(Name = "Last Login (UTC)")]
        public DateTime? LastLoginDateTimeUtc { get; set; }

        [Display(Name = "Last Password Change (UTC)")]
        public DateTime? LastPasswordChangeDateTimeUtc { get; set; }

        [Display(Name = "Windows/AD User")]
        public bool IsActiveDirectoryUser { get; set; }

        [StringLength(256, ErrorMessage = "Email Domain must not exceed {1} characters.")]
        [Display(Name = "Email Domain")]
        public string EmailDomainName { get; set; }

        [Display(Name = "Last Email Confirmed (UTC)")]
        public DateTime? LastEmailConfirmedUtc { get; set; }

        public virtual ICollection<AccAuthGroupUser> AccAuthUserGroups { get; set; }
    }

    public class AccAuthUserMetaData
    {
        [Display(Name = "Lockout End")]
        public virtual DateTimeOffset? LockoutEnd { get; set; }

        [Display(Name = "Two Factor Enabled")]
        public virtual bool TwoFactorEnabled { get; set; }

        [Display(Name = "Phone Number Confirmed")]
        public virtual bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "Phone Number")]
        public virtual string PhoneNumber { get; set; }

        [Display(Name = "Concurrency Stamp")]
        public virtual string ConcurrencyStamp { get; set; }

        [Display(Name = "Security Stamp")]
        public virtual string SecurityStamp { get; set; }

        [Display(Name = "Password Hash")]
        public virtual string PasswordHash { get; set; }

        [Display(Name = "Email Confirmed")]
        public virtual bool EmailConfirmed { get; set; }

        [Display(Name = "Normalized Email")]
        public virtual string NormalizedEmail { get; set; }

        [Display(Name = "Email")]
        public virtual string Email { get; set; }

        [Display(Name = "Normalized User Name")]
        public virtual string NormalizedUserName { get; set; }

        [Display(Name = "User Name")]
        public virtual string UserName { get; set; }

        [Display(Name = "Lockout Enabled")]
        public virtual bool LockoutEnabled { get; set; }

        [Display(Name = "Access Failed Count")]
        public virtual int AccessFailedCount { get; set; }
    }
}