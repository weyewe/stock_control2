using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Models
{
    public class DeliveryOrderModel
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string Code { get; set; }
        public System.DateTime DeliveryDate { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }

        public static DeliveryOrderModel ToModel (DeliveryOrder deliveryorder)
        {
            DeliveryOrderModel model = new DeliveryOrderModel();
            model.Id = deliveryorder.Id;
            model.ContactId = deliveryorder.ContactId;
            model.Code = deliveryorder.Code;
            model.DeliveryDate = deliveryorder.DeliveryDate;
            model.IsConfirmed = deliveryorder.IsConfirmed;
            model.IsDeleted = deliveryorder.IsDeleted;
            model.CreatedAt = deliveryorder.CreatedAt;
            model.UpdatedAt = deliveryorder.UpdatedAt;
            model.DeletedAt = deliveryorder.DeletedAt;
            return model;
        }
    }
}