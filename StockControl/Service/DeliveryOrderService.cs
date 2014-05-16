using StockControl.Models;
using StockControl.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace StockControl.Service
{
    public class DeliveryOrderService : IDeliveryOrderService
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("DeliveryOrderService");

        /*
         * GET
         */

        /// <summary>
        /// Get all delivery orders.
        /// </summary>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository object</param>
        /// <returns>all delivery orders</returns>
        public List<DeliveryOrderModel> GetDeliveryOrderList(IDeliveryOrderRepository _deliveryOrderRepository)
        {
            List<DeliveryOrderModel> model = new List<DeliveryOrderModel>();
            try
            {
                model = _deliveryOrderRepository.GetDeliveryOrderList();
            }
            catch (Exception ex)
            {
                LOG.Error("GetDeliveryOrderList Failed", ex);
            }

            return model;
        }

        /// <summary>
        /// Get a delivery order
        /// </summary>
        /// <param name="orderId">Id of the delivery order</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository object</param>
        /// <returns>a delivery order</returns>
        public DeliveryOrderModel GetDeliveryOrder(int orderId, IDeliveryOrderRepository _deliveryOrderRepository)
        {
            DeliveryOrderModel model = new DeliveryOrderModel();
            try
            {
                model = _deliveryOrderRepository.GetDeliveryOrder(orderId);
            }
            catch (Exception ex)
            {
                LOG.Error("GetDeliveryOrder Failed", ex);
            }

            return model;
        }

        /*
         * CREATE
         */

        /// <summary>
        /// Create a delivery order.
        /// </summary>
        /// <param name="deliveryOrder">DeliveryOrderModel object</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository object</param>
        /// <returns>a delivery order</returns>
        public ResponseModel CreateDeliveryOrder(DeliveryOrderModel deliveryOrder, IDeliveryOrderRepository _deliveryOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateCreateDeliveryOrder(deliveryOrder, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                DeliveryOrder newDeliveryOrder = new DeliveryOrder();
                newDeliveryOrder.Id = deliveryOrder.Id;
                newDeliveryOrder.ContactId = deliveryOrder.ContactId;
                newDeliveryOrder.Code = deliveryOrder.Code;
                newDeliveryOrder.DeliveryDate = deliveryOrder.DeliveryDate;
                newDeliveryOrder.IsConfirmed = deliveryOrder.IsConfirmed;
                newDeliveryOrder.IsDeleted = deliveryOrder.IsDeleted;
                newDeliveryOrder.CreatedAt = DateTime.Now;
                newDeliveryOrder.UpdatedAt = deliveryOrder.UpdatedAt;
                newDeliveryOrder.DeletedAt = deliveryOrder.DeletedAt;

                newDeliveryOrder = _deliveryOrderRepository.CreateDeliveryOrder(newDeliveryOrder);
                newDeliveryOrder.Id = newDeliveryOrder.Id;

                respModel.isValid = true;
                respModel.message = "Create Delivery Order Success...";
                respModel.objResult = deliveryOrder;

                LOG.Error("CreateDeliveryOrder Sucess");
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
                        LOG.ErrorFormat("CreateDeliveryOrder, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("CreateDeliveryOrder Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Create Delivery Order failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("CreateDeliveryOrder Failed", ex);
                respModel.isValid = false;
                respModel.message = "Create Delivery Order Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * DELETE
         */

        /// <summary>
        /// Delete a delivery order and all its children delivery order details
        /// </summary>
        /// <param name="deliveryOrderId">Id of the delivery order</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository</param>
        /// <returns>a response model</returns>
        public ResponseModel DeleteDeliveryOrder(int deliveryOrderId, IDeliveryOrderRepository _deliveryOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                DeliveryOrder deleteDeliveryOrder = _deliveryOrderRepository.Find(p => p.Id == deliveryOrderId && !p.IsDeleted);
                if (deleteDeliveryOrder != null)
                {
                    string message = "";
                    DeliveryOrderModel model = DeliveryOrderModel.ToModel(deleteDeliveryOrder);
                    respModel.isValid = this.ValidateDeleteDeliveryOrder(model, out message);
                    if (!respModel.isValid)
                    {
                        respModel.message = message;
                        return respModel;
                    }

                    // Delete DeliveryOrder
                    _deliveryOrderRepository.DeleteDeliveryOrder(deliveryOrderId);
                    respModel.objResult = model;

                    // Get all subsequent DeliveryOrderDetails
                    List<DeliveryOrderDetailModel> deleteDeliveryOrderDetails = _deliveryOrderRepository.GetDeliveryOrderDetailList(deliveryOrderId);

                    if (deleteDeliveryOrderDetails.Count() > 0)
                    {
                        // Delete all subsequent DeliveryOrderDetails
                        _deliveryOrderRepository.DeleteDeliveryOrderDetailByDeliveryOrderId(deliveryOrderId);
                        // Adding the line below will set respModel to all the subsequent details
                        // respModel.objResult = deleteDeliveryOrderDetails;
                    }

                    respModel.isValid = true;
                    respModel.message = "Delete delivery order and its delivery order details Success...";
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
                        LOG.ErrorFormat("DeleteDeliveryOrder, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("DeleteDeliveryOrder Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Delete Delivery Order failed, Please try again or deliveryOrder your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("DeleteDeliveryOrder Failed", ex);
                respModel.isValid = false;
                respModel.message = "Delete Delivery Order Failed, Please try again or deliveryOrder your administrator.";
            }

            return respModel;
        }

        /*
         * UPDATE
         */

        /// <summary>
        /// Update a delivery order.
        /// </summary>
        /// <param name="deliveryOrder">DeliveryOrderModel object</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel UpdateDeliveryOrder(DeliveryOrderModel deliveryOrder, IDeliveryOrderRepository _deliveryOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateUpdateDeliveryOrder (deliveryOrder, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                DeliveryOrder updateDeliveryOrder = _deliveryOrderRepository.Find(p => p.Id == deliveryOrder.Id && !p.IsDeleted);
                if (updateDeliveryOrder != null)
                {
                    updateDeliveryOrder.Id = deliveryOrder.Id;
                    updateDeliveryOrder.ContactId = deliveryOrder.ContactId;
                    updateDeliveryOrder.Code = deliveryOrder.Code;
                    updateDeliveryOrder.DeliveryDate = deliveryOrder.DeliveryDate;
                    updateDeliveryOrder.IsConfirmed = deliveryOrder.IsConfirmed;
                    updateDeliveryOrder.IsDeleted = deliveryOrder.IsDeleted;
                    updateDeliveryOrder.CreatedAt = deliveryOrder.CreatedAt;
                    updateDeliveryOrder.UpdatedAt = DateTime.Now;
                    updateDeliveryOrder.DeletedAt = deliveryOrder.DeletedAt;

                    _deliveryOrderRepository.UpdateDeliveryOrder(updateDeliveryOrder);

                    respModel.isValid = true;
                    respModel.message = "Update Delivery Order Success...";
                    respModel.objResult = deliveryOrder;

                    LOG.Info("UpdateDeliveryOrder Success");
                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "DeliveryOrder not found...";
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
                        LOG.ErrorFormat("UpdateDeliveryOrder, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UpdateDeliveryOrder Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Update delivery order failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UpdateDeliveryOrder Failed", ex);
                respModel.isValid = false;
                respModel.message = "Update delivery order Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }
        
        /*
         * CONFIRM
         */

        /// <summary>
        /// Confirm delivery order. This function also automatically confirms all its children delivery order details.
        /// </summary>
        /// <param name="deliveryOrder">DeliveryOrderModel object</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="_stockMutationRepository">IStockMutationRespository object</param>
        /// <returns>a response model</returns>
        public ResponseModel ConfirmDeliveryOrder(DeliveryOrderModel deliveryOrder, IDeliveryOrderRepository _deliveryOrderRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateConfirmDeliveryOrder(deliveryOrder, _deliveryOrderRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                DeliveryOrder confirmDeliveryOrder = _deliveryOrderRepository.Find(p => p.Id == deliveryOrder.Id && !p.IsDeleted);
                if (confirmDeliveryOrder != null)
                {
                    confirmDeliveryOrder.Id = deliveryOrder.Id;
                    confirmDeliveryOrder.ContactId = deliveryOrder.ContactId;
                    confirmDeliveryOrder.Code = deliveryOrder.Code;
                    confirmDeliveryOrder.DeliveryDate = deliveryOrder.DeliveryDate;
                    confirmDeliveryOrder.IsConfirmed = true;
                    confirmDeliveryOrder.IsDeleted = deliveryOrder.IsDeleted;
                    confirmDeliveryOrder.CreatedAt = deliveryOrder.CreatedAt;
                    confirmDeliveryOrder.UpdatedAt = DateTime.Now;
                    confirmDeliveryOrder.DeletedAt = deliveryOrder.DeletedAt;

                    _deliveryOrderRepository.UpdateDeliveryOrder(confirmDeliveryOrder);

                    LOG.Info("ConfirmDeliveryOrder Success");

                    // Confirm DeliveryOrder Detail
                    List<DeliveryOrderDetailModel> confirmDeliveryOrderDetails = _deliveryOrderRepository.GetDeliveryOrderDetailList(deliveryOrder.Id);

                    if (confirmDeliveryOrderDetails.Count() > 0)
                    {
                        foreach (var dod in confirmDeliveryOrderDetails)
                        {
                            respModel = this.ConfirmDeliveryOrderDetail(dod, _deliveryOrderRepository, _itemRepository, _stockMutationRepository);
                        }
                        LOG.Info("ConfirmDeliveryOrderDetails Success");
                    }

                    respModel.isValid = true;
                    respModel.message = "Confirm Delivery Order Success...";
                    respModel.objResult = confirmDeliveryOrder;
                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "DeliveryOrder not found...";
                }
                respModel.isValid = true;
                respModel.message = "Confirm Delivery Order Success...";
                respModel.objResult = deliveryOrder;

                LOG.Info("ConfirmDeliveryOrder Success");

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
                        LOG.ErrorFormat("ConfirmDeliveryOrder, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("ConfirmDeliveryOrder Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Confirm delivery order failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("ConfirmDeliveryOrder Failed", ex);
                respModel.isValid = false;
                respModel.message = "Confirm delivery order Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }
        
        /*
         * UNCONFIRM
         */

        /// <summary>
        /// Unconfirm delivery order.
        /// </summary>
        /// <param name="deliveryOrder">DeliveryOrderModel object</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel UnconfirmDeliveryOrder(DeliveryOrderModel deliveryOrder, IDeliveryOrderRepository _deliveryOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateUnconfirmDeliveryOrder(deliveryOrder, _deliveryOrderRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                DeliveryOrder unconfirmDeliveryOrder = _deliveryOrderRepository.Find(p => p.Id == deliveryOrder.Id && !p.IsDeleted);
                if (unconfirmDeliveryOrder != null)
                {
                    unconfirmDeliveryOrder.Id = deliveryOrder.Id;
                    unconfirmDeliveryOrder.ContactId = deliveryOrder.ContactId;
                    unconfirmDeliveryOrder.Code = deliveryOrder.Code;
                    unconfirmDeliveryOrder.DeliveryDate = deliveryOrder.DeliveryDate;
                    unconfirmDeliveryOrder.IsConfirmed = false;
                    unconfirmDeliveryOrder.IsDeleted = deliveryOrder.IsDeleted;
                    unconfirmDeliveryOrder.CreatedAt = deliveryOrder.CreatedAt;
                    unconfirmDeliveryOrder.UpdatedAt = DateTime.Now;
                    unconfirmDeliveryOrder.DeletedAt = deliveryOrder.DeletedAt;

                    _deliveryOrderRepository.UpdateDeliveryOrder(unconfirmDeliveryOrder);

                    LOG.Info("UnconfirmDeliveryOrder Success");

                    // Unconfirm DeliveryOrder Detail
                    List<DeliveryOrderDetailModel> unconfirmDeliveryOrderDetails = _deliveryOrderRepository.GetDeliveryOrderDetailList(deliveryOrder.Id);

                    if (unconfirmDeliveryOrderDetails.Count() > 0)
                    {
                        _deliveryOrderRepository.UpdateConfirmationDeliveryOrderDetailByDeliveryOrderId(deliveryOrder.Id, false);
                    }

                    LOG.Info("UnconfirmDeliveryOrderDetails Success");

                    respModel.isValid = true;
                    respModel.message = "Unconfirm Data Success...";
                    respModel.objResult = deliveryOrder;
                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "DeliveryOrder not found...";
                }
                respModel.isValid = true;
                respModel.message = "Unconfirm Delivery Order Success...";
                respModel.objResult = deliveryOrder;

                LOG.Info("UnconfirmDeliveryOrder Success");

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
                        LOG.ErrorFormat("UnconfirmDeliveryOrder, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UnconfirmDeliveryOrder Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Unconfirm delivery order failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UnconfirmDeliveryOrder Failed", ex);
                respModel.isValid = false;
                respModel.message = "Unconfirm delivery order Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * VALIDATE
         */

        /// <summary>
        /// Private function to validate a delivery order.
        /// </summary>
        /// <param name="model">DeliveryOrderModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        private bool Validate(DeliveryOrderModel model, out string message)
        {
            bool isValid = true;
            message = "OK";

            // Contact must be present. Contact will never be null.
            // Delivery date must be present
            // 
            /*
            if (model.ContactId == null)
            {
                message = "Invalid data...";
                return false;
            }
             */
            if (model.DeliveryDate == null)
            {
                message = "Invalid data...";
                return false;
            }

            return isValid;
        }

        /// <summary>
        /// Validate a delivery order when it is created.
        /// </summary>
        /// <param name="model">DeliveryOrderModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateCreateDeliveryOrder(DeliveryOrderModel model, out string message)
        {
            bool isValid = this.Validate(model, out message);

            return isValid;
        }

        /// <summary>
        /// Validate a delivery order when it is updated.
        /// </summary>
        /// <param name="model">DeliveryOrderModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateUpdateDeliveryOrder(DeliveryOrderModel model, out string message)
        {
            bool isValid = this.Validate(model, out message);

            if (model.IsConfirmed)
            {
                message = "Confirmed delivery order cannot be updated. Please try again or contact your administrator.";
                return false;
            }

            return isValid;
        }
        /// <summary>
        /// Validate a delivery order when it is deleted.
        /// It is invalid when a delivery order has been confirmed.
        /// </summary>
        /// <param name="model">DeliveryOrderModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateDeleteDeliveryOrder(DeliveryOrderModel model, out string message)
        {
            bool isValid = true;
            message = "";

            if (model.IsConfirmed)
            {
                message = "Can't destroyed confirmed delivery order. Please try again or contact your administrator";
                return false;
            }

            return isValid;
        }

        /// <summary>
        /// Validate a delivery order when it is confirmed.
        /// It is valid when it has any delivery order details.
        /// </summary>
        /// <param name="model">DeliveryOrderModel object</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateConfirmDeliveryOrder(DeliveryOrderModel model, IDeliveryOrderRepository _deliveryOrderRepository, out string message)
        {
            bool isValid = true;
            message = "";

            if (_deliveryOrderRepository.GetDeliveryOrderDetailList(model.Id).Count() == 0)
            {
                message = "There is no existing Delivery Order Detail...";
                return false;
            }
            return isValid;
        }

        /// <summary>
        /// TODO:
        /// Validate a delivery order when it is unconfirmed.
        /// Instruction: Can't unconfirm if item.ready is less than 0
        /// Problem: Upon unconfirmation, both item.Ready and item.PendingDelivery increase in number
        ///          Does this mean validation will be true at all times?
        /// </summary>
        /// <param name="deliveryOrder">DeliveryOrderModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateUnconfirmDeliveryOrder(DeliveryOrderModel deliveryOrder, IDeliveryOrderRepository _deliveryOrderRepository, out string message)
        {
            bool isValid = true;
            message = "";

            return isValid;
        }

        /*
         * GET DETAIL
         */

        /// <summary>
        /// Get all delivery order details.
        /// </summary>
        /// <param name="deliveryOrderId">Id of the delivery order</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository object</param>
        /// <returns>all delivery order details associated to the delivery order</returns>
        public List<DeliveryOrderDetailModel> GetDeliveryOrderDetailList(int deliveryOrderId, IDeliveryOrderRepository _deliveryOrderRepository)
        {
            List<DeliveryOrderDetailModel> model = new List<DeliveryOrderDetailModel>();
            try
            {
                model = _deliveryOrderRepository.GetDeliveryOrderDetailList(deliveryOrderId);
            }
            catch (Exception ex)
            {
                LOG.Error("GetDeliveryOrderDetailList Failed", ex);
            }

            return model;
        }

        /// <summary>
        /// Get a delivery order detail
        /// </summary>
        /// <param name="deliveryOrderDetailId">Id of the delivery order detail</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository object</param>
        /// <returns>a delivery order detail</returns>
        public DeliveryOrderDetailModel GetDeliveryOrderDetail(int deliveryOrderDetailId, IDeliveryOrderRepository _deliveryOrderRepository)
        {
            DeliveryOrderDetailModel model = new DeliveryOrderDetailModel();
            try
            {
                model = _deliveryOrderRepository.GetDeliveryOrderDetail(deliveryOrderDetailId);
            }
            catch (Exception ex)
            {
                LOG.Error("GetDeliveryOrderDetail Failed", ex);
            }

            return model;
        }

        /*
         * CREATE DETAIL
         */

        /// <summary>
        /// Create a delivery order detail.
        /// </summary>
        /// <param name="deliveryOrderDetail">DeliveryOrderDetail object</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel CreateDeliveryOrderDetail(DeliveryOrderDetailModel deliveryOrderDetail, IDeliveryOrderRepository _deliveryOrderRepository, ISalesOrderRepository _salesOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateCreateUpdateDeliveryOrderDetail(deliveryOrderDetail, _deliveryOrderRepository, _salesOrderRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                DeliveryOrderDetail newDeliveryOrderDetail = new DeliveryOrderDetail();
                newDeliveryOrderDetail.Id = deliveryOrderDetail.Id;
                newDeliveryOrderDetail.DeliveryOrderId = deliveryOrderDetail.Id;
                newDeliveryOrderDetail.Code = deliveryOrderDetail.Code;
                newDeliveryOrderDetail.Quantity = deliveryOrderDetail.Quantity;
                newDeliveryOrderDetail.ItemId = deliveryOrderDetail.ItemId;
                newDeliveryOrderDetail.SalesOrderDetailId = deliveryOrderDetail.SalesOrderDetailId;
                newDeliveryOrderDetail.IsConfirmed = deliveryOrderDetail.IsConfirmed;
                newDeliveryOrderDetail.IsDeleted = deliveryOrderDetail.IsDeleted;
                newDeliveryOrderDetail.CreatedAt = DateTime.Now;
                newDeliveryOrderDetail.UpdatedAt = deliveryOrderDetail.UpdatedAt;
                newDeliveryOrderDetail.DeletedAt = deliveryOrderDetail.DeletedAt;

                newDeliveryOrderDetail = _deliveryOrderRepository.CreateDeliveryOrderDetail(newDeliveryOrderDetail);
                newDeliveryOrderDetail.Id = newDeliveryOrderDetail.Id;

                respModel.isValid = true;
                respModel.message = "Create Delivery Order Detail Success...";
                respModel.objResult = deliveryOrderDetail;

                LOG.Error("CreateDeliveryOrderDetail Sucess");
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
                        LOG.ErrorFormat("CreateDeliveryOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("CreateDeliveryOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Create DeliveryOrderDetail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("CreateDeliveryOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Create DeliveryOrderDetail Failed, Please try again or contact your administrator.";
            }

            return respModel;

        }


        /// <summary>
        /// Please use DeleteDeliveryOrder() instead of this function. This only deletes all the deliver order details without deleting its
        /// parent. DeleteDeliveryOrder() will delete delivery order and all its delivery order details.
        /// </summary>
        /// <param name="deliveryOrderId">Id of the delivery order</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel DeleteDeliveryOrderDetailByDeliveryOrderId(int deliveryOrderId, IDeliveryOrderRepository _deliveryOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                List<DeliveryOrderDetailModel> model = _deliveryOrderRepository.GetDeliveryOrderDetailList(deliveryOrderId);
                if (model.Count() == 0)
                {
                    respModel.isValid = false;
                    respModel.message ="There's no Delivery Order Detail...";
                    return respModel;
                }
                _deliveryOrderRepository.DeleteDeliveryOrderDetailByDeliveryOrderId(deliveryOrderId);

                respModel.message ="Delete Delivery Order Details completed. Please make sure the that delivery order has already been deleted";
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
                        LOG.ErrorFormat("DeleteDeliveryOrderDetailByDeliveryOrderId, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("DeleteDeliveryOrderDetailByDeliveryOrderId Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Delete DeleteDeliveryOrderDetailByDeliveryOrderId failed, Please try again or deliveryOrder your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("DeleteDeliveryOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Delete DeliveryOrderDetail Failed, Please try again or deliveryOrder your administrator.";
            }

            return respModel;
        }
        
        /// <summary>
        /// Delete delivery order detail.
        /// </summary>
        /// <param name="deliveryOrderDetailId">Id of the delivery order detail</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel DeleteDeliveryOrderDetail(int deliveryOrderDetailId, IDeliveryOrderRepository _deliveryOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                DeliveryOrderDetailModel model = _deliveryOrderRepository.GetDeliveryOrderDetail(deliveryOrderDetailId);
                if (model != null)
                {
                    string message = "";
                    respModel.isValid = this.ValidateDeleteDeliveryOrderDetail(model, out message);
                    if (!respModel.isValid)
                    {
                        respModel.message = message;
                        return respModel;
                    }

                    // Delete DeliveryOrderDetail
                    _deliveryOrderRepository.DeleteDeliveryOrderDetail(deliveryOrderDetailId);

                    respModel.isValid = true;
                    respModel.message = "Delete DeliveryOrderDetail Success...";
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
                        LOG.ErrorFormat("DeleteDeliveryOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("DeleteDeliveryOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Delete DeliveryOrderDetail failed, Please try again or deliveryOrder your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("DeleteDeliveryOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Delete DeliveryOrderDetail Failed, Please try again or deliveryOrder your administrator.";
            }

            return respModel;
        }

        /*
         * UPDATE DETAIL
         */

        /// <summary>
        /// Update a delivery order detail.
        /// </summary>
        /// <param name="deliveryOrderDetail">DeliveryOrderDetailModel object</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel UpdateDeliveryOrderDetail(DeliveryOrderDetailModel deliveryOrderDetail, IDeliveryOrderRepository _deliveryOrderRepository, ISalesOrderRepository _salesOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateCreateUpdateDeliveryOrderDetail(deliveryOrderDetail, _deliveryOrderRepository, _salesOrderRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                DeliveryOrderDetail dod = new DeliveryOrderDetail();
                dod.Id = deliveryOrderDetail.Id;
                dod.DeliveryOrderId = deliveryOrderDetail.DeliveryOrderId;
                dod.Code = deliveryOrderDetail.Code;
                dod.Quantity = deliveryOrderDetail.Quantity;
                dod.ItemId = deliveryOrderDetail.ItemId;
                dod.SalesOrderDetailId = deliveryOrderDetail.SalesOrderDetailId;
                dod.IsConfirmed = deliveryOrderDetail.IsConfirmed;
                dod.IsDeleted = deliveryOrderDetail.IsDeleted;
                dod.CreatedAt = deliveryOrderDetail.CreatedAt;
                dod.UpdatedAt = DateTime.Now;
                dod.DeletedAt = deliveryOrderDetail.DeletedAt;

                _deliveryOrderRepository.UpdateDeliveryOrderDetail(dod);

                respModel.isValid = true;
                respModel.message = "Update Delivery Order Detail Success...";
                respModel.objResult = deliveryOrderDetail;

                LOG.Info("UpdateDeliveryOrderDetail Success");
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
                        LOG.ErrorFormat("UpdateDeliveryOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UpdateDeliveryOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Update DeliveryOrderDetail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UpdateDeliveryOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Update DeliveryOrderDetail Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /// <summary>
        /// This function calls an update function given the parameter 'Confirm' or 'Unconfirm'.
        /// </summary>
        /// <param name="deliveryOrderDetail">DeliveryOrderModel object</param>
        /// <param name="IsConfirm">Is this function Confirm or Unconfirm</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository object</param>
        /// <returns>a response model</returns>
        ResponseModel UpdateConfirmationDeliveryOrderDetail(DeliveryOrderDetailModel deliveryOrderDetail, bool IsConfirm, IDeliveryOrderRepository _deliveryOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                String message = IsConfirm ? "Update Confirm" : "Update Unconfirm";
                DeliveryOrderDetailModel confirmDeliveryOrderDetail = _deliveryOrderRepository.GetDeliveryOrderDetail(deliveryOrderDetail.Id);

                if (confirmDeliveryOrderDetail != null)
                {
                    _deliveryOrderRepository.UpdateConfirmationDeliveryOrderDetailByDeliveryOrderId(confirmDeliveryOrderDetail.Id, IsConfirm);

                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "DeliveryOrderDetail not found...";
                }
                respModel.isValid = true;
                respModel.message = message + "DeliveryOrderDetail Success...";
                respModel.objResult = deliveryOrderDetail;

                LOG.Info(message + "DeliveryOrderDetail Success");

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
                        LOG.ErrorFormat("UpdateConfirmationDeliveryOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UpdateConfirmationDeliveryOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Update Confirmation Delivery Order Detail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UpdateConfirmationDeliveryOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Update Confirmation Delivery Order Detail Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * CONFIRM DETAIL
         */

        /// <summary>
        /// Confirm a delivery order detail.
        /// </summary>
        /// <param name="deliveryOrderDetail">DeliveryOrderDetailModel object</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="_stockMutationRepository">IStockMutation object</param>
        /// <returns>a response model</returns>
        public ResponseModel ConfirmDeliveryOrderDetail(DeliveryOrderDetailModel deliveryOrderDetail, IDeliveryOrderRepository _deliveryOrderRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            String message = "";
            try
            {
                respModel.isValid = this.ValidateConfirmDeliveryOrderDetail(deliveryOrderDetail, _itemRepository, out message);
                respModel.message = message;
                if (!respModel.isValid) {return respModel;}

                respModel = this.UpdateConfirmationDeliveryOrderDetail(deliveryOrderDetail, true, _deliveryOrderRepository);

                Item item = _itemRepository.Find(x => x.Id == deliveryOrderDetail.ItemId && !x.IsDeleted);
                if (item == null)
                {
                    respModel.isValid = false;
                    respModel.message = "No item found...";
                    return respModel;
                }

                item.PendingDelivery -= deliveryOrderDetail.Quantity;
                item.Ready -= deliveryOrderDetail.Quantity;
                item.UpdatedAt = DateTime.Now;
                _itemRepository.UpdateItem(item);

                LOG.Info("Updating Item " + item.Sku + " Reducing Pending Delivery and Ready By " + deliveryOrderDetail.Quantity);

                StockMutation stockMutationPendingDelivery = new StockMutation();
                stockMutationPendingDelivery.ItemId = item.Sku;
                stockMutationPendingDelivery.Quantity = deliveryOrderDetail.Quantity;
                stockMutationPendingDelivery.SourceDocument = "DeliveryOrder";
                stockMutationPendingDelivery.SourceDocumentId = deliveryOrderDetail.DeliveryOrderId;
                stockMutationPendingDelivery.SourceDocumentDetail = "DeliveryOrderDetail";
                stockMutationPendingDelivery.SourceDocumentDetailId = deliveryOrderDetail.Id;
                stockMutationPendingDelivery.ItemCase = 3; // Pending Delivery
                stockMutationPendingDelivery.MutationCase = 2; // Reduction
                stockMutationPendingDelivery.IsDeleted = false;
                stockMutationPendingDelivery.CreatedAt = DateTime.Now;
                stockMutationPendingDelivery.UpdatedAt = null;
                stockMutationPendingDelivery.DeletedAt = null;

                stockMutationPendingDelivery = _stockMutationRepository.CreateStockMutation(stockMutationPendingDelivery);
                stockMutationPendingDelivery.Id = stockMutationPendingDelivery.Id;

                LOG.Info("Creating Stock Mutation Pending Delivery");

                StockMutation stockMutationReady = new StockMutation();
                stockMutationReady.ItemId = item.Sku;
                stockMutationReady.Quantity = deliveryOrderDetail.Quantity;
                stockMutationReady.SourceDocument = "DeliveryOrder";
                stockMutationReady.SourceDocumentId = deliveryOrderDetail.DeliveryOrderId;
                stockMutationReady.SourceDocumentDetail = "DeliveryOrderDetail";
                stockMutationReady.SourceDocumentDetailId = deliveryOrderDetail.Id;
                stockMutationReady.ItemCase = 1; // Ready
                stockMutationReady.MutationCase = 2; // Reduction
                stockMutationReady.IsDeleted = false;
                stockMutationReady.CreatedAt = DateTime.Now;
                stockMutationReady.UpdatedAt = null;
                stockMutationReady.DeletedAt = null;

                stockMutationReady = _stockMutationRepository.CreateStockMutation(stockMutationReady);
                stockMutationReady.Id = stockMutationReady.Id;

                LOG.Info("Creating Stock Mutation Ready");

                respModel.message = "Success updating item and creating two stock mutations";
                respModel.objResult = deliveryOrderDetail;
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
                        LOG.ErrorFormat("ConfirmDeliveryOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("ConfirmDeliveryOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Confirm DeliveryOrderDetail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("ConfirmDeliveryOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Confirm DeliveryOrderDetail Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * UNCONFIRM DETAIL
         */

        /// <summary>
        /// Unconfirm a delivery order detail.
        /// </summary>
        /// <param name="deliveryOrderDetail">DeliveryOrderDetailModel object</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel UnconfirmDeliveryOrderDetail(DeliveryOrderDetailModel deliveryOrderDetail, IDeliveryOrderRepository _deliveryOrderRepository, IItemRepository _itemRepository)
        {
            String message ="";
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "";
            respModel.objResult = null;

            try
            {
                respModel.isValid = this.ValidateUnconfirmDeliveryOrderDetail(deliveryOrderDetail, _itemRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                Item item = _itemRepository.Find(x => x.Id == deliveryOrderDetail.ItemId && !x.IsDeleted);
                if (item == null)
                {
                    respModel.isValid = false;
                    respModel.message = "No item found...";
                    return respModel;
                }

                respModel = this.UpdateConfirmationDeliveryOrderDetail(deliveryOrderDetail, false, _deliveryOrderRepository);

                item.PendingDelivery += deliveryOrderDetail.Quantity;
                item.Ready += deliveryOrderDetail.Quantity;
                item.UpdatedAt = DateTime.Now;
                _itemRepository.UpdateItem(item);

                LOG.Info("Updating Item " + item.Sku + " Adding Pending Delivery and Ready By " + deliveryOrderDetail.Quantity);

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
                        LOG.ErrorFormat("UnconfirmDeliveryOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UnconfirmDeliveryOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "UnconfirmDeliveryOrderDetail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UnconfirmDeliveryOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "UnconfirmDeliveryOrderDetail Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * VALIDATE DETAIL
         */

        /// <summary>
        /// Validate a delivery order detail when it is created / updated.
        /// It is valid if it asserts the following rules:
        /// The following attributes must be present:
        /// 1. SalesOrderDetailId, Contact must be present
        /// 2. Contact must be associated to SalesOrderDetailId 
        /// 3. Quantity gt 0 && Quantity le PendingDelivery of SalesOrderDetail
        /// 4. PendingDelivery of SalesOrderDetail gt 0
        /// 5. SalesOrderDetail isConfirmed
        /// 6. salesOrderModel.ContactId == deliveryOrderModel.ContactId
        /// 7. Unique SalesOrderDetailId in a given DeliveryOrder -- Unchecked !! TODO
        /// </summary>
        /// <param name="deliveryOrderDetail">DeliveryOrderDetailModel object</param>
        /// <param name="_deliveryOrderRepository">IDeliveryOrderRepository object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateCreateUpdateDeliveryOrderDetail(DeliveryOrderDetailModel deliveryOrderDetail, IDeliveryOrderRepository _deliveryOrderRepository, ISalesOrderRepository _salesOrderRepository, out string message)
        {
            bool isValid = true;
            message = "";
            DeliveryOrderModel deliveryOrderModel = _deliveryOrderRepository.GetDeliveryOrder(deliveryOrderDetail.DeliveryOrderId);
            if (deliveryOrderModel == null)
            {
                isValid = false;
                message = "Error Validation: No associated delivery order found...";
                return isValid;
            }
            SalesOrderDetailModel salesOrderDetailModel = _salesOrderRepository.GetSalesOrderDetail(deliveryOrderDetail.SalesOrderDetailId);
            if (salesOrderDetailModel == null)
            {
                isValid = false;
                message = "Error Validation: No associated sales order detail found...";
                return false;
            }
            SalesOrderModel salesOrderModel = _salesOrderRepository.GetSalesOrder (salesOrderDetailModel.SalesOrderId);
            if (salesOrderModel == null)
            {
                isValid = false;
                message = "Error Validation: No associated sales order found...";
                return false;
            }
            if (deliveryOrderDetail.SalesOrderDetailId == null ||
                deliveryOrderModel.ContactId == null ||
                salesOrderDetailModel.SalesOrderId == null ||

                deliveryOrderDetail.Quantity == 0 ||
                deliveryOrderDetail.Quantity > salesOrderDetailModel.PendingDelivery ||
                salesOrderDetailModel.PendingDelivery <= 0 ||
                salesOrderDetailModel.IsConfirmed == false)
            {
                isValid = false;
                message = "Error Validation: Incomplete data...";
                return false;

            }
            if (salesOrderModel.ContactId != deliveryOrderModel.ContactId)
            {
                isValid = false;
                message = "Error Validation: Different contact person...";
                return false;
            }
            message = "Successful Validation...";
            return true;
        }

        /// <summary>
        /// Validate delivery order detail when it is deleted.
        /// Delivery order detail can't be deleted if it is confirmed
        /// </summary>
        /// <param name="deliveryOrderDetail">DeliveryOrderDetailModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateDeleteDeliveryOrderDetail(DeliveryOrderDetailModel deliveryOrderDetail, out string message)
        {
            message = "";
            if (deliveryOrderDetail.IsConfirmed)
            {
                message = "Can't be destroyed. Delivery Order Detail is confirmed.";
                return false;
            }
            return true;
        }

        /// <summary>
        /// TODO: Confirm with Willy if this is for Confirm / Unconfirm
        /// Validate delivery order detail when it is unconfirmed.
        /// Can't unconfirm if associate item.pendingReceival will be changed to lt 0 after unconfirm
        /// Can't unconfirm if quantity of item.ready will be changed to lt 0 after unconfirm
        /// </summary>
        /// <param name="deliveryOrderDetail">DeliveryOrderDetailModel object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>       
        public bool ValidateConfirmDeliveryOrderDetail(DeliveryOrderDetailModel deliveryOrderDetail, IItemRepository _itemRepository, out string message)
        {
            message = "";
            Item item = _itemRepository.Find(x => x.Id == deliveryOrderDetail.ItemId && !x.IsDeleted);
            if (item == null)
            {
                message = "Item can't be found";
                return false;
            }
            if ((item.PendingReceival - deliveryOrderDetail.Quantity) < 0)
            {
                message = "Can't confirm. Not enough amount in stock Pending Receival...";
                return false;
            }
            if ((item.Ready - deliveryOrderDetail.Quantity) < 0)
            {
                message = "Can't confirm. Not enough amount in stock ready...";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate delivery order detail when it is unconfirmed
        /// </summary>
        /// <param name="deliveryOrderDetail"></param>
        /// <param name="_itemRepository"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ValidateUnconfirmDeliveryOrderDetail(DeliveryOrderDetailModel deliveryOrderDetail, IItemRepository _itemRepository, out string message)
        {
            message = "";
            return true;
        }
        

    }
}