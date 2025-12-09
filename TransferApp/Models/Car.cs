namespace TransferApp.Models
{
    public class Car
    {
        public int Id { get; set; }

        public string Make { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public string Registration { get; set; } = string.Empty;

        public int Seats { get; set; }

        // ВАЖНО: правим го nullable, за да НЕ е required от MVC
        public string? ImageUrl { get; set; }
    }
}

