using StockControl.Models;
using StockControl.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace StockControl.Service
{
    public class ItemService : IItemService
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ItemService");

        /*
         * GET
         */

        /// <summary>
        /// Get all items.
        /// </summary>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <returns>all items</returns>
        public List<ItemModel> GetItemList(IItemRepository _itemRepository)
        {
            List<ItemModel> model = new List<ItemModel>();
            try
            {
                model = _itemRepository.GetItemList();
            }
            catch (Exception ex)
            {
                LOG.Error("GetItemList Failed", ex);
            }

            return model;
        }

        /// <summary>
        /// Get an item.
        /// </summary>
        /// <param name="Id">Id of the item</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <returns>an item</returns>
        public ItemModel GetItem(int Id, IItemRepository _itemRepository)
        {
            ItemModel model = new ItemModel();
            try
            {
                model = _itemRepository.GetItem(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetItem Failed", ex);
            }

            return model;
        }

        /*
         * CREATE
         */

        /// <summary>
        /// Create an item.
        /// </summary>
        /// <param name="item">ItemModel object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <returns>A response model</returns>
        public ResponseModel CreateItem(ItemModel item, IItemRepository _itemRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateCreateUpdate(item, _itemRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                Item newItem = new Item();
                newItem.Id = item.Id;
                newItem.Sku = item.Sku;
                newItem.Name = item.Name;
                newItem.Ready = item.Ready;
                newItem.PendingDelivery = item.PendingDelivery;
                newItem.PendingReceival = item.PendingReceival;
                newItem.IsDeleted = item.IsDeleted;
                newItem.CreatedAt = DateTime.Now;
                newItem.UpdatedAt = item.UpdatedAt;
                newItem.DeletedAt = item.DeletedAt;

                newItem = _itemRepository.CreateItem(newItem);
                newItem.Id = newItem.Id;

                respModel.isValid = true;
                respModel.message = "Create Data Success...";
                respModel.objResult = item;

                LOG.Error("CreateItem Sucess");
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
                        LOG.ErrorFormat("CreateItem, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("CreateItem Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Create Item failed, Please try again or item your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("CreateItem Failed", ex);
                respModel.isValid = false;
                respModel.message = "Create Item Failed, Please try again or item your administrator.";
            }

            return respModel;
        }

        /*
         * DELETE
         */

        /// <summary>
        /// Delete an item.
        /// </summary>
        /// <param name="itemId">Id of the item</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="_stockMutationRepository">IStockMutation object</param>
        /// <returns>A response model</returns>
        public ResponseModel DeleteItem(int itemId, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                Item deleteItem = _itemRepository.Find(p => p.Id == itemId && !p.IsDeleted);
                if (deleteItem != null)
                {
                    ItemModel model = ItemModel.ToModel(deleteItem);
                    respModel.isValid = this.ValidateDelete(model, _stockMutationRepository, out message);
                    if (!respModel.isValid)
                    {
                        respModel.message = message;
                        return respModel;
                    }

                    // Delete Item
                    _itemRepository.DeleteItem(itemId);

                    respModel.isValid = true;
                    respModel.message = "Delete Item Success...";
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
                        LOG.ErrorFormat("DeleteItem, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("DeleteItem Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Delete Item failed, Please try again or item your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("DeleteItem Failed", ex);
                respModel.isValid = false;
                respModel.message = "Delete Item Failed, Please try again or item your administrator.";
            }

            return respModel;
        }

        /*
         * UPDATE
         */

        /// <summary>
        /// Update an item.
        /// </summary>
        /// <param name="item">ItemModel object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <returns>A response model</returns>
        public ResponseModel UpdateItem(ItemModel item, IItemRepository _itemRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateCreateUpdate (item, _itemRepository, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                Item updateItem = _itemRepository.Find(p => p.Id == item.Id && !p.IsDeleted);
                if (updateItem != null)
                {
                    updateItem.Id = item.Id;
                    updateItem.Sku = item.Sku;
                    updateItem.Name = item.Name;
                    updateItem.Ready = item.Ready;
                    updateItem.PendingDelivery = item.PendingDelivery;
                    updateItem.PendingReceival = item.PendingReceival;
                    updateItem.IsDeleted = item.IsDeleted;
                    updateItem.CreatedAt = item.CreatedAt;
                    updateItem.UpdatedAt = DateTime.Now;
                    updateItem.DeletedAt = item.DeletedAt;

                    _itemRepository.UpdateItem(updateItem);

                    respModel.isValid = true;
                    respModel.message = "Update Data Success...";
                    respModel.objResult = item;

                    LOG.Info("UpdateItem Success");
                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "Item not found...";
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
                        LOG.ErrorFormat("UpdateItem, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UpdateItem Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Update data failed, Please try again or item your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UpdateItem Failed", ex);
                respModel.isValid = false;
                respModel.message = "Update data Failed, Please try again or item your administrator.";
            }

            return respModel;
        }

        /*
         * VALIDATE
         */

        /// <summary>
        /// Validate an item. For an item to be valid, a name must be present and
        /// its Sku (Stock keeping unit) must be present and unique among non-deleted item.
        /// </summary>
        /// <param name="model">ContactModel object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        private bool Validate(ItemModel model, IItemRepository _itemRepository, out string message)
        {
            bool isValid = true;
            message = "OK";
            
            if (String.IsNullOrEmpty(model.Name) || model.Name.Trim() == "")
            {
                message = "Invalid Name...";
                return false;
            }
            if(_itemRepository.GetDuplicateSku(model) != null)
            {
                message = "Duplicate Sku " + model.Sku + "...";
                return false;
            }
            return isValid;
        }

        /// <summary>
        /// Validate an item when it is created / updated.
        /// Calls private function Validate (ItemModel, IItemRepository, out string). 
        /// </summary>
        /// <param name="model">ItemModel object</param>
        /// <param name="_itemRepository">IItemRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateCreateUpdate(ItemModel model, IItemRepository _itemRepository, out string message)
        {
            bool isValid = this.Validate(model, _itemRepository, out message);

            return isValid;
        }

        /// <summary>
        /// Validate an item when it is deleted.
        /// An item can't be deleted if it is associated to any stock mutation(s).
        /// </summary>
        /// <param name="model">ItemModel object</param>
        /// <param name="_stockMutationRepository">IIStockMutationRepository object</param>
        /// <param name="message">Message</param>
        /// <returns>true or false</returns>
        public bool ValidateDelete(ItemModel model, IStockMutationRepository _stockMutationRepository, out string message)
        {
            
            bool isValid = true;
            message = "OK";

            if (_stockMutationRepository.GetStockMutationByItem(model).Count() > 0)
            {
                    message = "This item has stock mutations associated to the item ...";
                    isValid = false;
                    return isValid;
            }

            return isValid;
        }
    }
}