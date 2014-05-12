using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Models
{
    public class PurchaseReceivalModel
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string Code { get; set; }
        public System.DateTime ReceivalDate { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }

        public static PurchaseReceivalModel ToModel(PurchaseReceival purchaseReceival)
        {
            PurchaseReceivalModel model = new PurchaseReceivalModel();
            model.Id = purchaseReceival.Id;
            model.ContactId = purchaseReceival.ContactId;
            model.Code = purchaseReceival.Code;
            model.ReceivalDate = purchaseReceival.ReceivalDate;
            model.IsConfirmed = purchaseReceival.IsConfirmed;
            model.IsDeleted = purchaseReceival.IsDeleted;
            model.CreatedAt = purchaseReceival.CreatedAt;
            model.UpdatedAt = purchaseReceival.UpdatedAt;
            model.DeletedAt = purchaseReceival.DeletedAt;
            return model;
        }
    }
}