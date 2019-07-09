using System.Threading.Tasks;

namespace Sjg.IdentityCore.Services
{
    public interface IAccAuthEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);

        void SendEmail(string email, string subject, string message);
    }
}