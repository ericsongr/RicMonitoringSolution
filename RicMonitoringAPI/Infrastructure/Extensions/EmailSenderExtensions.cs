using System.Threading.Tasks;
using RicCommon.Services;

namespace RicMonitoringAPI.Infrastructure.Extensions
{
    public static class EmailSenderExtensions
    {
        public static Task SendDueReminderEmailAsync(this IEmailSender emailSender, string email, string emailBody)
        {
            return emailSender.SendEmailAsync(email, "Due Reminder", emailBody);
        }
    }
}
