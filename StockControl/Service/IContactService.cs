using StockControl.Models;
using StockControl.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControl.Service
{
    public interface IContactService
    {

        List<ContactModel> GetContactList(IContactRepository _contactRepository);
        ContactModel GetContact(int Id, IContactRepository _contactRepository);

        ResponseModel CreateContact(ContactModel contact, IContactRepository _contactRepository);
        ResponseModel DeleteContact(int id, IContactRepository _contactRepository, IPurchaseOrderRepository _po, 
                        IPurchaseReceivalRepository _pr, ISalesOrderRepository _so, IDeliveryOrderRepository _do);
        ResponseModel UpdateContact(ContactModel contact, IContactRepository _contactRepository);

        // Name must be present
        bool ValidateCreateUpdate(ContactModel contact, out string message);

        // Can't be destroyed if there is PurchaseOrder, PurchaseReceival, SalesOrder,
        // DeliveryOrder associated with the given contact.
        bool ValidateDelete(ContactModel model, IPurchaseOrderRepository _po, IPurchaseReceivalRepository _pr,
                                     ISalesOrderRepository _so, IDeliveryOrderRepository _do, out string message);

    }
}