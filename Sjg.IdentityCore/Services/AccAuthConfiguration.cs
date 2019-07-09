using Microsoft.AspNetCore.Identity;
using Sjg.IdentityCore.Utilities.Mail;

namespace Sjg.IdentityCore.Services
{
    public class AccAuthConfiguration : IAccAuthConfiguration
    {
        public const string DefaultSiteTitle = "DEFAULT - Access/Authorization";
        public const int DefaultInvitationExpirationDays = 7;
        public const bool DefaultInvitationOnly = true;

        public const string DefaultLdapDomain = "";
        public const string DefaultLdapEamil = "";
        public const string DefaultLdapUsername = "";
        public const string DefaultLdapPassword = "";
        public const bool DefaultAllowLdap = false;
        public const bool DefaultOnlyLdap = false;

        public const string DefaultAppFolderPath = "";
        public string AppFolderPath { get; set; } = DefaultAppFolderPath;


        public string SiteTitle { get; set; } = DefaultSiteTitle;
        public bool InvitationOnly { get; set; } = DefaultInvitationOnly;
        public int InvitationExpirationDays { get; set; } = DefaultInvitationExpirationDays;


        //// Active Directory Options
        public string LdapDomain { get; set; } = DefaultLdapDomain;

        public string LdapEmail { get; set; } = DefaultLdapEamil;
        public string LdapUsername { get; set; } = DefaultLdapUsername;
        public string LdapPassword { get; set; } = DefaultLdapPassword;
        public bool AllowLdap { get; set; } = DefaultAllowLdap;
        public bool OnlyLdap { get; set; } = DefaultOnlyLdap;

        public UserOptions UserOptions { get; set; }
        public PasswordOptions PasswordOptions { get; set; }
        public LockoutOptions LockoutOptions { get; set; }
        public SignInOptions SignInOptions { get; set; }

        public AccAuthEmailConfiguration AccAuthEmailConfiguration { get; set; }

        public AccAuthConfiguration()
        {
            UserOptions = new UserOptions();
            PasswordOptions = new PasswordOptions();
            LockoutOptions = new LockoutOptions();
            SignInOptions = new SignInOptions();
        }
    }
}