using Sjg.IdentityCore.Services;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

//using MailKit.Net.Pop3;
//using MailKit.Net.Smtp;
//using MimeKit;

namespace Sjg.IdentityCore.Utilities.Mail
{
    /// <summary>
    /// Web Mail Service.  Send Email using SmtpClient
    /// </summary>
    public class AccAuthEmailService : IAccAuthEmailService
    {
        private readonly IAccAuthConfiguration _accAuthConfiguration;

        public AccAuthEmailService(IAccAuthConfiguration accAuthConfiguration)
        {
            _accAuthConfiguration = accAuthConfiguration;
        }

        // TODO: Not Yet Implemented
        ////////public List<WebEmailMessage> ReceiveEmail(int maxCount = 10)
        ////////{
        ////////    return new List<WebEmailMessage>();

        ////////    // Mail Kit
        ////////    //    using (var emailClient = new Pop3Client())
        ////////    //    {
        ////////    //        emailClient.Connect(_emailConfiguration.PopServer, _emailConfiguration.PopPort, true);

        ////////    //        emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

        ////////    //        emailClient.Authenticate(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);

        ////////    //        List<EmailMessage> emails = new List<EmailMessage>();
        ////////    //        for (int i = 0; i < emailClient.Count && i < maxCount; i++)
        ////////    //        {
        ////////    //            var message = emailClient.GetMessage(i);
        ////////    //            var emailMessage = new EmailMessage
        ////////    //            {
        ////////    //                //Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,

        ////////    //                HtmlBody = message.HtmlBody,
        ////////    //                TextBody = message.TextBody,

        ////////    //                Subject = message.Subject
        ////////    //            };
        ////////    //            emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
        ////////    //            emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
        ////////    //        }

        ////////    //        return emails;
        ////////    //    }
        ////////}

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="emailMessage">Mail Message to Send</param>
        /// <returns>Returns Task</returns>
        public Task SendAsync(AccAuthEmailMessage emailMessage)
        {
            System.Net.NetworkCredential credentials;
            if (_accAuthConfiguration.AccAuthEmailConfiguration != null)
            {
                if (!string.IsNullOrWhiteSpace(_accAuthConfiguration.AccAuthEmailConfiguration.SmtpUsername))
                {
                    credentials = new System.Net.NetworkCredential(_accAuthConfiguration.AccAuthEmailConfiguration.SmtpUsername, _accAuthConfiguration.AccAuthEmailConfiguration.SmtpPassword);
                }
                else
                {
                    credentials = new System.Net.NetworkCredential("DUMMY", "DUMMY");
                }
            }
            else
            {
                credentials = new System.Net.NetworkCredential("DUMMY", "DUMMY");
            }

            return SendAsync(emailMessage, credentials);
        }

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="emailMessage">Mail Message to Send</param>
        /// <returns>Returns Task</returns>
        public void Send(AccAuthEmailMessage emailMessage)
        {
            System.Net.NetworkCredential credentials;
            if (_accAuthConfiguration.AccAuthEmailConfiguration != null)
            {
                if (!string.IsNullOrWhiteSpace(_accAuthConfiguration.AccAuthEmailConfiguration.SmtpUsername))
                {
                    credentials = new System.Net.NetworkCredential(_accAuthConfiguration.AccAuthEmailConfiguration.SmtpUsername, _accAuthConfiguration.AccAuthEmailConfiguration.SmtpPassword);
                }
                else
                {
                    credentials = new System.Net.NetworkCredential("DUMMY", "DUMMY");
                }
            }
            else
            {
                credentials = new System.Net.NetworkCredential("DUMMY", "DUMMY");
            }

            Send(emailMessage, credentials);
        }

        private Task SendAsync(AccAuthEmailMessage emailMessage, System.Net.NetworkCredential credentials)
        {
            SmtpClient smtpClient;
            if (_accAuthConfiguration.AccAuthEmailConfiguration != null)
            {
                smtpClient = new SmtpClient()
                {
                    EnableSsl = _accAuthConfiguration.AccAuthEmailConfiguration.EnableSsl,
                    //PickupDirectoryLocation = _webEmailConfiguration.PickupDirectoryLocation,
                    DeliveryMethod = _accAuthConfiguration.AccAuthEmailConfiguration.SmtpDeliveryMethod,
                    DeliveryFormat = _accAuthConfiguration.AccAuthEmailConfiguration.SmtpDeliveryFormat,
                    Timeout = _accAuthConfiguration.AccAuthEmailConfiguration.Timeout,
                    UseDefaultCredentials = _accAuthConfiguration.AccAuthEmailConfiguration.UseDefaultCredentials,
                    Credentials = credentials
                };

                if (smtpClient.DeliveryMethod == SmtpDeliveryMethod.Network && !string.IsNullOrWhiteSpace(_accAuthConfiguration.AccAuthEmailConfiguration.SmtpServer))
                {
                    smtpClient.Host = _accAuthConfiguration.AccAuthEmailConfiguration.SmtpServer;
                    smtpClient.Port = _accAuthConfiguration.AccAuthEmailConfiguration.SmtpPort;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(_accAuthConfiguration.AccAuthEmailConfiguration.PickupDirectoryLocation))
                    {
                        smtpClient.PickupDirectoryLocation = _accAuthConfiguration.AccAuthEmailConfiguration.PickupDirectoryLocation;
                    }
                }
            }
            else
            {
                smtpClient = new SmtpClient();
            }

            return Task.Run(() => Send(emailMessage, smtpClient));
        }

        private void Send(AccAuthEmailMessage emailMessage, SmtpClient smtpClient)
        {
            using (var message = new MailMessage() { Subject = emailMessage.Subject })
            {
                message.Subject = emailMessage.Subject;

                // From comes from (emailMessage, configuration, or SMTP Client in Configuration..
                if (emailMessage.FromAddress != null)
                {
                    // From email Message
                    message.From = new MailAddress(emailMessage.FromAddress.Address, emailMessage.FromAddress.Name);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(_accAuthConfiguration.AccAuthEmailConfiguration.FromUserEmail))
                    {
                        // From Configuration
                        message.From = new MailAddress(_accAuthConfiguration.AccAuthEmailConfiguration.FromUserEmail, _accAuthConfiguration.AccAuthEmailConfiguration.FromUserName);
                    }
                }

                foreach (var item in emailMessage.ToAddresses)
                {
                    message.To.Add(new MailAddress(item.Address, item.Name));
                }
                foreach (var item in emailMessage.CcAddresses)
                {
                    message.CC.Add(new MailAddress(item.Address, item.Name));
                }
                foreach (var item in emailMessage.BccAddresses)
                {
                    message.Bcc.Add(new MailAddress(item.Address, item.Name));
                }

                if (!string.IsNullOrWhiteSpace(emailMessage.HtmlBody))
                {
                    if (!string.IsNullOrWhiteSpace(emailMessage.TextBody))
                    {
                        var alternateviewHTML = AlternateView.CreateAlternateViewFromString(emailMessage.HtmlBody, null, MediaTypeNames.Text.Html);
                        message.AlternateViews.Add(alternateviewHTML);

                        var alternateviewPlainText = AlternateView.CreateAlternateViewFromString(emailMessage.TextBody, null, MediaTypeNames.Text.Plain);
                        message.AlternateViews.Add(alternateviewPlainText);
                    }
                    else
                    {
                        message.Body = emailMessage.HtmlBody;
                        message.IsBodyHtml = true;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(emailMessage.TextBody))
                {
                    message.Body = emailMessage.TextBody;
                }

                smtpClient.SendMailAsync(message).Wait();
            }
        }

        private void Send(AccAuthEmailMessage emailMessage, System.Net.NetworkCredential credentials)
        {
            SendAsync(emailMessage, credentials).Wait();
        }

        //private void Send(AccAuthEmailMessage emailMessage, SmtpClient smtpClient)
        //{
        //    SendAsync(emailMessage, smtpClient).Wait();
        //}

        ////// MailKit
        //////public async Task SendAsync(WebEmailMessage emailMessage)
        //////{
        //////    // MailKit

        //////    //// Now we just need to set the message body and we're done
        //////    //message.Body = builder.ToMessageBody();

        //////    //var message = new MimeMessage();

        //////    //message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

        //////    //if (emailMessage.FromAddresses.Count > 0)
        //////    //{
        //////    //    message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
        //////    //}
        //////    //else
        //////    //{
        //////    //    message.From.Add(new MailboxAddress(_emailConfiguration.FromUserName, _emailConfiguration.FromUserEmail));
        //////    //}

        //////    //message.Subject = emailMessage.Subject;

        //////    //var builder = new BodyBuilder();

        //////    //if (!string.IsNullOrWhiteSpace(emailMessage.TextBody))
        //////    //{
        //////    //    builder.TextBody = emailMessage.TextBody;
        //////    //}
        //////    //if (!string.IsNullOrWhiteSpace(emailMessage.TextBody))
        //////    //{
        //////    //    builder.HtmlBody = emailMessage.HtmlBody;
        //////    //}

        //////    //// Now we just need to set the message body and we're done
        //////    //message.Body = builder.ToMessageBody();

        //////    //if (string.IsNullOrWhiteSpace(_emailConfiguration.SpecifiedPickupDirectory))
        //////    //{
        //////    //    //Be careful that the SmtpClient class is the one from Mailkit not the framework!
        //////    //    using (var emailClient = new SmtpClient())
        //////    //    {
        //////    //        //The last parameter here is to use SSL (Which you should!)
        //////    //        if (!string.IsNullOrWhiteSpace(_emailConfiguration.UseSsl) && _emailConfiguration.UseSsl.Equals("Yes", StringComparison.OrdinalIgnoreCase))
        //////    //        {
        //////    //            emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);
        //////    //        }
        //////    //        else
        //////    //        {
        //////    //            emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, false);
        //////    //        }

        //////    //        //Remove any OAuth functionality as we won't be using it.
        //////    //        emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

        //////    //        if (!string.IsNullOrWhiteSpace(_emailConfiguration.SmtpUsername))
        //////    //        {
        //////    //            emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
        //////    //        }

        //////    //        emailClient.Send(message);

        //////    //        emailClient.Disconnect(true);
        //////    //    }
        //////    //}
        //////    //else
        //////    //{
        //////    //    do
        //////    //    {
        //////    //        // Note: this will require that you know where the specified pickup directory is.
        //////    //        var path = Path.Combine(_emailConfiguration.SpecifiedPickupDirectory, Guid.NewGuid().ToString() + ".eml");

        //////    //        if (File.Exists(path))
        //////    //            continue;

        //////    //        try
        //////    //        {
        //////    //            using (var stream = new FileStream(path, FileMode.CreateNew))
        //////    //            {
        //////    //                message.WriteTo(stream);
        //////    //                return;
        //////    //            }
        //////    //        }
        //////    //        catch (IOException ex)
        //////    //        {
        //////    //            var sjg = ex.GetBaseException().Message;

        //////    //            // The file may have been created between our File.Exists() check and
        //////    //            // our attempt to create the stream.
        //////    //        }
        //////    //    } while (true);
        //////    //}
        //////}
    }
}