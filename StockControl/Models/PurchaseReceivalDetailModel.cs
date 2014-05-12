using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Models
{
    public class PurchaseReceivalDetailModel
    {
        public int Id { get; set; }
        public int PurchaseReceivalId { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public int ItemId { get; set; }
        public int PurchaseOrderDetailId { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }

        public static PurchaseReceivalDetailModel ToModel(PurchaseReceivalDetailModel purchaseReceivalDetail)
        {
            PurchaseReceivalDetailModel model = new PurchaseReceivalDetailModel();
            model.Id = purchaseReceivalDetail.Id;
            model.PurchaseReceivalId = purchaseReceivalDetail.PurchaseReceivalId;
            model.Code = purchaseReceivalDetail.Code;
            model.Quantity = purchaseReceivalDetail.Quantity;
            model.ItemId = purchaseReceivalDetail.ItemId;
            model.PurchaseOrderDetailId = purchaseReceivalDetail.PurchaseOrderDetailId;
            model.IsConfirmed = purchaseReceivalDetail.IsConfirmed;
            model.IsDeleted = purchaseReceivalDetail.IsDeleted;
            model.CreatedAt = purchaseReceivalDetail.CreatedAt;
            model.UpdatedAt = purchaseReceivalDetail.UpdatedAt;
            model.DeletedAt = purchaseReceivalDetail.DeletedAt;
            return model;
        }
    }
}