namespace TransferApp.ViewModels
{
    public class PricesPublicViewModel
    {
        public List<PricesPublicRow> Items { get; set; } = new();
    }

    public class PricesPublicRow
    {
        public int Id { get; set; }

        public string RouteKey { get; set; } = "";
        public string RouteBg { get; set; } = "";
        public string RouteEn { get; set; } = "";
        public string RouteRu { get; set; } = "";
        public string RouteFr { get; set; } = "";

        public decimal OneWayPrice { get; set; }
        public decimal RoundTripPrice { get; set; }

        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
