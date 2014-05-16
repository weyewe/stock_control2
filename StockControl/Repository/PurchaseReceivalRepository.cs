using StockControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace StockControl.Repository
{
    public class PurchaseReceivalRepository : EfRepository<PurchaseReceival>, IPurchaseReceivalRepository
    {

        /*
         * GET
         */

        /// <summary>
        /// Get all purchase receivals from Database.
        /// </summary>
        /// <returns>All purchase receivals</returns>
        public List<PurchaseReceivalModel> GetPurchaseReceivalList()
        {
             using (var db = GetContext())
            {
                IQueryable<PurchaseReceivalModel> prm = (from p in db.PurchaseReceivals
                                                           where !p.IsDeleted
                                                           select new PurchaseReceivalModel
                                                           {
                                                               Id = p.Id,
                                                               ContactId = p.ContactId,
                                                               Code = p.Code,
                                                               ReceivalDate = p.ReceivalDate,
                                                               IsConfirmed = p.IsConfirmed,
                                                               IsDeleted = p.IsDeleted,
                                                               CreatedAt = p.CreatedAt,
                                                               UpdatedAt = p.UpdatedAt,
                                                               DeletedAt = p.DeletedAt
                                                           }).AsQueryable();

                return prm.ToList();
            }
        }

        /// <summary>
        /// Get purchase receival from Database.
        /// </summary>
        /// <param name="Id">Id of the purchase receival.</param>
        /// <returns>A purchase receival</returns>
        public PurchaseReceivalModel GetPurchaseReceival(int Id)
        {
            using (var db = GetContext())
            {
                PurchaseReceivalModel prm = (from p in db.PurchaseReceivals
                                                where !p.IsDeleted && p.Id == Id
                                                select new PurchaseReceivalModel
                                                {
                                                    Id = p.Id,
                                                    ContactId = p.ContactId,
                                                    Code = p.Code,
                                                    ReceivalDate = p.ReceivalDate,
                                                    IsConfirmed = p.IsConfirmed,
                                                    IsDeleted = p.IsDeleted,
                                                    CreatedAt = p.CreatedAt,
                                                    UpdatedAt = p.UpdatedAt,
                                                    DeletedAt = p.DeletedAt
                                                }).FirstOrDefault();

                return prm;
            }
        }

        /// <summary>
        /// Get purchase receival from Database.
        /// </summary>
        /// <param name="contactId">Id of the contact</param>
        /// <returns>A purchase receival</returns>
        public List<PurchaseReceivalModel> GetPurchaseReceivalByContactId(int contactId)
        {
            using (var db = GetContext())
            {
                IQueryable<PurchaseReceivalModel> prm = (from p in db.PurchaseReceivals
                                                         where !p.IsDeleted && p.ContactId == contactId
                                                         select new PurchaseReceivalModel
                                                         {
                                                             Id = p.Id,
                                                             ContactId = p.ContactId,
                                                             Code = p.Code,
                                                             ReceivalDate = p.ReceivalDate,
                                                             IsConfirmed = p.IsConfirmed,
                                                             IsDeleted = p.IsDeleted,
                                                             CreatedAt = p.CreatedAt,
                                                             UpdatedAt = p.UpdatedAt,
                                                             DeletedAt = p.DeletedAt
                                                         }).AsQueryable();

                return prm.ToList();
            }
        }

        /*
         * CREATE
         */

        /// <summary>
        /// Create a new purchase receival.
        /// </summary>
        /// <param name="purchaseReceival">An object PurchaseReceival</param>
        /// <returns>The new purchase receival</returns>
        public PurchaseReceival CreatePurchaseReceival(PurchaseReceival purchaseReceival)
        {
            PurchaseReceival newPurchaseReceival = new PurchaseReceival();
            newPurchaseReceival.Id = purchaseReceival.Id;
            newPurchaseReceival.ContactId = purchaseReceival.ContactId;
            newPurchaseReceival.Code = purchaseReceival.Code;
            newPurchaseReceival.ReceivalDate = purchaseReceival.ReceivalDate;
            newPurchaseReceival.IsConfirmed = purchaseReceival.IsConfirmed;
            newPurchaseReceival.IsDeleted = purchaseReceival.IsDeleted;
            newPurchaseReceival.CreatedAt = purchaseReceival.CreatedAt;
            newPurchaseReceival.UpdatedAt = purchaseReceival.UpdatedAt;
            newPurchaseReceival.DeletedAt = purchaseReceival.DeletedAt;
            return Create(newPurchaseReceival);
        }

        /*
         * DELETE
         */

        /// <summary>
        /// Delete a certain purchase receival
        /// </summary>
        /// <param name="id">PurchaseReceival Id</param>
        public void DeletePurchaseReceival(int id)
        {
            PurchaseReceival p = Find(x => x.Id == id && !x.IsDeleted);
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
        /// Update a purchase receival.
        /// </summary>
        /// <param name="purchaseReceival">An object PurchaseReceival</param>
        /// <returns>The updated purchase receival</returns>
        public PurchaseReceival UpdatePurchaseReceival(PurchaseReceival purchaseReceival)
        {
            PurchaseReceival p = Find(x => x.Id == purchaseReceival.Id && !x.IsDeleted);
            if (p != null)
            {
                p.Id = purchaseReceival.Id;
                p.ContactId = purchaseReceival.ContactId;
                p.Code = purchaseReceival.Code;
                p.ReceivalDate = purchaseReceival.ReceivalDate;
                p.IsConfirmed = purchaseReceival.IsConfirmed;
                p.IsDeleted = purchaseReceival.IsDeleted;
                p.CreatedAt = purchaseReceival.CreatedAt;
                p.UpdatedAt = purchaseReceival.UpdatedAt;
                p.DeletedAt = purchaseReceival.DeletedAt;

                Update(p);
                return p;
            }
            return p;
        }

        /*
         * GET DETAIL
         */

        /// <summary>
        /// Get all purchase recevail details from Database.
        /// </summary>
        /// <param name="purchaseReceivalId">Id of the parent purchase receival</param>
        /// <returns>All purchase receival details belonging to the parent purchase receival</returns>
        public List<PurchaseReceivalDetailModel> GetPurchaseReceivalDetailList(int purchaseReceivalId)
        {
            using (var db = GetContext())
            {
                IQueryable<PurchaseReceivalDetailModel> podm = (from pod in db.PurchaseReceivalDetails
                                                                 where !pod.IsDeleted && pod.PurchaseReceivalId == purchaseReceivalId
                                                                 select new PurchaseReceivalDetailModel
                                                                 {
                                                                     Id = pod.Id,
                                                                     PurchaseReceivalId = pod.PurchaseReceivalId,
                                                                     Code = pod.Code,
                                                                     Quantity = pod.Quantity,
                                                                     ItemId = pod.ItemId,
                                                                     PurchaseOrderDetailId = pod.PurchaseOrderDetailId,
                                                                     IsConfirmed = pod.IsConfirmed,
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
        /// <param name="purchaseReceivalDetailId">Id of the purchase receival detail</param>
        /// <returns>A purchase receival detail</returns>
        public PurchaseReceivalDetailModel GetPurchaseReceivalDetail(int purchaseReceivalDetailId)
        {
            using (var db = GetContext())
            {
                PurchaseReceivalDetailModel podm = (from pod in db.PurchaseReceivalDetails
                                                     where pod.Id == purchaseReceivalDetailId && !pod.IsDeleted
                                                     select new PurchaseReceivalDetailModel
                                                     {
                                                         Id = pod.Id,
                                                         PurchaseReceivalId = pod.PurchaseReceivalId,
                                                         Code = pod.Code,
                                                         Quantity = pod.Quantity,
                                                         ItemId = pod.ItemId,
                                                         PurchaseOrderDetailId = pod.PurchaseOrderDetailId,
                                                         IsConfirmed = pod.IsConfirmed,
                                                         IsDeleted = pod.IsDeleted,
                                                         CreatedAt = pod.CreatedAt,
                                                         UpdatedAt = pod.UpdatedAt,
                                                         DeletedAt = pod.DeletedAt
                                                     }).FirstOrDefault();

                return podm;
            }
        }

        /// <summary>
        /// Get purchase receival detail from Database.
        /// </summary>
        /// <param name="purchaseOrderDetailId">Id of the purchase order detail</param>
        /// <returns>A purchase receival detail</returns>
        public List<PurchaseReceivalDetailModel> GetPurchaseReceivalDetailByPurchaseOrderDetail(int purchaseOrderDetailId)
        {
            using (var db = GetContext())
            {
                 IQueryable<PurchaseReceivalDetailModel> podm = (from pod in db.PurchaseReceivalDetails
                                                                where !pod.IsDeleted && pod.PurchaseOrderDetailId == purchaseOrderDetailId
                                                                select new PurchaseReceivalDetailModel
                                                                {
                                                                    Id = pod.Id,
                                                                    PurchaseReceivalId = pod.PurchaseReceivalId,
                                                                    Code = pod.Code,
                                                                    Quantity = pod.Quantity,
                                                                    ItemId = pod.ItemId,
                                                                    PurchaseOrderDetailId = pod.PurchaseOrderDetailId,
                                                                    IsConfirmed = pod.IsConfirmed,
                                                                    IsDeleted = pod.IsDeleted,
                                                                    CreatedAt = pod.CreatedAt,
                                                                    UpdatedAt = pod.UpdatedAt,
                                                                    DeletedAt = pod.DeletedAt
                                                                }).AsQueryable();

                return podm.ToList();
            }
        }
        /*
         * CREATE DETAIL
         */

        /// <summary>
        /// Create a new purchase receival detail.
        /// </summary>
        /// <param name="purchaseReceivalDetail">An object PurchaseReceivalDetail</param>
        /// <returns>The new purchase receival detail</returns>
        public PurchaseReceivalDetail CreatePurchaseReceivalDetail(PurchaseReceivalDetail purchaseReceivalDetail)
        {

            using (var db = GetContext())
            {

                PurchaseReceivalDetail newpod = new PurchaseReceivalDetail();
                newpod.Id = purchaseReceivalDetail.Id;
                newpod.PurchaseReceivalId = purchaseReceivalDetail.PurchaseReceivalId;
                newpod.Code = purchaseReceivalDetail.Code;
                newpod.Quantity = purchaseReceivalDetail.Quantity;
                newpod.ItemId = purchaseReceivalDetail.ItemId;
                newpod.PurchaseOrderDetailId = purchaseReceivalDetail.PurchaseOrderDetailId;
                newpod.IsConfirmed = purchaseReceivalDetail.IsConfirmed;
                newpod.IsDeleted = purchaseReceivalDetail.IsDeleted;
                newpod.CreatedAt = purchaseReceivalDetail.CreatedAt;
                newpod.UpdatedAt = purchaseReceivalDetail.UpdatedAt;
                newpod.DeletedAt = purchaseReceivalDetail.DeletedAt;

                newpod = db.PurchaseReceivalDetails.Add(newpod);
                db.SaveChanges();

                return newpod;
            }
        }

        /*
         * DELETE DETAIL
         */

        /// <summary>
        /// Delete a certain purchase receival detail
        /// </summary>
        /// <param name="purchaseReceivalDetailId">Id of the purchase receival detail</param>
        public void DeletePurchaseReceivalDetail(int purchaseReceivalDetailId)
        {
            using (var db = GetContext())
            {
                PurchaseReceivalDetail pod = (from p in db.PurchaseReceivalDetails
                                              where p.Id == purchaseReceivalDetailId && !p.IsDeleted
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
        /// Delete purchase receival details that connects to the parent purchase receival 
        /// </summary>
        /// <param name="purchaseReceivalDetailOrderId">Id of the parent purchase receival</param>
        public void DeletePurchaseReceivalDetailByPurchaseReceivalId(int purchaseReceivalId)
        {
            using (var db = GetContext())
            {
                List<PurchaseReceivalDetail> pod = (from p in db.PurchaseReceivalDetails
                                                    where p.PurchaseReceivalId == purchaseReceivalId && !p.IsDeleted
                                                    select p).ToList();

                if (pod != null)
                {
                    foreach (var eachdetail in pod)
                    {
                        var updatedod = (from p in db.PurchaseReceivalDetails
                                         where p.Id == eachdetail.Id && !p.IsDeleted
                                         select p).FirstOrDefault();
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
        /// Update a purchase receival detail.
        /// </summary>
        /// <param name="purchaseReceivalDetail">An object PurchaseReceivalDetail</param>
        /// <returns>The updated purchase receival detail</returns>
        public PurchaseReceivalDetail UpdatePurchaseReceivalDetail(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            using (var db = GetContext())
            {
                PurchaseReceivalDetail pod = (from d in db.PurchaseReceivalDetails
                                              where d.Id == purchaseReceivalDetail.Id && !d.IsDeleted
                                              select d).FirstOrDefault();
                if (pod != null)
                {
                    pod.Id = purchaseReceivalDetail.Id;
                    pod.PurchaseReceivalId = purchaseReceivalDetail.PurchaseReceivalId;
                    pod.Code = purchaseReceivalDetail.Code;
                    pod.Quantity = purchaseReceivalDetail.Quantity;
                    pod.ItemId = purchaseReceivalDetail.ItemId;
                    pod.PurchaseOrderDetailId = purchaseReceivalDetail.PurchaseOrderDetailId;
                    pod.IsConfirmed = purchaseReceivalDetail.IsConfirmed;
                    pod.IsDeleted = purchaseReceivalDetail.IsDeleted;
                    pod.CreatedAt = purchaseReceivalDetail.CreatedAt;
                    pod.UpdatedAt = purchaseReceivalDetail.UpdatedAt;
                    pod.DeletedAt = purchaseReceivalDetail.DeletedAt;

                    db.SaveChanges();
                }
                return pod;
            }
        }

        /// <summary>
        /// Update confirmation of all purchase receival details that connects to the parent PurchaseReceival 
        /// </summary>
        /// <param name="purchaseReceivalId">Id of the parent purchase receival</param>
        /// <param name="IsConfirmed">Setter for the Confirm / Unconfirm parameter</param>
        public void UpdateConfirmationPurchaseReceivalDetailByPurchaseReceivalId(int purchaseReceivalId, bool IsConfirmed)
        {
            using (var db = GetContext())
            {
                List<PurchaseReceivalDetail> dod = (from d in db.PurchaseReceivalDetails
                                              where d.PurchaseReceivalId == purchaseReceivalId && !d.IsDeleted
                                              select d).ToList();

                if (dod != null)
                {
                    foreach (var eachdetail in dod)
                    {
                        var updatedod = (from d in db.PurchaseReceivalDetails
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