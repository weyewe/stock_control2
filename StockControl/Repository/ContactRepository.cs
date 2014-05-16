using StockControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace StockControl.Repository
{
    public class ContactRepository : EfRepository<Contact>, IContactRepository
    {
        /*
         * GET
         */

        /// <summary>
        /// Get all contacts from Database.
        /// </summary>
        /// <returns>All contacts</returns>
        public List<ContactModel> GetContactList()
        {
             using (var db = GetContext())
            {
                IQueryable<ContactModel> cm = (from c in db.Contacts
                                               where !c.IsDeleted
                                               select new ContactModel
                                               {
                                                   Id = c.Id,
                                                   Name = c.Name,
                                                   Address = c.Address,
                                                   IsDeleted = c.IsDeleted,
                                                   CreatedAt = c.CreatedAt,
                                                   UpdatedAt = c.UpdatedAt,
                                                   DeletedAt = c.DeletedAt
                                               }).AsQueryable();

                return cm.ToList();
            }
        }

        /// <summary>
        /// Get all deleted contacts from Database. ONLY to be used when requested.
        /// </summary>
        /// <returns>All deleted contacts</returns>
        public List<ContactModel> GetDeletedList()
        {
            using (var db = GetContext())
            {
                IQueryable<ContactModel> cm = (from c in db.Contacts
                                               where c.IsDeleted
                                               select new ContactModel
                                               {
                                                   Id = c.Id,
                                                   Name = c.Name,
                                                   Address = c.Address,
                                                   IsDeleted = c.IsDeleted,
                                                   CreatedAt = c.CreatedAt,
                                                   UpdatedAt = c.UpdatedAt,
                                                   DeletedAt = c.DeletedAt
                                               }).AsQueryable();

                return cm.ToList();
            }
        }

        /// <summary>
        /// Get a contact from Database.
        /// </summary>
        /// <returns>A contact</returns>
        public ContactModel GetContact(int Id)
        {
            using (var db = GetContext())
            {
                ContactModel cm = (from c in db.Contacts
                                    where !c.IsDeleted && c.Id == Id
                                    select new ContactModel
                                    {
                                        Id = c.Id,
                                        Name = c.Name,
                                        Address = c.Address,
                                        IsDeleted = c.IsDeleted,
                                        CreatedAt = c.CreatedAt,
                                        UpdatedAt = c.UpdatedAt,
                                        DeletedAt = c.DeletedAt
                                    }).FirstOrDefault();

                return cm;
            }
        }

        /*
         * CREATE
         */

        /// <summary>
        /// Create a new contact.
        /// </summary>
        /// <param name="contact">An object Contact</param>
        /// <returns>The new contact</returns>
        public Contact CreateContact(Contact contact)
        {
            Contact newcontact = new Contact();
            newcontact.Id = contact.Id;
            newcontact.Name = contact.Name;
            newcontact.Address = contact.Address;
            newcontact.IsDeleted = contact.IsDeleted;
            newcontact.CreatedAt = contact.CreatedAt;
            newcontact.UpdatedAt = contact.UpdatedAt;
            newcontact.DeletedAt = contact.DeletedAt;
            return Create(newcontact);
        }

        /*
         * DELETE
         */

        /// <summary>
        /// Delete a certain contact
        /// </summary>
        /// <param name="id">Contact Id</param>
        public void DeleteContact(int id)
        {
            Contact c = Find(x => x.Id == id && !x.IsDeleted);
            if (c != null)
            {
                c.IsDeleted = true;
                c.DeletedAt = DateTime.Now;
                Update(c);
            }
        }

        /*
         * UPDATE
         */

        /// <summary>
        /// Update a contact.
        /// </summary>
        /// <param name="contact">An object Contact</param>
        /// <returns>The updated contact</returns>
        public Contact UpdateContact(Contact contact)
        {
            Contact c = Find(x => x.Id == contact.Id && !x.IsDeleted);
            if (c != null)
            {
                c.Id = contact.Id;
                c.Name = contact.Name;
                c.Address = contact.Address;
                c.IsDeleted = contact.IsDeleted;
                c.CreatedAt = contact.CreatedAt;
                c.UpdatedAt = contact.UpdatedAt;
                c.DeletedAt = contact.DeletedAt;

                Update(c);
                return c;
            }
            return c;
        }

    }
}