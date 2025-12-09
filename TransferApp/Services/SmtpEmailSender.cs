using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TransferApp.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public SmtpEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlMessage)
        {
            var smtp = _config.GetSection("Smtp");
            var host = smtp["Host"];
            var port = int.Parse(smtp["Port"]);
            var user = smtp["User"];
            var pass = smtp["Pass"];

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = true,
                Credentials = new System.Net.NetworkCredential(user, pass)
            };

            var mail = new MailMessage(user, to, subject, htmlMessage)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(mail);
        }
    }
}
