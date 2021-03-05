using System.Text.Encodings.Web;
using System.Threading.Tasks;
using RicAuthJwtServer.Data.Services;

namespace RicAuthJwtServer.Data.Extensions
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(link)}'>clicking here</a>.");
        }

        public static Task SendResetPasswordAsync(this IEmailSender emailSender, string email, string callbackUrl)
        {
            return emailSender.SendEmailAsync(email, "Reset Password",
                $"Please reset your password by <a href={HtmlEncoder.Default.Encode(callbackUrl)}' target='_blank'>clicking here</a>.");
        }
    }
}
