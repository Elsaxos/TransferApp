using System.ComponentModel.DataAnnotations;

namespace TransferApp.ViewModels
{
    public class ContactFormViewModel
    {
        [Required(ErrorMessage = "Името е задължително")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Телефонът е задължителен")]
        public string Phone { get; set; } = "";

        [Required(ErrorMessage = "Email е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден email адрес")]
        public string Email { get; set; } = "";

        public string? Message { get; set; }
    }
}
