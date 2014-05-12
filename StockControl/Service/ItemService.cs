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

        /// <summary>
        /// Get all items.
        /// </summary>
        /// <returns></returns>
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
        /// <param name="_itemRepository">Passed on parameter</param>
        /// <returns>An item.</returns>
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

        // Create Item
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

        // Delete Item
        public ResponseModel DeleteItem(int itemId, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                Item deleteItem = _itemRepository.Find(p => p.Id == itemId);
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

        // Update Item
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

        public bool Validate(ItemModel model, IItemRepository _itemRepository, out string message)
        {
            bool isValid = true;
            message = "OK";

            // Name must be present
            if (String.IsNullOrEmpty(model.Name) || model.Name.Trim() == "")
            {
                message = "Invalid Name...";
                return false;
            }
            // Sku must be present and unique among non-deleted item
            if(_itemRepository.getDuplicateSku(model) != null)
            {
                message = "Duplicate Sku " + model.Sku + "...";
                return false;
            }
            return isValid;
        }

        // Name must be present
        // Sku must be present and unique among non-deleted item
        public bool ValidateCreateUpdate(ItemModel model, IItemRepository _itemRepository, out string message)
        {
            bool isValid = this.Validate(model, _itemRepository, out message);

            return isValid;
        }

        // Item has any StockMutation associated to it
        public bool ValidateDelete(ItemModel model, IStockMutationRepository _stockMutationRepository, out string message)
        {
            
            bool isValid = true;
            message = "OK";

            if (_stockMutationRepository.GetStockMutationByItem(model).Count() > 0)
            {
                    message = "This item has stock mutations association ...";
                    isValid = false;
                    return isValid;
            }

            return isValid;
        }
    }
}