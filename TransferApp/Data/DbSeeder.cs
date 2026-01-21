using TransferApp.Models;

namespace TransferApp.Data
{
    public static class DbSeeder
    {
        public static void SeedPriceItems(ApplicationDbContext db)
        {
            
            if (db.PriceItems.Any())
                return;

            db.PriceItems.AddRange(
                new PriceItem
                {
                    RouteKey = "SOF_AIRPORT",
                    RouteBg = "София ⇄ Летище София",
                    RouteEn = "Sofia ⇄ Sofia Airport",
                    RouteRu = "София ⇄ Аэропорт София",
                    RouteFr = "Sofia ⇄ Aéroport de Sofia",
                    OneWayPrice = 35m,
                    RoundTripPrice = 65m,
                    SortOrder = 1,
                    IsActive = true
                },
                new PriceItem
                {
                    RouteKey = "SOF_PLOVDIV",
                    RouteBg = "София ⇄ Пловдив",
                    RouteEn = "Sofia ⇄ Plovdiv",
                    RouteRu = "София ⇄ Пловдив",
                    RouteFr = "Sofia ⇄ Plovdiv",
                    OneWayPrice = 120m,
                    RoundTripPrice = 220m,
                    SortOrder = 2,
                    IsActive = true
                },
                new PriceItem
                {
                    RouteKey = "SOF_VARNA",
                    RouteBg = "София ⇄ Варна",
                    RouteEn = "Sofia ⇄ Varna",
                    RouteRu = "София ⇄ Варна",
                    RouteFr = "Sofia ⇄ Varna",
                    OneWayPrice = 380m,
                    RoundTripPrice = 700m,
                    SortOrder = 3,
                    IsActive = true
                },
                new PriceItem
                {
                    RouteKey = "SOF_BURGAS",
                    RouteBg = "София ⇄ Бургас",
                    RouteEn = "Sofia ⇄ Burgas",
                    RouteRu = "София ⇄ Бургас",
                    RouteFr = "Sofia ⇄ Burgas",
                    OneWayPrice = 360m,
                    RoundTripPrice = 660m,
                    SortOrder = 4,
                    IsActive = true
                }
            );

            db.SaveChanges();
        }
    }
}
