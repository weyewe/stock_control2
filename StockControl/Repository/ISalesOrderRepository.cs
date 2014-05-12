using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockControl.Models;

namespace StockControl.Repository
{
    public interface ISalesOrderRepository : IRepository<SalesOrder>
    {

        List<SalesOrderModel> GetSalesOrderList();
        List<SalesOrderModel> GetSalesOrderByContactId(int contactId);
        SalesOrderModel GetSalesOrder(int id);
        SalesOrder CreateSalesOrder(SalesOrder salesOrder);
        void DeleteSalesOrder(int salesOrderId);
        SalesOrder UpdateSalesOrder(SalesOrder salesOrder);

        List<SalesOrderDetailModel> GetSalesOrderDetailList(int salesOrderId);
        SalesOrderDetailModel GetSalesOrderDetail(int SalesOrderDetailId);
        SalesOrderDetail CreateSalesOrderDetail(SalesOrderDetail SalesOrderDetail);
        void DeleteSalesOrderDetail(int SalesOrderDetailId);
        void DeleteSalesOrderDetailBySalesOrderId(int SalesOrderId);
        SalesOrderDetail UpdateSalesOrderDetail(SalesOrderDetail SalesOrderDetail);

    }
}