using StockControl.Models;
using StockControl.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Service
{
    public interface IDeliveryOrderService
    {
        /// <summary>
        /// Delivery Order -- Parent
        /// </summary>
        /// 
        List<DeliveryOrderModel> GetDeliveryOrderList(IDeliveryOrderRepository _deliveryOrderRepository);
        ResponseModel CreateDeliveryOrder(DeliveryOrderModel deliveryOrder, IDeliveryOrderRepository _deliveryOrderRepository);
        ResponseModel DeleteDeliveryOrder(int deliveryOrderId, IDeliveryOrderRepository _deliveryOrderRepository);
        ResponseModel UpdateDeliveryOrder(DeliveryOrderModel deliveryOrder, IDeliveryOrderRepository _deliveryOrderRepository);
        ResponseModel ConfirmDeliveryOrder(DeliveryOrderModel deliveryOrder, IDeliveryOrderRepository _deliveryOrderRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository);
        ResponseModel UnconfirmDeliveryOrder(DeliveryOrderModel deliveryOrder, IDeliveryOrderRepository _deliveryOrderRepository);

        // Contact must be present
        // Delivery date must be present
        bool ValidateCreateDeliveryOrder(DeliveryOrderModel deliveryOrder, out string message);
        // Contact must be present
        // Delivery date must be present
        // IsConfirmed must be false
        bool ValidateUpdateDeliveryOrder(DeliveryOrderModel deliveryOrder, out string message);
        // Can't destroy if it's confirmed
        bool ValidateDeleteDeliveryOrder(DeliveryOrderModel deliveryOrder, out string message);
        // DeliveryOrderDetail.count != 0
        bool ValidateConfirmDeliveryOrder(DeliveryOrderModel deliveryOrder, IDeliveryOrderRepository _deliveryOrderRepository, out string message);
        // Can't unconfirm if item.ready < 0
        bool ValidateUnconfirmDeliveryOrder(DeliveryOrderModel deliveryOrder, out string message);

        /// <summary>
        /// Delivery Order Detail -- Children
        /// </summary>
        /// 
        List<DeliveryOrderDetailModel> GetDeliveryOrderDetailList(int deliveryOrderId, IDeliveryOrderRepository _deliveryOrderRepository);
        DeliveryOrderDetailModel GetDeliveryOrderDetail(int deliveryOrderDetailId, IDeliveryOrderRepository _deliveryOrderRepository);
        ResponseModel CreateDeliveryOrderDetail(DeliveryOrderDetailModel deliveryOrderDetail, IDeliveryOrderRepository _deliveryOrderRepository, ISalesOrderRepository _salesOrderRepository);
        ResponseModel DeleteDeliveryOrderDetail(int deliveryOrderDetailId, IDeliveryOrderRepository _deliveryOrderRepository);
        ResponseModel DeleteDeliveryOrderDetailByDeliveryOrderId(int deliveryOrderId, IDeliveryOrderRepository _deliveryOrderRepository);
        ResponseModel UpdateDeliveryOrderDetail(DeliveryOrderDetailModel deliveryOrderDetail, IDeliveryOrderRepository _deliveryOrderRepository, ISalesOrderRepository _salesOrderRepository);
        ResponseModel ConfirmDeliveryOrderDetail(DeliveryOrderDetailModel deliveryOrderDetail, IDeliveryOrderRepository _deliveryOrderRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository);
        ResponseModel UnconfirmDeliveryOrderDetail(DeliveryOrderDetailModel deliveryOrderDetail, IDeliveryOrderRepository _deliveryOrderRepository, IItemRepository _itemRepository);

        // SalesOrderDetailId must be present
        // Contact must be present
        // SalesOrderDetailId belong to the Contact
        // Quantity > 0
        // Quantity <= PendingDelivery of the selected SalesOrderDetail
        // Unique SalesOrderDetailId in a given DeliveryOrder
        // SalesOrderDetail belongs to the same contact
        // PendingDelivery in the given SalesOrderDetail > 0
        // SalesOrderDetail.IsConfirmed == true
        bool ValidateCreateUpdateDeliveryOrderDetail(DeliveryOrderDetailModel deliveryOrderDetail, IDeliveryOrderRepository _deliveryOrderRepository, ISalesOrderRepository _salesOrderRepository, out string message);
        // Can't be destroyed if it is confirmed
        bool ValidateDeleteDeliveryOrderDetail(DeliveryOrderDetailModel deliveryOrderDetail, out string message);
        // Can't unconfirm if associate item.pendingReceival will be changed to < 0 after unconfirm
        // Can't unconfirm if quantity of item.ready will be changed to < 0 after unconfirm
        bool ValidateUnconfirmDeliveryOrderDetail(DeliveryOrderDetailModel deliveryOrderDetail, IItemRepository _itemRepository, out string message);


    }
}