using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockControl.Models;

namespace StockControl.Repository
{
    public interface IContactRepository : IRepository<Contact>
    {

        List<ContactModel> GetContactList();
        ContactModel GetContact(int Id);
        Contact CreateContact(Contact contact);
        void DeleteContact(int id);
        Contact UpdateContact(Contact contact);

    }
}