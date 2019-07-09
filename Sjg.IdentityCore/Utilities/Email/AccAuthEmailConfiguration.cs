using System.Net.Mail;

namespace Sjg.IdentityCore.Utilities.Mail
{
    /// <summary>
    /// SMTP Mail Configuration - Properties for SmtpClient
    /// </summary>
    public class AccAuthEmailConfiguration : IAccAuthEmailConfiguration
    {
        /// <summary>
        /// SMTP Server - See: .Net SmptpClient
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        /// SMTP Port - See: .Net SmptpClient
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// SMTP From Email - See: .Net SmptpClient
        /// </summary>
        public string FromUserEmail { get; set; }

        /// <summary>
        /// SMTP From User Name - See: .Net SmptpClient
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// SMTP Time Out - See: .Net SmptpClient
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// SMTP Pickup directory location - See: .Net SmptpClient
        /// </summary>
        public string PickupDirectoryLocation { get; set; }

        /// <summary>
        /// SMTP Enable SSL - See: .Net SmptpClient
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// SMTP Delivery Method - See: .Net SmptpClient
        /// </summary>
        public SmtpDeliveryMethod SmtpDeliveryMethod { get; set; }

        /// <summary>
        /// SMTP Delivery Format - See: .Net SmptpClient
        /// </summary>
        public SmtpDeliveryFormat SmtpDeliveryFormat { get; set; }

        /// <summary>
        /// SMTP User Name - used for SmtpClient.Credentials
        /// </summary>
        public string SmtpUsername { get; set; }

        /// <summary>
        /// SMTP Password - used for SmtpClient.Credentials
        /// </summary>
        public string SmtpPassword { get; set; }

        /// <summary>
        /// SMTP Use Default Credentials - If set,
        ///   System.Net.CredentialCache.DefaultNetworkCredentials will be used instead
        ///   of SmtpUsername and SmtpPassword
        /// </summary>
        public bool UseDefaultCredentials { get; set; }
    }
}