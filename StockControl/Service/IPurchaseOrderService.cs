using StockControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Service
{
    public interface IPurchaseOrderService
    {
        /// <summary>
        /// Delivery Order -- Parent
        /// </summary>
        /// 
        List<PurchaseOrderModel> GetPurchaseOrderList();
        PurchaseOrderModel GetPurchaserder(int purchaseOrderId);
        PurchaseOrderModel CreatePurchaseOrder(PurchaseOrderModel purchaseOrder);
        void DeletePurchaseOrder(int purchaseOrderId);
        PurchaseOrderModel UpdatePurchaseOrder(PurchaseOrderModel purchaseOrder);
        bool ConfirmPurchaseOrder(PurchaseOrderModel purchaseOrder);
        bool UnconfirmPurchaseOrder(PurchaseOrderModel purchaseOrder);

        // Contact must be present
        bool ValidateCreatePurchaseOrder(PurchaseOrderModel purchaseOrder);
        // Contact must be present
        // Can't update if it's confirmed
        bool ValidateUpdatePurchaseOrder(PurchaseOrderModel purchaseOrder);
        // Can't be destroyed if it's confirmed
        bool ValidateDeletePurchaseOrder(PurchaseOrderModel purchaseOrder);
        // PurchaseOrder.count != 0
        bool ValidateConfirmPurchaseOrder(PurchaseOrderModel purchaseOrder);
        // Can't unconfirm if item.PendingReceival < 0
        bool ValidateUnconfirmPurchaseOrder(PurchaseOrderModel purchaseOrder, ItemService _itemService);

        /// <summary>
        /// Delivery Order Detail -- Children
        /// </summary>
        /// 
        List<PurchaseOrderDetailModel> GetPurchaseOrderDetailList(int purchaseOrderId);
        PurchaseOrderDetailModel GetPurchaseOrderDetail(int purchaseOrderDetailId);
        PurchaseOrderDetailModel CreatePurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail);
        void DeletePurchaseOrderDetail(int purchaseOrderDetailId);
        void DeletePurchaseOrderDetailByPurchaseOrderId(int purchaseOrderId);
        PurchaseOrderDetailModel UpdatePurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail);
        bool ConfirmPurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail);
        bool UnconfirmPurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, ItemService _itemService);

        // Item must be present
        // PurchaseOrderId must be present
        // Quantity >= 0
        bool ValidateCreatePurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail);
        // Item must be present
        // PurchaseOrderId must be present
        // Quantity >= 0
        // Not Confirmed
        bool ValidateUpdatePurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail);
        // Can't be destroyed if it is confirmed
        bool ValidateDeletePurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail);
        // Can't unconfirm if item.PendingReceival will be changed to < 0 after unconfirm
        // Can't unconfirm if there are confirmed associated PurchaseReceivalDetail. Must unconfirm all associated PurchaseReceivalDetail
        bool ValidateUnconfirmPurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, ItemService _itemService, PurchaseReceivalService _purchaseReceivalService);

    }
}