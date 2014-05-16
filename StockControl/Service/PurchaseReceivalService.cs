using StockControl.Models;
using StockControl.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace StockControl.Service
{
    public class PurchaseReceivalService : IPurchaseReceivalService
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("PurchaseReceivalService");

        /*
         * GET
         */

        /// <summary>
        /// Get all purchase receivals.
        /// </summary>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <returns>all purchase receivals</returns>
        public List<PurchaseReceivalModel> GetPurchaseReceivalList(IPurchaseReceivalRepository _purchaseReceivalRepository)
        {
            List<PurchaseReceivalModel> model = new List<PurchaseReceivalModel>();
            try
            {
                model = _purchaseReceivalRepository.GetPurchaseReceivalList();
            }
            catch (Exception ex)
            {
                LOG.Error("GetPurchaseReceivalList Failed", ex);
            }

            return model;
        }

        /// <summary>
        /// Get a purchase receival
        /// </summary>
        /// <param name="orderId">Id of the purchase receival</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <returns>a purchase receival</returns>
        public PurchaseReceivalModel GetPurchaseReceival(int orderId, IPurchaseReceivalRepository _purchaseReceivalRepository)
        {
            PurchaseReceivalModel model = new PurchaseReceivalModel();
            try
            {
                model = _purchaseReceivalRepository.GetPurchaseReceival(orderId);
            }
            catch (Exception ex)
            {
                LOG.Error("GetPurchaseReceival Failed", ex);
            }

            return model;
        }

        /*
         * CREATE
         */

        /// <summary>
        /// Create a purchase receival.
        /// </summary>
        /// <param name="purchaseReceival">PurchaseReceivalModel object</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <returns>a purchase receival</returns>
        public ResponseModel CreatePurchaseReceival(PurchaseReceivalModel purchaseReceival, IPurchaseReceivalRepository _purchaseReceivalRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateCreatePurchaseReceival(purchaseReceival, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                PurchaseReceival newPurchaseReceival = new PurchaseReceival();
                newPurchaseReceival.Id = purchaseReceival.Id;
                newPurchaseReceival.ContactId = purchaseReceival.ContactId;
                newPurchaseReceival.Code = purchaseReceival.Code;
                newPurchaseReceival.ReceivalDate = purchaseReceival.ReceivalDate;
                newPurchaseReceival.IsConfirmed = purchaseReceival.IsConfirmed;
                newPurchaseReceival.IsDeleted = purchaseReceival.IsDeleted;
                newPurchaseReceival.CreatedAt = DateTime.Now;
                newPurchaseReceival.UpdatedAt = purchaseReceival.UpdatedAt;
                newPurchaseReceival.DeletedAt = purchaseReceival.DeletedAt;

                newPurchaseReceival = _purchaseReceivalRepository.CreatePurchaseReceival(newPurchaseReceival);
                newPurchaseReceival.Id = newPurchaseReceival.Id;

                respModel.isValid = true;
                respModel.message = "Create Purchase Receival Success...";
                respModel.objResult = purchaseReceival;

                LOG.Error("CreatePurchaseReceival Sucess");
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    LOG.ErrorFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        validationErrors.Entry.Entity.GetType().Name, validationErrors.Entry.State);
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        //Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        LOG.ErrorFormat("CreatePurchaseReceival, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("CreatePurchaseReceival Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Create Purchase Receival failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("CreatePurchaseReceival Failed", ex);
                respModel.isValid = false;
                respModel.message = "Create Purchase Receival Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * DELETE
         */

        /// <summary>
        /// Delete a purchase receival and all its children purchase receival details
        /// </summary>
        /// <param name="purchaseReceivalId">Id of the purchase receival</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository</param>
        /// <returns>a response model</returns>
        public ResponseModel DeletePurchaseReceival(int purchaseReceivalId, IPurchaseReceivalRepository _purchaseReceivalRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                PurchaseReceival deletePurchaseReceival = _purchaseReceivalRepository.Find(p => p.Id == purchaseReceivalId && !p.IsDeleted);
                if (deletePurchaseReceival != null)
                {
                    string message = "";
                    PurchaseReceivalModel model = PurchaseReceivalModel.ToModel(deletePurchaseReceival);
                    respModel.isValid = this.ValidateDeletePurchaseReceival(model, out message);
                    if (!respModel.isValid)
                    {
                        respModel.message = message;
                        return respModel;
                    }

                    // Delete PurchaseReceival
                    _purchaseReceivalRepository.DeletePurchaseReceival(purchaseReceivalId);
                    respModel.objResult = model;

                    // Get all subsequent PurchaseReceivalDetails
                    List<PurchaseReceivalDetailModel> deletePurchaseReceivalDetails = _purchaseReceivalRepository.GetPurchaseReceivalDetailList(purchaseReceivalId);

                    if (deletePurchaseReceivalDetails.Count() > 0)
                    {
                        // Delete all subsequent PurchaseReceivalDetails
                        _purchaseReceivalRepository.DeletePurchaseReceivalDetailByPurchaseReceivalId(purchaseReceivalId);
                        // Adding the line below will set respModel to all the subsequent details
                        // respModel.objResult = deletePurchaseReceivalDetails;
                    }

                    respModel.isValid = true;
                    respModel.message = "Delete purchase receival and its purchase receival details Success...";
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    LOG.ErrorFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        validationErrors.Entry.Entity.GetType().Name, validationErrors.Entry.State);
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        //Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        LOG.ErrorFormat("DeletePurchaseReceival, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("DeletePurchaseReceival Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Delete Purchase Receival failed, Please try again or purchaseReceival your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("DeletePurchaseReceival Failed", ex);
                respModel.isValid = false;
                respModel.message = "Delete Purchase Receival Failed, Please try again or purchaseReceival your administrator.";
            }

            return respModel;
        }

        /*
         * UPDATE
         */

        /// <summary>
        /// Update a purchase receival.
        /// </summary>
        /// <param name="purchaseReceival">PurchaseReceivalModel object</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel UpdatePurchaseReceival(PurchaseReceivalModel purchaseReceival, IPurchaseReceivalRepository _purchaseReceivalRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateUpdatePurchaseReceival(purchaseReceival, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                PurchaseReceival updatePurchaseReceival = _purchaseReceivalRepository.Find(p => p.Id == purchaseReceival.Id && !p.IsDeleted);
                if (updatePurchaseReceival != null)
                {
                    updatePurchaseReceival.Id = purchaseReceival.Id;
                    updatePurchaseReceival.ContactId = purchaseReceival.ContactId;
                    updatePurchaseReceival.Code = purchaseReceival.Code;
                    updatePurchaseReceival.ReceivalDate = purchaseReceival.ReceivalDate;
                    updatePurchaseReceival.IsConfirmed = purchaseReceival.IsConfirmed;
                    updatePurchaseReceival.IsDeleted = purchaseReceival.IsDeleted;
                    updatePurchaseReceival.CreatedAt = purchaseReceival.CreatedAt;
                    updatePurchaseReceival.UpdatedAt = DateTime.Now;
                    updatePurchaseReceival.DeletedAt = purchaseReceival.DeletedAt;

                    _purchaseReceivalRepository.UpdatePurchaseReceival(updatePurchaseReceival);

                    respModel.isValid = true;
                    respModel.message = "Update Purchase Receival Success...";
                    respModel.objResult = purchaseReceival;

                    LOG.Info("UpdatePurchaseReceival Success");
                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "PurchaseReceival not found...";
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    LOG.ErrorFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        validationErrors.Entry.Entity.GetType().Name, validationErrors.Entry.State);
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        //Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        LOG.ErrorFormat("UpdatePurchaseReceival, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UpdatePurchaseReceival Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Update purchase receival failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UpdatePurchaseReceival Failed", ex);
                respModel.isValid = false;
                respModel.message = "Update purchase receival Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * CONFIRM
         */

        /// <summary>
        /// Confirm purchase receival. This function also automatically confirms all its children purchase receival details.
        /// </summary>
        /// <param name="purchaseReceival">PurchaseReceivalModel object</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel ConfirmPurchaseReceival(PurchaseReceivalModel purchaseReceival, IPurchaseReceivalRepository _purchaseReceivalRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateConfirmPurchaseReceival(purchaseReceival, _purchaseReceivalRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                PurchaseReceival confirmPurchaseReceival = _purchaseReceivalRepository.Find(p => p.Id == purchaseReceival.Id && !p.IsDeleted);
                if (confirmPurchaseReceival != null)
                {
                    confirmPurchaseReceival.Id = purchaseReceival.Id;
                    confirmPurchaseReceival.ContactId = purchaseReceival.ContactId;
                    confirmPurchaseReceival.Code = purchaseReceival.Code;
                    confirmPurchaseReceival.ReceivalDate = purchaseReceival.ReceivalDate;
                    confirmPurchaseReceival.IsConfirmed = true;
                    confirmPurchaseReceival.IsDeleted = purchaseReceival.IsDeleted;
                    confirmPurchaseReceival.CreatedAt = purchaseReceival.CreatedAt;
                    confirmPurchaseReceival.UpdatedAt = DateTime.Now;
                    confirmPurchaseReceival.DeletedAt = purchaseReceival.DeletedAt;

                    _purchaseReceivalRepository.UpdatePurchaseReceival(confirmPurchaseReceival);

                    LOG.Info("ConfirmPurchaseReceival Success");

                    // Confirm PurchaseReceival Detail
                    List<PurchaseReceivalDetailModel> confirmPurchaseReceivalDetails = _purchaseReceivalRepository.GetPurchaseReceivalDetailList(purchaseReceival.Id);

                    if (confirmPurchaseReceivalDetails.Count() > 0)
                    {
                        foreach (var prd in confirmPurchaseReceivalDetails)
                        {
                            respModel = this.ConfirmPurchaseReceivalDetail(prd, _purchaseReceivalRepository, _itemRepository, _stockMutationRepository);
                        }
                        LOG.Info("ConfirmPurchaseReceivalDetails Success");
                    }

                    respModel.isValid = true;
                    respModel.message = "Confirm Data Success...";
                    respModel.objResult = confirmPurchaseReceival;
                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "PurchaseReceival not found...";
                }
                respModel.isValid = true;
                respModel.message = "Confirm Purchase Receival Success...";
                respModel.objResult = purchaseReceival;

                LOG.Info("ConfirmPurchaseReceival Success");

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    LOG.ErrorFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        validationErrors.Entry.Entity.GetType().Name, validationErrors.Entry.State);
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        //Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        LOG.ErrorFormat("ConfirmPurchaseReceival, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("ConfirmPurchaseReceival Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Confirm purchase receival failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("ConfirmPurchaseReceival Failed", ex);
                respModel.isValid = false;
                respModel.message = "Confirm purchase receival Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * UNCONFIRM
         */

        /// <summary>
        /// Unconfirm purchase receival.
        /// </summary>
        /// <param name="purchaseReceival">PurchaseReceivalModel object</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel UnconfirmPurchaseReceival(PurchaseReceivalModel purchaseReceival, IPurchaseReceivalRepository _purchaseReceivalRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateUnconfirmPurchaseReceival(purchaseReceival, _purchaseReceivalRepository, _itemRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                PurchaseReceival unconfirmPurchaseReceival = _purchaseReceivalRepository.Find(p => p.Id == purchaseReceival.Id && !p.IsDeleted);
                if (unconfirmPurchaseReceival != null)
                {
                    unconfirmPurchaseReceival.Id = purchaseReceival.Id;
                    unconfirmPurchaseReceival.ContactId = purchaseReceival.ContactId;
                    unconfirmPurchaseReceival.Code = purchaseReceival.Code;
                    unconfirmPurchaseReceival.ReceivalDate = purchaseReceival.ReceivalDate;
                    unconfirmPurchaseReceival.IsConfirmed = false;
                    unconfirmPurchaseReceival.IsDeleted = purchaseReceival.IsDeleted;
                    unconfirmPurchaseReceival.CreatedAt = purchaseReceival.CreatedAt;
                    unconfirmPurchaseReceival.UpdatedAt = DateTime.Now;
                    unconfirmPurchaseReceival.DeletedAt = purchaseReceival.DeletedAt;

                    _purchaseReceivalRepository.UpdatePurchaseReceival(unconfirmPurchaseReceival);

                    LOG.Info("UnconfirmPurchaseReceival Success");

                    // Unconfirm PurchaseReceival Detail
                    List<PurchaseReceivalDetailModel> unconfirmPurchaseReceivalDetails = _purchaseReceivalRepository.GetPurchaseReceivalDetailList(purchaseReceival.Id);

                    if (unconfirmPurchaseReceivalDetails.Count() > 0)
                    {
                        _purchaseReceivalRepository.UpdateConfirmationPurchaseReceivalDetailByPurchaseReceivalId(purchaseReceival.Id, false);
                    }

                    LOG.Info("UnconfirmPurchaseReceivalDetails Success");

                    respModel.isValid = true;
                    respModel.message = "Unconfirm Purchase Receival Success...";
                    respModel.objResult = purchaseReceival;
                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "PurchaseReceival not found...";
                }
                respModel.isValid = true;
                respModel.message = "Unconfirm Purchase Receival Success...";
                respModel.objResult = purchaseReceival;

                LOG.Info("UnconfirmPurchaseReceival Success");

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    LOG.ErrorFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        validationErrors.Entry.Entity.GetType().Name, validationErrors.Entry.State);
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        //Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        LOG.ErrorFormat("UnconfirmPurchaseReceival, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UnconfirmPurchaseReceival Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Unconfirm purchase receival failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UnconfirmPurchaseReceival Failed", ex);
                respModel.isValid = false;
                respModel.message = "Unconfirm purchase receival Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * VALIDATE
         */

        /// <summary>
        /// Private function to validate a purchase receival.
        /// </summary>
        /// <param name="model">PurchaseReceivalModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        private bool Validate(PurchaseReceivalModel model, out string message)
        {
            bool isValid = true;
            message = "OK";

            // Contact must be present. Contact will never be null.
            // 
            /*
            if (model.ContactId == null)
            {
                message = "Invalid data...";
                return false;
            }
             */
            // Receival Date must be present
            if (model.ReceivalDate == null)
            {
                message = "Invalid data...";
                return false;
            }
            return isValid;
        }

        /// <summary>
        /// Validate a purchase receival when it is created.
        /// </summary>
        /// <param name="model">PurchaseReceivalModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateCreatePurchaseReceival(PurchaseReceivalModel model, out string message)
        {
            bool isValid = this.Validate(model, out message);

            return isValid;
        }

        /// <summary>
        /// Validate a purchase receival when it is updated.
        /// </summary>
        /// <param name="model">PurchaseReceivalModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateUpdatePurchaseReceival(PurchaseReceivalModel model, out string message)
        {
            bool isValid = this.Validate(model, out message);

            if (model.IsConfirmed)
            {
                message = "Confirmed purchase receival cannot be updated. Please try again or contact your administrator.";
                return false;
            }

            return isValid;
        }

        /// <summary>
        /// Validate a purchase receival when it is deleted.
        /// It is invalid when a purchase receival has been confirmed.
        /// </summary>
        /// <param name="model">PurchaseReceivalModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateDeletePurchaseReceival(PurchaseReceivalModel model, out string message)
        {
            bool isValid = true;
            message = "";

            if (model.IsConfirmed)
            {
                message = "Can't destroyed confirmed purchase receival. Please try again or contact your administrator";
                return false;
            }

            return isValid;
        }

        /// <summary>
        /// Validate a purchase receival when it is confirmed.
        /// It is valid when it has any purchase receival details.
        /// </summary>
        /// <param name="model">PurchaseReceivalModel object</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateConfirmPurchaseReceival(PurchaseReceivalModel model, IPurchaseReceivalRepository _purchaseReceivalRepository, out string message)
        {
            bool isValid = true;
            message = "";

            if (_purchaseReceivalRepository.GetPurchaseReceivalDetailList(model.Id).Count() == 0)
            {
                message = "There is no existing Purchase Receival Detail...";
                return false;
            }
            return isValid;
        }


        /// <summary>
        /// Validate a purchase receival when it is unconfirmed.
        /// It is valid when all its purchase receival detail is joined to an item with PendingDelivery gt 0
        /// </summary>
        /// <param name="purchaseReceival">PurchaseReceivalModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateUnconfirmPurchaseReceival(PurchaseReceivalModel purchaseReceival, IPurchaseReceivalRepository _purchaseReceivalRepository, IItemRepository _itemRepository, out string message)
        {
            bool isValid = true;
            message = "";

            List<PurchaseReceivalDetailModel> model = _purchaseReceivalRepository.GetPurchaseReceivalDetailList(purchaseReceival.Id);
            if (model.Count() > 0)
            {
                foreach (var eachdetail in model)
                {
                    ItemModel item = _itemRepository.GetItem(eachdetail.ItemId);
                    if (item.Ready < 0)
                    {
                        isValid = false;
                        message = "Invalid unconfirmation. Ready quantity of the item is less than 0";
                    }
                }
            }
            return isValid;
        }

        /*
         * GET DETAIL
         */

        /// <summary>
        /// Get all purchase receival details.
        /// </summary>
        /// <param name="purchaseReceivalId">Id of the purchase receival</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <returns>all purchase receival details associated to the purchase receival</returns>
        public List<PurchaseReceivalDetailModel> GetPurchaseReceivalDetailList(int purchaseReceivalId, IPurchaseReceivalRepository _purchaseReceivalRepository)
        {
            List<PurchaseReceivalDetailModel> model = new List<PurchaseReceivalDetailModel>();
            try
            {
                model = _purchaseReceivalRepository.GetPurchaseReceivalDetailList(purchaseReceivalId);
            }
            catch (Exception ex)
            {
                LOG.Error("GetPurchaseReceivalDetailList Failed", ex);
            }

            return model;
        }

        /// <summary>
        /// Get a purchase receival detail
        /// </summary>
        /// <param name="purchaseReceivalDetailId">Id of the purchase receival detail</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <returns>a purchase receival detail</returns>
        public PurchaseReceivalDetailModel GetPurchaseReceivalDetail(int purchaseReceivalDetailId, IPurchaseReceivalRepository _purchaseReceivalRepository)
        {
            PurchaseReceivalDetailModel model = new PurchaseReceivalDetailModel();
            try
            {
                model = _purchaseReceivalRepository.GetPurchaseReceivalDetail(purchaseReceivalDetailId);
            }
            catch (Exception ex)
            {
                LOG.Error("GetPurchaseReceivalDetail Failed", ex);
            }

            return model;
        }

        /*
         * CREATE DETAIL
         */

        /// <summary>
        /// Create a purchase receival detail.
        /// </summary>
        /// <param name="purchaseReceivalDetail">PurchaseReceivalDetail object</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel CreatePurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, IPurchaseReceivalRepository _purchaseReceivalRepository, IPurchaseOrderRepository _purchaseOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateCreatePurchaseReceivalDetail(purchaseReceivalDetail, _purchaseReceivalRepository, _purchaseOrderRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                PurchaseReceivalDetail newPurchaseReceivalDetail = new PurchaseReceivalDetail();
                newPurchaseReceivalDetail.Id = purchaseReceivalDetail.Id;
                newPurchaseReceivalDetail.PurchaseReceivalId = purchaseReceivalDetail.PurchaseReceivalId;
                newPurchaseReceivalDetail.Code = purchaseReceivalDetail.Code;
                newPurchaseReceivalDetail.Quantity = purchaseReceivalDetail.Quantity;
                newPurchaseReceivalDetail.ItemId = purchaseReceivalDetail.ItemId;
                newPurchaseReceivalDetail.PurchaseOrderDetailId = purchaseReceivalDetail.PurchaseOrderDetailId;
                newPurchaseReceivalDetail.IsConfirmed = purchaseReceivalDetail.IsConfirmed;
                newPurchaseReceivalDetail.IsDeleted = purchaseReceivalDetail.IsDeleted;
                newPurchaseReceivalDetail.CreatedAt = DateTime.Now;
                newPurchaseReceivalDetail.UpdatedAt = purchaseReceivalDetail.UpdatedAt;
                newPurchaseReceivalDetail.DeletedAt = purchaseReceivalDetail.DeletedAt;

                newPurchaseReceivalDetail = _purchaseReceivalRepository.CreatePurchaseReceivalDetail(newPurchaseReceivalDetail);
                newPurchaseReceivalDetail.Id = newPurchaseReceivalDetail.Id;

                respModel.isValid = true;
                respModel.message = "Create Purchase Receival Detail Success...";
                respModel.objResult = purchaseReceivalDetail;

                LOG.Error("CreatePurchaseReceivalDetail Sucess");
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    LOG.ErrorFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        validationErrors.Entry.Entity.GetType().Name, validationErrors.Entry.State);
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        //Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        LOG.ErrorFormat("CreatePurchaseReceivalDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("CreatePurchaseReceivalDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Create PurchaseReceivalDetail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("CreatePurchaseReceivalDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Create PurchaseReceivalDetail Failed, Please try again or contact your administrator.";
            }

            return respModel;

        }


        /// <summary>
        /// Please use DeletePurchaseReceival() instead of this function. This only deletes all the deliver order details without deleting its
        /// parent. DeletePurchaseReceival() will delete purchase receival and all its purchase receival details.
        /// </summary>
        /// <param name="purchaseReceivalId">Id of the purchase receival</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel DeletePurchaseReceivalDetailByPurchaseReceivalId(int purchaseReceivalId, IPurchaseReceivalRepository _purchaseReceivalRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                List<PurchaseReceivalDetailModel> model = _purchaseReceivalRepository.GetPurchaseReceivalDetailList(purchaseReceivalId);
                if (model.Count() == 0)
                {
                    respModel.isValid = false;
                    respModel.message = "There's no Purchase Receival Detail...";
                    return respModel;
                }
                _purchaseReceivalRepository.DeletePurchaseReceivalDetailByPurchaseReceivalId(purchaseReceivalId);

                respModel.message = "Delete Purchase Receival Details completed. Please make sure the that purchase receival has already been deleted";
                return respModel;

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    LOG.ErrorFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        validationErrors.Entry.Entity.GetType().Name, validationErrors.Entry.State);
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        //Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        LOG.ErrorFormat("DeletePurchaseReceivalDetailByPurchaseReceivalId, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("DeletePurchaseReceivalDetailByPurchaseReceivalId Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Delete DeletePurchaseReceivalDetailByPurchaseReceivalId failed, Please try again or purchaseReceival your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("DeletePurchaseReceivalDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Delete PurchaseReceivalDetail Failed, Please try again or purchaseReceival your administrator.";
            }

            return respModel;
        }

        /// <summary>
        /// Delete purchase receival detail.
        /// </summary>
        /// <param name="purchaseReceivalDetailId">Id of the purchase receival detail</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel DeletePurchaseReceivalDetail(int purchaseReceivalDetailId, IPurchaseReceivalRepository _purchaseReceivalRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                PurchaseReceivalDetailModel model = _purchaseReceivalRepository.GetPurchaseReceivalDetail(purchaseReceivalDetailId);
                if (model != null)
                {
                    string message = "";
                    respModel.isValid = this.ValidateDeletePurchaseReceivalDetail(model, out message);
                    if (!respModel.isValid)
                    {
                        respModel.message = message;
                        return respModel;
                    }

                    // Delete PurchaseReceivalDetail
                    _purchaseReceivalRepository.DeletePurchaseReceivalDetail(purchaseReceivalDetailId);

                    respModel.isValid = true;
                    respModel.message = "Delete PurchaseReceivalDetail Success...";
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    LOG.ErrorFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        validationErrors.Entry.Entity.GetType().Name, validationErrors.Entry.State);
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        //Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        LOG.ErrorFormat("DeletePurchaseReceivalDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("DeletePurchaseReceivalDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Delete PurchaseReceivalDetail failed, Please try again or purchaseReceival your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("DeletePurchaseReceivalDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Delete PurchaseReceivalDetail Failed, Please try again or purchaseReceival your administrator.";
            }

            return respModel;
        }

        /*
         * UPDATE DETAIL
         */

        /// <summary>
        /// Update a purchase receival detail.
        /// </summary>
        /// <param name="purchaseReceivalDetail">PurchaseReceivalDetailModel object</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel UpdatePurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, IPurchaseReceivalRepository _purchaseReceivalRepository, IPurchaseOrderRepository _purchaseOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateUpdatePurchaseReceivalDetail(purchaseReceivalDetail, _purchaseReceivalRepository, _purchaseOrderRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                PurchaseReceivalDetail prd = new PurchaseReceivalDetail();
                prd.Id = purchaseReceivalDetail.Id;
                prd.PurchaseReceivalId = purchaseReceivalDetail.PurchaseReceivalId;
                prd.Code = purchaseReceivalDetail.Code;
                prd.Quantity = purchaseReceivalDetail.Quantity;
                prd.ItemId = purchaseReceivalDetail.ItemId;
                prd.PurchaseOrderDetailId = purchaseReceivalDetail.PurchaseOrderDetailId;
                prd.IsConfirmed = purchaseReceivalDetail.IsConfirmed;
                prd.IsDeleted = purchaseReceivalDetail.IsDeleted;
                prd.CreatedAt = purchaseReceivalDetail.CreatedAt;
                prd.UpdatedAt = DateTime.Now;
                prd.DeletedAt = purchaseReceivalDetail.DeletedAt;

                _purchaseReceivalRepository.UpdatePurchaseReceivalDetail(prd);

                respModel.isValid = true;
                respModel.message = "Update Purchase Receival Detail Success...";
                respModel.objResult = prd;

                LOG.Info("UpdatePurchaseReceivalDetail Success");
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    LOG.ErrorFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        validationErrors.Entry.Entity.GetType().Name, validationErrors.Entry.State);
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        //Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        LOG.ErrorFormat("UpdatePurchaseReceivalDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UpdatePurchaseReceivalDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Update PurchaseReceivalDetail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UpdatePurchaseReceivalDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Update PurchaseReceivalDetail Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /// <summary>
        /// This function calls an update function given the parameter 'Confirm' or 'Unconfirm'.
        /// </summary>
        /// <param name="purchaseReceivalDetail">PurchaseReceivalModel object</param>
        /// <param name="IsConfirm">Is this function Confirm or Unconfirm</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <returns>a response model</returns>
        ResponseModel UpdateConfirmationPurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, bool IsConfirm, IPurchaseReceivalRepository _purchaseReceivalRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                String message = IsConfirm ? "Update Confirm" : "Update Unconfirm";
                PurchaseReceivalDetailModel confirmPurchaseReceivalDetail = _purchaseReceivalRepository.GetPurchaseReceivalDetail(purchaseReceivalDetail.Id);

                if (confirmPurchaseReceivalDetail != null)
                {
                    _purchaseReceivalRepository.UpdateConfirmationPurchaseReceivalDetailByPurchaseReceivalId(confirmPurchaseReceivalDetail.Id, IsConfirm);

                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "PurchaseReceivalDetail not found...";
                }
                respModel.isValid = true;
                respModel.message = message + "PurchaseReceivalDetail Success...";
                respModel.objResult = purchaseReceivalDetail;

                LOG.Info(message + "PurchaseReceivalDetail Success");

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    LOG.ErrorFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        validationErrors.Entry.Entity.GetType().Name, validationErrors.Entry.State);
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        //Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        LOG.ErrorFormat("UpdateConfirmationPurchaseReceivalDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UpdateConfirmationPurchaseReceivalDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Update Confirmation Purchase Receival Detail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UpdateConfirmationPurchaseReceivalDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Update Confirmation Purchase Receival Detail Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * CONFIRM DETAIL
         */

        /// <summary>
        /// Confirm a purchase receival detail.
        /// </summary>
        /// <param name="purchaseReceivalDetail">PurchaseReceivalDetailModel object</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="_stockMutationRepository">IStockMutation object</param>
        /// <returns>a response model</returns>
        public ResponseModel ConfirmPurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, IPurchaseReceivalRepository _purchaseReceivalRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            String message = "";
            try
            {
                respModel.isValid = this.ValidateConfirmPurchaseReceivalDetail(purchaseReceivalDetail, out message);
                if (!respModel.isValid) { return respModel; }

                respModel = this.UpdateConfirmationPurchaseReceivalDetail(purchaseReceivalDetail, true, _purchaseReceivalRepository);

                Item item = _itemRepository.Find(x => x.Id == purchaseReceivalDetail.ItemId && !x.IsDeleted);
                if (item == null)
                {
                    respModel.isValid = false;
                    respModel.message = "No item found...";
                    return respModel;
                }

                PurchaseReceivalDetail prd = new PurchaseReceivalDetail();
                prd.Id = purchaseReceivalDetail.Id;
                prd.PurchaseReceivalId = purchaseReceivalDetail.PurchaseReceivalId;
                prd.Code = purchaseReceivalDetail.Code;
                prd.Quantity = purchaseReceivalDetail.Quantity;
                prd.ItemId = purchaseReceivalDetail.ItemId;
                prd.PurchaseOrderDetailId = purchaseReceivalDetail.PurchaseOrderDetailId;
                prd.IsConfirmed = purchaseReceivalDetail.IsConfirmed;
                prd.IsDeleted = purchaseReceivalDetail.IsDeleted;
                prd.CreatedAt = purchaseReceivalDetail.CreatedAt;
                prd.UpdatedAt = DateTime.Now;
                prd.DeletedAt = purchaseReceivalDetail.DeletedAt;

                PurchaseReceivalDetail model = _purchaseReceivalRepository.UpdatePurchaseReceivalDetail(prd);

                item.PendingReceival -= purchaseReceivalDetail.Quantity;
                item.Ready += purchaseReceivalDetail.Quantity;
                item.UpdatedAt = DateTime.Now;
                _itemRepository.UpdateItem(item);

                LOG.Info("Updating Item " + item.Sku + " Reducing Pending Receival and Adding Ready By " + purchaseReceivalDetail.Quantity);

                StockMutation stockMutationPendingReceival = new StockMutation();
                stockMutationPendingReceival.ItemId = item.Sku;
                stockMutationPendingReceival.Quantity = purchaseReceivalDetail.Quantity;
                stockMutationPendingReceival.SourceDocument = "PurchaseReceival";
                stockMutationPendingReceival.SourceDocumentId = purchaseReceivalDetail.PurchaseReceivalId;
                stockMutationPendingReceival.SourceDocumentDetail = "PurchaseReceivalDetail";
                stockMutationPendingReceival.SourceDocumentDetailId = purchaseReceivalDetail.Id;
                stockMutationPendingReceival.ItemCase = 2; // Pending Receival
                stockMutationPendingReceival.MutationCase = 2; // Reduction
                stockMutationPendingReceival.IsDeleted = false;
                stockMutationPendingReceival.CreatedAt = DateTime.Now;
                stockMutationPendingReceival.UpdatedAt = null;
                stockMutationPendingReceival.DeletedAt = null;

                stockMutationPendingReceival = _stockMutationRepository.CreateStockMutation(stockMutationPendingReceival);
                stockMutationPendingReceival.Id = stockMutationPendingReceival.Id;

                StockMutation stockMutationReady = new StockMutation();
                stockMutationReady.ItemId = item.Sku;
                stockMutationReady.Quantity = purchaseReceivalDetail.Quantity;
                stockMutationReady.SourceDocument = "PurchaseReceival";
                stockMutationReady.SourceDocumentId = purchaseReceivalDetail.PurchaseReceivalId;
                stockMutationReady.SourceDocumentDetail = "PurchaseReceivalDetail";
                stockMutationReady.SourceDocumentDetailId = purchaseReceivalDetail.Id;
                stockMutationReady.ItemCase = 1; // Ready
                stockMutationReady.MutationCase = 1; // Addition
                stockMutationReady.IsDeleted = false;
                stockMutationReady.CreatedAt = DateTime.Now;
                stockMutationReady.UpdatedAt = null;
                stockMutationReady.DeletedAt = null;

                stockMutationReady = _stockMutationRepository.CreateStockMutation(stockMutationReady);
                stockMutationReady.Id = stockMutationReady.Id;
                LOG.Info("Creating Stock Mutation Pending Receival and Ready");

                respModel.message = "Success updating item and creating 2 stock mutations";
                respModel.objResult = purchaseReceivalDetail;
                return respModel;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    LOG.ErrorFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        validationErrors.Entry.Entity.GetType().Name, validationErrors.Entry.State);
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        //Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        LOG.ErrorFormat("ConfirmPurchaseReceivalDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("ConfirmPurchaseReceivalDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Confirm PurchaseReceivalDetail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("ConfirmPurchaseReceivalDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Confirm PurchaseReceivalDetail Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * UNCONFIRM DETAIL
         */

        /// <summary>
        /// Unconfirm a purchase receival detail.
        /// </summary>
        /// <param name="purchaseReceivalDetail">PurchaseReceivalDetailModel object</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="_stockMutationRepository">IStockMutationRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel UnconfirmPurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, IPurchaseReceivalRepository _purchaseReceivalRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository)
        {
            String message = "";
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "";
            respModel.objResult = null;

            try
            {
                respModel.isValid = ValidateUnconfirmPurchaseReceivalDetail(purchaseReceivalDetail, _purchaseReceivalRepository, _itemRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                Item item = _itemRepository.Find(x => x.Id == purchaseReceivalDetail.ItemId && !x.IsDeleted);
                if (item == null)
                {
                    respModel.isValid = false;
                    respModel.message = "No item found...";
                    return respModel;
                }

                respModel = this.UpdateConfirmationPurchaseReceivalDetail(purchaseReceivalDetail, false, _purchaseReceivalRepository);

                item.PendingDelivery += purchaseReceivalDetail.Quantity;
                item.Ready -= purchaseReceivalDetail.Quantity;
                item.UpdatedAt = DateTime.Now;
                _itemRepository.UpdateItem(item);

                List<StockMutation> sm = _stockMutationRepository.GetStockMutationBySourceDocumentDetail(ItemModel.ToModel(item), "PurchaseReceivalDetail", purchaseReceivalDetail.Id);
                if (sm.Count() != 0)
                {
                    foreach (var stockMutation in sm)
                    {
                        _stockMutationRepository.DeleteStockMutation(stockMutation.Id);
                    }
                }

                LOG.Info("Updating Item " + item.Sku + " Reducing Pending Delivery By " + purchaseReceivalDetail.Quantity);

                return respModel;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    LOG.ErrorFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        validationErrors.Entry.Entity.GetType().Name, validationErrors.Entry.State);
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        //Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        LOG.ErrorFormat("UnconfirmPurchaseReceivalDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UnconfirmPurchaseReceivalDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "UnconfirmPurchaseReceivalDetail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UnconfirmPurchaseReceivalDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "UnconfirmPurchaseReceivalDetail Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /// <summary>
        /// Validate a delivery order detail when it is created / updated.
        /// It is valid if it asserts the following rules:
        /// The following attributes must be present:
        /// 1. PurchaseOrderDetailId, Contact must be present
        /// 2. Contact must be associated to PurchaseOrderDetailId 
        /// 3. Quantity gt 0 && Quantity le PendingReceival of PurchaseOrderDetail
        /// 4. PendingReceival of PurchaseOrderDetail gt 0
        /// 5. PurchaseOrderDetail isConfirmed
        /// 6. purchaseOrderModel.ContactId == purchaseReceivalModel.ContactId
        /// 7. Unique PurchaseOrderDetailId in a given PurchaseReceival -- Unchecked !! TODO
        /// </summary>
        /// <param name="purchaseReceivalDetail">PurchaseReceivalDetailModel object</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateCreatePurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, IPurchaseReceivalRepository _purchaseReceivalRepository, IPurchaseOrderRepository _purchaseOrderRepository, out string message)
        {
            bool isValid = true;
            message = "";
            PurchaseReceivalModel purchaseReceivalModel = _purchaseReceivalRepository.GetPurchaseReceival(purchaseReceivalDetail.PurchaseReceivalId);
            if (purchaseReceivalModel == null)
            {
                isValid = false;
                message = "Error Validation: No associated delivery order found...";
                return isValid;
            }
            PurchaseOrderDetailModel purchaseOrderDetailModel = _purchaseOrderRepository.GetPurchaseOrderDetail(purchaseReceivalDetail.PurchaseOrderDetailId);
            if (purchaseOrderDetailModel == null)
            {
                isValid = false;
                message = "Error Validation: No associated sales order detail found...";
                return false;
            }
            PurchaseOrderModel purchaseOrderModel = _purchaseOrderRepository.GetPurchaseOrder(purchaseOrderDetailModel.PurchaseOrderId);
            if (purchaseOrderModel == null)
            {
                isValid = false;
                message = "Error Validation: No associated sales order found...";
                return false;
            }
            if (purchaseReceivalDetail.PurchaseOrderDetailId == null ||
                purchaseReceivalModel.ContactId == null ||
                purchaseOrderDetailModel.PurchaseOrderId == null ||

                purchaseReceivalDetail.Quantity == 0 ||
                purchaseReceivalDetail.Quantity > purchaseOrderDetailModel.PendingReceival ||
                purchaseOrderDetailModel.PendingReceival <= 0 ||
                purchaseOrderDetailModel.IsConfirmed == false)
            {
                isValid = false;
                message = "Error Validation: Incomplete data...";
                return false;

            }
            if (purchaseOrderModel.ContactId != purchaseReceivalModel.ContactId)
            {
                isValid = false;
                message = "Error Validation: Different contact person...";
                return false;
            }
            message = "Successful Validation...";
            return true;
        }

        /// <summary>
        /// Validate purchase receival detail when it is updated.
        /// </summary>
        /// <param name="purchaseReceivalDetail">PurchaseReceivalDetailModel object</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateUpdatePurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, IPurchaseReceivalRepository _purchaseReceivalRepository, IPurchaseOrderRepository _purchaseOrderRepository, out string message)
        {
            return this.ValidateCreatePurchaseReceivalDetail(purchaseReceivalDetail, _purchaseReceivalRepository, _purchaseOrderRepository, out message);
        }

        /// <summary>
        /// Validate purchase receival detail when it is deleted.
        /// Purchase receival detail can't be deleted if it is confirmed
        /// </summary>
        /// <param name="purchaseReceivalDetail">PurchaseReceivalDetailModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateDeletePurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, out string message)
        {
            message = "";
            if (purchaseReceivalDetail.IsConfirmed)
            {
                message = "Can't be destroyed. Purchase Receival Detail is confirmed.";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validate purchase receival detail when it is confirmed.
        /// </summary>
        /// <param name="purchaseReceivalDetail">PurchaseReceivalDetailModel object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>       
        public bool ValidateConfirmPurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, out string message)
        {
            message = "";
            return true;
        }

        /// <summary>
        /// Validate purchase receival detail when it is unconfirmed.
        /// Invalid if:
        /// 1. Quantity of item.PendingReceival will be less than 0 after unconfirmation
        /// 2. Quantity of item.Ready will be less than 0 after unconfirmation
        /// </summary>
        /// <param name="purchaseReceivalDetail">PurchaseReceivalDetailModel object</param>
        /// <param name="_purchaseReceivalRepository">IPurchaseReceivalRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>       
        public bool ValidateUnconfirmPurchaseReceivalDetail(PurchaseReceivalDetailModel purchaseReceivalDetail, IPurchaseReceivalRepository _purchaseReceivalRepository, IItemRepository _itemRepository, out string message)
        {
            message = "";
            Item item = _itemRepository.Find(x => x.Id == purchaseReceivalDetail.ItemId && !x.IsDeleted);
            if (item == null)
            {
                message = "Item can't be found";
                return false;
            }
            if ((item.PendingReceival - purchaseReceivalDetail.Quantity) < 0)
            {
                message = "Can't unconfirm. Not enough amount in stock Pending Receival...";
                return false;
            }
            if ((item.Ready - purchaseReceivalDetail.Quantity) < 0)
            {
                message = "Can't unconfirm. Not enough amount in stock Ready...";
            }

            return true;
        }

    }
}