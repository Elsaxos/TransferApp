namespace TransferApp.Models
{
    public class PriceItem
    {
        public int Id { get; set; }

        // вътрешен ключ (не се показва)
        public string RouteKey { get; set; } = "";

        // текст по езици
        public string RouteBg { get; set; } = "";
        public string RouteEn { get; set; } = "";
        public string RouteRu { get; set; } = "";
        public string RouteFr { get; set; } = "";

        // цени в EUR
        public decimal OneWayPrice { get; set; }
        public decimal RoundTripPrice { get; set; }

        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
