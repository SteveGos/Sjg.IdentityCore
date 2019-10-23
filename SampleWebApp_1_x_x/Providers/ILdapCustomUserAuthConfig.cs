namespace SampleWebApp_1_x_x.Providers
{
    public interface ILdapCustomUserAuthConfig
    {
        /// <summary>
        /// LDAP Domain
        /// </summary>
        string LdapDomain { get; }

        /// <summary>
        /// This Email Will be associated to an LDAP (AD) Account
        /// </summary>
        string LdapEmailDomain { get; }

        /// <summary>
        /// LDAP (AD) Account User Name - If Required
        /// </summary>
        string LdapUsername { get; }

        /// <summary>
        /// LDAP (AD) Account Password - If Required
        /// </summary>
        string LdapPassword { get; }


    }
}