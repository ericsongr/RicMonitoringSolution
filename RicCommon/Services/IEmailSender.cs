using System.Threading.Tasks;

namespace RicCommon.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
