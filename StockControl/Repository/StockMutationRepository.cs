using StockControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace StockControl.Repository
{
    public class StockMutationRepository : EfRepository<StockMutation>, IStockMutationRepository
    {

        /*
         * GET
         */

        /// <summary>
        /// Get all stock mutations from Database.
        /// </summary>
        /// <returns>All stock mutations</returns>
        public List<StockMutationModel> GetStockMutationList()
        {
             using (var db = GetContext())
            {
                IQueryable<StockMutationModel> sm = (from c in db.StockMutations
                                                       where !c.IsDeleted
                                                       select new StockMutationModel
                                                       {
                                                           Id = c.Id,
                                                           ItemId = c.ItemId,
                                                           Quantity = c.Quantity,
                                                           MutationCase = c.MutationCase,
                                                           ItemCase = c.ItemCase,
                                                           SourceDocumentId = c.ItemCase,
                                                           SourceDocument = c.SourceDocument,
                                                           SourceDocumentDetailId = c.SourceDocumentDetailId,
                                                           SourceDocumentDetail = c.SourceDocumentDetail,
                                                           IsDeleted = c.IsDeleted,
                                                           CreatedAt = c.CreatedAt,
                                                           UpdatedAt = c.UpdatedAt,
                                                           DeletedAt = c.DeletedAt
                                                       }).AsQueryable();

                return sm.ToList();
            }
        }


        /// <summary>
        /// Get all stock mutations given the item Stock keeping unit
        /// </summary>
        /// <param name="item">An object Item</param>
        /// <returns>All the list of stock mutations</returns>
        public List<StockMutation> GetStockMutationByItem(ItemModel item)
        {
            IQueryable<StockMutation> sm = FindAll(x => x.ItemId == item.Sku && !x.IsDeleted);
            return sm.ToList();
        }

        /// <summary>
        /// Get all stock mutations given the source document
        /// </summary>
        /// <param name="item">An object Item</param>
        /// <param name="sourceDocument">Source Document: Purchase Order, Purchase Receival, Sales Order, or Delivery Order</param>
        /// <param name="sourceDocumentId">Id of the corresponding source document</param>
        /// <returns>All stock mutations for the given source document</returns>
        public List<StockMutation> GetStockMutationBySourceDocument (ItemModel item, string sourceDocument, int sourceDocumentId)
        {
            IQueryable<StockMutation> sm = FindAll(x => x.ItemId == item.Sku && !x.IsDeleted &&
                                                    x.SourceDocument == sourceDocument && x.SourceDocumentId == sourceDocumentId);
            return sm.ToList();
        }

        /// <summary>
        /// Get all stock mutations given the source document detail
        /// </summary>
        /// <param name="item">An object Item</param>
        /// <param name="sourceDocumentDetail">Source Document Detail: Purchase Order Detail, Purchase Receival Detail,
        /// Sales Order, or Delivery Order</param>
        /// <param name="sourceDocumentDetailId">Id of the corresponding source document detail</param>
        /// <returns>All stock mutations for the given source document detail</returns>
        public List<StockMutation> GetStockMutationBySourceDocumentDetail (ItemModel item, string sourceDocumentDetail, int sourceDocumentDetailId)
        {
            IQueryable<StockMutation> sm = FindAll(x => x.ItemId == item.Sku && !x.IsDeleted &&
                                                    x.SourceDocumentDetail == sourceDocumentDetail && x.SourceDocumentDetailId == sourceDocumentDetailId);
            return sm.ToList();
        }

        /*
         * CREATE
         */

        /// <summary>
        /// Create a new stockMutation.
        /// </summary>
        /// <param name="stockMutation">An object StockMutation</param>
        /// <returns>The new stock mutation</returns>
        public StockMutation CreateStockMutation(StockMutation stockMutation)
        {
            StockMutation newstockMutation = new StockMutation();
            newstockMutation.Id = stockMutation.Id;
            newstockMutation.ItemId = stockMutation.ItemId;
            newstockMutation.Quantity = stockMutation.Quantity;
            newstockMutation.MutationCase = stockMutation.MutationCase;
            newstockMutation.ItemCase = stockMutation.ItemCase;
            newstockMutation.SourceDocumentId = stockMutation.ItemCase;
            newstockMutation.SourceDocument = stockMutation.SourceDocument;
            newstockMutation.SourceDocumentDetailId = stockMutation.SourceDocumentDetailId;
            newstockMutation.SourceDocumentDetail = stockMutation.SourceDocumentDetail;
            newstockMutation.IsDeleted = stockMutation.IsDeleted;
            newstockMutation.CreatedAt = stockMutation.CreatedAt;
            newstockMutation.UpdatedAt = stockMutation.UpdatedAt;
            newstockMutation.DeletedAt = stockMutation.DeletedAt;
            return Create(newstockMutation);
        }

        /* 
         * DELETE
         */
        /// <summary>
        /// Delete a certain stock mutation
        /// </summary>
        /// <param name="id">Id of the stock mutation</param>
        public void DeleteStockMutation(int id)
        {
            StockMutation sm = Find(x => x.Id == id && !x.IsDeleted);
            if (sm != null)
            {
                sm.IsDeleted = true;
                sm.DeletedAt = DateTime.Now;
                Update(sm);
            }
        }

        /*
         * UPDATE
         */

        /// <summary>
        /// Update a stock mutation.
        /// </summary>
        /// <param name="stockMutation">An object StockMutation</param>
        /// <returns>The updated stock mutation</returns>
        public StockMutation UpdateStockMutation(StockMutation stockMutation)
        {
            StockMutation s = Find(x => x.Id == stockMutation.Id && !x.IsDeleted);
            if (s != null)
            {
                s.Id = stockMutation.Id;
                s.ItemId = stockMutation.ItemId;
                s.Quantity = stockMutation.Quantity;
                s.MutationCase = stockMutation.MutationCase;
                s.ItemCase = stockMutation.ItemCase;
                s.SourceDocumentId = stockMutation.ItemCase;
                s.SourceDocument = stockMutation.SourceDocument;
                s.SourceDocumentDetailId = stockMutation.SourceDocumentDetailId;
                s.SourceDocumentDetail = stockMutation.SourceDocumentDetail;
                s.IsDeleted = stockMutation.IsDeleted;
                s.CreatedAt = stockMutation.CreatedAt;
                s.UpdatedAt = stockMutation.UpdatedAt;
                s.DeletedAt = stockMutation.DeletedAt;

                Update(s);
                return s;
            }
            return s;
        }

    }
}