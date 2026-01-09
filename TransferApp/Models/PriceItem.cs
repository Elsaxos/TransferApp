using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TransferApp.Models
{
    public class PriceItem : IValidatableObject
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string RouteKey { get; set; } = "";

        [Required, StringLength(200)]
        public string RouteBg { get; set; } = "";

        [Required, StringLength(200)]
        public string RouteEn { get; set; } = "";

        [Required, StringLength(200)]
        public string RouteRu { get; set; } = "";

        [Required, StringLength(200)]
        public string RouteFr { get; set; } = "";

        [Precision(18, 2)]
        [Range(1, 10000)]
        public decimal OneWayPrice { get; set; }

        [Precision(18, 2)]
        [Range(1, 20000)]
        public decimal RoundTripPrice { get; set; }

        [Range(0, 9999)]
        public int SortOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RoundTripPrice < OneWayPrice)
            {
                yield return new ValidationResult(
                    "Цената за отиване и връщане трябва да е по-голяма или равна на еднопосочната.",
                    new[] { nameof(RoundTripPrice) }
                );
            }
        }
    }
}


