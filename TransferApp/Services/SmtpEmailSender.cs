using Microsoft.Extensions.Configuration;
using System.Net;
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
            var portStr = smtp["Port"];
            var user = smtp["User"];
            var pass = smtp["Pass"];

            var fromEmail = smtp["FromEmail"] ?? user;
            var fromName = smtp["FromName"] ?? "ProTransfer";

            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(portStr) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(pass))
            {
                throw new System.InvalidOperationException("Missing SMTP configuration in appsettings.json (Smtp:Host/Port/User/Pass).");
            }

            var port = int.Parse(portStr);

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(user, pass)
            };

            using var mail = new MailMessage
            {
                From = new MailAddress(fromEmail!, fromName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            mail.To.Add(to);

            await client.SendMailAsync(mail);
        }
    }
}

