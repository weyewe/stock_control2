using StockControl.Models;
using StockControl.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Service
{
    public interface ISalesOrderService
    {
        /// <summary>
        /// Sales Order -- Parent
        /// </summary>
        List<SalesOrderModel> GetSalesOrderList(ISalesOrderRepository _salesOrderRepository);
        SalesOrderModel GetSalesOrder(int orderId, ISalesOrderRepository _salesOrderRepository);
        ResponseModel CreateSalesOrder(SalesOrderModel salesOrder, ISalesOrderRepository _salesOrderRepository);
        ResponseModel DeleteSalesOrder(int salesOrderId, ISalesOrderRepository _salesOrderRepository);
        ResponseModel UpdateSalesOrder(SalesOrderModel salesOrder, ISalesOrderRepository _salesOrderRepository);
        ResponseModel ConfirmSalesOrder(SalesOrderModel salesOrder, ISalesOrderRepository _salesOrderRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository);
        ResponseModel UnconfirmSalesOrder(SalesOrderModel salesOrder, ISalesOrderRepository _salesOrderRepository, IItemRepository _itemRepository);

        // Contact must be present
        bool ValidateCreateSalesOrder(SalesOrderModel salesOrder, out string message);
        // Contact must be present
        // Can't update if it's confirmed
        bool ValidateUpdateSalesOrder(SalesOrderModel salesOrder, out string message);
        // Can't be destroyed if it's confirmed
        bool ValidateDeleteSalesOrder(SalesOrderModel salesOrder, out string message);
        // SalesOrder.count != 0
        bool ValidateConfirmSalesOrder(SalesOrderModel salesOrder, ISalesOrderRepository _salesOrderRepository, out string message);
        // Can't unconfirm if item.PendingDelivery < 0
        bool ValidateUnconfirmSalesOrder(SalesOrderModel salesOrder, ISalesOrderRepository _salesOrderRepository, IItemRepository _itemRepository, out string message);

        /// <summary>
        /// Sales Order Detail -- Children
        /// </summary>
        List<SalesOrderDetailModel> GetSalesOrderDetailList(int salesOrderId, ISalesOrderRepository _salesOrderRepository);
        SalesOrderDetailModel GetSalesOrderDetail(int salesOrderDetailId, ISalesOrderRepository _salesOrderRepository);
        ResponseModel CreateSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, ISalesOrderRepository _salesOrderRepository, IItemRepository _itemRepository);
        ResponseModel DeleteSalesOrderDetail(int salesOrderDetailId, ISalesOrderRepository _salesOrderRepository);
        ResponseModel DeleteSalesOrderDetailBySalesOrderId(int salesOrderId, ISalesOrderRepository _salesOrderRepository);
        ResponseModel UpdateSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, ISalesOrderRepository _salesOrderRepository, IItemRepository _itemRepository);
        ResponseModel ConfirmSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, ISalesOrderRepository _salesOrderRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository);
        ResponseModel UnconfirmSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, ISalesOrderRepository _salesOrderRepository, IDeliveryOrderRepository _deliveryOrderRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository);
        // Item must be present
        // SalesOrderId must be present
        // SalesOrderDetail must be unique
        // Quantity >= 0
        bool ValidateCreateSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, ISalesOrderRepository _salesOrderRepository, IItemRepository _itemRepository, out string message);
        // Item must be present
        // SalesOrderId must be present
        // SalesOrderDetail must be unique
        // Quantity >= 0
        // Not Confirmed
        bool ValidateUpdateSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, ISalesOrderRepository _salesOrderRepository, IItemRepository _itemRepository, out string message);
        // Can't be destroyed if it is confirmed
        bool ValidateDeleteSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, out string message);
        bool ValidateConfirmSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, out string message);
        // Can't unconfirm if item.PendingDelivery will be changed to < 0 after unconfirm
        // Can't unconfirm if there are confirmed associated DeliveryOrderDetail. Must unconfirm all associated PurchaseDeliveryDetail
        bool ValidateUnconfirmSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, ISalesOrderRepository _salesOrderRepository, IDeliveryOrderRepository _deliveryOrderRepository, IItemRepository _itemRepository, out string message);

    }
}