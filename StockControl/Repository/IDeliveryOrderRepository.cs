using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockControl.Models;

namespace StockControl.Repository
{
    public interface IDeliveryOrderRepository : IRepository<DeliveryOrder>
    {

        List<DeliveryOrderModel> GetDeliveryOrderList();
        DeliveryOrderModel GetDeliveryOrder(int deliveryOrderId);
        DeliveryOrder CreateDeliveryOrder(DeliveryOrder deliveryOrder);
        void DeleteDeliveryOrder(int deliveryOrderId);
        DeliveryOrder UpdateDeliveryOrder(DeliveryOrder deliveryOrder);

        List<DeliveryOrderDetailModel> GetDeliveryOrderDetailList(int deliveryOrderId);
        DeliveryOrderDetailModel GetDeliveryOrderDetail(int deliveryOrderDetailId);
        DeliveryOrderDetail CreateDeliveryOrderDetail(DeliveryOrderDetail deliveryOrderDetail);
        void DeleteDeliveryOrderDetail(int deliveryOrderDetailId);
        void DeleteDeliveryOrderDetailByDeliveryOrderId(int deliveryOrderId);
        DeliveryOrderDetail UpdateDeliveryOrderDetail(DeliveryOrderDetail deliveryOrderDetail);
        void UpdateConfirmationDeliveryOrderDetailByDeliveryOrderId(int deliveryOrderId, bool IsConfirmed);

        List<DeliveryOrderModel> GetDeliveryOrderByContactId(int contactId);
    }
}