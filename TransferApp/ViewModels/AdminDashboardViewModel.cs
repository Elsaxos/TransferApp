using TransferApp.Models;

namespace TransferApp.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<TransferRequest> Inquiries { get; set; } = new();
        public List<TransferRequest> Reservations { get; set; } = new();
    }
}
