using StockControl.Models;
using StockControl.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace StockControl.Service
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("PurchaseOrderService");

        /*
         * GET
         */

        /// <summary>
        /// Get all purchase orders.
        /// </summary>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <returns>all purchase orders</returns>
        public List<PurchaseOrderModel> GetPurchaseOrderList(IPurchaseOrderRepository _purchaseOrderRepository)
        {
            List<PurchaseOrderModel> model = new List<PurchaseOrderModel>();
            try
            {
                model = _purchaseOrderRepository.GetPurchaseOrderList();
            }
            catch (Exception ex)
            {
                LOG.Error("GetPurchaseOrderList Failed", ex);
            }

            return model;
        }

        /// <summary>
        /// Get a purchase order
        /// </summary>
        /// <param name="orderId">Id of the purchase order</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <returns>a purchase order</returns>
        public PurchaseOrderModel GetPurchaseOrder(int orderId, IPurchaseOrderRepository _purchaseOrderRepository)
        {
            PurchaseOrderModel model = new PurchaseOrderModel();
            try
            {
                model = _purchaseOrderRepository.GetPurchaseOrder(orderId);
            }
            catch (Exception ex)
            {
                LOG.Error("GetPurchaseOrder Failed", ex);
            }

            return model;
        }

        /*
         * CREATE
         */

        /// <summary>
        /// Create a purchase order.
        /// </summary>
        /// <param name="purchaseOrder">PurchaseOrderModel object</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <returns>a purchase order</returns>
        public ResponseModel CreatePurchaseOrder(PurchaseOrderModel purchaseOrder, IPurchaseOrderRepository _purchaseOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateCreatePurchaseOrder(purchaseOrder, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                PurchaseOrder newPurchaseOrder = new PurchaseOrder();
                newPurchaseOrder.Id = purchaseOrder.Id;
                newPurchaseOrder.ContactId = purchaseOrder.ContactId;
                // Code: #{year}/#{total_number_of_purchase_order}
                // TODO:
								
								/*
									total_number_purchase_order = purchaseOrderRepository.Count() + 1;   << something like this. 
									challenge in crafting the LINQ sql = find the total number of purchase orders in the current year
									
								*/
                newPurchaseOrder.Code = "#" + DateTime.Today.Year.ToString() + "/#" + "{total_number_purchase_order}";
                newPurchaseOrder.PurchaseDate = purchaseOrder.PurchaseDate;
                newPurchaseOrder.IsConfirmed = purchaseOrder.IsConfirmed;
                newPurchaseOrder.IsDeleted = purchaseOrder.IsDeleted;
                newPurchaseOrder.CreatedAt = DateTime.Now;
                newPurchaseOrder.UpdatedAt = purchaseOrder.UpdatedAt;
                newPurchaseOrder.DeletedAt = purchaseOrder.DeletedAt;

                newPurchaseOrder = _purchaseOrderRepository.CreatePurchaseOrder(newPurchaseOrder);
                newPurchaseOrder.Id = newPurchaseOrder.Id;

                respModel.isValid = true;
                respModel.message = "Create Purchase Order Success...";
                respModel.objResult = purchaseOrder;

                LOG.Error("CreatePurchaseOrder Sucess");
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
                        LOG.ErrorFormat("CreatePurchaseOrder, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("CreatePurchaseOrder Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Create Purchase Order failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("CreatePurchaseOrder Failed", ex);
                respModel.isValid = false;
                respModel.message = "Create Purchase Order Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * DELETE
         */

        /// <summary>
        /// Delete a purchase order and all its children purchase order details
        /// </summary>
        /// <param name="purchaseOrderId">Id of the purchase order</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository</param>
        /// <returns>a response model</returns>
        public ResponseModel DeletePurchaseOrder(int purchaseOrderId, IPurchaseOrderRepository _purchaseOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                PurchaseOrder deletePurchaseOrder = _purchaseOrderRepository.Find(p => p.Id == purchaseOrderId && !p.IsDeleted);
									
								
                if (deletePurchaseOrder != null)
                {
                    string message = "";
                    PurchaseOrderModel model = PurchaseOrderModel.ToModel(deletePurchaseOrder);
                    respModel.isValid = this.ValidateDeletePurchaseOrder(model, out message);
                    if (!respModel.isValid)
                    {
                        respModel.message = message;
                        return respModel;
                    }

                    // Delete PurchaseOrder
                    _purchaseOrderRepository.DeletePurchaseOrder(purchaseOrderId);
                    respModel.objResult = model;

                    // Get all subsequent PurchaseOrderDetails
                    List<PurchaseOrderDetailModel> deletePurchaseOrderDetails = _purchaseOrderRepository.GetPurchaseOrderDetailList(purchaseOrderId);

                    if (deletePurchaseOrderDetails.Count() > 0)
                    {
                        // Delete all subsequent PurchaseOrderDetails
                        _purchaseOrderRepository.DeletePurchaseOrderDetailByPurchaseOrderId(purchaseOrderId);
                        // Adding the line below will set respModel to all the subsequent details
                        // respModel.objResult = deletePurchaseOrderDetails;
                    }

                    respModel.isValid = true;
                    respModel.message = "Delete purchase order and its purchase order details Success...";
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
                        LOG.ErrorFormat("DeletePurchaseOrder, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("DeletePurchaseOrder Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Delete Purchase Order failed, Please try again or purchaseOrder your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("DeletePurchaseOrder Failed", ex);
                respModel.isValid = false;
                respModel.message = "Delete Purchase Order Failed, Please try again or purchaseOrder your administrator.";
            }

            return respModel;
        }

        /*
         * UPDATE
         */

        /// <summary>
        /// Update a purchase order.
        /// </summary>
        /// <param name="purchaseOrder">PurchaseOrderModel object</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel UpdatePurchaseOrder(PurchaseOrderModel purchaseOrder, IPurchaseOrderRepository _purchaseOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateUpdatePurchaseOrder(purchaseOrder, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                PurchaseOrder updatePurchaseOrder = _purchaseOrderRepository.Find(p => p.Id == purchaseOrder.Id && !p.IsDeleted);
                if (updatePurchaseOrder != null)
                {
                    updatePurchaseOrder.Id = purchaseOrder.Id;
                    updatePurchaseOrder.ContactId = purchaseOrder.ContactId;
                    updatePurchaseOrder.Code = purchaseOrder.Code;
                    updatePurchaseOrder.PurchaseDate = purchaseOrder.PurchaseDate;
                    updatePurchaseOrder.IsConfirmed = purchaseOrder.IsConfirmed;
                    updatePurchaseOrder.IsDeleted = purchaseOrder.IsDeleted;
                    updatePurchaseOrder.CreatedAt = purchaseOrder.CreatedAt;
                    updatePurchaseOrder.UpdatedAt = DateTime.Now;
                    updatePurchaseOrder.DeletedAt = purchaseOrder.DeletedAt;

                    _purchaseOrderRepository.UpdatePurchaseOrder(updatePurchaseOrder);

                    respModel.isValid = true;
                    respModel.message = "Update Purchase Order Success...";
                    respModel.objResult = purchaseOrder;

                    LOG.Info("UpdatePurchaseOrder Success");
                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "PurchaseOrder not found...";
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
                        LOG.ErrorFormat("UpdatePurchaseOrder, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UpdatePurchaseOrder Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Update purchase order failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UpdatePurchaseOrder Failed", ex);
                respModel.isValid = false;
                respModel.message = "Update purchase order Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * CONFIRM
         */

        /// <summary>
        /// Confirm purchase order. This function also automatically confirms all its children purchase order details.
        /// </summary>
        /// <param name="purchaseOrder">PurchaseOrderModel object</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel ConfirmPurchaseOrder(PurchaseOrderModel purchaseOrder, IPurchaseOrderRepository _purchaseOrderRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
	
								/*
									purchaseOrder variable come from the user. Dangerous.
									
									When we are about to do action on purchase order, it is better to get the verbatim data from db.
									
									Think about it in this way: all data from user are evil. They can type any value inside. 
									
									For example: they emulate purchaseOrder.Id to be 000 . In the database, there is no such value. You will have error. 
									
								
									
								*/
                string message = "";
                respModel.isValid = this.ValidateConfirmPurchaseOrder(purchaseOrder, _purchaseOrderRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                PurchaseOrder confirmPurchaseOrder = _purchaseOrderRepository.Find(p => p.Id == purchaseOrder.Id && !p.IsDeleted);
                if (confirmPurchaseOrder != null)
                {
										/*
											Just update the necessary field. 
											
											confirmPurchaseOrder.IsConfirmed = true; 
											_purchaseOrderRepository.UpdatePurchaseOrder(confirmPurchaseOrder);
											
											done.. you don't need to assign value from other such as Id, ContactId. 
										*/
                    confirmPurchaseOrder.Id = purchaseOrder.Id;
                    confirmPurchaseOrder.ContactId = purchaseOrder.ContactId;
                    confirmPurchaseOrder.Code = purchaseOrder.Code;
                    confirmPurchaseOrder.PurchaseDate = purchaseOrder.PurchaseDate;
                    confirmPurchaseOrder.IsConfirmed = true;
                    confirmPurchaseOrder.IsDeleted = purchaseOrder.IsDeleted;
                    confirmPurchaseOrder.CreatedAt = purchaseOrder.CreatedAt;
                    confirmPurchaseOrder.UpdatedAt = DateTime.Now;
                    confirmPurchaseOrder.DeletedAt = purchaseOrder.DeletedAt;

                    _purchaseOrderRepository.UpdatePurchaseOrder(confirmPurchaseOrder);

                    LOG.Info("ConfirmPurchaseOrder Success");

                    // Confirm PurchaseOrder Detail
                    List<PurchaseOrderDetailModel> confirmPurchaseOrderDetails = _purchaseOrderRepository.GetPurchaseOrderDetailList(purchaseOrder.Id);
										
										/*
											Double work. Based on the precondition you validated, 
											there must be purchase order details in this given purchase order
										*/
                    if (confirmPurchaseOrderDetails.Count() > 0)
                    {
                        foreach (var dod in confirmPurchaseOrderDetails)
                        {
                            respModel = this.ConfirmPurchaseOrderDetail(dod, _purchaseOrderRepository, _itemRepository, _stockMutationRepository);
                        }
                        LOG.Info("ConfirmPurchaseOrderDetails Success");
                    }

                    respModel.isValid = true;
                    respModel.message = "Confirm Data Success...";
                    respModel.objResult = confirmPurchaseOrder;
                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "PurchaseOrder not found...";
                }
                respModel.isValid = true;
                respModel.message = "Confirm Purchase Order Success...";
                respModel.objResult = purchaseOrder;

                LOG.Info("ConfirmPurchaseOrder Success");

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
                        LOG.ErrorFormat("ConfirmPurchaseOrder, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("ConfirmPurchaseOrder Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Confirm purchase order failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("ConfirmPurchaseOrder Failed", ex);
                respModel.isValid = false;
                respModel.message = "Confirm purchase order Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * UNCONFIRM
         */

        /// <summary>
        /// Unconfirm purchase order.
        /// </summary>
        /// <param name="purchaseOrder">PurchaseOrderModel object</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel UnconfirmPurchaseOrder(PurchaseOrderModel purchaseOrder, IPurchaseOrderRepository _purchaseOrderRepository, IItemRepository _itemRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateUnconfirmPurchaseOrder(purchaseOrder, _purchaseOrderRepository, _itemRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                PurchaseOrder unconfirmPurchaseOrder = _purchaseOrderRepository.Find(p => p.Id == purchaseOrder.Id && !p.IsDeleted);
                if (unconfirmPurchaseOrder != null)
                {
                    unconfirmPurchaseOrder.Id = purchaseOrder.Id;
                    unconfirmPurchaseOrder.ContactId = purchaseOrder.ContactId;
                    unconfirmPurchaseOrder.Code = purchaseOrder.Code;
                    unconfirmPurchaseOrder.PurchaseDate = purchaseOrder.PurchaseDate;
                    unconfirmPurchaseOrder.IsConfirmed = false;
                    unconfirmPurchaseOrder.IsDeleted = purchaseOrder.IsDeleted;
                    unconfirmPurchaseOrder.CreatedAt = purchaseOrder.CreatedAt;
                    unconfirmPurchaseOrder.UpdatedAt = DateTime.Now;
                    unconfirmPurchaseOrder.DeletedAt = purchaseOrder.DeletedAt;

                    _purchaseOrderRepository.UpdatePurchaseOrder(unconfirmPurchaseOrder);

                    LOG.Info("UnconfirmPurchaseOrder Success");

                    // Unconfirm PurchaseOrder Detail
                    List<PurchaseOrderDetailModel> unconfirmPurchaseOrderDetails = _purchaseOrderRepository.GetPurchaseOrderDetailList(purchaseOrder.Id);

                    if (unconfirmPurchaseOrderDetails.Count() > 0)
                    {
                        _purchaseOrderRepository.UpdateConfirmationPurchaseOrderDetailByPurchaseOrderId(purchaseOrder.Id, false);
                    }

                    LOG.Info("UnconfirmPurchaseOrderDetails Success");

                    respModel.isValid = true;
                    respModel.message = "Unconfirm Data Success...";
                    respModel.objResult = purchaseOrder;
                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "PurchaseOrder not found...";
                }
                respModel.isValid = true;
                respModel.message = "Unconfirm Purchase Order Success...";
                respModel.objResult = purchaseOrder;

                LOG.Info("UnconfirmPurchaseOrder Success");

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
                        LOG.ErrorFormat("UnconfirmPurchaseOrder, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UnconfirmPurchaseOrder Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Unconfirm purchase order failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UnconfirmPurchaseOrder Failed", ex);
                respModel.isValid = false;
                respModel.message = "Unconfirm purchase order Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * VALIDATE
         */

        /// <summary>
        /// Private function to validate a purchase order.
        /// </summary>
        /// <param name="model">PurchaseOrderModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        private bool Validate(PurchaseOrderModel model, out string message)
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
            return isValid;
        }

        /// <summary>
        /// Validate a purchase order when it is created.
        /// </summary>
        /// <param name="model">PurchaseOrderModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateCreatePurchaseOrder(PurchaseOrderModel model, out string message)
        {
            bool isValid = this.Validate(model, out message);

            return isValid;
        }

        /// <summary>
        /// Validate a purchase order when it is updated.
        /// </summary>
        /// <param name="model">PurchaseOrderModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateUpdatePurchaseOrder(PurchaseOrderModel model, out string message)
        {
            bool isValid = this.Validate(model, out message);

            if (model.IsConfirmed)
            {
                message = "Confirmed purchase order cannot be updated. Please try again or contact your administrator.";
                return false;
            }

            return isValid;
        }

        /// <summary>
        /// Validate a purchase order when it is deleted.
        /// It is invalid when a purchase order has been confirmed.
        /// </summary>
        /// <param name="model">PurchaseOrderModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateDeletePurchaseOrder(PurchaseOrderModel model, out string message)
        {
            bool isValid = true;
            message = "";

            if (model.IsConfirmed)
            {
                message = "Can't destroyed confirmed purchase order. Please try again or contact your administrator";
                return false;
            }

            return isValid;
        }

        /// <summary>
        /// Validate a purchase order when it is confirmed.
        /// It is valid when it has any purchase order details.
        /// </summary>
        /// <param name="model">PurchaseOrderModel object</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateConfirmPurchaseOrder(PurchaseOrderModel model, IPurchaseOrderRepository _purchaseOrderRepository, out string message)
        {
            bool isValid = true;
            message = "";
						/*
							Kurang.
							
							if( current purchase order is confirmed, return false. Already confirmed).
							
							if( model.isConfirmed){
								message = "Sudah di konfirmasi"
								return false 
							}
							
							rationale: current pending_receival stock == 5 
							on confirming pending_receival stock, you will have added x 
							
							if you do double or triple confirm, you will have 5+ x  + x + x pending_receival 
						*/

            if (_purchaseOrderRepository.GetPurchaseOrderDetailList(model.Id).Count() == 0)
            {
                message = "There is no existing Purchase Order Detail...";
                return false;
            }
						
						

            return isValid;
        }


        /// <summary>
        /// Validate a purchase order when it is unconfirmed.
        /// It is valid when all its purchase order detail is joined to an item with PendingReceival gt 0
        /// </summary>
        /// <param name="purchaseOrder">PurchaseOrderModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateUnconfirmPurchaseOrder(PurchaseOrderModel purchaseOrder, IPurchaseOrderRepository _purchaseOrderRepository, IItemRepository _itemRepository, out string message)
        {
            bool isValid = true;
            message = "";

            List<PurchaseOrderDetailModel> model = _purchaseOrderRepository.GetPurchaseOrderDetailList(purchaseOrder.Id);
            if (model.Count() > 0)
            {
                foreach (var eachdetail in model)
                {
                    ItemModel item = _itemRepository.GetItem(eachdetail.ItemId);
                    if (item.PendingReceival < 0)
                    {
                        isValid = false;
                        message = "Invalid unconfirmation. Pending Receival quantity of the item is less than 0";
                    }
                }
            }
            return isValid;
        }

        /*
         * GET DETAIL
         */

        /// <summary>
        /// Get all purchase order details.
        /// </summary>
        /// <param name="purchaseOrderId">Id of the purchase order</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <returns>all purchase order details associated to the purchase order</returns>
        public List<PurchaseOrderDetailModel> GetPurchaseOrderDetailList(int purchaseOrderId, IPurchaseOrderRepository _purchaseOrderRepository)
        {
            List<PurchaseOrderDetailModel> model = new List<PurchaseOrderDetailModel>();
            try
            {
                model = _purchaseOrderRepository.GetPurchaseOrderDetailList(purchaseOrderId);
            }
            catch (Exception ex)
            {
                LOG.Error("GetPurchaseOrderDetailList Failed", ex);
            }

            return model;
        }

        /// <summary>
        /// Get a purchase order detail
        /// </summary>
        /// <param name="purchaseOrderDetailId">Id of the purchase order detail</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <returns>a purchase order detail</returns>
        public PurchaseOrderDetailModel GetPurchaseOrderDetail(int purchaseOrderDetailId, IPurchaseOrderRepository _purchaseOrderRepository)
        {
            PurchaseOrderDetailModel model = new PurchaseOrderDetailModel();
            try
            {
                model = _purchaseOrderRepository.GetPurchaseOrderDetail(purchaseOrderDetailId);
            }
            catch (Exception ex)
            {
                LOG.Error("GetPurchaseOrderDetail Failed", ex);
            }

            return model;
        }

        /*
         * CREATE DETAIL
         */

        /// <summary>
        /// Create a purchase order detail.
        /// </summary>
        /// <param name="purchaseOrderDetail">PurchaseOrderDetail object</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel CreatePurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, IPurchaseOrderRepository _purchaseOrderRepository, IItemRepository _itemRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateCreatePurchaseOrderDetail(purchaseOrderDetail, _purchaseOrderRepository, _itemRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                PurchaseOrderDetail newPurchaseOrderDetail = new PurchaseOrderDetail();
                newPurchaseOrderDetail.Id = purchaseOrderDetail.Id;
                newPurchaseOrderDetail.PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId;
                // Code: #{year}/#{purchase_order.code}/#{total_number_of_non_deleted_purchase_order_Entry}
                // TODO:
                newPurchaseOrderDetail.Code = "#" + DateTime.Now.Year.ToString() + "/#" + "Purchase Order Code" + "/#" + "Total Number of Non Deleted Purchase Order Entry";
                newPurchaseOrderDetail.Quantity = purchaseOrderDetail.Quantity;
                newPurchaseOrderDetail.ItemId = purchaseOrderDetail.ItemId;
                newPurchaseOrderDetail.PendingReceival = purchaseOrderDetail.PendingReceival;
                newPurchaseOrderDetail.IsConfirmed = purchaseOrderDetail.IsConfirmed;
                newPurchaseOrderDetail.IsFulfilled = purchaseOrderDetail.IsFulfilled;
                newPurchaseOrderDetail.IsDeleted = purchaseOrderDetail.IsDeleted;
                newPurchaseOrderDetail.CreatedAt = DateTime.Now;
                newPurchaseOrderDetail.UpdatedAt = purchaseOrderDetail.UpdatedAt;
                newPurchaseOrderDetail.DeletedAt = purchaseOrderDetail.DeletedAt;

                newPurchaseOrderDetail = _purchaseOrderRepository.CreatePurchaseOrderDetail(newPurchaseOrderDetail);
                newPurchaseOrderDetail.Id = newPurchaseOrderDetail.Id;

                respModel.isValid = true;
                respModel.message = "Create Purchase Order Detail Success...";
                respModel.objResult = purchaseOrderDetail;

                LOG.Error("CreatePurchaseOrderDetail Sucess");
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
                        LOG.ErrorFormat("CreatePurchaseOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("CreatePurchaseOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Create PurchaseOrderDetail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("CreatePurchaseOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Create PurchaseOrderDetail Failed, Please try again or contact your administrator.";
            }

            return respModel;

        }


        /// <summary>
        /// Please use DeletePurchaseOrder() instead of this function. This only deletes all the deliver order details without deleting its
        /// parent. DeletePurchaseOrder() will delete purchase order and all its purchase order details.
        /// </summary>
        /// <param name="purchaseOrderId">Id of the purchase order</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel DeletePurchaseOrderDetailByPurchaseOrderId(int purchaseOrderId, IPurchaseOrderRepository _purchaseOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                List<PurchaseOrderDetailModel> model = _purchaseOrderRepository.GetPurchaseOrderDetailList(purchaseOrderId);
                if (model.Count() == 0)
                {
                    respModel.isValid = false;
                    respModel.message = "There's no Purchase Order Detail...";
                    return respModel;
                }
                _purchaseOrderRepository.DeletePurchaseOrderDetailByPurchaseOrderId(purchaseOrderId);

                respModel.message = "Delete Purchase Order Details completed. Please make sure the that purchase order has already been deleted";
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
                        LOG.ErrorFormat("DeletePurchaseOrderDetailByPurchaseOrderId, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("DeletePurchaseOrderDetailByPurchaseOrderId Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Delete DeletePurchaseOrderDetailByPurchaseOrderId failed, Please try again or purchaseOrder your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("DeletePurchaseOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Delete PurchaseOrderDetail Failed, Please try again or purchaseOrder your administrator.";
            }

            return respModel;
        }

        /// <summary>
        /// Delete purchase order detail.
        /// </summary>
        /// <param name="purchaseOrderDetailId">Id of the purchase order detail</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel DeletePurchaseOrderDetail(int purchaseOrderDetailId, IPurchaseOrderRepository _purchaseOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                PurchaseOrderDetailModel model = _purchaseOrderRepository.GetPurchaseOrderDetail(purchaseOrderDetailId);
                if (model != null)
                {
                    string message = "";
                    respModel.isValid = this.ValidateDeletePurchaseOrderDetail(model, out message);
                    if (!respModel.isValid)
                    {
                        respModel.message = message;
                        return respModel;
                    }

                    // Delete PurchaseOrderDetail
                    _purchaseOrderRepository.DeletePurchaseOrderDetail(purchaseOrderDetailId);

                    respModel.isValid = true;
                    respModel.message = "Delete PurchaseOrderDetail Success...";
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
                        LOG.ErrorFormat("DeletePurchaseOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("DeletePurchaseOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Delete PurchaseOrderDetail failed, Please try again or purchaseOrder your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("DeletePurchaseOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Delete PurchaseOrderDetail Failed, Please try again or purchaseOrder your administrator.";
            }

            return respModel;
        }

        /*
         * UPDATE DETAIL
         */

        /// <summary>
        /// Update a purchase order detail.
        /// </summary>
        /// <param name="purchaseOrderDetail">PurchaseOrderDetailModel object</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel UpdatePurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, IPurchaseOrderRepository _purchaseOrderRepository, IItemRepository _itemRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateUpdatePurchaseOrderDetail(purchaseOrderDetail, _purchaseOrderRepository, _itemRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                PurchaseOrderDetail dod = new PurchaseOrderDetail();
                dod.Id = purchaseOrderDetail.Id;
                dod.PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId;
                dod.Code = purchaseOrderDetail.Code;
                dod.Quantity = purchaseOrderDetail.Quantity;
                dod.ItemId = purchaseOrderDetail.ItemId;
                dod.PendingReceival = purchaseOrderDetail.PendingReceival;
                dod.IsConfirmed = purchaseOrderDetail.IsConfirmed;
                dod.IsFulfilled = purchaseOrderDetail.IsFulfilled;
                dod.IsDeleted = purchaseOrderDetail.IsDeleted;
                dod.CreatedAt = purchaseOrderDetail.CreatedAt;
                dod.UpdatedAt = DateTime.Now;
                dod.DeletedAt = purchaseOrderDetail.DeletedAt;

                _purchaseOrderRepository.UpdatePurchaseOrderDetail(dod);

                respModel.isValid = true;
                respModel.message = "Update Purchase Order Detail Success...";
                respModel.objResult = purchaseOrderDetail;

                LOG.Info("UpdatePurchaseOrderDetail Success");
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
                        LOG.ErrorFormat("UpdatePurchaseOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UpdatePurchaseOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Update PurchaseOrderDetail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UpdatePurchaseOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Update PurchaseOrderDetail Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /// <summary>
        /// This function calls an update function given the parameter 'Confirm' or 'Unconfirm'.
        /// </summary>
        /// <param name="purchaseOrderDetail">PurchaseOrderModel object</param>
        /// <param name="IsConfirm">Is this function Confirm or Unconfirm</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <returns>a response model</returns>
        ResponseModel UpdateConfirmationPurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, bool IsConfirm, IPurchaseOrderRepository _purchaseOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                String message = IsConfirm ? "Update Confirm" : "Update Unconfirm";
                PurchaseOrderDetailModel confirmPurchaseOrderDetail = _purchaseOrderRepository.GetPurchaseOrderDetail(purchaseOrderDetail.Id);

                if (confirmPurchaseOrderDetail != null)
                {
                    _purchaseOrderRepository.UpdateConfirmationPurchaseOrderDetailByPurchaseOrderId(confirmPurchaseOrderDetail.Id, IsConfirm);

                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "PurchaseOrderDetail not found...";
                }
                respModel.isValid = true;
                respModel.message = message + "PurchaseOrderDetail Success...";
                respModel.objResult = purchaseOrderDetail;

                LOG.Info(message + "PurchaseOrderDetail Success");

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
                        LOG.ErrorFormat("UpdateConfirmationPurchaseOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UpdateConfirmationPurchaseOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Update Confirmation Purchase Order Detail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UpdateConfirmationPurchaseOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Update Confirmation Purchase Order Detail Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * CONFIRM DETAIL
         */

        /// <summary>
        /// Confirm a purchase order detail.
        /// </summary>
        /// <param name="purchaseOrderDetail">PurchaseOrderDetailModel object</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="_stockMutationRepository">IStockMutation object</param>
        /// <returns>a response model</returns>
        public ResponseModel ConfirmPurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, IPurchaseOrderRepository _purchaseOrderRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            String message = "";
            try
            {
								/*
									Error prone. Think about this.
									
									PurchaseOrder can be confirmed. If the PurchaseOrderDetails can't be confirmed, you will have
										a. A confirmed PurchaseOrder
										b. N Unconfirmed PurchaseOrderDetails whose PurchaseOrder is confirmed. error? 
										
									You should validate the byproducts first thing first. If the byproducts (PurcahseOrderDetail) can be validated, validate the original source (PurchaseOrder)
								*/
                respModel.isValid = this.ValidateConfirmPurchaseOrderDetail(purchaseOrderDetail, out message);
                if (!respModel.isValid) { return respModel; }
								/*
									else{
										1. get the purchaseOrderDetail
										2. purchaseOrderDetail.IsConfirmed = true 
										3. purchaseOrderDetailRepository.Update( purchaseOrderDetail ) 
									
									}
								*/

                respModel = this.UpdateConfirmationPurchaseOrderDetail(purchaseOrderDetail, true, _purchaseOrderRepository);

                Item item = _itemRepository.Find(x => x.Id == purchaseOrderDetail.ItemId && !x.IsDeleted);
                if (item == null)
                {
                    respModel.isValid = false;
                    respModel.message = "No item found...";
                    return respModel;
                }

								/*
									Why do you create a new purchase order detail?  
									
									it will produce error since Id is primary key. Must be unique. 
								*/
                PurchaseOrderDetail sod = new PurchaseOrderDetail();
                sod.Id = purchaseOrderDetail.Id;
                sod.PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId;
                sod.Code = purchaseOrderDetail.Code;
                sod.Quantity = purchaseOrderDetail.Quantity;
                sod.ItemId = purchaseOrderDetail.ItemId;
                sod.PendingReceival = purchaseOrderDetail.Quantity; // updated
                sod.IsConfirmed = true;
                sod.IsFulfilled = purchaseOrderDetail.IsFulfilled;
                sod.IsDeleted = purchaseOrderDetail.IsDeleted;
                sod.CreatedAt = purchaseOrderDetail.CreatedAt;
                sod.UpdatedAt = DateTime.Now;
                sod.DeletedAt = purchaseOrderDetail.DeletedAt;
                PurchaseOrderDetail model = _purchaseOrderRepository.UpdatePurchaseOrderDetail(sod);

                item.PendingReceival += purchaseOrderDetail.Quantity;
                item.UpdatedAt = DateTime.Now;
                _itemRepository.UpdateItem(item);

                LOG.Info("Updating Item " + item.Sku + " Adding Pending Receival By " + purchaseOrderDetail.Quantity);

                StockMutation stockMutationPendingReceival = new StockMutation();
                stockMutationPendingReceival.ItemId = item.Sku;
                stockMutationPendingReceival.Quantity = purchaseOrderDetail.Quantity;
                stockMutationPendingReceival.SourceDocument = "PurchaseOrder";
                stockMutationPendingReceival.SourceDocumentId = purchaseOrderDetail.PurchaseOrderId;
                stockMutationPendingReceival.SourceDocumentDetail = "PurchaseOrderDetail";
                stockMutationPendingReceival.SourceDocumentDetailId = purchaseOrderDetail.Id;

								/*
									Use Constant. 
									
									create another class. 
									
									public class StockMutationConstant{
										public static int PendingReceivalCase = 2;
										public static int Ready = 0  ; 
										public static int PendingDelivery = 1;  << something like this. 
										
										public static int AdditionMutationCase = 0;  << something like this. 
										public static int DeductionMutationCase = 1; 
									}
									
									Hence, when you are assigning value to the object, you can do it like this:
									
									stockMutationPendingReceival.ItemCase = StockMutationConstant.PendingReceivalCase ; 
									stockMutationPendingReceival.MutationCase = StockMutationConstant.AdditionMutationCase ; // Addition
								*/
                stockMutationPendingReceival.ItemCase = 2; // Pending Receival
                stockMutationPendingReceival.MutationCase = 1; // Addition

                stockMutationPendingReceival.IsDeleted = false;
                stockMutationPendingReceival.CreatedAt = DateTime.Now;
                stockMutationPendingReceival.UpdatedAt = null;
                stockMutationPendingReceival.DeletedAt = null;

                stockMutationPendingReceival = _stockMutationRepository.CreateStockMutation(stockMutationPendingReceival);
                stockMutationPendingReceival.Id = stockMutationPendingReceival.Id;

                LOG.Info("Creating Stock Mutation Pending Receival");

                respModel.message = "Success updating item and creating stock mutation";
                respModel.objResult = purchaseOrderDetail;
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
                        LOG.ErrorFormat("ConfirmPurchaseOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("ConfirmPurchaseOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Confirm PurchaseOrderDetail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("ConfirmPurchaseOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Confirm PurchaseOrderDetail Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * UNCONFIRM DETAIL
         */

        /// <summary>
        /// Unconfirm a purchase order detail.
        /// </summary>
        /// <param name="purchaseOrderDetail">PurchaseOrderDetailModel object</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel UnconfirmPurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, IPurchaseOrderRepository _purchaseOrderRepository, IPurchaseReceivalRepository _purchaseReceivalRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository)
        {
            String message = "";
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "";
            respModel.objResult = null;

            try
            {
                respModel.isValid = ValidateUnconfirmPurchaseOrderDetail(purchaseOrderDetail, _purchaseOrderRepository, _purchaseReceivalRepository, _itemRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                Item item = _itemRepository.Find(x => x.Id == purchaseOrderDetail.ItemId && !x.IsDeleted);
                if (item == null)
                {
                    respModel.isValid = false;
                    respModel.message = "No item found...";
                    return respModel;
                }

                respModel = this.UpdateConfirmationPurchaseOrderDetail(purchaseOrderDetail, false, _purchaseOrderRepository);

								
								
                PurchaseOrderDetail updatedPurchaseOrderDetail = new PurchaseOrderDetail();
                updatedPurchaseOrderDetail.Id = purchaseOrderDetail.Id;
                updatedPurchaseOrderDetail.PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId;
                updatedPurchaseOrderDetail.Code = purchaseOrderDetail.Code;
                updatedPurchaseOrderDetail.Quantity = purchaseOrderDetail.Quantity;
                updatedPurchaseOrderDetail.ItemId = purchaseOrderDetail.ItemId;
                updatedPurchaseOrderDetail.PendingReceival = purchaseOrderDetail.PendingReceival - purchaseOrderDetail.Quantity;
                updatedPurchaseOrderDetail.IsConfirmed = false;
                updatedPurchaseOrderDetail.IsFulfilled = purchaseOrderDetail.IsFulfilled;
                updatedPurchaseOrderDetail.IsDeleted = purchaseOrderDetail.IsDeleted;
                updatedPurchaseOrderDetail.CreatedAt = purchaseOrderDetail.CreatedAt;
                updatedPurchaseOrderDetail.UpdatedAt = DateTime.Now;
                updatedPurchaseOrderDetail.DeletedAt = purchaseOrderDetail.DeletedAt;

                updatedPurchaseOrderDetail = _purchaseOrderRepository.UpdatePurchaseOrderDetail(updatedPurchaseOrderDetail);

                item.PendingReceival -= purchaseOrderDetail.Quantity; // PendingReceival should be 0
                item.UpdatedAt = DateTime.Now;
                _itemRepository.UpdateItem(item);

                List<StockMutation> sm = _stockMutationRepository.GetStockMutationBySourceDocumentDetail(ItemModel.ToModel(item), "PurchaseOrderDetail", purchaseOrderDetail.Id);
                if (sm.Count() != 0)
                {
                    foreach (var stockMutation in sm)
                    {
                        _stockMutationRepository.DeleteStockMutation(stockMutation.Id);
                    }
                }

                LOG.Info("Updating Item " + item.Sku + " Reducing Pending Receival By " + purchaseOrderDetail.Quantity);

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
                        LOG.ErrorFormat("UnconfirmPurchaseOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UnconfirmPurchaseOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "UnconfirmPurchaseOrderDetail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UnconfirmPurchaseOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "UnconfirmPurchaseOrderDetail Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * VALIDATE DETAIL
         */

        /// <summary>
        /// Validate a purchase order detail when it is created.
        /// It is valid if it asserts the following rules:
        /// The following attributes must be present:
        /// 1. PurchaseOrderId, Item must be present
        /// TODO:
        /// 2. In a given purchase order, the sold item at each PurchaseOrderDetail must be unique
        /// 3. Quantity ge 0
        /// </summary>
        /// <param name="purchaseOrderDetail">PurchaseOrderDetailModel object</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateCreatePurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, IPurchaseOrderRepository _purchaseOrderRepository, IItemRepository _itemRepository, out string message)
        {
            bool isValid = true;
            message = "";
            PurchaseOrderModel purchaseOrderModel = _purchaseOrderRepository.GetPurchaseOrder(purchaseOrderDetail.PurchaseOrderId);
            if (purchaseOrderModel == null)
            {
                isValid = false;
                message = "Error Validation: No associated purchase order found...";
                return isValid;
            }
            ItemModel itemModel = _itemRepository.GetItem(purchaseOrderDetail.ItemId);
            if (itemModel == null)
            {
                isValid = false;
                message = "Error Validation: No associated item found...";
                return false;
            }
            if (purchaseOrderDetail.PurchaseOrderId == null ||
                purchaseOrderDetail.ItemId == null ||
                purchaseOrderDetail.Quantity < 0 ||
                purchaseOrderDetail.IsConfirmed == false)
            {
                isValid = false;
                message = "Error Validation: Incomplete data...";
                return false;

            }
            message = "Successful Validation...";
            return true;
        }

        /// <summary>
        /// Validate purchase order detail when it is updated.
        /// </summary>
        /// <param name="purchaseOrderDetail">PurchaseOrderDetailModel object</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateUpdatePurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, IPurchaseOrderRepository _purchaseOrderRepository, IItemRepository _itemRepository, out string message)
        {
            return this.ValidateCreatePurchaseOrderDetail(purchaseOrderDetail, _purchaseOrderRepository, _itemRepository, out message);
        }

        /// <summary>
        /// Validate purchase order detail when it is deleted.
        /// Purchase order detail can't be deleted if it is confirmed
        /// </summary>
        /// <param name="purchaseOrderDetail">PurchaseOrderDetailModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateDeletePurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, out string message)
        {
            message = "";
            if (purchaseOrderDetail.IsConfirmed)
            {
                message = "Can't be destroyed. Purchase Order Detail is confirmed.";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validate purchase order detail when it is confirmed.
        /// </summary>
        /// <param name="purchaseOrderDetail">PurchaseOrderDetailModel object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>       
        public bool ValidateConfirmPurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, out string message)
        {
           

						message = "";
            return true;
        }

        /// <summary>
        /// Validate purchase order detail when it is unconfirmed.
        /// Invalid if:
        /// 1. Quantity of item.PendingReceival will be less than 0 after unconfirmation
        /// 2. There is confirmed purchase receival detail
        /// </summary>
        /// <param name="purchaseOrderDetail">PurchaseOrderDetailModel object</param>
        /// <param name="_purchaseOrderRepository">IPurchaseOrderRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>       
        public bool ValidateUnconfirmPurchaseOrderDetail(PurchaseOrderDetailModel purchaseOrderDetail, IPurchaseOrderRepository _purchaseOrderRepository, IPurchaseReceivalRepository _purchaseReceivalRepository, IItemRepository _itemRepository, out string message)
        {
            message = "";
            Item item = _itemRepository.Find(x => x.Id == purchaseOrderDetail.ItemId && !x.IsDeleted);
            if (item == null)
            {
                message = "Item can't be found";
                return false;
            }
            if ((item.PendingReceival - purchaseOrderDetail.Quantity) < 0)
            {
                message = "Can't unconfirm. Not enough amount in stock Pending Delivery...";
                return false;
            }

            List<PurchaseReceivalDetailModel> prdm = _purchaseReceivalRepository.GetPurchaseReceivalDetailByPurchaseOrderDetail(purchaseOrderDetail.Id);

            if (prdm.Count() > 0)
            {
                foreach (var model in prdm)
                {
                    if (model.IsConfirmed)
                    {
                        message = "Invalid Unconfirmation. There is confirmed purchase receival detail in the system.";
                        return false;
                    }
                }
            }

            return true;
        }

    }
}