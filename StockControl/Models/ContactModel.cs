using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Models
{
    public class ContactModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }
       
        public static ContactModel ToModel (Contact contact) {
            ContactModel model = new ContactModel();      
            model.Id = contact.Id;
            model.Name = contact.Name;
            model.Address = contact.Address;
            model.IsDeleted = contact.IsDeleted;
            model.CreatedAt = contact.CreatedAt;
            model.UpdatedAt = contact.UpdatedAt;
            model.DeletedAt = contact.DeletedAt;
            return model;
        }
    }
}