using StockControl.Models;
using StockControl.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace StockControl.Service
{
    public class ContactService : IContactService
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ContactService");

        /// <summary>
        /// Get all contacts.
        /// </summary>
        /// <returns></returns>
        public List<ContactModel> GetContactList(IContactRepository _contactRepository)
        {
            List<ContactModel> model = new List<ContactModel>();
            try
            {
                model = _contactRepository.GetContactList();
            }
            catch (Exception ex)
            {
                LOG.Error("GetContactList Failed", ex);
            }

            return model;
        }

        /// <summary>
        /// Get all a contact.
        /// </summary>
        /// <param name="Id">Id of the contact</param>
        /// <param name="_contactRepository">Passed on parameter</param>
        /// <returns>A contact</returns>
        public ContactModel GetContact(int Id, IContactRepository _contactRepository)
        {
            ContactModel model = new ContactModel();
            try
            {
                model = _contactRepository.GetContact(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetContact Failed", ex);
            }

            return model;
        }

        // Create Contact
        public ResponseModel CreateContact(ContactModel contact, IContactRepository _contactRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateCreateUpdate(contact, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                Contact newContact = new Contact();
                newContact.Id = contact.Id;
                newContact.Name = contact.Name;
                newContact.Address = contact.Address;
                newContact.IsDeleted = contact.IsDeleted;
                newContact.CreatedAt = DateTime.Now;
                newContact.UpdatedAt = contact.UpdatedAt;
                newContact.DeletedAt = contact.DeletedAt;

                newContact = _contactRepository.CreateContact(newContact);
                newContact.Id = newContact.Id;

                respModel.isValid = true;
                respModel.message = "Create Data Success...";
                respModel.objResult = contact;

                LOG.Error("CreateContact Sucess");
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
                        LOG.ErrorFormat("CreateContact, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("CreateContact Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Create Contact failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("CreateContact Failed", ex);
                respModel.isValid = false;
                respModel.message = "Create Contact Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        // Delete Contact
        public ResponseModel DeleteContact(int contactId, IContactRepository _contactRepository, IPurchaseOrderRepository _po, IPurchaseReceivalRepository _pr,
                                     ISalesOrderRepository _so, IDeliveryOrderRepository _do)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                Contact deleteContact = _contactRepository.Find(p => p.Id == contactId);
                if (deleteContact != null)
                {
                    string message = "";
                    ContactModel model = ContactModel.ToModel(deleteContact);
                    respModel.isValid = this.ValidateDelete(model, _po, _pr, _so, _do, out message);
                    if (!respModel.isValid)
                    {
                        respModel.message = message;
                        return respModel;
                    }

                    // Delete Contact
                    _contactRepository.DeleteContact(contactId);

                    respModel.isValid = true;
                    respModel.message = "Delete Contact Success...";
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
                        LOG.ErrorFormat("DeleteContact, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("DeleteContact Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Delete Contact failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("DeleteContact Failed", ex);
                respModel.isValid = false;
                respModel.message = "Delete Contact Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        // Update Contact
        public ResponseModel UpdateContact(ContactModel contact, IContactRepository _contactRepository)
        {
            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;
            try
            {
                string message = "";
                respModel.isValid = this.ValidateCreateUpdate (contact, out message);
                if (!respModel.isValid)
                {
                    respModel.message = message;
                    return respModel;
                }

                Contact updateContact = _contactRepository.Find(p => p.Id == contact.Id);
                if (updateContact != null)
                {
                    updateContact.Id = contact.Id;
                    updateContact.Name = contact.Name;
                    updateContact.Address = contact.Address;
                    updateContact.IsDeleted = contact.IsDeleted;
                    updateContact.CreatedAt = contact.CreatedAt;
                    updateContact.UpdatedAt = DateTime.Now;
                    updateContact.DeletedAt = contact.DeletedAt;

                    _contactRepository.UpdateContact(updateContact);

                    respModel.isValid = true;
                    respModel.message = "Update Data Success...";
                    respModel.objResult = contact;

                    LOG.Info("UpdateContact Success");
                }
                else
                {
                    respModel.isValid = false;
                    respModel.message = "Contact not found...";
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
                        LOG.ErrorFormat("UpdateContact, Error:Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                LOG.Error("UpdateContact Failed", dbEx);
                respModel.isValid = false;
                respModel.message = "Update data failed, Please try again or contact your administrator.";
            }
            catch (Exception ex)
            {
                LOG.Error("UpdateContact Failed", ex);
                respModel.isValid = false;
                respModel.message = "Update data Failed, Please try again or contact your administrator.";
            }

            return respModel;
        }

        private bool Validate(ContactModel model, out string message)
        {
            bool isValid = true;
            message = "OK";

            // Description
            if (String.IsNullOrEmpty(model.Name) || model.Name.Trim() == "")
            {
                message = "Invalid Name...";
                return false;
            }

            return isValid;
        }

        public bool ValidateCreateUpdate(ContactModel model, out string message)
        {
            bool isValid = this.Validate(model, out message);

            return isValid;
        }

        // Can't be destroyed if there is PurchaseOrder, PurchaseReceival, SalesOrder,
        // DeliveryOrder associated with the given contact.
        public bool ValidateDelete(ContactModel model, IPurchaseOrderRepository _po, IPurchaseReceivalRepository _pr,
                                     ISalesOrderRepository _so, IDeliveryOrderRepository _do, out string message)
        {
            bool isValid = this.Validate(model, out message);
            
            if (_po.GetPurchaseOrderByContactId(model.Id).Count() > 0)
            {
                    message = "This contact is an active member ...";
                    isValid = false;
                    return isValid;
            }

            if (_pr.GetPurchaseReceivalByContactId(model.Id).Count() > 0)
            {
                    message = "This contact is an active member ...";
                    isValid = false;
                    return isValid;
            }
            if (_so.GetSalesOrderByContactId(model.Id).Count() > 0)
            {
                message = "This contact is an active member ...";
                isValid = false;
                return isValid;
            }
            if (_do.GetDeliveryOrderByContactId(model.Id).Count() > 0)
            {
                message = "This contact is an active member ...";
                isValid = false;
                return isValid;
            }

            return isValid;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}