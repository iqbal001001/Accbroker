using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Reflection;
using System.Web;
using AccBroker.Domain;

namespace AccBroker.WebAPI
{
    public static class ProductInvoiceItemExtension
    {
        public static ProductInvoiceItemDTO ToDTO(this ProductInvoiceItem productInvoiceItem)
        {
            return new ProductInvoiceItemDTO
            {
                ID = productInvoiceItem.ID,
                InvoiceItemID = productInvoiceItem.InvoiceItemID,
                ProductID = productInvoiceItem.ProductID,
                ProductCode = productInvoiceItem.ProductCode,
                ProductName = productInvoiceItem.ProductName,
                Quantity = productInvoiceItem.Quantity,
                UnitPrice = productInvoiceItem.UnitPrice,
               
                 
            };
        }

        public static object ToDataShapeObject(this ProductInvoiceItemDTO invoiceItem, List<string> lstOfFields)
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



        public static ProductInvoiceItem ToDomain(this ProductInvoiceItemDTO productInvoiceItem, ProductInvoiceItem originalProductInvoiceItem = null)
        {
            if (originalProductInvoiceItem != null && originalProductInvoiceItem.ID == productInvoiceItem.ID)
            {

                originalProductInvoiceItem.InvoiceItemID = productInvoiceItem.InvoiceItemID;
                originalProductInvoiceItem.ProductID = productInvoiceItem.ProductID;
                originalProductInvoiceItem.ProductCode = productInvoiceItem.ProductCode;
                originalProductInvoiceItem.ProductName = productInvoiceItem.ProductName;
                originalProductInvoiceItem.Quantity = productInvoiceItem.Quantity;
                originalProductInvoiceItem.UnitPrice = productInvoiceItem.UnitPrice;

                return originalProductInvoiceItem;
            }

            return new ProductInvoiceItem()
            {
                ID = productInvoiceItem.ID,
                InvoiceItemID = productInvoiceItem.InvoiceItemID,
                ProductID = productInvoiceItem.ProductID,
                ProductCode = productInvoiceItem.ProductCode,
                ProductName = productInvoiceItem.ProductName,
                Quantity = productInvoiceItem.Quantity,
                UnitPrice = productInvoiceItem.UnitPrice
            };

        }
    }
}