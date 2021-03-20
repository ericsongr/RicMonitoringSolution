using System.Net.Mail;
using System.Threading.Tasks;

namespace RicCommon.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var host = "mail.ericsonramos.com";
            var pass = "P@ssw0rd)123";
            var from = "info@ericsonramos.com";
            var to = email;

            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();
            m.From = new MailAddress(from);
            m.To.Add(to);
            m.Subject = subject;
            m.Body = message;
            m.IsBodyHtml = true;
            sc.Host = host;

            string str1 = "gmail.com";
            string str2 = from;
            if (str2.Contains(str1))
            {
                try
                {
                    sc.Port = 8889;
                    sc.Credentials = new System.Net.NetworkCredential(from, pass);
                    sc.EnableSsl = true;
                    sc.Send(m);

                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    sc.Port = 8889;
                    sc.Credentials = new System.Net.NetworkCredential(from, pass);
                    sc.EnableSsl = false;
                    sc.Send(m);

                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }

            return Task.CompletedTask;
        }
    }
}
