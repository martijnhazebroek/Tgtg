using Newtonsoft.Json;

namespace Hazebroek.Tgtg.Pickups
{
    internal sealed class Store
    {
        [JsonProperty("store_id")] public string? Id { get; set; }
        [JsonProperty("store_name")] public string? Name { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}";
        }
    }
}