using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace LdapActiveDirectoryHelper
{
    public class Api
    {
        /// <summary>
        /// Validate Credentials.  Returns True if Valid.
        /// </summary>
        /// <param name="ldapDomain">LDAP Domain</param>
        /// <param name="ldapDomainServiceAccount">LDAP Service Account - leave null or empty id not needed</param>
        /// <param name="ldapDomainServiceAccountPassword">LDAP Service Account Password - leave null or empty id not needed.</param>
        /// <param name="userName">User Name</param>
        /// <param name="password">Password</param>
        /// <returns> Returns True if Valid.</returns>
        /// <returns></returns>
        public static bool ValidateCredentialsWithEmail(
            string ldapDomain,
            string ldapDomainServiceAccount,
            string ldapDomainServiceAccountPassword,
            string email,
            string password)
        {
            //////// CAN BE VERY SLOW (25 - 35 seconds)
            //////// set up domain context
            //////using (var principalContext = new PrincipalContext(ContextType.Domain, domain))
            //////{
            //////    return principalContext.ValidateCredentials(userName, password);
            //////}

            // Faster than principalContext.ValidateCredentials(userName, password)

            try
            {
                var userName = GetLoginNameFromEmail(
                        ldapDomain,
                        ldapDomainServiceAccount,
                        ldapDomainServiceAccountPassword,
                        email);

                using (var de = new DirectoryEntry("LDAP://" + ldapDomain, userName, password))
                {
                    using (var ds = new DirectorySearcher(de))
                    {
                        ds.FindOne();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                var sjg = ex.GetBaseException().Message;

                return false;
            }
        }

        /// <summary>
        /// Returns a list of AD Users matching email criteria.
        /// </summary>
        /// <param name="ldapDomain">LDAP Domain</param>
        /// <param name="ldapDomainServiceAccount">LDAP Service Account - leave null or empty id not needed</param>
        /// <param name="ldapDomainServiceAccountPassword">LDAP Service Account Password - leave null or empty id not needed.</param>
        /// <param name="searchFor">Search For Criteria</param>
        /// <param name="pageNumer">Page of records to return. Page Number Starts at One (1)</param>
        /// <param name="pageSize">Number of records to return.  Used for paging.</param>
        /// <returns>
        /// An AdUser record for the given email.  If no Active Directory record is found, NULL is returned
        /// </returns>
        public static List<ActDirUser> SearchOrderByEmail(
            string ldapDomain,
            string ldapDomainServiceAccount,
            string ldapDomainServiceAccountPassword,
            string searchFor,
            int pageNumer,
            int pageSize,
            out int userCount)
        {
            userCount = 0;

            if (string.IsNullOrWhiteSpace(searchFor) ||
                pageSize <= 0 || pageNumer <= 0)
            {
                return new List<ActDirUser>();
            }

            if (string.IsNullOrWhiteSpace(ldapDomain))
            {
                throw new ArgumentNullException("ldapDomain", "Domain name is required.");
            }

            var searchString = searchFor.Replace("*", "");
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return new List<ActDirUser>();
            }

            searchString = searchString.Trim();

            PrincipalContext principalContext;
            if (string.IsNullOrWhiteSpace(ldapDomainServiceAccount))
            {
                principalContext = new PrincipalContext(ContextType.Domain, ldapDomain);
            }
            else
            {
                principalContext = new PrincipalContext(ContextType.Domain, ldapDomain, ldapDomainServiceAccount, ldapDomainServiceAccountPassword);
            }

            var actDirUsers = searchString.Contains("*") ?
              RetrieveData(principalContext, searchString, true) :
              RetrieveData(principalContext, searchString + "*", true);

            if (actDirUsers == null || !actDirUsers.Any())
            {
                return new List<ActDirUser>();
            }

            userCount = actDirUsers.Count();

            return actDirUsers.OrderBy(o => o.EmailAddress).Skip(pageSize * (pageNumer - 1)).Take(pageSize).ToList();
        }

        private static List<ActDirUser> RetrieveData(PrincipalContext principalContext, string searchString, bool excludeGenericAndServiceAccounts)
        {
            //***********************
            // Search By Account Name
            //***********************
            var userPrincipal = new UserPrincipal(principalContext) { SamAccountName = searchString };

            // create a principal searcher for running a search operation
            var pS = new PrincipalSearcher(userPrincipal);

            // run the query
            var collAcctName = pS.FindAll();

            pS.Dispose();

            //***********************
            // Search By Email Name
            //***********************
            userPrincipal = new UserPrincipal(principalContext) { EmailAddress = searchString };

            // create a principal searcher for running a search operation
            pS = new PrincipalSearcher(userPrincipal);

            // run the query
            var collEmail = pS.FindAll();

            pS.Dispose();

            //***********************
            // Search By SurName
            //***********************
            userPrincipal = new UserPrincipal(principalContext) { Surname = searchString };

            // create a principal searcher for running a search operation
            pS = new PrincipalSearcher(userPrincipal);

            // run the query
            var collSurname = pS.FindAll();

            pS.Dispose();

            //***********************
            // Search By GivenName
            //***********************
            userPrincipal = new UserPrincipal(principalContext) { GivenName = searchString };

            // create a principal searcher for running a search operation
            pS = new PrincipalSearcher(userPrincipal);

            // run the query
            var collGivenName = pS.FindAll();

            pS.Dispose();

            //***********************
            // Search By DisplayName
            //***********************
            userPrincipal = new UserPrincipal(principalContext) { DisplayName = searchString };

            // create a principal searcher for running a search operation
            pS = new PrincipalSearcher(userPrincipal);

            // run the query
            var collDisplayName = pS.FindAll();

            pS.Dispose();

            //***********************
            // Concatenate and Remove Duplicates
            // Remove Excluded OUs
            //***********************

            // OUs to Exclude
            // OU=Generic User Accounts
            // OU=Service Accounts

            const string excludeOu1 = "OU=Generic User Accounts";
            const string excludeOu2 = "OU=Service Accounts";

            //var hasWildCard = (searchString.IndexOf("*", StringComparison.Ordinal) >= 0);

            List<Principal> coll;

            if (excludeGenericAndServiceAccounts)
            {
                coll = collAcctName
                    .Concat(collEmail)
                    .Concat(collSurname)
                    .Concat(collGivenName)
                    .Concat(collDisplayName)
                    //.Where(o => (HasEmail(o.GetUnderlyingObject() as DirectoryEntry)))
                    .Where(o => (o.DistinguishedName.IndexOf(excludeOu1, StringComparison.Ordinal) < 0 &&
                                 o.DistinguishedName.IndexOf(excludeOu2, StringComparison.Ordinal) < 0))
                    .Distinct(new DistinctPrincipalComparer()).ToList();
            }
            else
            {
                coll = collAcctName
                    .Concat(collEmail)
                    .Concat(collSurname)
                    .Concat(collGivenName)
                    .Concat(collDisplayName)
                    .Distinct(new DistinctPrincipalComparer()).ToList();
            }

            var adUsers = (from obj in coll
                           where obj.GetUnderlyingObject().GetType() == typeof(DirectoryEntry)
                           select Hydrate.HydrateAdUser(obj.GetUnderlyingObject() as DirectoryEntry)
                ).Where(o => !string.IsNullOrWhiteSpace(o.EmailAddress))
                .ToList();

            return adUsers
                .Where(o => !o.IsNonUserAccount && o.IsNormalAccount.HasValue && o.IsNormalAccount.Value && o.IsAccountDisabled.HasValue && !o.IsAccountDisabled.Value)
                .ToList();
        }

        /// <summary>
        /// Get Login Name from Email.  Returns null or whitespace if not found or more than one exists.
        /// </summary>
        /// <param name="ldapDomain">LDAP Domain</param>
        /// <param name="ldapDomainServiceAccount">LDAP Service Account - leave null or empty id not needed</param>
        /// <param name="ldapDomainServiceAccountPassword">LDAP Service Account Password - leave null or empty id not needed.</param>
        /// <param name="email">Email to get user name for</param>
        /// <returns></returns>
        private static string GetLoginNameFromEmail(
            string ldapDomain,
            string ldapDomainServiceAccount,
            string ldapDomainServiceAccountPassword,
            string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            // set up domain context

            //***********************
            // Search By Email Name
            //***********************

            PrincipalContext principalcontext;

            if (string.IsNullOrWhiteSpace(ldapDomainServiceAccount) || string.IsNullOrWhiteSpace(ldapDomainServiceAccountPassword))
            {
                principalcontext = new PrincipalContext(ContextType.Domain, ldapDomain);
            }
            else
            {
                principalcontext = new PrincipalContext(ContextType.Domain, ldapDomain, ldapDomainServiceAccount, ldapDomainServiceAccountPassword);
            }

            var userPrincipal = new UserPrincipal(principalcontext) { EmailAddress = email };

            // create a principal searcher for running a search operation
            var pS = new PrincipalSearcher(userPrincipal);

            // Principal Searcher
            var collEmail = pS.FindAll();

            pS.Dispose();

            var actDirUsers = (from obj in collEmail
                               where obj.GetUnderlyingObject().GetType() == typeof(DirectoryEntry)
                               select Hydrate.HydrateAdUser(obj.GetUnderlyingObject() as DirectoryEntry))
                .ToList()
                .Where(o => !o.IsNonUserAccount && o.IsNormalAccount.HasValue && o.IsNormalAccount.Value && o.IsAccountDisabled.HasValue && !o.IsAccountDisabled.Value)
                .ToList();

            if (actDirUsers.Count <= 0)
            {
                //message = $"{userName} not defined to {applicationName} (AD0-1).";
                return string.Empty;
            }

            if (actDirUsers.Count > 1)
            {
                //message = $"{userName} not defined to {applicationName} (AD2).";
                return string.Empty;
            }

            return actDirUsers[0].LoginName;
        }
    }
}