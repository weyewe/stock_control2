using StockControl.Models;
using StockControl.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Service
{
    public interface IStockMutationService
    {
        List<StockMutationModel> GetStockMutationList(IStockMutationRepository _stockMutationRepository);
        ResponseModel CreateStockMutation(StockMutationModel stockMutation, IStockMutationRepository _stockMutationRepository);
        // No deletion for StockMutation
        // void DeleteStockMutation(int id);
        ResponseModel UpdateStockMutation(StockMutationModel stockMutation, IStockMutationRepository _stockMutationRepository);

        // Id, ItemId, Quantity, MutationCase, ItemCase, SourceDocumentId, SourceDocument
        // SourceDocumentDetailId, SourceDocumentDetail must be present
        bool ValidateCreate(StockMutationModel stockMutation, out string message);
        bool ValidateUpdate(StockMutationModel stockMutation, out string message);
    }
}