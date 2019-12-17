using Newtonsoft.Json;

namespace Hazebroek.Tgtg.Pickups
{
    internal sealed class AvailableFavoritesRequest : PickupRequest
    {
        [JsonProperty("favorites_only")] public bool FavoritesOnly { get; set; } = true;
        [JsonProperty("with_stock_only")] public bool WithStockOnly { get; set; }
        [JsonProperty("radius")] public double Radius { get; set; } = 5000.0;
    }

    internal abstract class PickupRequest
    {
        [JsonProperty("user_id")] public long UserId { get; set; }

        [JsonProperty("origin")] public Origin Origin { get; set; } = new Origin();
    }

    internal class Origin
    {
        [JsonProperty("latitude")] public double Latitude { get; set; }
        [JsonProperty("longitude")] public double Longitude { get; set; }
    }
}