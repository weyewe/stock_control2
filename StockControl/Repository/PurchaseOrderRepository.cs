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

        /// <summary>
        /// Get all purchase orders from Database.
        /// </summary>
        /// <returns>All purchase orders</returns>
        public List<PurchaseOrderModel> GetPurchaseOrderList()
        {
             using (var db = GetContext())
            {
                IQueryable<PurchaseOrderModel> pom = (from p in db.PurchaseOrders
                                               where p.IsDeleted == false
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
        /// Get all purchase orders from Database.
        /// </summary>
        /// <returns>All purchase orders</returns>
        public PurchaseOrderModel GetPurchaseOrder(int Id)
        {
            using (var db = GetContext())
            {
                PurchaseOrderModel pom = (from p in db.PurchaseOrders
                                            where p.IsDeleted == false && p.Id == Id
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


        public List<PurchaseOrderModel> GetPurchaseOrderByContactId(int contactId)
        {
            using (var db = GetContext())
            {
                IQueryable<PurchaseOrderModel> pom = (from p in db.PurchaseOrders
                                                      where p.IsDeleted == false && p.ContactId == contactId
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

        /// <summary>
        /// Delete a certain purchase order
        /// </summary>
        /// <param name="id">PurchaseOrder Id</param>
        public void DeletePurchaseOrder(int id)
        {
            PurchaseOrder p = Find(x => x.Id == id && x.IsDeleted == false);
            if (p != null)
            {
                p.IsDeleted = true;
                p.DeletedAt = DateTime.Now;
                Update(p);
            }
        }

        /// <summary>
        /// Update a purchase order.
        /// </summary>
        /// <param name="purchaseOrder">An object PurchaseOrder</param>
        /// <returns>The updated purchase order</returns>
        public PurchaseOrder UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            PurchaseOrder p = Find(x => x.Id == purchaseOrder.Id && x.IsDeleted == false);
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

        /// <summary>
        /// Get all purchaseOrderDetail order details from Database.
        /// </summary>
        /// <returns>All purchaseOrderDetail order details</returns>
        public List<PurchaseOrderDetailModel> GetPurchaseOrderDetailList(int purchaseOrderDetailOrderId)
        {
            using (var db = GetContext())
            {
                IQueryable<PurchaseOrderDetailModel> podm = (from pod in db.PurchaseOrderDetails
                                                             join p in db.PurchaseOrders on pod.PurchaseOrderId equals p.Id
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
        /// Get specific purchaseOrderDetail order details from Database.
        /// </summary>
        /// <param name="purchaseOrderDetailId">PurchaseOrderDetail Id</param>
        /// <returns>A purchaseOrderDetail order detail</returns>
        public PurchaseOrderDetailModel GetPurchaseOrderDetail(int purchaseOrderDetailId)
        {
            using (var db = GetContext())
            {
                PurchaseOrderDetailModel podm = (from pod in db.PurchaseOrderDetails
                                                 where pod.Id == purchaseOrderDetailId
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

        /// <summary>
        /// Create a new purchaseOrderDetail order detail.
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

        /// <summary>
        /// Delete a certain purchaseOrderDetail order detail
        /// </summary>
        /// <param name="purchaseOrderDetailId">PurchaseOrderDetail Id</param>
        public void DeletePurchaseOrderDetail(int purchaseOrderDetailId)
        {
            using (var db = GetContext())
            {
                PurchaseOrderDetail pod = (from p in db.PurchaseOrderDetails where p.Id == purchaseOrderDetailId select p).FirstOrDefault();
                if (pod != null)
                {
                    pod.IsDeleted = true;
                    pod.DeletedAt = DateTime.Now;

                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Delete purchaseOrderDetail order details that connects to the parent PurchaseOrder 
        /// </summary>
        /// <param name="purchaseOrderDetailOrderId">Id of the parent PurchaseOrder</param>
        public void DeletePurchaseOrderDetailByPurchaseOrderId(int purchaseOrderDetailOrderId)
        {
            using (var db = GetContext())
            {
                List<PurchaseOrderDetail> pod = (from p in db.PurchaseOrderDetails where p.PurchaseOrderId == purchaseOrderDetailOrderId select p).ToList();

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

        /// <summary>
        /// Update a purchaseOrderDetail order detail.
        /// </summary>
        /// <param name="purchaseOrderDetail">An object PurchaseOrderDetail</param>
        /// <returns>The updated purchaseOrderDetail order detail</returns>
        public PurchaseOrderDetail UpdatePurchaseOrderDetail(PurchaseOrderDetail purchaseOrderDetail)
        {
            using (var db = GetContext())
            {
                PurchaseOrderDetail pod = (from d in db.PurchaseOrderDetails where d.Id == purchaseOrderDetail.Id select d).FirstOrDefault();
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

    }   
}