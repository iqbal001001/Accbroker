using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Reflection;
using System.Web;
using AccBroker.Domain;

namespace AccBroker.WebAPI
{
    public static class ContactExtension
    {
        public static ContactDTO ToDTO(this Contact contact)
        {
            return new ContactDTO
            {
                ID = contact.ID,
                ContactType = contact.ContactType,
                Name = contact.Name,
                Position = contact.Position,
                HomePhone = contact.HomePhone,
                Mobile = contact.Mobile,
                WorkPhone = contact.WorkPhone,
                ClientID = contact.ClientID,
                CompanyID = contact.CompanyID,
                Concurrency = contact.Concurrency,
                CreateDate = contact.CreateDate,
                ChangeDate = contact.ChangeDate
            };
        }

        public static object ToDataShapeObject(this ContactDTO contact, List<string> lstOfFields)
        {
            if (!lstOfFields.Any())
            {
                return contact;
            }
            else
            {
                ExpandoObject objectToReturn = new ExpandoObject();
                foreach (var field in lstOfFields)
                {
                    var fieldValue = contact.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(contact, null);

                    ((IDictionary<string, object>)objectToReturn).Add(field, fieldValue);

                }
                return objectToReturn;
            }

        }

        public static Contact ToDomain(this ContactDTO contact)
        {
            return new Contact
            {
                ID = contact.ID,
                ContactType = contact.ContactType,
                Name = contact.Name,
                Position = contact.Position,
                HomePhone = contact.HomePhone,
                Mobile = contact.Mobile,
                WorkPhone = contact.WorkPhone,
                ClientID = contact.ClientID,
                CompanyID = contact.CompanyID,
                Concurrency = contact.Concurrency,
                CreateDate = contact.CreateDate,
                ChangeDate = contact.ChangeDate
            };
        }

        public static Contact ToDomain(this ContactDTO contact, Contact originalContact = null)
        {
            if (originalContact != null && originalContact.ID == contact.ID)
            {
               originalContact.ContactType = contact.ContactType;
                originalContact.Name = contact.Name;
                originalContact.Position = contact.Position;
                originalContact.HomePhone = contact.HomePhone;
               originalContact.Mobile = contact.Mobile;
                originalContact.WorkPhone = contact.WorkPhone;
                originalContact.ClientID = contact.ClientID;
                originalContact.CompanyID = contact.CompanyID;
                originalContact.ChangeDate = contact.ChangeDate;
                originalContact.CreateDate = contact.CreateDate;

                return originalContact;
            }

            return new Contact
            {
                ID = contact.ID,
                ContactType = contact.ContactType,
                Name = contact.Name,
                Position = contact.Position,
                HomePhone = contact.HomePhone,
                Mobile = contact.Mobile,
                WorkPhone = contact.WorkPhone,
                ClientID = contact.ClientID,
                CompanyID = contact.CompanyID,
                Concurrency = contact.Concurrency,
                CreateDate = contact.CreateDate,
                ChangeDate = contact.ChangeDate
            };

        }
    }
}