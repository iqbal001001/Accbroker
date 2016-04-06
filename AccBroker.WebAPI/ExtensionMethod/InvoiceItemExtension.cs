using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Reflection;
using System.Web;
using AccBroker.Domain;

namespace AccBroker.WebAPI
{
    public static class InvoiceItemExtension
    {
        public static InvoiceItemDTO ToDTO(this InvoiceItem invoiceItem)
        {
            return new InvoiceItemDTO
            {
                ID = invoiceItem.ID,
                SequenceNo = invoiceItem.SequenceNo,
                Discount = 0,
                GST = invoiceItem.GST,
                Amount = invoiceItem.Amount,
                Total = invoiceItem.Amount + invoiceItem.GST,
                Description = invoiceItem.Description,
                InvoiceType = invoiceItem.InvocieType,
                InvoiceID = invoiceItem.InvoiceID,
                Concurrency = invoiceItem.Concurrency,
                CreateDate = invoiceItem.CreateDate,
                ChangeDate = invoiceItem.ChangeDate,
                JsonInvoiceItem = invoiceItem.JsonInvoiceItem != null ? invoiceItem.JsonInvoiceItem.ToDTO() : null,
                ProductInvoiceItem = invoiceItem.ProductInvoiceItem != null ? invoiceItem.ProductInvoiceItem.ToDTO() : null
            };
        }

        public static object ToDataShapeObject(this InvoiceItemDTO invoiceItem, List<string> lstOfFields)
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

    

        public static InvoiceItem ToDomain(this InvoiceItemDTO invoiceItem, InvoiceItem originalInvoiceItem = null)
        {
            if (originalInvoiceItem != null && originalInvoiceItem.ID == invoiceItem.ID)
            {
                originalInvoiceItem.SequenceNo = invoiceItem.SequenceNo;
                originalInvoiceItem.Discount = invoiceItem.Discount;
                originalInvoiceItem.GST = invoiceItem.GST;
                originalInvoiceItem.Amount = invoiceItem.Amount;
                originalInvoiceItem.Description = invoiceItem.Description;
                originalInvoiceItem.InvoiceID = invoiceItem.InvoiceID;
                originalInvoiceItem.InvocieType = invoiceItem.InvoiceType;
                originalInvoiceItem.CreateDate = invoiceItem.CreateDate;
                originalInvoiceItem.ChangeDate = invoiceItem.ChangeDate;
                originalInvoiceItem.JsonInvoiceItem =
                    invoiceItem.JsonInvoiceItem != null ? invoiceItem.JsonInvoiceItem.ToDomain(originalInvoiceItem.JsonInvoiceItem) : null;
                originalInvoiceItem.ProductInvoiceItem =
                    invoiceItem.ProductInvoiceItem != null ? invoiceItem.ProductInvoiceItem.ToDomain(originalInvoiceItem.ProductInvoiceItem) : null;

                return originalInvoiceItem;
            }

            return new InvoiceItem()
            {
                ID = invoiceItem.ID,
                SequenceNo = invoiceItem.SequenceNo,
                Discount = invoiceItem.Discount,
                GST = invoiceItem.GST,
                Amount = invoiceItem.Amount,
                Description = invoiceItem.Description,
                InvoiceID = invoiceItem.InvoiceID,
                InvocieType = invoiceItem.InvoiceType,
                Concurrency = invoiceItem.Concurrency,
                CreateDate = invoiceItem.CreateDate,
                ChangeDate = invoiceItem.ChangeDate,
                JsonInvoiceItem = invoiceItem.JsonInvoiceItem != null ? invoiceItem.JsonInvoiceItem.ToDomain() : null,
                ProductInvoiceItem = invoiceItem.ProductInvoiceItem != null ? invoiceItem.ProductInvoiceItem.ToDomain() : null
            };

        }
    }
}