using StockControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace StockControl.Repository
{
    public class ItemRepository : EfRepository<Item>, IItemRepository
    {

        /*
         * GET
         */

        /// <summary>
        /// Get all items from Database.
        /// </summary>
        /// <returns>All items</returns>
        public List<ItemModel> GetItemList()
        {
             using (var db = GetContext())
            {
                IQueryable<ItemModel> im = (from i in db.Items
                                            where !i.IsDeleted
                                               select new ItemModel
                                               {
                                                   Id = i.Id,
                                                   Sku = i.Sku,
                                                   Name = i.Name,
                                                   Ready = i.Ready,
                                                   PendingDelivery = i.PendingDelivery,
                                                   PendingReceival = i.PendingReceival,
                                                   IsDeleted = i.IsDeleted,
                                                   CreatedAt = i.CreatedAt,
                                                   UpdatedAt = i.UpdatedAt,
                                                   DeletedAt = i.DeletedAt
                                               }).AsQueryable();

                return im.ToList();
            }
        }

        /// <summary>
        /// Get an item from Database.
        /// </summary>
        /// <param name="Id">Id of the item</param>
        /// <returns>An item</returns>
        public ItemModel GetItem(int Id)
        {
            using (var db = GetContext())
            {
                ItemModel im = (from i in db.Items
                                            where !i.IsDeleted && i.Id == Id
                                            select new ItemModel
                                            {
                                                Id = i.Id,
                                                Sku = i.Sku,
                                                Name = i.Name,
                                                Ready = i.Ready,
                                                PendingDelivery = i.PendingDelivery,
                                                PendingReceival = i.PendingReceival,
                                                IsDeleted = i.IsDeleted,
                                                CreatedAt = i.CreatedAt,
                                                UpdatedAt = i.UpdatedAt,
                                                DeletedAt = i.DeletedAt
                                            }).FirstOrDefault();

                return im;
            }
        }

        /// <summary>
        /// Get one duplicate Stock keeping unit.
        /// </summary>
        /// <param name="item">Item to be checked with the database</param>
        /// <returns>Duplicated Sku</returns>
        public Item GetDuplicateSku(ItemModel item)
        {
            Item i = Find(x => x.Sku == item.Sku && !x.IsDeleted && x.Id != item.Id);
            return i;
        }

        /*
         * CREATE
         */
        /// <summary>
        /// Create a new item.
        /// </summary>
        /// <param name="item">An object Item</param>
        /// <returns>The new item</returns>
        public Item CreateItem(Item item)
        {
            Item newitem = new Item();                                                   
            newitem.Id = item.Id;
            newitem.Sku = item.Sku;
            newitem.Name = item.Name;
            newitem.Ready = item.Ready;
            newitem.PendingDelivery = item.PendingDelivery;
            newitem.PendingReceival = item.PendingReceival;
            newitem.IsDeleted = item.IsDeleted;
            newitem.CreatedAt = item.CreatedAt;
            newitem.UpdatedAt = item.UpdatedAt;
            newitem.DeletedAt = item.DeletedAt;
            return Create(newitem);
        }

        /* 
         * DELETE
         */
        /// <summary>
        /// Delete a certain item
        /// </summary>
        /// <param name="id">Item Id</param>
        public void DeleteItem(int id)
        {
            Item i = Find(x => x.Id == id && !x.IsDeleted);
            if (i != null)
            {
                i.IsDeleted = true;
                i.DeletedAt = DateTime.Now;
                Update(i);
            }
        }

        /*
         * UPDATE
         */

        /// <summary>
        /// Update a item.
        /// </summary>
        /// <param name="item">An object Item</param>
        /// <returns>The updated item</returns>
        public Item UpdateItem(Item item)
        {
            Item i = Find(x => x.Id == item.Id && !x.IsDeleted);
            if (i != null)
            {
                i.Id = item.Id;
                i.Sku = item.Sku;
                i.Name = item.Name;
                i.Ready = item.Ready;
                i.PendingDelivery = item.PendingDelivery;
                i.PendingReceival = item.PendingReceival;
                i.IsDeleted = item.IsDeleted;
                i.CreatedAt = item.CreatedAt;
                i.UpdatedAt = item.UpdatedAt;
                i.DeletedAt = item.DeletedAt;

                Update(i);
                return i;
            }
            return i;
        }
    }
}