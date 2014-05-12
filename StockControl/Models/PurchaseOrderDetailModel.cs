using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Models
{
    public class PurchaseOrderDetailModel
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public int PendingReceival { get; set; }
        public int ItemId { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsFulfilled { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }

        public static PurchaseOrderDetailModel ToModel(PurchaseOrderDetail pod)
        {
            PurchaseOrderDetailModel model = new PurchaseOrderDetailModel();
            model.Id = pod.Id;
            model.PurchaseOrderId = pod.PurchaseOrderId;
            model.Code = pod.Code;
            model.Quantity = pod.Quantity;
            model.PendingReceival = pod.PendingReceival;
            model.ItemId = pod.ItemId;
            model.IsConfirmed = pod.IsConfirmed;
            model.IsFulfilled = pod.IsFulfilled;
            model.IsDeleted = pod.IsDeleted;
            model.CreatedAt = pod.CreatedAt;
            model.UpdatedAt = pod.UpdatedAt;
            model.DeletedAt = pod.DeletedAt;
            return model;
        }
    }
}