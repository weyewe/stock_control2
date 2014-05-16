using StockControl.Models;
using StockControl.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace StockControl.Service
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("SalesOrderService");

        /*
         * GET
         */

        /// <summary>
        /// Get all sales orders.
        /// </summary>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <returns>all sales orders</returns>
        public List<SalesOrderModel> GetSalesOrderList(ISalesOrderRepository _salesOrderRepository)
        {
            List<SalesOrderModel> model = new List<SalesOrderModel>();
            try
            {
                model = _salesOrderRepository.GetSalesOrderList();
            }
            catch (Exception ex)
            {
                LOG.Error("GetSalesOrderList Failed", ex);
            }

            return model;
        }

        /// <summary>
        /// Get a sales order
        /// </summary>
        /// <param name="orderId">Id of the sales order</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <returns>a sales order</returns>
        public SalesOrderModel GetSalesOrder(int orderId, ISalesOrderRepository _salesOrderRepository)
        {
            SalesOrderModel model = new SalesOrderModel();
            try
            {
                model = _salesOrderRepository.GetSalesOrder(orderId);
            }
            catch (Exception ex)
            {
                LOG.Error("GetSalesOrder Failed", ex);
            }

            return model;
        }

        /*
         * CREATE
         */

        /// <summary>
        /// Create a sales order.
        /// </summary>
        /// <param name="salesOrder">SalesOrderModel object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <returns>a sales order</returns>
        public ResponseModel CreateSalesOrder(SalesOrderModel salesOrder, ISalesOrderRepository _salesOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateCreateSalesOrder(salesOrder, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                SalesOrder newSalesOrder = new SalesOrder();
                newSalesOrder.Id = salesOrder.Id;
                newSalesOrder.ContactId = salesOrder.ContactId;
                // Code: #{year}/#{total_number_of_sales_order}
                // TODO:
                newSalesOrder.Code = "#" + DateTime.Today.Year.ToString() + "/#" + "{total_number_sales_order}";
                newSalesOrder.SalesDate = salesOrder.SalesDate;
                newSalesOrder.IsConfirmed = salesOrder.IsConfirmed;
                newSalesOrder.IsDeleted = salesOrder.IsDeleted;
                newSalesOrder.CreatedAt = DateTime.Now;
                newSalesOrder.UpdatedAt = salesOrder.UpdatedAt;
                newSalesOrder.DeletedAt = salesOrder.DeletedAt;

                newSalesOrder = _salesOrderRepository.CreateSalesOrder(newSalesOrder);
                newSalesOrder.Id = newSalesOrder.Id;

                respModel.isValid = true;
                respModel.message = "Create Sales Order Success...";
                respModel.objResult = salesOrder;

                LOG.Error("CreateSalesOrder Sucess");
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
                        LOG.ErrorFormat("CreateSalesOrder, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("CreateSalesOrder Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Create Sales Order failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("CreateSalesOrder Failed", ex);
                respModel.isValid = false;
                respModel.message = "Create Sales Order Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * DELETE
         */

        /// <summary>
        /// Delete a sales order and all its children sales order details
        /// </summary>
        /// <param name="salesOrderId">Id of the sales order</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository</param>
        /// <returns>a response model</returns>
        public ResponseModel DeleteSalesOrder(int salesOrderId, ISalesOrderRepository _salesOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                SalesOrder deleteSalesOrder = _salesOrderRepository.Find(p => p.Id == salesOrderId && !p.IsDeleted);
                if (deleteSalesOrder != null)
                {
                    string message = "";
                    SalesOrderModel model = SalesOrderModel.ToModel(deleteSalesOrder);
                    respModel.isValid = this.ValidateDeleteSalesOrder(model, out message);
                    if (!respModel.isValid)
                    {
                        respModel.message = message;
                        return respModel;
                    }

                    // Delete SalesOrder
                    _salesOrderRepository.DeleteSalesOrder(salesOrderId);
                    respModel.objResult = model;

                    // Get all subsequent SalesOrderDetails
                    List<SalesOrderDetailModel> deleteSalesOrderDetails = _salesOrderRepository.GetSalesOrderDetailList(salesOrderId);

                    if (deleteSalesOrderDetails.Count() > 0)
                    {
                        // Delete all subsequent SalesOrderDetails
                        _salesOrderRepository.DeleteSalesOrderDetailBySalesOrderId(salesOrderId);
                        // Adding the line below will set respModel to all the subsequent details
                        // respModel.objResult = deleteSalesOrderDetails;
                    }

                    respModel.isValid = true;
                    respModel.message = "Delete sales order and its sales order details Success...";
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
                        LOG.ErrorFormat("DeleteSalesOrder, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("DeleteSalesOrder Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Delete Sales Order failed, Please try again or salesOrder your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("DeleteSalesOrder Failed", ex);
                respModel.isValid = false;
                respModel.message = "Delete Sales Order Failed, Please try again or salesOrder your administrator.";
            }

            return respModel;
        }

        /*
         * UPDATE
         */

        /// <summary>
        /// Update a sales order.
        /// </summary>
        /// <param name="salesOrder">SalesOrderModel object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel UpdateSalesOrder(SalesOrderModel salesOrder, ISalesOrderRepository _salesOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateUpdateSalesOrder(salesOrder, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                SalesOrder updateSalesOrder = _salesOrderRepository.Find(p => p.Id == salesOrder.Id && !p.IsDeleted);
                if (updateSalesOrder != null)
                {
                    updateSalesOrder.Id = salesOrder.Id;
                    updateSalesOrder.ContactId = salesOrder.ContactId;
                    updateSalesOrder.Code = salesOrder.Code;
                    updateSalesOrder.SalesDate = salesOrder.SalesDate;
                    updateSalesOrder.IsConfirmed = salesOrder.IsConfirmed;
                    updateSalesOrder.IsDeleted = salesOrder.IsDeleted;
                    updateSalesOrder.CreatedAt = salesOrder.CreatedAt;
                    updateSalesOrder.UpdatedAt = DateTime.Now;
                    updateSalesOrder.DeletedAt = salesOrder.DeletedAt;

                    _salesOrderRepository.UpdateSalesOrder(updateSalesOrder);

                    respModel.isValid = true;
                    respModel.message = "Update Sales Order Success...";
                    respModel.objResult = salesOrder;

                    LOG.Info("UpdateSalesOrder Success");
                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "SalesOrder not found...";
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
                        LOG.ErrorFormat("UpdateSalesOrder, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UpdateSalesOrder Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Update sales order failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UpdateSalesOrder Failed", ex);
                respModel.isValid = false;
                respModel.message = "Update sales order Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * CONFIRM
         */

        /// <summary>
        /// Confirm sales order. This function also automatically confirms all its children sales order details.
        /// </summary>
        /// <param name="salesOrder">SalesOrderModel object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel ConfirmSalesOrder(SalesOrderModel salesOrder, ISalesOrderRepository _salesOrderRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateConfirmSalesOrder(salesOrder, _salesOrderRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                SalesOrder confirmSalesOrder = _salesOrderRepository.Find(p => p.Id == salesOrder.Id && !p.IsDeleted);
                if (confirmSalesOrder != null)
                {
                    confirmSalesOrder.Id = salesOrder.Id;
                    confirmSalesOrder.ContactId = salesOrder.ContactId;
                    confirmSalesOrder.Code = salesOrder.Code;
                    confirmSalesOrder.SalesDate = salesOrder.SalesDate;
                    confirmSalesOrder.IsConfirmed = true;
                    confirmSalesOrder.IsDeleted = salesOrder.IsDeleted;
                    confirmSalesOrder.CreatedAt = salesOrder.CreatedAt;
                    confirmSalesOrder.UpdatedAt = DateTime.Now;
                    confirmSalesOrder.DeletedAt = salesOrder.DeletedAt;

                    _salesOrderRepository.UpdateSalesOrder(confirmSalesOrder);

                    LOG.Info("ConfirmSalesOrder Success");

                    // Confirm SalesOrder Detail
                    List<SalesOrderDetailModel> confirmSalesOrderDetails = _salesOrderRepository.GetSalesOrderDetailList(salesOrder.Id);

                    if (confirmSalesOrderDetails.Count() > 0)
                    {
                        foreach (var dod in confirmSalesOrderDetails)
                        {
                            respModel = this.ConfirmSalesOrderDetail(dod, _salesOrderRepository, _itemRepository, _stockMutationRepository);
                        }
                        LOG.Info("ConfirmSalesOrderDetails Success");
                    }

                    respModel.isValid = true;
                    respModel.message = "Confirm Data Success...";
                    respModel.objResult = confirmSalesOrder;
                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "SalesOrder not found...";
                }
                respModel.isValid = true;
                respModel.message = "Confirm Sales Order Success...";
                respModel.objResult = salesOrder;

                LOG.Info("ConfirmSalesOrder Success");

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
                        LOG.ErrorFormat("ConfirmSalesOrder, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("ConfirmSalesOrder Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Confirm sales order failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("ConfirmSalesOrder Failed", ex);
                respModel.isValid = false;
                respModel.message = "Confirm sales order Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * UNCONFIRM
         */

        /// <summary>
        /// Unconfirm sales order.
        /// </summary>
        /// <param name="salesOrder">SalesOrderModel object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel UnconfirmSalesOrder(SalesOrderModel salesOrder, ISalesOrderRepository _salesOrderRepository, IItemRepository _itemRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateUnconfirmSalesOrder(salesOrder, _salesOrderRepository, _itemRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                SalesOrder unconfirmSalesOrder = _salesOrderRepository.Find(p => p.Id == salesOrder.Id && !p.IsDeleted);
                if (unconfirmSalesOrder != null)
                {
                    unconfirmSalesOrder.Id = salesOrder.Id;
                    unconfirmSalesOrder.ContactId = salesOrder.ContactId;
                    unconfirmSalesOrder.Code = salesOrder.Code;
                    unconfirmSalesOrder.SalesDate = salesOrder.SalesDate;
                    unconfirmSalesOrder.IsConfirmed = false;
                    unconfirmSalesOrder.IsDeleted = salesOrder.IsDeleted;
                    unconfirmSalesOrder.CreatedAt = salesOrder.CreatedAt;
                    unconfirmSalesOrder.UpdatedAt = DateTime.Now;
                    unconfirmSalesOrder.DeletedAt = salesOrder.DeletedAt;

                    _salesOrderRepository.UpdateSalesOrder(unconfirmSalesOrder);

                    LOG.Info("UnconfirmSalesOrder Success");

                    // Unconfirm SalesOrder Detail
                    List<SalesOrderDetailModel> unconfirmSalesOrderDetails = _salesOrderRepository.GetSalesOrderDetailList(salesOrder.Id);

                    if (unconfirmSalesOrderDetails.Count() > 0)
                    {
                        _salesOrderRepository.UpdateConfirmationSalesOrderDetailBySalesOrderId(salesOrder.Id, false);
                    }

                    LOG.Info("UnconfirmSalesOrderDetails Success");

                    respModel.isValid = true;
                    respModel.message = "Unconfirm Data Success...";
                    respModel.objResult = salesOrder;
                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "SalesOrder not found...";
                }
                respModel.isValid = true;
                respModel.message = "Unconfirm Sales Order Success...";
                respModel.objResult = salesOrder;

                LOG.Info("UnconfirmSalesOrder Success");

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
                        LOG.ErrorFormat("UnconfirmSalesOrder, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UnconfirmSalesOrder Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Unconfirm sales order failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UnconfirmSalesOrder Failed", ex);
                respModel.isValid = false;
                respModel.message = "Unconfirm sales order Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * VALIDATE
         */

        /// <summary>
        /// Private function to validate a sales order.
        /// </summary>
        /// <param name="model">SalesOrderModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        private bool Validate(SalesOrderModel model, out string message)
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
        /// Validate a sales order when it is created.
        /// </summary>
        /// <param name="model">SalesOrderModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateCreateSalesOrder(SalesOrderModel model, out string message)
        {
            bool isValid = this.Validate(model, out message);

            return isValid;
        }

        /// <summary>
        /// Validate a sales order when it is updated.
        /// </summary>
        /// <param name="model">SalesOrderModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateUpdateSalesOrder(SalesOrderModel model, out string message)
        {
            bool isValid = this.Validate(model, out message);

            if (model.IsConfirmed)
            {
                message = "Confirmed sales order cannot be updated. Please try again or contact your administrator.";
                return false;
            }

            return isValid;
        }

        /// <summary>
        /// Validate a sales order when it is deleted.
        /// It is invalid when a sales order has been confirmed.
        /// </summary>
        /// <param name="model">SalesOrderModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateDeleteSalesOrder(SalesOrderModel model, out string message)
        {
            bool isValid = true;
            message = "";

            if (model.IsConfirmed)
            {
                message = "Can't destroyed confirmed sales order. Please try again or contact your administrator";
                return false;
            }

            return isValid;
        }

        /// <summary>
        /// Validate a sales order when it is confirmed.
        /// It is valid when it has any sales order details.
        /// </summary>
        /// <param name="model">SalesOrderModel object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateConfirmSalesOrder(SalesOrderModel model, ISalesOrderRepository _salesOrderRepository, out string message)
        {
            bool isValid = true;
            message = "";

            if (_salesOrderRepository.GetSalesOrderDetailList(model.Id).Count() == 0)
            {
                message = "There is no existing Sales Order Detail...";
                return false;
            }
            return isValid;
        }


        /// <summary>
        /// Validate a sales order when it is unconfirmed.
        /// It is valid when all its sales order detail is joined to an item with PendingDelivery gt 0
        /// </summary>
        /// <param name="salesOrder">SalesOrderModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateUnconfirmSalesOrder(SalesOrderModel salesOrder, ISalesOrderRepository _salesOrderRepository, IItemRepository _itemRepository, out string message)
        {
            bool isValid = true;
            message = "";

            List<SalesOrderDetailModel> model = _salesOrderRepository.GetSalesOrderDetailList(salesOrder.Id);
            if (model.Count() > 0)
            {
                foreach (var eachdetail in model)
                {
                    ItemModel item = _itemRepository.GetItem(eachdetail.ItemId);
                    if (item.PendingDelivery < 0)
                    {
                        isValid = false;
                        message = "Invalid unconfirmation. Pending Delivery quantity of the item is less than 0"; 
                    }
                }
            }
            return isValid;
        }

        /*
         * GET DETAIL
         */

        /// <summary>
        /// Get all sales order details.
        /// </summary>
        /// <param name="salesOrderId">Id of the sales order</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <returns>all sales order details associated to the sales order</returns>
        public List<SalesOrderDetailModel> GetSalesOrderDetailList(int salesOrderId, ISalesOrderRepository _salesOrderRepository)
        {
            List<SalesOrderDetailModel> model = new List<SalesOrderDetailModel>();
            try
            {
                model = _salesOrderRepository.GetSalesOrderDetailList(salesOrderId);
            }
            catch (Exception ex)
            {
                LOG.Error("GetSalesOrderDetailList Failed", ex);
            }

            return model;
        }

        /// <summary>
        /// Get a sales order detail
        /// </summary>
        /// <param name="salesOrderDetailId">Id of the sales order detail</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <returns>a sales order detail</returns>
        public SalesOrderDetailModel GetSalesOrderDetail(int salesOrderDetailId, ISalesOrderRepository _salesOrderRepository)
        {
            SalesOrderDetailModel model = new SalesOrderDetailModel();
            try
            {
                model = _salesOrderRepository.GetSalesOrderDetail(salesOrderDetailId);
            }
            catch (Exception ex)
            {
                LOG.Error("GetSalesOrderDetail Failed", ex);
            }

            return model;
        }

        /*
         * CREATE DETAIL
         */

        /// <summary>
        /// Create a sales order detail.
        /// </summary>
        /// <param name="salesOrderDetail">SalesOrderDetail object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel CreateSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, ISalesOrderRepository _salesOrderRepository, IItemRepository _itemRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateCreateSalesOrderDetail(salesOrderDetail, _salesOrderRepository, _itemRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                SalesOrderDetail newSalesOrderDetail = new SalesOrderDetail();
                newSalesOrderDetail.Id = salesOrderDetail.Id;
                newSalesOrderDetail.SalesOrderId = salesOrderDetail.SalesOrderId;
                // Code: #{year}/#{purchase_order.code}/#{total_number_of_non_deleted_purchase_order_Entry}
                // TODO:
                newSalesOrderDetail.Code = "#" + DateTime.Now.Year.ToString() + "/#" + "Purchase Order Code" + "/#" + "Total Number of Non Deleted Purchase Order Entry";
                newSalesOrderDetail.Quantity = salesOrderDetail.Quantity;
                newSalesOrderDetail.ItemId = salesOrderDetail.ItemId;
                newSalesOrderDetail.PendingDelivery = salesOrderDetail.PendingDelivery;
                newSalesOrderDetail.IsConfirmed = salesOrderDetail.IsConfirmed;
                newSalesOrderDetail.IsFulfilled = salesOrderDetail.IsFulfilled;
                newSalesOrderDetail.IsDeleted = salesOrderDetail.IsDeleted;
                newSalesOrderDetail.CreatedAt = DateTime.Now;
                newSalesOrderDetail.UpdatedAt = salesOrderDetail.UpdatedAt;
                newSalesOrderDetail.DeletedAt = salesOrderDetail.DeletedAt;

                newSalesOrderDetail = _salesOrderRepository.CreateSalesOrderDetail(newSalesOrderDetail);
                newSalesOrderDetail.Id = newSalesOrderDetail.Id;

                respModel.isValid = true;
                respModel.message = "Create Sales Order Detail Success...";
                respModel.objResult = salesOrderDetail;

                LOG.Error("CreateSalesOrderDetail Sucess");
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
                        LOG.ErrorFormat("CreateSalesOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("CreateSalesOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Create SalesOrderDetail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("CreateSalesOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Create SalesOrderDetail Failed, Please try again or contact your administrator.";
            }

            return respModel;

        }


        /// <summary>
        /// Please use DeleteSalesOrder() instead of this function. This only deletes all the deliver order details without deleting its
        /// parent. DeleteSalesOrder() will delete sales order and all its sales order details.
        /// </summary>
        /// <param name="salesOrderId">Id of the sales order</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel DeleteSalesOrderDetailBySalesOrderId(int salesOrderId, ISalesOrderRepository _salesOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                List<SalesOrderDetailModel> model = _salesOrderRepository.GetSalesOrderDetailList(salesOrderId);
                if (model.Count() == 0)
                {
                    respModel.isValid = false;
                    respModel.message = "There's no Sales Order Detail...";
                    return respModel;
                }
                _salesOrderRepository.DeleteSalesOrderDetailBySalesOrderId(salesOrderId);

                respModel.message = "Delete Sales Order Details completed. Please make sure the that sales order has already been deleted";
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
                        LOG.ErrorFormat("DeleteSalesOrderDetailBySalesOrderId, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("DeleteSalesOrderDetailBySalesOrderId Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Delete DeleteSalesOrderDetailBySalesOrderId failed, Please try again or salesOrder your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("DeleteSalesOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Delete SalesOrderDetail Failed, Please try again or salesOrder your administrator.";
            }

            return respModel;
        }

        /// <summary>
        /// Delete sales order detail.
        /// </summary>
        /// <param name="salesOrderDetailId">Id of the sales order detail</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel DeleteSalesOrderDetail(int salesOrderDetailId, ISalesOrderRepository _salesOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                SalesOrderDetailModel model = _salesOrderRepository.GetSalesOrderDetail(salesOrderDetailId);
                if (model != null)
                {
                    string message = "";
                    respModel.isValid = this.ValidateDeleteSalesOrderDetail(model, out message);
                    if (!respModel.isValid)
                    {
                        respModel.message = message;
                        return respModel;
                    }

                    // Delete SalesOrderDetail
                    _salesOrderRepository.DeleteSalesOrderDetail(salesOrderDetailId);

                    respModel.isValid = true;
                    respModel.message = "Delete SalesOrderDetail Success...";
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
                        LOG.ErrorFormat("DeleteSalesOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("DeleteSalesOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Delete SalesOrderDetail failed, Please try again or salesOrder your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("DeleteSalesOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Delete SalesOrderDetail Failed, Please try again or salesOrder your administrator.";
            }

            return respModel;
        }

        /*
         * UPDATE DETAIL
         */

        /// <summary>
        /// Update a sales order detail.
        /// </summary>
        /// <param name="salesOrderDetail">SalesOrderDetailModel object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel UpdateSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, ISalesOrderRepository _salesOrderRepository, IItemRepository _itemRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateUpdateSalesOrderDetail(salesOrderDetail, _salesOrderRepository, _itemRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                SalesOrderDetail dod = new SalesOrderDetail();
                dod.Id = salesOrderDetail.Id;
                dod.SalesOrderId = salesOrderDetail.SalesOrderId;
                dod.Code = salesOrderDetail.Code;
                dod.Quantity = salesOrderDetail.Quantity;
                dod.ItemId = salesOrderDetail.ItemId;
                dod.PendingDelivery = salesOrderDetail.PendingDelivery;
                dod.IsConfirmed = salesOrderDetail.IsConfirmed;
                dod.IsFulfilled = salesOrderDetail.IsFulfilled;
                dod.IsDeleted = salesOrderDetail.IsDeleted;
                dod.CreatedAt = salesOrderDetail.CreatedAt;
                dod.UpdatedAt = DateTime.Now;
                dod.DeletedAt = salesOrderDetail.DeletedAt;

                _salesOrderRepository.UpdateSalesOrderDetail(dod);

                respModel.isValid = true;
                respModel.message = "Update Sales Order Detail Success...";
                respModel.objResult = salesOrderDetail;

                LOG.Info("UpdateSalesOrderDetail Success");
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
                        LOG.ErrorFormat("UpdateSalesOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UpdateSalesOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Update SalesOrderDetail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UpdateSalesOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Update SalesOrderDetail Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /// <summary>
        /// This function calls an update function given the parameter 'Confirm' or 'Unconfirm'.
        /// </summary>
        /// <param name="salesOrderDetail">SalesOrderModel object</param>
        /// <param name="IsConfirm">Is this function Confirm or Unconfirm</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <returns>a response model</returns>
        ResponseModel UpdateConfirmationSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, bool IsConfirm, ISalesOrderRepository _salesOrderRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                String message = IsConfirm ? "Update Confirm" : "Update Unconfirm";
                SalesOrderDetailModel confirmSalesOrderDetail = _salesOrderRepository.GetSalesOrderDetail(salesOrderDetail.Id);

                if (confirmSalesOrderDetail != null)
                {
                    _salesOrderRepository.UpdateConfirmationSalesOrderDetailBySalesOrderId(confirmSalesOrderDetail.Id, IsConfirm);

                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "SalesOrderDetail not found...";
                }
                respModel.isValid = true;
                respModel.message = message + "SalesOrderDetail Success...";
                respModel.objResult = salesOrderDetail;

                LOG.Info(message + "SalesOrderDetail Success");

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
                        LOG.ErrorFormat("UpdateConfirmationSalesOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UpdateConfirmationSalesOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Update Confirmation Sales Order Detail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UpdateConfirmationSalesOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Update Confirmation Sales Order Detail Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * CONFIRM DETAIL
         */

        /// <summary>
        /// Confirm a sales order detail.
        /// </summary>
        /// <param name="salesOrderDetail">SalesOrderDetailModel object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="_stockMutationRepository">IStockMutation object</param>
        /// <returns>a response model</returns>
        public ResponseModel ConfirmSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, ISalesOrderRepository _salesOrderRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            String message = "";
            try
            {
                respModel.isValid = this.ValidateConfirmSalesOrderDetail(salesOrderDetail, out message);
                if (!respModel.isValid) { return respModel; }

                respModel = this.UpdateConfirmationSalesOrderDetail(salesOrderDetail, true, _salesOrderRepository);

                Item item = _itemRepository.Find(x => x.Id == salesOrderDetail.ItemId && !x.IsDeleted);
                if (item == null)
                {
                    respModel.isValid = false;
                    respModel.message = "No item found...";
                    return respModel;
                }

                SalesOrderDetail sod = new SalesOrderDetail();
                sod.Id = salesOrderDetail.Id;
                sod.SalesOrderId = salesOrderDetail.SalesOrderId;
                sod.Code = salesOrderDetail.Code;
                sod.Quantity = salesOrderDetail.Quantity;
                sod.ItemId = salesOrderDetail.ItemId;
                sod.PendingDelivery = salesOrderDetail.Quantity; // updated
                sod.IsConfirmed = salesOrderDetail.IsConfirmed;
                sod.IsFulfilled = salesOrderDetail.IsFulfilled;
                sod.IsDeleted = salesOrderDetail.IsDeleted;
                sod.CreatedAt = salesOrderDetail.CreatedAt;
                sod.UpdatedAt = DateTime.Now;
                sod.DeletedAt = salesOrderDetail.DeletedAt;
                SalesOrderDetail model = _salesOrderRepository.UpdateSalesOrderDetail(sod);

                item.PendingDelivery += salesOrderDetail.Quantity;
                item.UpdatedAt = DateTime.Now;
                _itemRepository.UpdateItem(item);

                LOG.Info("Updating Item " + item.Sku + " Adding Pending Delivery By " + salesOrderDetail.Quantity);

                StockMutation stockMutationPendingDelivery = new StockMutation();
                stockMutationPendingDelivery.ItemId = item.Sku;
                stockMutationPendingDelivery.Quantity = salesOrderDetail.Quantity;
                stockMutationPendingDelivery.SourceDocument = "SalesOrder";
                stockMutationPendingDelivery.SourceDocumentId = salesOrderDetail.SalesOrderId;
                stockMutationPendingDelivery.SourceDocumentDetail = "SalesOrderDetail";
                stockMutationPendingDelivery.SourceDocumentDetailId = salesOrderDetail.Id;
                stockMutationPendingDelivery.ItemCase = 3; // Pending Sales
                stockMutationPendingDelivery.MutationCase = 1; // Addition
                stockMutationPendingDelivery.IsDeleted = false;
                stockMutationPendingDelivery.CreatedAt = DateTime.Now;
                stockMutationPendingDelivery.UpdatedAt = null;
                stockMutationPendingDelivery.DeletedAt = null;

                stockMutationPendingDelivery = _stockMutationRepository.CreateStockMutation(stockMutationPendingDelivery);
                stockMutationPendingDelivery.Id = stockMutationPendingDelivery.Id;

                LOG.Info("Creating Stock Mutation Pending Sales");

                respModel.message = "Success updating item and creating stock mutation";
                respModel.objResult = salesOrderDetail;
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
                        LOG.ErrorFormat("ConfirmSalesOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("ConfirmSalesOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Confirm SalesOrderDetail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("ConfirmSalesOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "Confirm SalesOrderDetail Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * UNCONFIRM DETAIL
         */

        /// <summary>
        /// Unconfirm a sales order detail.
        /// </summary>
        /// <param name="salesOrderDetail">SalesOrderDetailModel object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <returns>a response model</returns>
        public ResponseModel UnconfirmSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, ISalesOrderRepository _salesOrderRepository, IDeliveryOrderRepository _deliveryOrderRepository, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository)
        {
            String message = "";
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "";
            respModel.objResult = null;

            try
            {
                respModel.isValid = this.ValidateUnconfirmSalesOrderDetail(salesOrderDetail, _salesOrderRepository, _deliveryOrderRepository, _itemRepository, out message);
                if (!respModel.isValid) {
                    respModel.message = message;
                    return respModel;
                }

                Item item = _itemRepository.Find(x => x.Id == salesOrderDetail.ItemId && !x.IsDeleted);
                if (item == null)
                {
                    respModel.isValid = false;
                    respModel.message = "No item found...";
                    return respModel;
                }

                respModel = this.UpdateConfirmationSalesOrderDetail(salesOrderDetail, false, _salesOrderRepository);

                SalesOrderDetail updatedSalesOrderDetail = new SalesOrderDetail();
                updatedSalesOrderDetail.Id = salesOrderDetail.Id;
                updatedSalesOrderDetail.SalesOrderId = salesOrderDetail.SalesOrderId;
                updatedSalesOrderDetail.Code = salesOrderDetail.Code;
                updatedSalesOrderDetail.Quantity = salesOrderDetail.Quantity;
                updatedSalesOrderDetail.ItemId = salesOrderDetail.ItemId;
                updatedSalesOrderDetail.PendingDelivery = salesOrderDetail.PendingDelivery - salesOrderDetail.Quantity;
                updatedSalesOrderDetail.IsConfirmed = salesOrderDetail.IsConfirmed;
                updatedSalesOrderDetail.IsFulfilled = salesOrderDetail.IsFulfilled;
                updatedSalesOrderDetail.IsDeleted = salesOrderDetail.IsDeleted;
                updatedSalesOrderDetail.CreatedAt = salesOrderDetail.CreatedAt;
                updatedSalesOrderDetail.UpdatedAt = DateTime.Now;
                updatedSalesOrderDetail.DeletedAt = salesOrderDetail.DeletedAt;

                updatedSalesOrderDetail = _salesOrderRepository.UpdateSalesOrderDetail(updatedSalesOrderDetail);

                item.PendingDelivery -= salesOrderDetail.Quantity; // PendingDelivery should be 0
                item.UpdatedAt = DateTime.Now;
                _itemRepository.UpdateItem(item);

                List<StockMutation> sm = _stockMutationRepository.GetStockMutationBySourceDocumentDetail(ItemModel.ToModel(item), "SalesOrderDetail", salesOrderDetail.Id);
                if (sm.Count() != 0)
                {
                    foreach (var stockMutation in sm)
                    {
                        _stockMutationRepository.DeleteStockMutation(stockMutation.Id);
                    }
                }

                LOG.Info("Updating Item " + item.Sku + " Reducing Pending Delivery By " + salesOrderDetail.Quantity);

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
                        LOG.ErrorFormat("UnconfirmSalesOrderDetail, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UnconfirmSalesOrderDetail Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "UnconfirmSalesOrderDetail failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UnconfirmSalesOrderDetail Failed", ex);
                respModel.isValid = false;
                respModel.message = "UnconfirmSalesOrderDetail Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        /*
         * VALIDATE DETAIL
         */

        /// <summary>
        /// Validate a sales order detail when it is created.
        /// It is valid if it asserts the following rules:
        /// The following attributes must be present:
        /// 1. SalesOrderId, Item must be present
        /// TODO:
        /// 2. In a given sales order, the sold item at each SalesOrderDetail must be unique
        /// 3. Quantity ge 0
        /// </summary>
        /// <param name="salesOrderDetail">SalesOrderDetailModel object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateCreateSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, ISalesOrderRepository _salesOrderRepository, IItemRepository _itemRepository, out string message)
        {
            bool isValid = true;
            message = "";
            SalesOrderModel salesOrderModel = _salesOrderRepository.GetSalesOrder(salesOrderDetail.SalesOrderId);
            if (salesOrderModel == null)
            {
                isValid = false;
                message = "Error Validation: No associated sales order found...";
                return isValid;
            }
            ItemModel itemModel = _itemRepository.GetItem(salesOrderDetail.ItemId);
            if (itemModel == null)
            {
                isValid = false;
                message = "Error Validation: No associated item found...";
                return false;
            }
            if (salesOrderDetail.SalesOrderId == null ||
                salesOrderDetail.ItemId == null ||
                salesOrderDetail.Quantity < 0 ||
                salesOrderDetail.IsConfirmed == false)
            {
                isValid = false;
                message = "Error Validation: Incomplete data...";
                return false;

            }
            message = "Successful Validation...";
            return true;
        }

        /// <summary>
        /// Validate sales order detail when it is updated.
        /// </summary>
        /// <param name="salesOrderDetail">SalesOrderDetailModel object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateUpdateSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, ISalesOrderRepository _salesOrderRepository, IItemRepository _itemRepository, out string message)
        {
            return this.ValidateCreateSalesOrderDetail(salesOrderDetail, _salesOrderRepository, _itemRepository, out message);
        }

        /// <summary>
        /// Validate sales order detail when it is deleted.
        /// Sales order detail can't be deleted if it is confirmed
        /// </summary>
        /// <param name="salesOrderDetail">SalesOrderDetailModel object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateDeleteSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, out string message)
        {
            message = "";
            if (salesOrderDetail.IsConfirmed)
            {
                message = "Can't be destroyed. Sales Order Detail is confirmed.";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validate sales order detail when it is confirmed.
        /// </summary>
        /// <param name="salesOrderDetail">SalesOrderDetailModel object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>       
        public bool ValidateConfirmSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, out string message)
        {
            message ="";
            return true;
        }

        /// <summary>
        /// Validate sales order detail when it is unconfirmed.
        /// Invalid if:
        /// 1. Quantity of item.PendingDelivery will be less than 0 after unconfirmation
        /// 2. There is confirmed sales order detail
        /// </summary>
        /// <param name="salesOrderDetail">SalesOrderDetailModel object</param>
        /// <param name="_salesOrderRepository">ISalesOrderRepository object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>       
        public bool ValidateUnconfirmSalesOrderDetail(SalesOrderDetailModel salesOrderDetail, ISalesOrderRepository _salesOrderRepository, IDeliveryOrderRepository _deliveryOrderRepository, IItemRepository _itemRepository, out string message)
        {        
            message = "";
            Item item = _itemRepository.Find(x => x.Id == salesOrderDetail.ItemId && !x.IsDeleted);
            if (item == null)
            {
                message = "Item can't be found";
                return false;
            }
            if ((item.PendingDelivery - salesOrderDetail.Quantity) < 0)
            {
                message = "Can't unconfirm. Not enough amount in stock Pending Delivery...";
                return false;
            }
            List<DeliveryOrderDetailModel> dod = _deliveryOrderRepository.GetDeliveryOrderDetailBySalesOrderDetail(salesOrderDetail.Id);

            if (dod.Count() > 0)
            {
                foreach (var model in dod)
                {
                    if (model.IsConfirmed)
                    {
                        message = "Invalid Unconfirmation. There is confirmed delivery order detail in the system.";
                        return false;
                    }
                }
            }
            return true;
        }

    }
}