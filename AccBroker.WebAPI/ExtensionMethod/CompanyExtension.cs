using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Reflection;
using AccBroker.Domain;

namespace AccBroker.WebAPI
{
    public static class CompanyExtension
    {
        public static CompanyDTO ToDTO(this Company company)
        {
            return new CompanyDTO() {
                 ID = company.ID,
                 Code = company.Code != null ? company.Code.Trim() : null,
                 ABN = company.ABN != null ? company.ABN.Trim() : null,
                 Name = company.Name != null ? company.Name.Trim() : null,
                 ChangeDate = company.ChangeDate,
                 CreateDate = company.CreateDate,
                 Concurrency = company.Concurrency,
                 Addresses = company.Addresses != null ?  company.Addresses.Select(add => add.ToDTO()).ToList() : new List<AddressDTO>(),
                 Contacts = company.Contacts != null ? company.Contacts.Select(con => con.ToDTO()).ToList() : new List<ContactDTO>(),
            };
        }

        public static object ToDataShapeObjectx(this CompanyDTO company, List<string> lstOfFields)
        {
            if (!lstOfFields.Any())
            {
                return company;
            }
            else
            {
                ExpandoObject objectToReturn = new ExpandoObject();
                foreach (var field in lstOfFields)
                {
                    var fieldValue = company.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(company, null);

                    ((IDictionary<string, object>)objectToReturn).Add(field, fieldValue);

                }
                return objectToReturn;
            }
            
        }

        public static object ToDataShapeObject(this CompanyDTO company, List<string> lstOfFields)
        {
            List<string> lstOfFieldsToWorkWith = new List<string>(lstOfFields);

            if (!lstOfFieldsToWorkWith.Any())
            {
                return company;
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
                    var fieldValue = company.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(company, null);

                    ((IDictionary<string, object>)objectToReturn).Add(field, fieldValue);

                }

                if (returnPartialAddress)
                {
                    List<object> addresses = new List<object>();
                    foreach (var address in company.Addresses)
                    {
                        addresses.Add(address.ToDataShapeObject(lstOfAddressFields));
                    }

                    ((IDictionary<string, object>)objectToReturn).Add("addresses", addresses);

                }
                return objectToReturn;
            }
        }

        public static Company ToDomain(this CompanyDTO company, Company originalCompany = null)
        {
            if (originalCompany != null && originalCompany.ID == company.ID)
            {
                originalCompany.Code = company.Code;
                originalCompany.ABN = company.ABN;
                originalCompany.Name = company.Name;
                originalCompany.ChangeDate = company.ChangeDate;
                originalCompany.CreateDate = company.CreateDate;
                 originalCompany.Addresses = company.Addresses != null ? 
                     company.Addresses.Select(add => 
                         add.ToDomain(
                            originalCompany.Addresses != null ? originalCompany.Addresses.FirstOrDefault(ad=>ad.ID == add.ID)  :  null
                         )).ToList() 
                         : new List<Address>();
                 originalCompany.Contacts = company.Contacts != null ?
                    company.Contacts.Select(con =>
                        con.ToDomain(
                           originalCompany.Contacts != null ? originalCompany.Contacts.FirstOrDefault(co => co.ID == con.ID) : null
                        )).ToList()
                        : new List<Contact>();
                return originalCompany;
            }

            return new Company()
            {
                ID = company.ID,
                Code = company.Code,
                ABN = company.ABN,
                Name = company.Name,
                ChangeDate = company.ChangeDate,
                CreateDate = company.CreateDate,
                Addresses = company.Addresses != null ? company.Addresses.Select(add=>add.ToDomain()).ToList() : null,
                Contacts = company.Contacts != null ? company.Contacts.Select(con=>con.ToDomain()).ToList() : null
            };

        }
    }
}