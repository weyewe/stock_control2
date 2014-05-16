using StockControl.Models;
using StockControl.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Service
{
    public interface IPurchaseOrderService
    {
        /// <summary>
        /// Purchase Order -- Parent
        /// </summary>
        List<PurchaseOrderModel> GetPurchaseOrderList(IPurchaseOrderRepository _purchaseOrderRepository);
        PurchaseOrderModel GetPurchaseOrder(int orderId, IPurchaseOrderRepository _purchaseOrderRepository);
        ResponseModel CreatePurchaseOrder(PurchaseOrderModel purchaseOrder, IPurchaseOrderRepository _purchaseOrderRepository);
        ResponseModel DeletePurchaseOrder(int purchaseOrderId, IPurchaseOrderRepository _purchaseOrderRepository);
        ResponseModel UpdatePurchaseOrder(PurchaseOrderModel purchaseOrder, IPurchaseOrderRepository _purchaseOrderRepository);
        ResponseModel ConfirmPurchaseOrder(PurchaseOrderModel purchaseOrder, IPurchaseOrderRepository _purchaseOrderRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository);

        ResponseModel UnconfirmPurchaseOrder(PurchaseOrderModel purchaseOrder, IPurchaseOrderRepository _purchaseOrderRepository, IItemRepository _itemRepository);

        // Contact must be present
        bool ValidateCreatePurchaseOrder(PurchaseOrderModel purchaseOrder, out string message);
        // Contact must be present
        // Can't update if it's confirmed
        bool ValidateUpdatePurchaseOrder(PurchaseOrderModel purchaseOrder, out string message);
        // Can't be destroyed if it's confirmed
        bool ValidateDeletePurchaseOrder(PurchaseOrderModel purchaseOrder, out string message);
        // PurchaseOrder.count != 0
        bool ValidateConfirmPurchaseOrder(PurchaseOrderModel purchaseOrder, IPurchaseOrderRepository _purchaseOrderRepository, out string message);
        // Can't unconfirm if item.PendingDelivery < 0
        bool ValidateUnconfirmPurchaseOrder(PurchaseOrderModel purchaseOrder, IPurchaseOrderRepository _purchaseOrderRepository, IItemRepository _itemRepository, out string message);

        /// <summary>
        /// Purchase Order Detail -- Children
        /// </summary>
        List<PurchaseOrderDetailModel> GetPurchaseOrderDetailList(int purchaseOrderId, IPurchaseOrderRepository _purchaseOrderRepository);
        PurchaseOrderDetailModel GetPurchaseOrderDetail(int purchaseOrderDetailId, IPurchaseOrderRepository _purchaseOrderRepository);
        ResponseModel CreatePurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, IPurchaseOrderRepository _purchaseOrderRepository, IItemRepository _itemRepository);
        ResponseModel DeletePurchaseOrderDetail(int purchaseOrderDetailId, IPurchaseOrderRepository _purchaseOrderRepository);
        ResponseModel DeletePurchaseOrderDetailByPurchaseOrderId(int purchaseOrderId, IPurchaseOrderRepository _purchaseOrderRepository);
        ResponseModel UpdatePurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, IPurchaseOrderRepository _purchaseOrderRepository, IItemRepository _itemRepository);
        ResponseModel ConfirmPurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, IPurchaseOrderRepository _purchaseOrderRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository);
        ResponseModel UnconfirmPurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, IPurchaseOrderRepository _purchaseOrderRepository, IPurchaseReceivalRepository _purchaseReceivalRepository,
                                                    IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository);

        // Item must be present
        // PurchaseOrderId must be present
        // PurchaseOrderDetail must be unique
        // Quantity >= 0
        bool ValidateCreatePurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, IPurchaseOrderRepository _purchaseOrderRepository, IItemRepository _itemRepository, out string message);
        // Item must be present
        // PurchaseOrderId must be present
        // PurchaseOrderDetail must be unique
        // Quantity >= 0
        // Not Confirmed
        bool ValidateUpdatePurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, IPurchaseOrderRepository _purchaseOrderRepository, IItemRepository _itemRepository, out string message);
        // Can't be destroyed if it is confirmed
        bool ValidateDeletePurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, out string message);
        bool ValidateConfirmPurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, out string message);
        // Can't unconfirm if item.PendingReceival will be changed to < 0 after unconfirm
        // Can't unconfirm if there are confirmed associated PurchaseReceivalDetail. Must unconfirm all associated PurchaseReceivalDetail
        bool ValidateUnconfirmPurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, IPurchaseOrderRepository _purchaseOrderRepository, IPurchaseReceivalRepository _purchaseReceivalRepository, IItemRepository _itemRepository, out string message);

    }
}