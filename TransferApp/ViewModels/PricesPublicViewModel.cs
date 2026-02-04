using System.Collections.Generic;
using TransferApp.Models;

namespace TransferApp.ViewModels
{
    public class PricesPublicViewModel
    {
        public List<PriceItem> Prices { get; set; } = new();
    }
}

