using System.Collections.Generic;

namespace Sjg.IdentityCore.Utilities.Mail
{
    /// <summary>
    /// Wen Mail Message
    /// </summary>
    public class AccAuthEmailMessage
    {
        public AccAuthEmailMessage()
        {
            ToAddresses = new List<AccAuthEmailAddress>();
            CcAddresses = new List<AccAuthEmailAddress>(); // SmtpClient
            BccAddresses = new List<AccAuthEmailAddress>(); // SmtpClient
        }

        /// <summary>
        /// From Address
        /// </summary>
        public AccAuthEmailAddress FromAddress { get; set; }

        /// <summary>
        /// To Addresses
        /// </summary>
        public List<AccAuthEmailAddress> ToAddresses { get; set; }

        /// <summary>
        /// CC Addresses
        /// </summary>
        public List<AccAuthEmailAddress> CcAddresses { get; set; } // SmtpClient

        /// <summary>
        /// BCC Addresses
        /// </summary>
        public List<AccAuthEmailAddress> BccAddresses { get; set; } // SmtpClient

        /// <summary>
        /// Subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// HTML Body
        /// </summary>
        public string HtmlBody { get; set; }

        /// <summary>
        /// Text Body
        /// </summary>
        public string TextBody { get; set; }
    }
}