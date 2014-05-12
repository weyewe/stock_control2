using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Models
{
    public class StockMutationModel
    {
        public int Id { get; set; }
        public string ItemId { get; set; } // Stock Keeping Unit
        public int Quantity { get; set; }
        public int MutationCase { get; set; }
        public int ItemCase { get; set; }
        public int SourceDocumentId { get; set; }
        public string SourceDocument { get; set; }
        public int SourceDocumentDetailId { get; set; }
        public string SourceDocumentDetail { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }

        public static StockMutationModel ToModel(StockMutation stockMutation)
        {
            StockMutationModel model = new StockMutationModel();
            model.Id = stockMutation.Id;
            model.ItemId = stockMutation.ItemId;
            model.Quantity = stockMutation.Quantity;
            model.MutationCase = stockMutation.MutationCase;
            model.ItemCase = stockMutation.ItemCase;
            model.SourceDocumentId = stockMutation.ItemCase;
            model.SourceDocument = stockMutation.SourceDocument;
            model.SourceDocumentDetailId = stockMutation.SourceDocumentDetailId;
            model.SourceDocumentDetail = stockMutation.SourceDocumentDetail;
            model.IsDeleted = stockMutation.IsDeleted;
            model.CreatedAt = stockMutation.CreatedAt;
            model.UpdatedAt = stockMutation.UpdatedAt;
            model.DeletedAt = stockMutation.DeletedAt;
            return model;
        }
    }
}