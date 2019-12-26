using System;

namespace Hazebroek.Tgtg.Notify
{
    internal sealed class ItemNotification
    {
        public string ItemId { get; }
        public string StoreName { get; }
        public uint Quantity { get; }
        public Uri StorePicture { get; }

        public ItemNotification(string itemId, string storeName, uint quantity, Uri storePicture)
        {
            ItemId = itemId;
            StoreName = storeName;
            Quantity = quantity;
            StorePicture = storePicture;
        }
    }
}