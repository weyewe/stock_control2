using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Models
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public int Ready { get; set; }
        public int PendingDelivery { get; set; }
        public int PendingReceival { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }

        public static ItemModel ToModel(Item item)
        {
            ItemModel model = new ItemModel();

            model.Id = item.Id;
            model.Sku = item.Sku;
            model.Name = item.Name;
            model.Ready = item.Ready;
            model.PendingDelivery = item.PendingDelivery;
            model.PendingReceival = item.PendingReceival;
            model.IsDeleted = item.IsDeleted;
            model.CreatedAt = item.CreatedAt;
            model.UpdatedAt = item.UpdatedAt;
            model.DeletedAt = item.DeletedAt;

            return model;
        }
    }
}