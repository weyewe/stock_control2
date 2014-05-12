using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockControl.Models;

namespace StockControl.Repository
{
    public interface IItemRepository : IRepository<Item>
    {

        List<ItemModel> GetItemList();
        ItemModel GetItem(int id);
        Item CreateItem(Item item);
        void DeleteItem(int id);
        Item UpdateItem(Item item);
        Item getDuplicateSku(ItemModel item);
    }
}