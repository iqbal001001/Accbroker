using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Reflection;
using System.Web;
using AccBroker.Domain;

namespace AccBroker.WebAPI
{
    public static class ClientExtension
    {
        public static ClientDTO ToDTO(this Client client)
        {
            return new ClientDTO
            {
                ID = client.ID,
                Code = client.Code != null ? client.Code.Trim() : null,
                ABN = client.ABN != null ? client.ABN.Trim() : null,
                Name = client.Name != null ? client.Name.Trim() : null,
                ChangeDate = client.ChangeDate,
                CreateDate = client.CreateDate,
                Concurrency = client.Concurrency,
                AppliedGST = client.AppliedGST,
                AutoApplyGST = client.AutoApplyGST,
                Addresses = client.Addresses != null ? client.Addresses.Select(add => add.ToDTO()).ToList() : new List<AddressDTO>(),
                Contacts = client.Contacts != null ? client.Contacts.Select(con => con.ToDTO()).ToList() : new List<ContactDTO>(),
            };
        }

        public static object ToDataShapeObject(this ClientDTO client, List<string> lstOfFields)
        {
            List<string> lstOfFieldsToWorkWith = new List<string>(lstOfFields);

            if (!lstOfFieldsToWorkWith.Any())
            {
                return client;
            }
            else
            {
                var lstOfAddressFields = lstOfFieldsToWorkWith.Where(f => f.Contains("addresses.")).ToList();

                bool returnPartialAddress = lstOfAddressFields.Any() && !lstOfAddressFields.Contains("addresses.");

                if (returnPartialAddress)
                {
                    lstOfFieldsToWorkWith.RemoveAll(f => f.Contains("addresses."));
                    lstOfAddressFields = lstOfAddressFields.Select(f => f.Substring(f.IndexOf(".") + 1)).ToList();
                }
                else
                {
                    //lstOfAddressFields.Remove("addresses");
                    lstOfFieldsToWorkWith.RemoveAll(f => f.Contains("addresses."));
                }

                ExpandoObject objectToReturn = new ExpandoObject();
                foreach (var field in lstOfFieldsToWorkWith)
                {
                    var fieldValue = client.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(client, null);

                    ((IDictionary<string, object>)objectToReturn).Add(field, fieldValue);

                }

                if (returnPartialAddress)
                {
                    List<object> addresses = new List<object>();
                    foreach (var address in client.Addresses)
                    {
                        addresses.Add(address.ToDataShapeObject(lstOfAddressFields));
                    }

                    ((IDictionary<string, object>)objectToReturn).Add("addresses", addresses);

                }
                return objectToReturn;
            }
        }

        public static Client ToDomain(this ClientDTO client, Client originalClient = null)
        {
            if (originalClient != null && originalClient.ID == client.ID)
            {
                originalClient.Code = client.Code;
                originalClient.ABN = client.ABN;
                originalClient.Name = client.Name;
                originalClient.ChangeDate = client.ChangeDate;
                originalClient.CreateDate = client.CreateDate;
                originalClient.AppliedGST = client.AppliedGST;
                originalClient.AutoApplyGST = client.AutoApplyGST;
                originalClient.Addresses = client.Addresses != null ?
                     client.Addresses.Select(add =>
                         add.ToDomain(
                            originalClient.Addresses != null ? originalClient.Addresses.FirstOrDefault(ad => ad.ID == add.ID) : null
                         )).ToList()
                         : new List<Address>();
                originalClient.Contacts = client.Contacts != null ?
                   client.Contacts.Select(con =>
                       con.ToDomain(
                          originalClient.Contacts != null ? originalClient.Contacts.FirstOrDefault(co => co.ID == con.ID) : null
                       )).ToList()
                       : new List<Contact>();
                return originalClient;
            }

            return new Client()
            {
                ID = client.ID,
                Code = client.Code,
                ABN = client.ABN,
                Name = client.Name,
                ChangeDate = client.ChangeDate,
                CreateDate = client.CreateDate,
                AppliedGST = client.AppliedGST,
                AutoApplyGST = client.AutoApplyGST,
                Addresses = client.Addresses != null ? client.Addresses.Select(add => add.ToDomain()).ToList() : null,
                Contacts = client.Contacts != null ? client.Contacts.Select(con => con.ToDomain()).ToList() : null
            };

        }
    }
}