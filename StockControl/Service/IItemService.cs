using StockControl.Models;
using StockControl.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Service
{
    public interface IItemService
    {

        List<ItemModel> GetItemList(IItemRepository _itemRepository);
        ItemModel GetItem(int Id, IItemRepository _itemRepository);
        ResponseModel CreateItem(ItemModel item, IItemRepository _i);
        ResponseModel DeleteItem(int itemId, IItemRepository _itemRepository, IStockMutationRepository _stockMutationRepository);
        ResponseModel UpdateItem(ItemModel item, IItemRepository _itemRepository);

        // Sku must be present and unique among non-deleted item
        // Name must be present
        bool ValidateCreateUpdate(ItemModel model, IItemRepository _itemRepository, out string message);
        
        // Item has any StockMutation associated to it
        bool ValidateDelete(ItemModel model, IStockMutationRepository _stockMutationRepository, out string message);

        /*
         * PO -> Item.pendingReceival ++
         * PR -> Item.pendingReceival --, Ready ++
         * SO -> Item.pendingDelivery ++
         * DO -> Item.pendingDelivery --, Ready --
         * 
         * Update (item)
         * Create (stockMutation)
         */

    }
}