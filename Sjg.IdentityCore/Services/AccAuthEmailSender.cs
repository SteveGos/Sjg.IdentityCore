using Sjg.IdentityCore.Utilities.Mail;
using System.Threading.Tasks;

namespace Sjg.IdentityCore.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class AccAuthEmailSender : IAccAuthEmailSender
    {
        //private readonly IAccAuthEmailService _emailService;
        private readonly IAccAuthConfiguration _accAuthConfiguration;

        public AccAuthEmailSender(IAccAuthConfiguration accAuthConfiguration)
        {
            _accAuthConfiguration = accAuthConfiguration;
            //_emailService = new AccAuthEmailService(_accAuthConfiguration);
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return ExecuteAsync(email, subject, message);
            //return Task.CompletedTask;
        }

        public void SendEmail(string email, string subject, string message)
        {
            Execute(email, subject, message);
        }

        // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-2.1&tabs=visual-studio

        private Task ExecuteAsync(string email, string subject, string message)
        {
            return Task.Run(() => { Execute(email, subject, message); });
        }

        private void Execute(string email, string subject, string message)
        {
            //var html = _viewRenderService.RenderToStringAsync(@"EmailTemplates/Sample", null);
            //var plain = _viewRenderService.RenderToStringAsync(@"EmailTemplates/Sample.text", null);

            //EmailMessage emailMessage = new EmailMessage
            //{
            //    Subject = "Test Email",
            //    HtmlBody = html.Result,
            //    TextBody = plain.Result,
            //};

            AccAuthEmailMessage emailMessage = new AccAuthEmailMessage
            {
                Subject = subject,
                HtmlBody = message,
                TextBody = message,
            };

            emailMessage.ToAddresses.Add(new AccAuthEmailAddress
            {
                Address = email,
                Name = email
            });

            var emailService = new AccAuthEmailService(_accAuthConfiguration);
            emailService.SendAsync(emailMessage).Wait();
        }
    }
}