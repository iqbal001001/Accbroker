using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Reflection;
using System.Web;
using AccBroker.Domain;

namespace AccBroker.WebAPI
{
    public static class AddressExtension
    {
        public static AddressDTO ToDTO(this Address address)
        {
            return new AddressDTO
            {
                ID = address.ID,
                AddressLine1 = address.AddressLine1 != null ? address.AddressLine1.Trim() : null,
                AddressLine2 = address.AddressLine2 != null ? address.AddressLine2.Trim() : null,
                Suburb = address.Suburb != null ? address.Suburb.Trim() : null,
                State = address.State != null ? address.State.Trim() : null,
                PostCode = address.PostCode != null ? address.PostCode.Trim() : null,
                AddressType = address.AddressType,
                ClientID = address.ClientID,
                CompanyID = address.CompanyID,
                Concurrency = address.Concurrency,
                CreateDate = address.CreateDate,
                ChangeDate = address.ChangeDate
            };
        }

        public static object ToDataShapeObject(this AddressDTO address, List<string> lstOfFields)
        {
            if (!lstOfFields.Any())
            {
                return address;
            }
            else
            {
                ExpandoObject objectToReturn = new ExpandoObject();
                foreach (var field in lstOfFields)
                {
                    var fieldValue = address.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(address, null);

                    ((IDictionary<string, object>)objectToReturn).Add(field, fieldValue);

                }
                return objectToReturn;
            }

        }

        public static Address ToDomain(this AddressDTO address, Address originalAddress = null)
        {
            if (originalAddress != null && originalAddress.ID == address.ID)
            {
                originalAddress.AddressLine1 = address.AddressLine1;
                 originalAddress.AddressLine2 = address.AddressLine2;
                 originalAddress.Suburb = address.Suburb;
                originalAddress.State = address.State;
                 originalAddress.PostCode = address.PostCode;
                 originalAddress.AddressType = address.AddressType;
                 originalAddress.ClientID = address.ClientID;
                 originalAddress.CompanyID = address.CompanyID;
                 originalAddress.CreateDate = address.CreateDate;
                 originalAddress.ChangeDate = address.ChangeDate;

                return originalAddress;
            }
            return new Address
            {
                ID = address.ID,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                Suburb = address.Suburb,
                State = address.State,
                PostCode = address.PostCode,
                AddressType = address.AddressType,
                ClientID = address.ClientID,
                CompanyID = address.CompanyID,
                Concurrency = address.Concurrency,
                CreateDate = address.CreateDate,
                ChangeDate = address.ChangeDate
            };
        }

      
    }
}