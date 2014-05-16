using StockControl.Models;
using StockControl.Repository;
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
        List<PurchaseReceivalModel> GetPurchaseReceivalList(IPurchaseReceivalRepository _purchaseReceivalRepository);
        ResponseModel CreatePurchaseReceival(PurchaseReceivalModel purchaseReceival, IPurchaseReceivalRepository _purchaseReceivalRepository);
        ResponseModel DeletePurchaseReceival(int purchaseReceivalId, IPurchaseReceivalRepository _purchaseReceivalRepository);
        ResponseModel UpdatePurchaseReceival(PurchaseReceivalModel purchaseReceival, IPurchaseReceivalRepository _purchaseReceivalRepository);
        ResponseModel ConfirmPurchaseReceival(PurchaseReceivalModel purchaseReceival, IPurchaseReceivalRepository _purchaseReceivalRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository);
        ResponseModel UnconfirmPurchaseReceival(PurchaseReceivalModel purchaseReceival, IPurchaseReceivalRepository _purchaseReceivalRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository);

        // Contact must be present
        // Receival date must be present
        bool ValidateCreatePurchaseReceival(PurchaseReceivalModel purchaseReceival, out string message);
        // Contact must be present
        // Can't update if it's confirmed
        bool ValidateUpdatePurchaseReceival(PurchaseReceivalModel purchaseReceival, out string message);
        // Can't destroy if it's confirmed
        bool ValidateDeletePurchaseReceival(PurchaseReceivalModel purchaseReceival, out string message);
        // PurchaseReceivalDetail.count != 0
        bool ValidateConfirmPurchaseReceival(PurchaseReceivalModel purchaseReceival, IPurchaseReceivalRepository _purchaseReceivalRepository, out string message);
        // Can't unconfirm if item.ready < 0
        bool ValidateUnconfirmPurchaseReceival(PurchaseReceivalModel purchaseReceival, IPurchaseReceivalRepository _purchaseReceivalRepository, IItemRepository _itemRepository, out string message);

        /// <summary>
        /// Purchase Receival Detail -- Children
        /// </summary>
        /// 
        List<PurchaseReceivalDetailModel> GetPurchaseReceivalDetailList(int purchaseReceivalId, IPurchaseReceivalRepository _purchaseReceivalRepository);
        PurchaseReceivalDetailModel GetPurchaseReceivalDetail(int purchaseReceivalDetailId, IPurchaseReceivalRepository _purchaseReceivalRepository);
        ResponseModel CreatePurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, IPurchaseReceivalRepository _purchaseReceivalRepository, IPurchaseOrderRepository _purchaseOrderRepository);

        ResponseModel DeletePurchaseReceivalDetail(int purchaseReceivalDetailId, IPurchaseReceivalRepository _purchaseReceivalRepository);
        ResponseModel DeletePurchaseReceivalDetailByPurchaseReceivalId(int purchaseReceivalId, IPurchaseReceivalRepository _purchaseReceivalRepository);
        ResponseModel UpdatePurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, IPurchaseReceivalRepository _purchaseReceivalRepository, IPurchaseOrderRepository _purchaseOrderRepository);
        ResponseModel ConfirmPurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, IPurchaseReceivalRepository _purchaseReceivalRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository);
        ResponseModel UnconfirmPurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, IPurchaseReceivalRepository _purchaseReceivalRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository);

        // PurchaseOrderDetailId must be present
        // Contact must be present
        // PurchaseOrderDetailId belong to the Contact
        // Quantity > 0
        // Quantity <= PendingReceival of the selected PurchaseOrderDetail
        // Unique PurchaseOrderDetailId in a given PurchaseReceival
        // PurchaseOrderDetail belongs to the same contact
        // PendingReceival in the given PurchaseOrderDetail > 0
        // PurchaseOrderDetail.IsConfirmed == true
        bool ValidateCreatePurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, IPurchaseReceivalRepository _purchaseReceivalRepository, IPurchaseOrderRepository _purchaseOrderRepository, out string message);
        bool ValidateUpdatePurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, IPurchaseReceivalRepository _purchaseReceivalRepository, IPurchaseOrderRepository _purchaseOrderRepository, out string message);
        // Can't be destroyed if it is confirmed
        bool ValidateDeletePurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, out string message);
        bool ValidateConfirmPurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, out string message);
        // Can't unconfirm if associate item.pendingReceival will be changed to < 0 after unconfirm
        // Can't unconfirm if quantity of item.ready will be changed to < 0 after unconfirm
        bool ValidateUnconfirmPurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, IPurchaseReceivalRepository _purchaseReceivalRepository, IItemRepository _itemRepository, out string message);

    }
}