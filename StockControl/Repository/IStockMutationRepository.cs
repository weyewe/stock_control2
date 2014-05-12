using StockControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Repository
{
    public interface IStockMutationRepository: IRepository<StockMutation>
    {
        List<StockMutationModel> GetStockMutationList();
        List<StockMutation> GetStockMutationByItem(ItemModel item);
        List<StockMutation> GetStockMutationBySourceDocument (ItemModel item, string sourceDocument, int sourceDocumentId);
        List<StockMutation> GetStockMutationBySourceDocumentDetail(ItemModel item, string sourceDocumentDetail, int sourceDocumentDetailId);

        StockMutation CreateStockMutation(StockMutation stockMutation);
        StockMutation UpdateStockMutation(StockMutation stockMutation);
    }
}