using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockControl.Models;

namespace StockControl.Repository
{
    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
    {

        List<PurchaseOrderModel> GetPurchaseOrderList();
        PurchaseOrderModel GetPurchaseOrder(int Id);
        List<PurchaseOrderModel> GetPurchaseOrderByContactId(int contactId);
        PurchaseOrder CreatePurchaseOrder(PurchaseOrder purchaseOrder);
        void DeletePurchaseOrder(int purchaseOrderId);
        PurchaseOrder UpdatePurchaseOrder(PurchaseOrder purchaseOrder);

        List<PurchaseOrderDetailModel> GetPurchaseOrderDetailList(int purchaseOrderId);
        PurchaseOrderDetailModel GetPurchaseOrderDetail(int purchaseOrderDetailId);
        PurchaseOrderDetail CreatePurchaseOrderDetail(PurchaseOrderDetail purchaseOrderDetail);
        void DeletePurchaseOrderDetail(int purchaseOrderDetailId);
        void DeletePurchaseOrderDetailByPurchaseOrderId(int purchaseOrderId);
        PurchaseOrderDetail UpdatePurchaseOrderDetail(PurchaseOrderDetail purchaseOrderDetail);

    }
}