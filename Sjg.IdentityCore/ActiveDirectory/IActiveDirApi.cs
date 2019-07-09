namespace Sjg.IdentityCore.ActiveDirectory
{
    public interface IActiveDirApi
    {
        /// <summary>
        /// Validate against Environment.UserDomainName
        /// </summary>
        /// <param name="ldapDomain">LDAP Domain to Validate against.</param>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <returns>True if validated; otherwise false</returns>
        bool ValidateCredentials(
            string ldapDomain,
            string userName,
            string password);

        /// <summary>
        /// Using Email - Get User Name
        /// </summary>
        /// <param name="ldapDomain">LDAP Domain to Validate against.</param>
        /// <param name="email">email</param>
        /// <returns></returns>
        string GetLoginNameFromEmail(
           string ldapDomain,
           string ldapDomainUsername,
           string ldapDomainPassword,
           string email);
    }
}