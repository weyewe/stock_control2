using StockControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace StockControl.Repository
{
    public class DeliveryOrderRepository : EfRepository<DeliveryOrder>, IDeliveryOrderRepository
    {

        /// <summary>
        /// Get all delivery orders from Database.
        /// </summary>
        /// <returns>All delivery orders</returns>
        public List<DeliveryOrderModel> GetDeliveryOrderList()
        {
             using (var db = GetContext())
            {
                IQueryable<DeliveryOrderModel> dom = (from d in db.DeliveryOrders
                                               select new DeliveryOrderModel
                                               {
                                                   Id = d.Id,
                                                   ContactId = d.ContactId,
                                                   Code = d.Code,
                                                   DeliveryDate = d.DeliveryDate,
                                                   IsConfirmed = d.IsConfirmed,
                                                   IsDeleted = d.IsDeleted,
                                                   CreatedAt = d.CreatedAt,
                                                   UpdatedAt = d.UpdatedAt,
                                                   DeletedAt = d.DeletedAt
                                               }).AsQueryable();

                return dom.ToList();
            }
        }

        /// <summary>
        /// Get delivery order from Database.
        /// </summary>
        /// <param name="deliveryOrderId">Id of the delivery order</param>
        /// <returns>The delivery order model</returns>
        public DeliveryOrderModel GetDeliveryOrder(int deliveryOrderId)
        {
            using (var db = GetContext())
            {
                DeliveryOrderModel dom = (from d in db.DeliveryOrders
                                                      where d.IsDeleted == false && d.Id == deliveryOrderId
                                                      select new DeliveryOrderModel
                                                      {
                                                          Id = d.Id,
                                                          ContactId = d.ContactId,
                                                          Code = d.Code,
                                                          DeliveryDate = d.DeliveryDate,
                                                          IsConfirmed = d.IsConfirmed,
                                                          IsDeleted = d.IsDeleted,
                                                          CreatedAt = d.CreatedAt,
                                                          UpdatedAt = d.UpdatedAt,
                                                          DeletedAt = d.DeletedAt
                                                      }).FirstOrDefault();

                return dom;
            }
        }

        /// <summary>
        /// Create a new delivery order.
        /// </summary>
        /// <param name="deliveryOrder">An object DeliveryOrder</param>
        /// <returns>The new delivery order</returns>
        public DeliveryOrder CreateDeliveryOrder(DeliveryOrder deliveryOrder)
        {
            DeliveryOrder newdeliveryorder = new DeliveryOrder();
            newdeliveryorder.Id = deliveryOrder.Id;
            newdeliveryorder.ContactId = deliveryOrder.ContactId;
            newdeliveryorder.Code = deliveryOrder.Code;
            newdeliveryorder.DeliveryDate = deliveryOrder.DeliveryDate;
            newdeliveryorder.IsConfirmed = deliveryOrder.IsConfirmed;
            newdeliveryorder.IsDeleted = deliveryOrder.IsDeleted;
            newdeliveryorder.CreatedAt = deliveryOrder.CreatedAt;
            newdeliveryorder.UpdatedAt = deliveryOrder.UpdatedAt;
            newdeliveryorder.DeletedAt = deliveryOrder.DeletedAt;
            return Create(newdeliveryorder);
        }

        /// <summary>
        /// Delete a certain deliveryOrder
        /// </summary>
        /// <param name="id">DeliveryOrder Id</param>
        public void DeleteDeliveryOrder(int id)
        {
            DeliveryOrder d = Find(x => x.Id == id);
            if (d != null)
            {
                d.IsDeleted = true;
                d.DeletedAt = DateTime.Now;
                Update(d);
            }
        }

        /// <summary>
        /// Update a delivery order.
        /// </summary>
        /// <param name="deliveryOrder">An object DeliveryOrder</param>
        /// <returns>The updated delivery order</returns>
        public DeliveryOrder UpdateDeliveryOrder(DeliveryOrder deliveryOrder)
        {
            DeliveryOrder d = Find(x => x.Id == deliveryOrder.Id);
            if (d != null)
            {
                d.Id = deliveryOrder.Id;
                d.ContactId = deliveryOrder.ContactId;
                d.Code = deliveryOrder.Code;
                d.DeliveryDate = deliveryOrder.DeliveryDate;
                d.IsConfirmed = deliveryOrder.IsConfirmed;
                d.IsDeleted = deliveryOrder.IsDeleted;
                d.CreatedAt = deliveryOrder.CreatedAt;
                d.UpdatedAt = deliveryOrder.UpdatedAt;
                d.DeletedAt = deliveryOrder.DeletedAt;

                Update(d);
                return d;
            }
            return d;
        }

        /// <summary>
        /// Get all delivery order details from Database.
        /// </summary>
        /// <returns>All delivery order details</returns>
        public List<DeliveryOrderDetailModel> GetDeliveryOrderDetailList(int deliveryOrderId)
        {
            using (var db = GetContext())
            {
                IQueryable<DeliveryOrderDetailModel> dodm = (from dod in db.DeliveryOrderDetails
                                                      join d in db.DeliveryOrders on dod.DeliveryOrderId equals d.Id
                                                      select new DeliveryOrderDetailModel
                                                      {
                                                          Id = dod.Id,
                                                          DeliveryOrderId = dod.DeliveryOrderId,
                                                          Code = dod.Code,
                                                          Quantity = dod.Quantity,
                                                          ItemId = dod.ItemId,
                                                          SalesOrderDetailId = dod.SalesOrderDetailId,
                                                          IsConfirmed = dod.IsConfirmed,
                                                          IsDeleted = d.IsDeleted,
                                                          CreatedAt = d.CreatedAt,
                                                          UpdatedAt = d.UpdatedAt,
                                                          DeletedAt = d.DeletedAt
                                                      }).AsQueryable();

                return dodm.ToList();
            }
        }

        /// <summary>
        /// Get specific delivery order details from Database.
        /// </summary>
        /// <param name="deliveryOrderDetailId">DeliveryOrderDetail Id</param>
        /// <returns>A delivery order detail</returns>
        public DeliveryOrderDetailModel GetDeliveryOrderDetail(int deliveryOrderDetailId)
        {
            using (var db = GetContext())
            {
                DeliveryOrderDetailModel dodm = (from dod in db.DeliveryOrderDetails
                                                             where dod.Id == deliveryOrderDetailId
                                                             select new DeliveryOrderDetailModel
                                                             {
                                                                 Id = dod.Id,
                                                                 DeliveryOrderId = dod.DeliveryOrderId,
                                                                 Code = dod.Code,
                                                                 Quantity = dod.Quantity,
                                                                 ItemId = dod.ItemId,
                                                                 SalesOrderDetailId = dod.SalesOrderDetailId,
                                                                 IsConfirmed = dod.IsConfirmed,
                                                                 IsDeleted = dod.IsDeleted,
                                                                 CreatedAt = dod.CreatedAt,
                                                                 UpdatedAt = dod.UpdatedAt,
                                                                 DeletedAt = dod.DeletedAt
                                                             }).FirstOrDefault();

                return dodm;
            }
        }

        /// <summary>
        /// Create a new delivery order detail.
        /// </summary>
        /// <param name="deliveryOrderDetail">An object DeliveryOrderDetail</param>
        /// <returns>The new delivery order detail</returns>
        public DeliveryOrderDetail CreateDeliveryOrderDetail(DeliveryOrderDetail deliveryOrderDetail)
        {

            using (var db = GetContext())
            {

                DeliveryOrderDetail newdod = new DeliveryOrderDetail();
                newdod.Id = deliveryOrderDetail.Id;
                newdod.DeliveryOrderId = deliveryOrderDetail.DeliveryOrderId;
                newdod.Code = deliveryOrderDetail.Code;
                newdod.Quantity = deliveryOrderDetail.Quantity;
                newdod.ItemId = deliveryOrderDetail.ItemId;
                newdod.SalesOrderDetailId = deliveryOrderDetail.SalesOrderDetailId;
                newdod.IsConfirmed = deliveryOrderDetail.IsConfirmed;
                newdod.IsDeleted = deliveryOrderDetail.IsDeleted;
                newdod.CreatedAt = deliveryOrderDetail.CreatedAt;
                newdod.UpdatedAt = deliveryOrderDetail.UpdatedAt;
                newdod.DeletedAt = deliveryOrderDetail.DeletedAt;

                newdod = db.DeliveryOrderDetails.Add(newdod);
                db.SaveChanges();

                return newdod;
            }
        }

        /// <summary>
        /// Delete a certain delivery order detail
        /// </summary>
        /// <param name="deliveryOrderDetailId">DeliveryOrderDetail Id</param>
        public void DeleteDeliveryOrderDetail(int deliveryOrderDetailId)
        {
            using (var db = GetContext())
            {
                DeliveryOrderDetail dod = (from d in db.DeliveryOrderDetails where d.Id == deliveryOrderDetailId select d).FirstOrDefault();
                if (dod != null)
                {
                    dod.IsDeleted = true;
                    dod.DeletedAt = DateTime.Now;

                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Delete delivery order details that connects to the parent DeliveryOrder 
        /// </summary>
        /// <param name="deliveryOrderId">Id of the parent DeliveryOrder</param>
        public void DeleteDeliveryOrderDetailByDeliveryOrderId(int deliveryOrderId)
        {
            using (var db = GetContext())
            {
                List<DeliveryOrderDetail> dod = (from d in db.DeliveryOrderDetails where d.DeliveryOrderId == deliveryOrderId select d).ToList();
                
                if (dod != null)
                {
                    foreach (var eachdetail in dod)
                    {
                        var updatedod = (from d in db.DeliveryOrderDetails where d.Id == eachdetail.Id select d).FirstOrDefault();
                        if (updatedod != null)
                        {
                            updatedod.IsDeleted = true;
                            updatedod.DeletedAt = DateTime.Now;

                            db.SaveChanges();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update confirmation of all delivery order details that connects to the parent DeliveryOrder 
        /// </summary>
        /// <param name="deliveryOrderId">Id of the parent DeliveryOrder</param>
        /// <param name="IsConfirmed">Setter for the Confirm / Unconfirm parameter</param>
        public void UpdateConfirmationDeliveryOrderDetailByDeliveryOrderId(int deliveryOrderId, bool IsConfirmed)
        {
            using (var db = GetContext())
            {
                List<DeliveryOrderDetail> dod = (from d in db.DeliveryOrderDetails where d.DeliveryOrderId == deliveryOrderId select d).ToList();

                if (dod != null)
                {
                    foreach (var eachdetail in dod)
                    {
                        var updatedod = (from d in db.DeliveryOrderDetails where d.Id == eachdetail.Id select d).FirstOrDefault();
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

        /// <summary>
        /// Update a delivery order detail.
        /// </summary>
        /// <param name="deliveryOrderDetail">An object DeliveryOrderDetail</param>
        /// <returns>The updated delivery order detail</returns>
        public DeliveryOrderDetail UpdateDeliveryOrderDetail(DeliveryOrderDetail deliveryOrderDetail)
        {
            using (var db = GetContext())
            {
                DeliveryOrderDetail dod = (from d in db.DeliveryOrderDetails where d.Id == deliveryOrderDetail.Id select d).FirstOrDefault();
                if (dod != null)
                {
                    dod.Id = deliveryOrderDetail.Id;
                    dod.DeliveryOrderId = deliveryOrderDetail.DeliveryOrderId;
                    dod.Code = deliveryOrderDetail.Code;
                    dod.Quantity = deliveryOrderDetail.Quantity;
                    dod.ItemId = deliveryOrderDetail.ItemId;
                    dod.SalesOrderDetailId = deliveryOrderDetail.SalesOrderDetailId;
                    dod.IsConfirmed = deliveryOrderDetail.IsConfirmed;
                    dod.IsDeleted = deliveryOrderDetail.IsDeleted;
                    dod.CreatedAt = deliveryOrderDetail.CreatedAt;
                    dod.UpdatedAt = deliveryOrderDetail.UpdatedAt;
                    dod.DeletedAt = deliveryOrderDetail.DeletedAt;

                    db.SaveChanges();
                    return dod;
                }
                return dod;
            }
        }

        public List<DeliveryOrderModel> GetDeliveryOrderByContactId(int contactId)
        {
            using (var db = GetContext())
            {
                IQueryable<DeliveryOrderModel> dom = (from p in db.DeliveryOrders
                                                      where p.IsDeleted == false && p.ContactId == contactId
                                                      select new DeliveryOrderModel
                                                   {
                                                       Id = p.Id,
                                                       ContactId = p.ContactId,
                                                       Code = p.Code,
                                                       DeliveryDate = p.DeliveryDate,
                                                       IsConfirmed = p.IsConfirmed,
                                                       IsDeleted = p.IsDeleted,
                                                       CreatedAt = p.CreatedAt,
                                                       UpdatedAt = p.UpdatedAt,
                                                       DeletedAt = p.DeletedAt
                                                   }).AsQueryable();

                return dom.ToList();
            }
        }
    }
}