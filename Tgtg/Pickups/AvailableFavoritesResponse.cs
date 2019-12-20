using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hazebroek.Tgtg.Pickups
{
    internal sealed class AvailableFavoritesResponse
    {
        [JsonProperty("items")] public ICollection<StoreItem>? StoreItems { get; set; }
    }

    internal class StoreItem
    {
        [JsonProperty("item")] public Item? Item { get; set; }
        [JsonProperty("store")] public Store? Store { get; set; }
        [JsonProperty("items_available")] public int ItemsAvailable { get; set; }
        [JsonProperty("sold_out_at")] public DateTime SoldOutAt { get; set; }

        public bool HasItems => ItemsAvailable > 0;

        public override string ToString()
        {
            return
                $"{nameof(Item)}: {Item}, {nameof(Store)}: {Store}, {nameof(ItemsAvailable)}: {ItemsAvailable}, {nameof(SoldOutAt)}: {SoldOutAt}";
        }
    }

    internal class Item
    {
        [JsonProperty("item_id")] public string? Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}";
        }
    }
}