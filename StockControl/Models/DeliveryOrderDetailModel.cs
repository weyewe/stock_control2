using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Models
{
    public class DeliveryOrderDetailModel
    {
        public int Id { get; set; }
        public int DeliveryOrderId { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public int ItemId { get; set; }
        public int SalesOrderDetailId { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }

        public static DeliveryOrderDetailModel ToModel(DeliveryOrderDetail dod)
        {
            DeliveryOrderDetailModel model = new DeliveryOrderDetailModel();
            model.Id = dod.Id;
            model.DeliveryOrderId = dod.Id;
            model.Code = dod.Code;
            model.Quantity = dod.Quantity;
            model.ItemId = dod.ItemId;
            model.SalesOrderDetailId = dod.SalesOrderDetailId;
            model.IsConfirmed = dod.IsConfirmed;
            model.IsDeleted = dod.IsDeleted;
            model.CreatedAt = dod.CreatedAt;
            model.UpdatedAt = dod.UpdatedAt;
            model.DeletedAt = dod.DeletedAt;
            return model;
        }
    }
}