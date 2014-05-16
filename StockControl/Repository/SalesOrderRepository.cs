using StockControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace StockControl.Repository
{
    public class SalesOrderRepository : EfRepository<SalesOrder>, ISalesOrderRepository
    {

        /*
         * GET
        */

        /// <summary>
        /// Get all sales orders from Database.
        /// </summary>
        /// <returns>All sales orders</returns>
        public List<SalesOrderModel> GetSalesOrderList()
        {
             using (var db = GetContext())
            {
                IQueryable<SalesOrderModel> som = (from s in db.SalesOrders
                                                   where !s.IsDeleted
                                                   select new SalesOrderModel
                                                   {
                                                       Id = s.Id,
                                                       ContactId = s.ContactId,
                                                       Code = s.Code,
                                                       SalesDate = s.SalesDate,
                                                       IsConfirmed = s.IsConfirmed,
                                                       IsDeleted = s.IsDeleted,
                                                       CreatedAt = s.CreatedAt,
                                                       UpdatedAt = s.UpdatedAt,
                                                       DeletedAt = s.DeletedAt
                                                   }).AsQueryable();

                return som.ToList();
            }
        }

        /// <summary>
        /// Get sales order from Database.
        /// </summary>
        /// <param name="Id">Id of the sales order</param>
        /// <returns>A sales order</returns>
        public SalesOrderModel GetSalesOrder(int Id)
        {
            using (var db = GetContext())
            {
                SalesOrderModel som = (from s in db.SalesOrders
                                        where !s.IsDeleted && s.Id == Id
                                        select new SalesOrderModel
                                        {
                                            Id = s.Id,
                                            ContactId = s.ContactId,
                                            Code = s.Code,
                                            SalesDate = s.SalesDate,
                                            IsConfirmed = s.IsConfirmed,
                                            IsDeleted = s.IsDeleted,
                                            CreatedAt = s.CreatedAt,
                                            UpdatedAt = s.UpdatedAt,
                                            DeletedAt = s.DeletedAt
                                        }).FirstOrDefault();

                return som;
            }
        }


        /// <summary>
        /// Get sales order from Database.
        /// </summary>
        /// <param name="contactId">Id of the contact</param>
        /// <returns>A sales order</returns>
        public List<SalesOrderModel> GetSalesOrderByContactId(int contactId)
        {
            using (var db = GetContext())
            {
                IQueryable<SalesOrderModel> som = (from s in db.SalesOrders
                                                   where !s.IsDeleted && s.ContactId == contactId
                                                   select new SalesOrderModel
                                                   {
                                                       Id = s.Id,
                                                       ContactId = s.ContactId,
                                                       Code = s.Code,
                                                       SalesDate = s.SalesDate,
                                                       IsConfirmed = s.IsConfirmed,
                                                       IsDeleted = s.IsDeleted,
                                                       CreatedAt = s.CreatedAt,
                                                       UpdatedAt = s.UpdatedAt,
                                                       DeletedAt = s.DeletedAt
                                                   }).AsQueryable();

                return som.ToList();
            }
        }

        /*
         * CREATE
         */

        /// <summary>
        /// Create a new sales order.
        /// </summary>
        /// <param name="salesOrder">An object SalesOrder</param>
        /// <returns>The new sales order</returns>
        public SalesOrder CreateSalesOrder(SalesOrder salesOrder)
        {
            SalesOrder newsalesorder = new SalesOrder();
            newsalesorder.Id = salesOrder.Id;
            newsalesorder.ContactId = salesOrder.ContactId;
            newsalesorder.Code = salesOrder.Code;
            newsalesorder.SalesDate = salesOrder.SalesDate;
            newsalesorder.IsConfirmed = salesOrder.IsConfirmed;
            newsalesorder.IsDeleted = salesOrder.IsDeleted;
            newsalesorder.CreatedAt = salesOrder.CreatedAt;
            newsalesorder.UpdatedAt = salesOrder.UpdatedAt;
            newsalesorder.DeletedAt = salesOrder.DeletedAt;
            return Create(newsalesorder);
        }

        /*
         * SALES
         */

        /// <summary>
        /// Delete a certain sales order.
        /// </summary>
        /// <param name="id">Id of the sales order</param>
        public void DeleteSalesOrder(int id)
        {
            SalesOrder d = Find(x => x.Id == id && !x.IsDeleted);
            if (d != null)
            {
                d.IsDeleted = true;
                d.DeletedAt = DateTime.Now;
                Update(d);
            }
        }

        /*
         * UPDATE
         */

        /// <summary>
        /// Update a sales order.
        /// </summary>
        /// <param name="salesOrder">An object SalesOrder</param>
        /// <returns>The updated sales order</returns>
        public SalesOrder UpdateSalesOrder(SalesOrder salesOrder)
        {
            SalesOrder s = Find(x => x.Id == salesOrder.Id && !x.IsDeleted);
            if (s != null)
            {
                s.Id = salesOrder.Id;
                s.ContactId = salesOrder.ContactId;
                s.Code = salesOrder.Code;
                s.SalesDate = salesOrder.SalesDate;
                s.IsConfirmed = salesOrder.IsConfirmed;
                s.IsDeleted = salesOrder.IsDeleted;
                s.CreatedAt = salesOrder.CreatedAt;
                s.UpdatedAt = salesOrder.UpdatedAt;
                s.DeletedAt = salesOrder.DeletedAt;

                Update(s);
                return s;
            }
            return s;
        }

        /*
         * GET DETAIL
         */

        /// <summary>
        /// Get all sales order details from Database.
        /// </summary>
        /// <param name="salesOrderId">Id of the sales order</param>
        /// <returns>All sales order details</returns>
        public List<SalesOrderDetailModel> GetSalesOrderDetailList(int salesOrderId)
        {
            using (var db = GetContext())
            {
                IQueryable<SalesOrderDetailModel> sodm = (from sod in db.SalesOrderDetails
                                                      where !sod.IsDeleted && sod.SalesOrderId == salesOrderId
                                                      select new SalesOrderDetailModel
                                                      {
                                                          Id = sod.Id,
                                                          SalesOrderId = sod.SalesOrderId,
                                                          Code = sod.Code,
                                                          Quantity = sod.Quantity,
                                                          ItemId = sod.ItemId,
                                                          IsConfirmed = sod.IsConfirmed,
                                                          IsFulfilled = sod.IsFulfilled,
                                                          IsDeleted = sod.IsDeleted,
                                                          CreatedAt = sod.CreatedAt,
                                                          UpdatedAt = sod.UpdatedAt,
                                                          DeletedAt = sod.DeletedAt
                                                      }).AsQueryable();

                return sodm.ToList();
            }
        }

        /// <summary>
        /// Get specific sales order details from Database.
        /// </summary>
        /// <param name="salesOrderDetailId">Id of the sales order detail</param>
        /// <returns>A sales order detail</returns>
        public SalesOrderDetailModel GetSalesOrderDetail(int salesOrderDetailId)
        {
            using (var db = GetContext())
            {
                SalesOrderDetailModel sodm = (from sod in db.SalesOrderDetails
                                                where sod.Id == salesOrderDetailId && !sod.IsDeleted
                                                select new SalesOrderDetailModel
                                                {
                                                    Id = sod.Id,
                                                    SalesOrderId = sod.SalesOrderId,
                                                    Code = sod.Code,
                                                    Quantity = sod.Quantity,
                                                    ItemId = sod.ItemId,
                                                    IsConfirmed = sod.IsConfirmed,
                                                    IsFulfilled = sod.IsFulfilled,
                                                    IsDeleted = sod.IsDeleted,
                                                    CreatedAt = sod.CreatedAt,
                                                    UpdatedAt = sod.UpdatedAt,
                                                    DeletedAt = sod.DeletedAt
                                                }).FirstOrDefault();

                return sodm;
            }
        }

        /*
         * CREATE DETAIL
         */

        /// <summary>
        /// Create a new sales order detail.
        /// </summary>
        /// <param name="salesOrderDetail">An object SalesOrderDetail</param>
        /// <returns>The new sales order detail</returns>
        public SalesOrderDetail CreateSalesOrderDetail(SalesOrderDetail salesOrderDetail)
        {

            using (var db = GetContext())
            {

                SalesOrderDetail newsod = new SalesOrderDetail();
                newsod.Id = salesOrderDetail.Id;
                newsod.SalesOrderId = salesOrderDetail.SalesOrderId;
                newsod.Code = salesOrderDetail.Code;
                newsod.Quantity = salesOrderDetail.Quantity;
                newsod.ItemId = salesOrderDetail.ItemId;
                newsod.IsConfirmed = salesOrderDetail.IsConfirmed;
                newsod.IsFulfilled = salesOrderDetail.IsFulfilled;
                newsod.IsDeleted = salesOrderDetail.IsDeleted;
                newsod.CreatedAt = salesOrderDetail.CreatedAt;
                newsod.UpdatedAt = salesOrderDetail.UpdatedAt;
                newsod.DeletedAt = salesOrderDetail.DeletedAt;

                newsod = db.SalesOrderDetails.Add(newsod);
                db.SaveChanges();

                return newsod;
            }
        }

        /*
         * DELETE DETAIL
         */

        /// <summary>
        /// Delete a certain sales order detail
        /// </summary>
        /// <param name="salesOrderDetailId">Id of the sales order detail</param>
        public void DeleteSalesOrderDetail(int salesOrderDetailId)
        {
            using (var db = GetContext())
            {
                SalesOrderDetail sod = (from s in db.SalesOrderDetails
                                        where s.Id == salesOrderDetailId && !s.IsDeleted
                                        select s).FirstOrDefault();
                if (sod != null)
                {
                    sod.IsDeleted = true;
                    sod.DeletedAt = DateTime.Now;

                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Delete sales order details that connects to the parent sales order 
        /// </summary>
        /// <param name="salesOrderId">Id of the parent sales order</param>
        public void DeleteSalesOrderDetailBySalesOrderId(int salesOrderId)
        {
            using (var db = GetContext())
            {
                List<SalesOrderDetail> sod = (from s in db.SalesOrderDetails
                                              where s.SalesOrderId == salesOrderId && !s.IsDeleted
                                              select s).ToList();
                
                if (sod != null)
                {
                    foreach (var eachdetail in sod)
                    {
                        var updatesod = (from s in db.SalesOrderDetails
                                         where s.Id == eachdetail.Id && !s.IsDeleted
                                         select s).FirstOrDefault();
                        if (updatesod != null)
                        {
                            updatesod.IsDeleted = true;
                            updatesod.DeletedAt = DateTime.Now;

                            db.SaveChanges();
                        }
                    }
                }
            }
        }

        /*
         * UPDATE DETAIL
         */

        /// <summary>
        /// Update a sales order detail.
        /// </summary>
        /// <param name="salesOrderDetail">An object SalesOrderDetail</param>
        /// <returns>The updated sales order detail</returns>
        public SalesOrderDetail UpdateSalesOrderDetail(SalesOrderDetail salesOrderDetail)
        {
            using (var db = GetContext())
            {
                SalesOrderDetail sod = (from s in db.SalesOrderDetails
                                        where s.Id == salesOrderDetail.Id && !s.IsDeleted
                                        select s).FirstOrDefault();
                if (sod != null)
                {
                    sod.Id = salesOrderDetail.Id;
                    sod.SalesOrderId = salesOrderDetail.SalesOrderId;
                    sod.Code = salesOrderDetail.Code;
                    sod.Quantity = salesOrderDetail.Quantity;
                    sod.ItemId = salesOrderDetail.ItemId;
                    sod.IsConfirmed = salesOrderDetail.IsConfirmed;
                    sod.IsFulfilled = salesOrderDetail.IsFulfilled;
                    sod.IsDeleted = salesOrderDetail.IsDeleted;
                    sod.CreatedAt = salesOrderDetail.CreatedAt;
                    sod.UpdatedAt = salesOrderDetail.UpdatedAt;
                    sod.DeletedAt = salesOrderDetail.DeletedAt;

                    db.SaveChanges();
                }
                return sod;
            }
        }

        /// <summary>
        /// Update confirmation of all sales order details that connects to the parent SalesOrder 
        /// </summary>
        /// <param name="salesOrderId">Id of the parent sales order</param>
        /// <param name="IsConfirmed">Setter for the Confirm / Unconfirm parameter</param>
        public void UpdateConfirmationSalesOrderDetailBySalesOrderId(int salesOrderId, bool IsConfirmed)
        {
            using (var db = GetContext())
            {
                List<SalesOrderDetail> dod = (from d in db.SalesOrderDetails
                                                 where d.SalesOrderId == salesOrderId && !d.IsDeleted
                                                 select d).ToList();

                if (dod != null)
                {
                    foreach (var eachdetail in dod)
                    {
                        var updatedod = (from d in db.SalesOrderDetails
                                         where d.Id == eachdetail.Id && !d.IsDeleted
                                         select d).FirstOrDefault();
                        if (updatedod != null)
                        {
                            updatedod.IsConfirmed = IsConfirmed;
                            updatedod.UpdatedAt = DateTime.Now;

                            db.SaveChanges();
                        }
                    }
                }
            }
        }
    }

}