using StockControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Service
{
    public interface IPurchaseReceivalService
    {
        /// <summary>
        /// Purchase Receival -- Parent
        /// </summary>
        /// 
        List<PurchaseReceivalModel> GetPurchaseReceivalList();
        PurchaseReceivalModel CreatePurchaseReceival(PurchaseReceivalModel purchaseReceival);
        void DeletePurchaseReceival(int purchaseReceivalId);
        PurchaseReceivalModel UpdatePurchaseReceival(PurchaseReceivalModel purchaseReceival);
        bool ConfirmPurchaseReceival(PurchaseReceivalModel purchaseReceival);
        bool UnconfirmPurchaseReceival(PurchaseReceivalModel purchaseReceival);

        // Contact must be present
        // Receival date must be present
        bool ValidateCreatePurchaseReceival(PurchaseReceivalModel purchaseReceival);
        // Contact must be present
        // Can't update if it's confirmed
        bool ValidateUpdatePurchaseReceival(PurchaseReceivalModel purchaseReceival);
        // Can't destroy if it's confirmed
        bool ValidateDeletePurchaseReceival(PurchaseReceivalModel purchaseReceival);
        // PurchaseReceivalDetail.count != 0
        bool ValidateConfirmPurchaseReceival(PurchaseReceivalModel purchaseReceival);
        // Can't unconfirm if item.ready < 0
        bool ValidateUnconfirmPurchaseReceival(PurchaseReceivalModel purchaseReceival, ItemService _itemService);

        /// <summary>
        /// Purchase Receival Detail -- Children
        /// </summary>
        /// 
        List<PurchaseReceivalDetailModel> GetPurchaseReceivalDetailList(int purchaseReceivalId);
        PurchaseReceivalDetailModel GetPurchaseReceivalDetail(int purchaseReceivalDetailId);
        PurchaseReceivalDetailModel CreatePurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail);
        void DeletePurchaseReceivalDetail(int purchaseReceivalDetailId);
        void DeletePurchaseReceivalDetailByPurchaseReceivalId(int purchaseReceivalId);
        PurchaseReceivalDetailModel UpdatePurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail);
        bool ConfirmPurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail);
        bool UnconfirmPurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, ItemService _itemService);

        // PurchaseOrderDetailId must be present
        // Contact must be present
        // PurchaseOrderDetailId belong to the Contact
        // Quantity > 0
        // Quantity <= PendingReceival of the selected PurchaseOrderDetail
        // Unique PurchaseOrderDetailId in a given PurchaseReceival
        // PurchaseOrderDetail belongs to the same contact
        // PendingReceival in the given PurchaseOrderDetail > 0
        // PurchaseOrderDetail.IsConfirmed == true
        bool ValidateCreateUpdatePurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail);
        // Can't be destroyed if it is confirmed
        bool ValidateDeletePurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail);
        // Can't unconfirm if associate item.pendingReceival will be changed to < 0 after unconfirm
        // Can't unconfirm if quantity of item.ready will be changed to < 0 after unconfirm
        bool ValidateUnconfirmPurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, ItemService _itemService);

    }
}