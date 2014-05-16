using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockControl.Models;

namespace StockControl.Repository
{
    public interface IPurchaseReceivalRepository : IRepository<PurchaseReceival>
    {

        List<PurchaseReceivalModel> GetPurchaseReceivalList();
        PurchaseReceivalModel GetPurchaseReceival(int Id);
        List<PurchaseReceivalModel> GetPurchaseReceivalByContactId(int contactId);
        PurchaseReceival CreatePurchaseReceival(PurchaseReceival purchaseReceival);
        void DeletePurchaseReceival(int purchaseReceivalId);
        PurchaseReceival UpdatePurchaseReceival(PurchaseReceival purchaseReceival);

        List<PurchaseReceivalDetailModel> GetPurchaseReceivalDetailList(int purchaseReceivalId);
        PurchaseReceivalDetailModel GetPurchaseReceivalDetail(int purchaseReceivalDetailId);
        List<PurchaseReceivalDetailModel> GetPurchaseReceivalDetailByPurchaseOrderDetail(int purchaseOrderDetailId);
        PurchaseReceivalDetail CreatePurchaseReceivalDetail(PurchaseReceivalDetail purchaseReceivalDetail);
        void DeletePurchaseReceivalDetail(int PurchaseReceivalDetailId);
        void DeletePurchaseReceivalDetailByPurchaseReceivalId(int PurchaseReceivalId);
        PurchaseReceivalDetail UpdatePurchaseReceivalDetail(PurchaseReceivalDetail purchaseReceivalDetail);
        void UpdateConfirmationPurchaseReceivalDetailByPurchaseReceivalId(int purchaseReceivalId, bool IsConfirmed);

    }
}