using System.Threading.Tasks;

namespace TransferApp.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string htmlMessage);
    }
}

