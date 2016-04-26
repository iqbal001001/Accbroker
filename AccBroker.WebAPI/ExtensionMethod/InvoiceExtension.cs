using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Reflection;
using System.Web;
using AccBroker.Domain;

namespace AccBroker.WebAPI
{
    public static class InvoiceExtension
    {
        public static InvoiceDTO ToDTO(this Invoice invoice)
        {
            return new InvoiceDTO
            {
                ID = invoice.ID,
                InvoiceNo = invoice.InvoiceNo != null ? invoice.InvoiceNo.Trim() : null,
                InvoiceDescription = invoice.InvoiceDescription != null ? invoice.InvoiceDescription.Trim() : null,
                BillingAddress = invoice.BillingAddress,
                Total = invoice.Amount +invoice.GST,
                Amount = invoice.Amount,
                GST = invoice.GST,
                InvoiceType = invoice.InvoiceType,
                InvoiceDate = invoice.InvoiceDate,
                DueDate = invoice.DueDate,
                DebtorLinkInvoiceID = invoice.DebtorLinkInvoiceID,
                CreditorLinkInvoiceID = invoice.CreditorLinkInvoiceID,
                ClientID = invoice.ClientID,
                ClientName = invoice.Client != null ? invoice.Client.Name : null,
                CompanyID = invoice.CompanyID,
                CompanyName = invoice.Company != null ? invoice.Company.Name : null,
                Concurrency = invoice.Concurrency,
                CreateDate = invoice.CreateDate,
                ChangeDate = invoice.ChangeDate,
                InvoiceItems = invoice.InvoiceItems != null ? invoice.InvoiceItems.Select(add => add.ToDTO()).ToList() : new List<InvoiceItemDTO>(),
                PaymentItems = invoice.PaymentItems != null ? invoice.PaymentItems.Select(pay => pay.ToDTO()).ToList() : new List<PaymentItemDTO>()
            };
        }

        public static object ToDataShapeObject(this InvoiceDTO invoice, List<string> lstOfFields)
        {
            if (!lstOfFields.Any())
            {
                return invoice;
            }
            else
            {
                ExpandoObject objectToReturn = new ExpandoObject();
                foreach (var field in lstOfFields)
                {
                    var fieldValue = invoice.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(invoice, null);

                    ((IDictionary<string, object>)objectToReturn).Add(field, fieldValue);

                }
                return objectToReturn;
            }

        }

       

        public static Invoice ToDomain(this InvoiceDTO invoice, Invoice originalinvoice = null)
        {
            if (originalinvoice != null && originalinvoice.ID == invoice.ID)
            {
                originalinvoice.ID = invoice.ID;
                originalinvoice.InvoiceNo = invoice.InvoiceNo;
                originalinvoice.InvoiceDescription = invoice.InvoiceDescription;
                originalinvoice.BillingAddress = invoice.BillingAddress;
                originalinvoice.Amount = invoice.Amount;
                originalinvoice.GST = invoice.GST;
                originalinvoice.InvoiceType = invoice.InvoiceType;
                originalinvoice.InvoiceDate = invoice.InvoiceDate;
                originalinvoice.DueDate = invoice.DueDate;
                originalinvoice.DebtorLinkInvoiceID = invoice.DebtorLinkInvoiceID;
                originalinvoice.CreditorLinkInvoiceID = invoice.CreditorLinkInvoiceID;
                originalinvoice.ClientID = invoice.ClientID;
                originalinvoice.CompanyID = invoice.CompanyID;
                originalinvoice.CreateDate = invoice.CreateDate;
                originalinvoice.ChangeDate = invoice.ChangeDate;
                originalinvoice.InvoiceItems = invoice.InvoiceItems != null ?
                    invoice.InvoiceItems.Select(add => add.ToDomain(
                        originalinvoice.InvoiceItems != null ? originalinvoice.InvoiceItems.FirstOrDefault(oi => oi.ID == add.ID) : null
                        )).ToList() : new List<InvoiceItem>();
                    //invoice.InvoiceItems.Select(i => i.ToDomain(new InvoiceItem()));

                return originalinvoice;
            }

            return new Invoice()
            {
                ID = invoice.ID,
                InvoiceNo = invoice.InvoiceNo,
                InvoiceDescription = invoice.InvoiceDescription,
                BillingAddress = invoice.BillingAddress,
                Amount = invoice.Amount,
                GST = invoice.GST,
                InvoiceType = invoice.InvoiceType,
                InvoiceDate = invoice.InvoiceDate,
                DueDate = invoice.DueDate,
                DebtorLinkInvoiceID = invoice.DebtorLinkInvoiceID,
                CreditorLinkInvoiceID = invoice.CreditorLinkInvoiceID,
                ClientID = invoice.ClientID,
                CompanyID = invoice.CompanyID,
                Concurrency = invoice.Concurrency,
                CreateDate = invoice.CreateDate,
                ChangeDate = invoice.ChangeDate,
                InvoiceItems = invoice.InvoiceItems.Select(ii=>ii.ToDomain()).ToList()
            };

        }
    }
}