using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Reflection;
using System.Web;
using AccBroker.Domain;

namespace AccBroker.WebAPI
{
    public static class JsonInvoiceItemExtension
    {
        public static JsonInvoiceItemDTO ToDTO(this JsonInvoiceItem jsonInvoiceItem)
        {
            return new JsonInvoiceItemDTO
            {
                ID = jsonInvoiceItem.ID,
                InvoiceItemID = jsonInvoiceItem.InvoiceItemID,
                JsonString = jsonInvoiceItem.JsonString
            };
        }

        public static object ToDataShapeObject(this JsonInvoiceItemDTO invoiceItem, List<string> lstOfFields)
        {
            if (!lstOfFields.Any())
            {
                return invoiceItem;
            }
            else
            {
                ExpandoObject objectToReturn = new ExpandoObject();
                foreach (var field in lstOfFields)
                {
                    var fieldValue = invoiceItem.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(invoiceItem, null);

                    ((IDictionary<string, object>)objectToReturn).Add(field, fieldValue);

                }
                return objectToReturn;
            }

        }



        public static JsonInvoiceItem ToDomain(this JsonInvoiceItemDTO jsonInvoiceItem, JsonInvoiceItem originalJsonInvoiceItem = null)
        {
            if (originalJsonInvoiceItem != null && originalJsonInvoiceItem.ID == jsonInvoiceItem.ID)
            {

                originalJsonInvoiceItem.InvoiceItemID = jsonInvoiceItem.InvoiceItemID;
                originalJsonInvoiceItem.JsonString = jsonInvoiceItem.JsonString;

                return originalJsonInvoiceItem;
            }

            return new JsonInvoiceItem()
            {
                ID = jsonInvoiceItem.ID,
                InvoiceItemID = jsonInvoiceItem.InvoiceItemID,
                JsonString = jsonInvoiceItem.JsonString
            };

        }
    }
}