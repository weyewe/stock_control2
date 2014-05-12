using StockControl.Models;
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
        /// 
        List<SalesOrderModel> GetSalesOrderList();
        SalesOrderModel CreateSalesOrder(SalesOrderModel salesOrder);
        void DeleteSalesOrder(int salesOrderId);
        SalesOrderModel UpdateSalesOrder(SalesOrderModel salesOrder);
        bool ConfirmSalesOrder(SalesOrderModel salesOrder);
        bool UnconfirmSalesOrder(SalesOrderModel salesOrder);

        // Contact must be present
        bool ValidateCreateSalesOrder(SalesOrderModel salesOrder);
        // Contact must be present
        // Can't update if it's confirmed
        bool ValidateUpdateSalesOrder(SalesOrderModel salesOrder);
        // Can't be destroyed if it's confirmed
        bool ValidateDeleteSalesOrder(SalesOrderModel salesOrder);
        // SalesOrder.count != 0
        bool ValidateConfirmSalesOrder(SalesOrderModel salesOrder);
        // Can't unconfirm if item.PendingDelivery < 0
        bool ValidateUnconfirmSalesOrder(SalesOrderModel salesOrder, ItemService _itemService);

        /// <summary>
        /// Sales Order Detail -- Children
        /// </summary>
        /// 
        List<SalesOrderDetailModel> GetSalesOrderDetailList(int salesOrderId);
        SalesOrderDetailModel GetSalesOrderDetail(int salesOrderDetailId);
        SalesOrderDetailModel CreateSalesOrderDetail(SalesOrderDetailModel salesOrderDetail);
        void DeleteSalesOrderDetail(int salesOrderDetailId);
        void DeleteSalesOrderDetailBySalesOrderId(int salesOrderId);
        SalesOrderDetailModel UpdateSalesOrderDetail(SalesOrderDetailModel salesOrderDetail);
        bool ConfirmSalesOrderDetail(SalesOrderDetailModel salesOrderDetail);
        bool UnconfirmSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, ItemService _itemService);

        // Item must be present
        // SalesOrderId must be present
        // SalesOrderDetail must be unique
        // Quantity >= 0
        bool ValidateCreateSalesOrderDetail(SalesOrderDetailModel salesOrderDetail);
        // Item must be present
        // SalesOrderId must be present
        // SalesOrderDetail must be unique
        // Quantity >= 0
        // Not Confirmed
        bool ValidateUpdateSalesOrderDetail(SalesOrderDetailModel salesOrderDetail);
        // Can't be destroyed if it is confirmed
        bool ValidateDeleteSalesOrderDetail(SalesOrderDetailModel salesOrderDetail);
        // Can't unconfirm if item.PendingDelivery will be changed to < 0 after unconfirm
        // Can't unconfirm if there are confirmed associated DeliveryOrderDetail. Must unconfirm all associated PurchaseDeliveryDetail
        bool ValidateUnconfirmSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, ItemService _itemService, DeliveryOrderService _deliveryOrderService);

    }
}