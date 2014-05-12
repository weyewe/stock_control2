using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Models
{
    public class PurchaseOrderModel
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string Code { get; set; }
        public System.DateTime PurchaseDate { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }

        public static PurchaseOrderModel ToModel(PurchaseOrder purchaseOrder)
        {
            PurchaseOrderModel model = new PurchaseOrderModel();
            model.Id = purchaseOrder.Id;
            model.ContactId = purchaseOrder.ContactId;
            model.Code = purchaseOrder.Code;
            model.PurchaseDate = purchaseOrder.PurchaseDate;
            model.IsConfirmed = purchaseOrder.IsConfirmed;
            model.IsDeleted = purchaseOrder.IsDeleted;
            model.CreatedAt = purchaseOrder.CreatedAt;
            model.UpdatedAt = purchaseOrder.UpdatedAt;
            model.DeletedAt = purchaseOrder.DeletedAt;
            return model;
        }
    }
}