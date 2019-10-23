namespace SampleWebApp_1_x_x.Providers
{
    public class LdapCustomUserAuthConfig : ILdapCustomUserAuthConfig
    {
        /// <summary>
        /// LDAP Domain
        /// </summary>
        public string LdapDomain { get; set; }

        /// <summary>
        /// This Email Will be associated to an LDAP (AD) Account
        /// </summary>
        public string LdapEmailDomain { get; set; }

        /// <summary>
        /// LDAP (AD) Account User Name - If Required
        /// </summary>
        public string LdapUsername { get; set; }

        /// <summary>
        /// LDAP (AD) Account Password - If Required
        /// </summary>
        public string LdapPassword { get; set; }
    }
}