using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Models
{
    public class SalesOrderDetailModel
    {
        public int Id { get; set; }
        public int SalesOrderId { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public int ItemId { get; set; }
        public int PendingDelivery { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsFulfilled { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }

        public static SalesOrderDetailModel ToModel(SalesOrderDetail salesOrderDetail)
        {
            SalesOrderDetailModel model = new SalesOrderDetailModel();
            model.Id = salesOrderDetail.Id;
            model.SalesOrderId = salesOrderDetail.SalesOrderId;
            model.Code = salesOrderDetail.Code;
            model.Quantity = salesOrderDetail.Quantity;
            model.ItemId = salesOrderDetail.ItemId;
            model.PendingDelivery = salesOrderDetail.PendingDelivery;
            model.IsConfirmed = salesOrderDetail.IsConfirmed;
            model.IsFulfilled = salesOrderDetail.IsFulfilled;
            model.IsDeleted = salesOrderDetail.IsDeleted;
            model.CreatedAt = salesOrderDetail.CreatedAt;
            model.UpdatedAt = salesOrderDetail.UpdatedAt;
            model.DeletedAt = salesOrderDetail.DeletedAt;
            return model;
        }
    }
}