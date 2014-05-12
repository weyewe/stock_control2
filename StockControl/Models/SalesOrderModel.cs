using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Models
{
    public class SalesOrderModel
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string Code { get; set; }
        public System.DateTime SalesDate { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }

        public static SalesOrderModel ToModel(SalesOrder salesOrder)
        {
            SalesOrderModel model = new SalesOrderModel();
            model.Id = salesOrder.Id;
            model.ContactId = salesOrder.ContactId;
            model.Code = salesOrder.Code;
            model.SalesDate = salesOrder.SalesDate;
            model.IsConfirmed = salesOrder.IsConfirmed;
            model.IsDeleted = salesOrder.IsDeleted;
            model.CreatedAt = salesOrder.CreatedAt;
            model.UpdatedAt = salesOrder.UpdatedAt;
            model.DeletedAt = salesOrder.DeletedAt;
            return model;
        }
    }
}