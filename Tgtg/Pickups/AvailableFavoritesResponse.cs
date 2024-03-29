using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hazebroek.Tgtg.Pickups
{
    internal sealed class AvailableFavoritesResponse
    {
        [JsonProperty("items")] public ICollection<StoreItem> StoreItems { get; } = new List<StoreItem>();
    }

    internal class StoreItem
    {
        [JsonProperty("item")] public Item? Item { get; set; }
        [JsonProperty("store")] public Store? Store { get; set; }
        [JsonProperty("items_available")] public uint ItemsAvailable { get; set; }
        [JsonProperty("purchase_end")] public DateTime PurchaseEnd { get; set; }
        [JsonProperty("sold_out_at")] public DateTime SoldOutAt { get; set; }

        public bool HasItems => ItemsAvailable > 0;

        public override string ToString() => $"{nameof(Item)}: {Item}, {nameof(Store)}: {Store}, {nameof(ItemsAvailable)}: {ItemsAvailable}, {nameof(PurchaseEnd)}: {PurchaseEnd}, {nameof(SoldOutAt)}: {SoldOutAt}, {nameof(HasItems)}: {HasItems}";
    }

    internal class Item
    {
        [JsonProperty("item_id")] public string? Id { get; set; }
        [JsonProperty("logo_picture")] public Picture? Picture { get; set; }

        public override string ToString() => $"{nameof(Id)}: {Id}";
    }

    internal sealed class Picture
    {
        [JsonProperty("current_url")] public Uri? Uri { get; set; }

        public override string ToString() => $"{nameof(Uri)}: {Uri}";
    }
}