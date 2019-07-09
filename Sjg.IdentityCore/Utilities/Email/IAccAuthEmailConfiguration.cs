using System.Net.Mail;

namespace Sjg.IdentityCore.Utilities.Mail
{
    /// <summary>
    /// SMTP Mail Configuration
    /// </summary>
    public interface IAccAuthEmailConfiguration
    {
        /// <summary>
        /// SMTP Server Name
        /// </summary>
        string SmtpServer { get; set; }

        /// <summary>
        /// SMTP Port
        /// </summary>
        int SmtpPort { get; set; }

        /// <summary>
        /// SMTP User name if not using default credentials.
        /// </summary>
        string SmtpUsername { get; set; }

        /// <summary>
        /// SMTP Password if not using default credentials.
        /// </summary>
        string SmtpPassword { get; set; }

        /// <summary>
        /// From Email
        /// </summary>
        string FromUserEmail { get; set; }

        /// <summary>
        /// From Name
        /// </summary>
        string FromUserName { get; set; }

        ////// SMTPCLIENT properties

        /// <summary>
        /// Specifies the time-out value in milliseconds. The default value is 100,000 (100 seconds).
        /// Must not be less than zero.
        /// </summary>
        int Timeout { get; set; }

        /////////////// <summary>
        /////////////// Service Provider Name (SPN) to use for authentication when using extended protection.
        /////////////// The default value for this SPN is of the form "SMTPSVC/" where is the hostname of the SMTP mail server.
        /////////////// </summary>
        ////////////string TargetName { get; set; }

        /////////// <summary>
        /////////// Summary:
        ///////////    A System.Net.ServicePoint that connects to the System.Net.Mail.SmtpClient.Host
        ///////////    property used for SMTP.
        /////////// </summary>
        ////////ServicePoint ServicePoint { get; }

        /////////// <summary></summary>
        /////////// Name or IP address of the host used for SMTP transactions.
        /////////// </summary>
        ////////string Host { get; set; }

        /////////// <summary></summary>
        /////////// Port used for SMTP transactions.
        /////////// </summary>
        ////////int Port { get; set; }

        /// <summary></summary>
        /// Folder where applications save mail messages to be processed by the local SMTP server.
        /// </summary>
        string PickupDirectoryLocation { get; set; }

        /// <summary></summary>
        /// Specify whether the System.Net.Mail.SmtpClient uses Secure Sockets Layer (SSL) to encrypt the connection.
        /// If true then the System.Net.Mail.SmtpClient uses SSL; otherwise, false. The default is false.
        /// </summary>
        bool EnableSsl { get; set; }

        /// <summary></summary>
        /// Specifies how outgoing email messages will be handled.
        /// Value is a a System.Net.Mail.SmtpDeliveryMethod that indicates how email messages are delivered.
        /// </summary>
        SmtpDeliveryMethod SmtpDeliveryMethod { get; set; }

        /// <summary></summary>
        /// Delivery format used by System.Net.Mail.SmtpClient to send e-mail.
        /// Value is System.Net.Mail.SmtpDeliveryFormat. The delivery format used by System.Net.Mail.SmtpClient.
        /// </summary>
        SmtpDeliveryFormat SmtpDeliveryFormat { get; set; }

        /// <summary></summary>
        /// Controls whether the System.Net.CredentialCache.DefaultCredentials are sent with requests.
        /// If True if the default credentials are used; otherwise false. The default value is false.
        bool UseDefaultCredentials { get; set; }

        /////////////// <summary></summary>
        /////////////// Specify which certificates should be used to establish the Secure Sockets Layer (SSL) connection.
        ///////////////
        /////////////// Returns:
        ///////////////     An System.Security.Cryptography.X509Certificates.X509CertificateCollection, holding
        ///////////////     one or more client certificates. The default value is derived from the mail configuration
        ///////////////     attributes in a configuration file.
        /////////////// </summary>
        ////////////X509CertificateCollection ClientCertificates { get; }

        /////////////// <summary>
        /////////////// Summary:
        ///////////////     Gets or sets the credentials used to authenticate the sender.
        ///////////////
        /////////////// Returns:
        ///////////////     An System.Net.ICredentialsByHost that represents the credentials to use for authentication;
        ///////////////     or null if no credentials have been specified.
        ///////////////
        /////////////// Exceptions:
        ///////////////   T:System.InvalidOperationException:
        ///////////////     You cannot change the value of this property when an email is being sent.
        /////////////// </summary>
        ////////////ICredentialsByHost Credentials { get; set; }
    }
}