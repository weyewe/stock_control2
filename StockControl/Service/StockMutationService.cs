using StockControl.Models;
using StockControl.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace StockControl.Service
{
    public class StockMutationService : IStockMutationService
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("StockMutationService");

        /// <summary>
        /// Get all stock mutations.
        /// </summary>
        /// <returns></returns>
        public List<StockMutationModel> GetStockMutationList(IStockMutationRepository _stockMutationRepository)
        {
            List<StockMutationModel> model = new List<StockMutationModel>();
            try
            {
                model = _stockMutationRepository.GetStockMutationList();
            }
            catch (Exception ex)
            {
                LOG.Error("GetStockMutationList Failed", ex);
            }

            return model;
        }

        /// <summary>
        /// Get all stock mutations.
        /// </summary>
        /// <returns></returns>
        /// 
        public List<StockMutationModel> GetStockMutationByItem(ItemModel item, IStockMutationRepository _stockMutationRepository)
        {
            List<StockMutationModel> models = new List<StockMutationModel>();
            try
            {
                List<StockMutation> stockMutations = _stockMutationRepository.GetStockMutationByItem(item);
                foreach (var stockMutation in stockMutations)
                {
                    StockMutationModel model = new StockMutationModel();
                    model.Id = stockMutation.Id;
                    model.Id = stockMutation.Id;
                    model.ItemId = stockMutation.ItemId;
                    model.Quantity = stockMutation.Quantity;
                    model.MutationCase = stockMutation.MutationCase;
                    model.ItemCase = stockMutation.ItemCase;
                    model.SourceDocumentId = stockMutation.ItemCase;
                    model.SourceDocument = stockMutation.SourceDocument;
                    model.SourceDocumentDetailId = stockMutation.SourceDocumentDetailId;
                    model.SourceDocumentDetail = stockMutation.SourceDocumentDetail;
                    model.IsDeleted = stockMutation.IsDeleted;
                    model.CreatedAt = stockMutation.CreatedAt;
                    model.UpdatedAt = stockMutation.UpdatedAt;
                    model.DeletedAt = stockMutation.DeletedAt;
                    models.Add(model);
                }
            }
            catch (Exception ex)
            {
                LOG.Error("GetStockMutationList Failed", ex);
            }

            return models;
        }

        public List<StockMutationModel> GetStockMutationBySourceDocument(ItemModel item, string sourceDocument, int sourceDocumentId, IStockMutationRepository _stockMutationRepository)
        {
            List<StockMutationModel> models = new List<StockMutationModel>();
            try
            {

                List<StockMutation> stockMutations = _stockMutationRepository.GetStockMutationBySourceDocument(item, sourceDocument, sourceDocumentId);
                foreach (var stockMutation in stockMutations)
                {
                    StockMutationModel model = new StockMutationModel();
                    model.Id = stockMutation.Id;
                    model.Id = stockMutation.Id;
                    model.ItemId = stockMutation.ItemId;
                    model.Quantity = stockMutation.Quantity;
                    model.MutationCase = stockMutation.MutationCase;
                    model.ItemCase = stockMutation.ItemCase;
                    model.SourceDocumentId = stockMutation.ItemCase;
                    model.SourceDocument = stockMutation.SourceDocument;
                    model.SourceDocumentDetailId = stockMutation.SourceDocumentDetailId;
                    model.SourceDocumentDetail = stockMutation.SourceDocumentDetail;
                    model.IsDeleted = stockMutation.IsDeleted;
                    model.CreatedAt = stockMutation.CreatedAt;
                    model.UpdatedAt = stockMutation.UpdatedAt;
                    model.DeletedAt = stockMutation.DeletedAt;
                    models.Add(model);
                }
            }
            catch (Exception ex)
            {
                LOG.Error("GetStockMutationList Failed", ex);
            }

            return models;
        }

        public List<StockMutationModel> GetStockMutationBySourceDocumentDetail(ItemModel item, string sourceDocumentDetail, int sourceDocumentDetailId, IStockMutationRepository _stockMutationRepository)
        {
            List<StockMutationModel> models = new List<StockMutationModel>();
            try
            {

                List<StockMutation> stockMutations = _stockMutationRepository.GetStockMutationBySourceDocumentDetail(item, sourceDocumentDetail, sourceDocumentDetailId);
                foreach (var stockMutation in stockMutations)
                {
                    StockMutationModel model = new StockMutationModel();
                    model.Id = stockMutation.Id;
                    model.Id = stockMutation.Id;
                    model.ItemId = stockMutation.ItemId;
                    model.Quantity = stockMutation.Quantity;
                    model.MutationCase = stockMutation.MutationCase;
                    model.ItemCase = stockMutation.ItemCase;
                    model.SourceDocumentId = stockMutation.ItemCase;
                    model.SourceDocument = stockMutation.SourceDocument;
                    model.SourceDocumentDetailId = stockMutation.SourceDocumentDetailId;
                    model.SourceDocumentDetail = stockMutation.SourceDocumentDetail;
                    model.IsDeleted = stockMutation.IsDeleted;
                    model.CreatedAt = stockMutation.CreatedAt;
                    model.UpdatedAt = stockMutation.UpdatedAt;
                    model.DeletedAt = stockMutation.DeletedAt;
                    models.Add(model);
                }
            }
            catch (Exception ex)
            {
                LOG.Error("GetStockMutationList Failed", ex);
            }

            return models;
        }

        // Create StockMutation
        public ResponseModel CreateStockMutation(StockMutationModel stockMutation, IStockMutationRepository _stockMutationRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateCreate(stockMutation, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                StockMutation newStockMutation = new StockMutation();
                newStockMutation.Id = stockMutation.Id;
                newStockMutation.ItemId = stockMutation.ItemId;
                newStockMutation.Quantity = stockMutation.Quantity;
                newStockMutation.MutationCase = stockMutation.MutationCase;
                newStockMutation.ItemCase = stockMutation.ItemCase;
                newStockMutation.SourceDocumentId = stockMutation.ItemCase;
                newStockMutation.SourceDocument = stockMutation.SourceDocument;
                newStockMutation.SourceDocumentDetailId = stockMutation.SourceDocumentDetailId;
                newStockMutation.SourceDocumentDetail = stockMutation.SourceDocumentDetail;
                newStockMutation.IsDeleted = stockMutation.IsDeleted;
                newStockMutation.CreatedAt = DateTime.Now;
                newStockMutation.UpdatedAt = stockMutation.UpdatedAt;
                newStockMutation.DeletedAt = stockMutation.DeletedAt;

                newStockMutation = _stockMutationRepository.CreateStockMutation(newStockMutation);
                newStockMutation.Id = newStockMutation.Id;

                respModel.isValid = true;
                respModel.message = "Create Data Success...";
                respModel.objResult = stockMutation;

                LOG.Error("CreateStockMutation Sucess");
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
                        LOG.ErrorFormat("CreateStockMutation, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("CreateStockMutation Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Create StockMutation failed, Please try again or stockMutation your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("CreateStockMutation Failed", ex);
                respModel.isValid = false;
                respModel.message = "Create StockMutation Failed, Please try again or stockMutation your administrator.";
            }

            return respModel;
        }

        // Update StockMutation
        public ResponseModel UpdateStockMutation(StockMutationModel stockMutation, IStockMutationRepository _stockMutationRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateUpdate (stockMutation, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                StockMutation updateStockMutation = _stockMutationRepository.Find(p => p.Id == stockMutation.Id);
                if (updateStockMutation != null)
                {
                    updateStockMutation.Id = stockMutation.Id;
                    updateStockMutation.ItemId = stockMutation.ItemId;
                    updateStockMutation.Quantity = stockMutation.Quantity;
                    updateStockMutation.MutationCase = stockMutation.MutationCase;
                    updateStockMutation.ItemCase = stockMutation.ItemCase;
                    updateStockMutation.SourceDocumentId = stockMutation.ItemCase;
                    updateStockMutation.SourceDocument = stockMutation.SourceDocument;
                    updateStockMutation.SourceDocumentDetailId = stockMutation.SourceDocumentDetailId;
                    updateStockMutation.SourceDocumentDetail = stockMutation.SourceDocumentDetail;
                    updateStockMutation.IsDeleted = stockMutation.IsDeleted;
                    updateStockMutation.CreatedAt = stockMutation.CreatedAt;
                    updateStockMutation.UpdatedAt = DateTime.Now;
                    updateStockMutation.DeletedAt = stockMutation.DeletedAt;

                    _stockMutationRepository.UpdateStockMutation(updateStockMutation);

                    respModel.isValid = true;
                    respModel.message = "Update Data Success...";
                    respModel.objResult = stockMutation;

                    LOG.Info("UpdateStockMutation Success");
                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "StockMutation not found...";
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
                        LOG.ErrorFormat("UpdateStockMutation, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UpdateStockMutation Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Update data failed, Please try again or stockMutation your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UpdateStockMutation Failed", ex);
                respModel.isValid = false;
                respModel.message = "Update data Failed, Please try again or stockMutation your administrator.";
            }

            return respModel;
        }

        public bool ValidateCreate(StockMutationModel model, out string message)
        {
            bool isValid = true;
            message = "OK";

            /* Id, ItemId, Quantity, MutationCase, ItemCase, SourceDocumentId, SourceDocument,
             * SourceDocumentDetailId, SourceDocumentDetail must be present
             */

            if (String.IsNullOrEmpty(model.ItemId) || model.ItemId.Trim() == "")
            {
                message = "Invalid input data...";
                return false;
            }

            // These value will never be null
            /*
            if (model.Quantity == null || model.SourceDocumentId == null || model.SourceDocumentDetailId == null)
            {
                message = "Invalid input data...";
                return false;
            }
            */
            if (model.MutationCase != 1 && model.MutationCase != 2)
            {
                message = "Invalid input data...";
                return false;
            }

            if (model.ItemCase != 1 && model.ItemCase != 2 && model.ItemCase != 3)
            {
                message = "Invalid input data...";
                return false;
            }

            if (!(model.SourceDocument.Equals("PurchaseOrder") ||
                    model.SourceDocument.Equals("PurchaseReceival") ||
                    model.SourceDocument.Equals("SalesOrder") ||
                    model.SourceDocument.Equals("DeliveryOrder")))
            {
                message = "Invalid Source Document " + model.SourceDocument + "...";
                return false;
            }

            if (!(model.SourceDocumentDetail.Equals("PurchaseOrderDetail") ||
                    model.SourceDocumentDetail.Equals("PurchaseReceivalDetail") ||
                    model.SourceDocumentDetail.Equals("SalesOrderDetail") ||
                    model.SourceDocumentDetail.Equals("DeliveryOrderDetail")))
            {
                message = "Invalid Source Document Detail " + model.SourceDocumentDetail + "...";
                return false;
            }

            return isValid;
        }

        public bool ValidateUpdate(StockMutationModel model, out string message)
        {
            bool isValid = ValidateCreate(model, out message);
            // This value will never be null.
            /*
            if (model.Id == null)
            {
                message = "Invalid id...";
                return false;
            }
             */ 
            return isValid;
        }
    }
}