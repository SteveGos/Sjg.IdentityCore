using System.Threading.Tasks;

namespace Sjg.IdentityCore.Utilities.Mail
{
    /// <summary>
    /// Email Service Interface
    /// </summary>
    public interface IAccAuthEmailService
    {
        Task SendAsync(AccAuthEmailMessage emailMessage); // SmtpClient

        // List<WebEmailMessage> ReceiveEmail(int maxCount = 10);
    }
}