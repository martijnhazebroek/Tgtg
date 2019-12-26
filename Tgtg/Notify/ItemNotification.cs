using System;
using MediatR;

namespace Hazebroek.Tgtg.Notify
{
    internal sealed class ItemNotification : INotification
    {
        public string UserDisplayName { get; }
        public string ItemId { get; }
        public string StoreName { get; }
        public uint Quantity { get; }
        public Uri StorePicture { get; }

        public ItemNotification(
            string userDisplayName,
            string itemId,
            string storeName,
            uint quantity,
            Uri storePicture
        )
        {
            UserDisplayName = userDisplayName;
            ItemId = itemId;
            StoreName = storeName;
            Quantity = quantity;
            StorePicture = storePicture;
        }
    }
}