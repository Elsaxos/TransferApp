using System.ComponentModel.DataAnnotations;

namespace TransferApp.Models
{
    public class TransferRequest
    {
        public int Id { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string PickupAddress { get; set; } = string.Empty;
        public string DropoffAddress { get; set; } = string.Empty;

        public DateTime PickupDateTime { get; set; }
        public int Passengers { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; } = "Запитване";
        public string Notes { get; set; } = "";
    }
}


