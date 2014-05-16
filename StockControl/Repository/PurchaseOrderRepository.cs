using StockControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace StockControl.Repository
{
    public class PurchaseOrderRepository : EfRepository<PurchaseOrder>, IPurchaseOrderRepository
    {

        /*
         * GET
         */

        /// <summary>
        /// Get all purchase orders from Database.
        /// </summary>
        /// <returns>All purchase orders</returns>
        public List<PurchaseOrderModel> GetPurchaseOrderList()
        {
             using (var db = GetContext())
            {
                IQueryable<PurchaseOrderModel> pom = (from p in db.PurchaseOrders
                                               where !p.IsDeleted
                                               select new PurchaseOrderModel
                                               {
                                                   Id = p.Id,
                                                   ContactId = p.ContactId,
                                                   Code = p.Code,
                                                   PurchaseDate = p.PurchaseDate,
                                                   IsConfirmed = p.IsConfirmed,
                                                   IsDeleted = p.IsDeleted,
                                                   CreatedAt = p.CreatedAt,
                                                   UpdatedAt = p.UpdatedAt,
                                                   DeletedAt = p.DeletedAt
                                               }).AsQueryable();

                return pom.ToList();
            }
        }

        /// <summary>
        /// Get one purchase order from Database.
        /// </summary>
        /// <param name="Id">Id of the purchase order</param>
        /// <returns>A purchase order</returns>
        public PurchaseOrderModel GetPurchaseOrder(int Id)
        {
            using (var db = GetContext())
            {
                PurchaseOrderModel pom = (from p in db.PurchaseOrders
                                            where !p.IsDeleted && p.Id == Id
                                            select new PurchaseOrderModel
                                            {
                                                Id = p.Id,
                                                ContactId = p.ContactId,
                                                Code = p.Code,
                                                PurchaseDate = p.PurchaseDate,
                                                IsConfirmed = p.IsConfirmed,
                                                IsDeleted = p.IsDeleted,
                                                CreatedAt = p.CreatedAt,
                                                UpdatedAt = p.UpdatedAt,
                                                DeletedAt = p.DeletedAt
                                            }).FirstOrDefault();

                return pom;
            }
        }

        /// <summary>
        /// Get one purchase order from Database.
        /// </summary>
        /// <param name="contactId">Id of the contact</param>
        /// <returns>A purchase order</returns>
        public List<PurchaseOrderModel> GetPurchaseOrderByContactId(int contactId)
        {
            using (var db = GetContext())
            {
                IQueryable<PurchaseOrderModel> pom = (from p in db.PurchaseOrders
                                                      where !p.IsDeleted == false && p.ContactId == contactId
                                                      select new PurchaseOrderModel
                                                      {
                                                          Id = p.Id,
                                                          ContactId = p.ContactId,
                                                          Code = p.Code,
                                                          PurchaseDate = p.PurchaseDate,
                                                          IsConfirmed = p.IsConfirmed,
                                                          IsDeleted = p.IsDeleted,
                                                          CreatedAt = p.CreatedAt,
                                                          UpdatedAt = p.UpdatedAt,
                                                          DeletedAt = p.DeletedAt
                                                      }).AsQueryable();

                return pom.ToList();
            }
        }

        /*
         * CREATE
         */

        /// <summary>
        /// Create a new purchase order.
        /// </summary>
        /// <param name="purchaseOrder">An object PurchaseOrder</param>
        /// <returns>The new purchase order</returns>
        public PurchaseOrder CreatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            PurchaseOrder newPurchaseOrder = new PurchaseOrder();
            newPurchaseOrder.Id = purchaseOrder.Id;
            newPurchaseOrder.ContactId = purchaseOrder.ContactId;
            newPurchaseOrder.Code = purchaseOrder.Code;
            newPurchaseOrder.PurchaseDate = purchaseOrder.PurchaseDate;
            newPurchaseOrder.IsConfirmed = purchaseOrder.IsConfirmed;
            newPurchaseOrder.IsDeleted = purchaseOrder.IsDeleted;
            newPurchaseOrder.CreatedAt = purchaseOrder.CreatedAt;
            newPurchaseOrder.UpdatedAt = purchaseOrder.UpdatedAt;
            newPurchaseOrder.DeletedAt = purchaseOrder.DeletedAt;
            return Create(newPurchaseOrder);
        }

        /*
         * DELETE
         */

        /// <summary>
        /// Delete a certain purchase order
        /// </summary>
        /// <param name="id">PurchaseOrder Id</param>
        public void DeletePurchaseOrder(int id)
        {
            PurchaseOrder p = Find(x => x.Id == id && !x.IsDeleted);
            if (p != null)
            {
                p.IsDeleted = true;
                p.DeletedAt = DateTime.Now;
                Update(p);
            }
        }

        /*
         * UPDATE
         */

        /// <summary>
        /// Update a purchase order.
        /// </summary>
        /// <param name="purchaseOrder">An object PurchaseOrder</param>
        /// <returns>The updated purchase order</returns>
        public PurchaseOrder UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            PurchaseOrder p = Find(x => x.Id == purchaseOrder.Id && !x.IsDeleted);
            if (p != null)
            {
                p.Id = purchaseOrder.Id;
                p.ContactId = purchaseOrder.ContactId;
                p.Code = purchaseOrder.Code;
                p.PurchaseDate = purchaseOrder.PurchaseDate;
                p.IsConfirmed = purchaseOrder.IsConfirmed;
                p.IsDeleted = purchaseOrder.IsDeleted;
                p.CreatedAt = purchaseOrder.CreatedAt;
                p.UpdatedAt = purchaseOrder.UpdatedAt;
                p.DeletedAt = purchaseOrder.DeletedAt;

                Update(p);
                return p;
            }
            return p;
        }

        /*
         * GET DETAIL
         */

        /// <summary>
        /// Get all purchase order details from Database.
        /// </summary>
        /// <param name="purchaseOrderId">Id of the parent purchase order</param>
        /// <returns>All purchase order details belonging to the parent purchase order</returns>
        public List<PurchaseOrderDetailModel> GetPurchaseOrderDetailList(int purchaseOrderId)
        {
            using (var db = GetContext())
            {
                IQueryable<PurchaseOrderDetailModel> podm = (from pod in db.PurchaseOrderDetails
                                                             where !pod.IsDeleted && pod.PurchaseOrderId == purchaseOrderId 
                                                             select new PurchaseOrderDetailModel
                                                             {
                                                                 Id = pod.Id,
                                                                 PurchaseOrderId = pod.PurchaseOrderId,
                                                                 Code = pod.Code,
                                                                 Quantity = pod.Quantity,
                                                                 PendingReceival = pod.PendingReceival,
                                                                 ItemId = pod.ItemId,
                                                                 IsConfirmed = pod.IsConfirmed,
                                                                 IsFulfilled = pod.IsFulfilled,
                                                                 IsDeleted = pod.IsDeleted,
                                                                 CreatedAt = pod.CreatedAt,
                                                                 UpdatedAt = pod.UpdatedAt,
                                                                 DeletedAt = pod.DeletedAt
                                                             }).AsQueryable();

                return podm.ToList();
            }
        }

        /// <summary>
        /// Get specific purchase order detail from Database.
        /// </summary>
        /// <param name="purchaseOrderDetailId">Id of the purchase order detail</param>
        /// <returns>A purchase order detail</returns>
        public PurchaseOrderDetailModel GetPurchaseOrderDetail(int purchaseOrderDetailId)
        {
            using (var db = GetContext())
            {
                PurchaseOrderDetailModel podm = (from pod in db.PurchaseOrderDetails
                                                 where pod.Id == purchaseOrderDetailId && !pod.IsDeleted
                                                 select new PurchaseOrderDetailModel
                                                 {
                                                     Id = pod.Id,
                                                     PurchaseOrderId = pod.PurchaseOrderId,
                                                     Code = pod.Code,
                                                     Quantity = pod.Quantity,
                                                     PendingReceival = pod.PendingReceival,
                                                     ItemId = pod.ItemId,
                                                     IsConfirmed = pod.IsConfirmed,
                                                     IsFulfilled = pod.IsFulfilled,
                                                     IsDeleted = pod.IsDeleted,
                                                     CreatedAt = pod.CreatedAt,
                                                     UpdatedAt = pod.UpdatedAt,
                                                     DeletedAt = pod.DeletedAt
                                                 }).FirstOrDefault();

                return podm;
            }
        }

        /*
         * CREATE DETAIL
         */

        /// <summary>
        /// Create a new purchase order detail.
        /// </summary>
        /// <param name="purchaseOrderDetail">An object PurchaseOrderDetail</param>
        /// <returns>The new purchaseOrderDetail order detail</returns>
        public PurchaseOrderDetail CreatePurchaseOrderDetail(PurchaseOrderDetail purchaseOrderDetail)
        {

            using (var db = GetContext())
            {

                PurchaseOrderDetail newpod = new PurchaseOrderDetail();
                newpod.Id = purchaseOrderDetail.Id;
                newpod.PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId;
                newpod.Code = purchaseOrderDetail.Code;
                newpod.Quantity = purchaseOrderDetail.Quantity;
                newpod.PendingReceival = purchaseOrderDetail.PendingReceival;
                newpod.ItemId = purchaseOrderDetail.ItemId;
                newpod.IsConfirmed = purchaseOrderDetail.IsConfirmed;
                newpod.IsFulfilled = purchaseOrderDetail.IsFulfilled;
                newpod.IsDeleted = purchaseOrderDetail.IsDeleted;
                newpod.CreatedAt = purchaseOrderDetail.CreatedAt;
                newpod.UpdatedAt = purchaseOrderDetail.UpdatedAt;
                newpod.DeletedAt = purchaseOrderDetail.DeletedAt;

                newpod = db.PurchaseOrderDetails.Add(newpod);
                db.SaveChanges();

                return newpod;
            }
        }

        /*
         * DELETE DETAIL
         */

        /// <summary>
        /// Delete a certain purchase order detail
        /// </summary>
        /// <param name="purchaseOrderDetailId">PurchaseOrderDetail Id</param>
        public void DeletePurchaseOrderDetail(int purchaseOrderDetailId)
        {
            using (var db = GetContext())
            {
                PurchaseOrderDetail pod = (from p in db.PurchaseOrderDetails
                                           where p.Id == purchaseOrderDetailId && !p.IsDeleted
                                           select p).FirstOrDefault();
                if (pod != null)
                {
                    pod.IsDeleted = true;
                    pod.DeletedAt = DateTime.Now;

                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Delete purchase order details that connects to the parent purchase order 
        /// </summary>
        /// <param name="purchaseOrderId">Id of the parent purchase order</param>
        public void DeletePurchaseOrderDetailByPurchaseOrderId(int purchaseOrderId)
        {
            using (var db = GetContext())
            {
                List<PurchaseOrderDetail> pod = (from p in db.PurchaseOrderDetails
                                                 where p.PurchaseOrderId == purchaseOrderId && !p.IsDeleted
                                                 select p).ToList();

                if (pod != null)
                {
                    foreach (var eachdetail in pod)
                    {
                        var updatedod = (from p in db.PurchaseOrderDetails where p.Id == eachdetail.Id select p).FirstOrDefault();
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

        /*
         * UPDATE DETAIL
         */

        /// <summary>
        /// Update a purchase order detail.
        /// </summary>
        /// <param name="purchaseOrderDetail">An object PurchaseOrderDetail</param>
        /// <returns>The updated purchase order detail</returns>
        public PurchaseOrderDetail UpdatePurchaseOrderDetail(PurchaseOrderDetail purchaseOrderDetail)
        {
            using (var db = GetContext())
            {
                PurchaseOrderDetail pod = (from d in db.PurchaseOrderDetails
                                           where d.Id == purchaseOrderDetail.Id && !d.IsDeleted
                                           select d).FirstOrDefault();
                if (pod != null)
                {
                    pod.Id = purchaseOrderDetail.Id;
                    pod.PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId;
                    pod.Code = purchaseOrderDetail.Code;
                    pod.Quantity = purchaseOrderDetail.Quantity;
                    pod.PendingReceival = purchaseOrderDetail.PendingReceival;
                    pod.ItemId = purchaseOrderDetail.ItemId;
                    pod.IsConfirmed = purchaseOrderDetail.IsConfirmed;
                    pod.IsFulfilled = purchaseOrderDetail.IsFulfilled;
                    pod.IsDeleted = purchaseOrderDetail.IsDeleted;
                    pod.CreatedAt = purchaseOrderDetail.CreatedAt;
                    pod.UpdatedAt = purchaseOrderDetail.UpdatedAt;
                    pod.DeletedAt = purchaseOrderDetail.DeletedAt;

                    db.SaveChanges();
                }
                return pod;
            }
        }

        /// <summary>
        /// Update confirmation of all purchase order details that connects to the parent PurchaseOrder 
        /// </summary>
        /// <param name="purchaseOrderId">Id of the parent purchase order</param>
        /// <param name="IsConfirmed">Setter for the Confirm / Unconfirm parameter</param>
        public void UpdateConfirmationPurchaseOrderDetailByPurchaseOrderId(int purchaseOrderId, bool IsConfirmed)
        {
            using (var db = GetContext())
            {
                List<PurchaseOrderDetail> dod = (from d in db.PurchaseOrderDetails
                                                 where d.PurchaseOrderId == purchaseOrderId && !d.IsDeleted
                                                 select d).ToList();

                if (dod != null)
                {
                    foreach (var eachdetail in dod)
                    {
                        var updatedod = (from d in db.PurchaseOrderDetails
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