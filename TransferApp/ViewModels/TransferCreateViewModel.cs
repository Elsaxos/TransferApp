using System.ComponentModel.DataAnnotations;
using TransferApp.Models;

namespace TransferApp.ViewModels
{
    public class TransferCreateViewModel
    {
        [Required]
        public string CustomerName { get; set; } = "";

        [Required]
        public string Phone { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string PickupAddress { get; set; } = "";

        [Required]
        public string DropoffAddress { get; set; } = "";

        public DateTime PickupDateTime { get; set; } = DateTime.Now.AddHours(1);

        [Range(1, 50)]
        public int Passengers { get; set; } = 1;

        [Required]
        public int? PriceItemId { get; set; }

        [Required]
        public string TripType { get; set; } = "OneWay"; // OneWay / RoundTrip

        public string Notes { get; set; } = "";

        public List<PriceItem> ActivePrices { get; set; } = new();
    }
}

