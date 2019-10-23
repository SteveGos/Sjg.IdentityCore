using LdapActiveDirectoryHelper;
using Sjg.IdentityCore.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleWebApp_1_x_x.Providers
{
    public class LdapCustomUserAuth : ICustomUserAuthenticatorProvider
    {
        private readonly ILdapCustomUserAuthConfig _config;

        public string ProviderName()
        {
            return "LDAP/AD";
        }

        /// <summary>
        /// Validate Credentials.
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="password">password</param>
        /// <returns>True if validated; otherwise false</returns>
        public bool ValidateCredentialsWithEmail(string email, string password)
        {
            if (IsUser(email))
            {
                return LdapActiveDirectoryHelper.Api.ValidateCredentialsWithEmail(_config.LdapDomain, _config.LdapUsername, _config.LdapPassword, email, password);
            }

            return false;
        }

        /// <summary>
        /// Implement Is User.... Based on Email Domain in LdapCustomUserAuthConfig
        /// </summary>
        /// <param name="email">User email to check to see if user.</param>
        /// <returns>True if the email is that of a user in <see cref="ICustomUserAuthenticatorProvider"/></returns>
        public bool IsUser(string email)
        {
            return email.EndsWith(_config.LdapEmailDomain, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Message to return if validation fails.
        /// </summary>
        /// <returns>A message to use if validation fails.</returns>
        public string VailidationFailedMessage()
        {
            return "Incorrect user ID or password. Type the correct user ID and password and try again. (LDAP-AD)";
        }

        /// <summary>
        /// Determines if search implemented.
        /// If true - the provider has user search capabilities.  Otherwise not used by <see cref="Sjg.IdentityCore"/>.
        /// </summary>
        /// <returns>If true - the provider has user search capabilities.  Otherwise not used by <see cref="Sjg.IdentityCore"/>.</returns>
        public bool AllowSearch()
        {
            return true;
        }

        /// <summary>
        /// Search Method. If <see cref="AllowSearch"/> is true, then used by <see cref="Sjg.IdentityCore"/> for
        /// searching for users in user management.  Returns List of Paged <see cref="CustomUserAuthenticatorUserInfo"/> records sorted by Email.
        /// </summary>
        /// <param name="searchFor">Search for criteria.  If blank, no data returned.</param>
        /// <param name="pageNumer">Page of records to return. Page Number Starts at One (1)</param>
        /// <param name="pageSize">Number of records to return.  Used for paging.</param>
        /// <param name="userCount">Number users matching search criteria.</param>
        /// <returns>List of <see cref="CustomUserAuthenticatorUserInfo"/> records.</returns>
        public List<CustomUserAuthenticatorUserInfo> Search(string searchFor, int pageNumer, int pageSize, out int userCount)
        {
            var list = LdapActiveDirectoryHelper.Api.SearchOrderByEmail(_config.LdapDomain, _config.LdapUsername, _config.LdapPassword, searchFor, pageNumer, pageSize, out int recCnt);
            userCount = recCnt;

            if (list == null || !list.Any())
            {
                return new List<CustomUserAuthenticatorUserInfo>(); ;
            }

            return list.Select(o =>
                new CustomUserAuthenticatorUserInfo
                {
                    Email = o.EmailAddress,
                    DisplayName = o.DisplayName,
                    Data1 = o.TelephoneNumber,
                    Data2 = o.Title,
                    Data3 = (o.IsAccountDisabled.HasValue) ? (o.IsAccountDisabled.Value ? "Disabled" : "Active") : "Unknown"
                }).ToList();
        }

        private string SetData4(ActDirUser user)
        {
            if (user.IsAccountDisabled.HasValue && user.IsAccountDisabled.Value)
            {
                return "Disabled";
            }

            return string.Empty;
        }

        /// <summary>
        /// Label for <see cref="CustomUserAuthenticatorUserInfo.Data1"/>.  If Null or empty, Data1 not used.
        /// </summary>
        /// <returns>Label of Data1. If Null or empty, Data1 not used. </returns>
        public string Data1Label()
        {
            return "Phone";
        }

        /// <summary>
        /// Label for <see cref="CustomUserAuthenticatorUserInfo.Data2"/>.  If Null or empty, Data2 not used.
        /// </summary>
        /// <returns>Label of Data2. If Null or empty, Data2 not used. </returns>
        public string Data2Label()
        {
            return "Title";
        }

        /// <summary>
        /// Label for <see cref="CustomUserAuthenticatorUserInfo.Data3"/>.  If Null or empty, Data3 not used.
        /// </summary>
        /// <returns>Label of Data3. If Null or empty, Data3 not used. </returns>
        public string Data3Label()
        {
            return "Status";
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">Configuration</param>
        public LdapCustomUserAuth(ILdapCustomUserAuthConfig config)
        {
            _config = config;
        }
    }
}