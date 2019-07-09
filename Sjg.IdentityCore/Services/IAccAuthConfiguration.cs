using Sjg.IdentityCore.Utilities.Mail;
using System.ComponentModel.DataAnnotations;

namespace Sjg.IdentityCore.Services
{
    /// <summary>
    /// Access Authorization Configuration
    /// </summary>
    public interface IAccAuthConfiguration  // : IValidatableObject
    {
        /// <summary>
        /// AppFolderPath is used to resolve URL...
        /// <para/>
        /// https://testapps.itd.idaho.gov/apps/qasp/Identity/Account/Register
        /// <para/>
        /// AppFolderPath = /apps/qasp
        /// <para/>
        /// Used in Sjg.IdentityCore.Areas.UserMgmt.Pages.AccAuthInvites.CreateModel to resolve URL for Email... Needs to be determined proper way!
        /// <para/>
        /// </summary>
        [Display(Name = "AppFolderPath")]
        string AppFolderPath { get; }

        /// <summary>
        /// Site Title used in Access Authorization Navigation Bar
        /// </summary>
        [Display(Name = "Site Title")]
        string SiteTitle { get; }

        /// <summary>
        /// If true, then registrtion requires an invitation
        /// </summary>
        [Display(Name = "Invite Only")]
        bool InvitationOnly { get; }

        /// <summary>
        /// Number of Days until an Invitation Expires
        /// </summary>
        [Display(Name = "Invite Expires (Days)")]
        int InvitationExpirationDays { get; }

        //// Active Directory Options
        //public string LdapDomain { get; set; }
        //public string LdapEamil { get; set; };

        /// <summary>
        /// LDAP Domain
        /// </summary>
        [Display(Name = "LDAP (AD) Domain")]
        string LdapDomain { get; }

        ///////// <summary>
        ///////// Domain of User Name.. Example: XXX\BJones Where XXX is Domain of User Name
        ///////// </summary>
        //////[Display(Name = "LDAP (AD) Domain")]
        //////string LdapUserNameDomain { get; }

        /// <summary>
        /// This Email Will be associated to an LDAP (AD) Account
        /// </summary>
        [Display(Name = "LDAP (AD) Email")]
        string LdapEmail { get; }

        /// <summary>
        /// LDAP (AD) Account User Name - If Required
        /// </summary>
        [Display(Name = "LDAP (AD) User name")]
        string LdapUsername { get; }

        /// <summary>
        /// LDAP (AD) Account Password - If Required
        /// </summary>
        [Display(Name = "LDAP (AD) Password")]
        string LdapPassword { get; }

        /// <summary>
        /// If set, LDAP (AD) Authentication Allowed
        /// </summary>
        [Display(Name = "LDAP (AD) Allowed")]
        bool AllowLdap { get; }

        /// <summary>
        /// If set, all authentication done through LDAP (AD)
        /// </summary>
        [Display(Name = "Only LDAP (AD) Allowed")]
        bool OnlyLdap { get; }

        Microsoft.AspNetCore.Identity.UserOptions UserOptions { get; }
        Microsoft.AspNetCore.Identity.PasswordOptions PasswordOptions { get; }
        Microsoft.AspNetCore.Identity.LockoutOptions LockoutOptions { get; }
        Microsoft.AspNetCore.Identity.SignInOptions SignInOptions { get; }

        // Not Yey Implemented
        //Microsoft.AspNetCore.Identity.ClaimsIdentityOptions ClaimsIdentityOptions { get; }
        //Microsoft.AspNetCore.Identity.TokenOptions TokenOptions { get; }
        //Microsoft.AspNetCore.Identity.StoreOptions StoreOptions { get; }

        /// <summary>
        /// SMTP Mail Configuration - Properties for SmtpClient - If over
        /// </summary>
        AccAuthEmailConfiguration AccAuthEmailConfiguration { get; }
    }
}