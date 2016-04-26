using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Reflection;
using System.Web;
using AccBroker.Domain;


namespace AccBroker.WebAPI
{
    public static class InvoiceTotalExtension
    {
        public static InvoiceTotalDTO ToTotalDTO(this Invoice invoice)
        {
            return new InvoiceTotalDTO
            {
                ID = invoice.ID,
                InvoiceNo = invoice.InvoiceNo != null ? invoice.InvoiceNo.Trim() : null,
                InvoiceDescription = invoice.InvoiceDescription != null ? invoice.InvoiceDescription.Trim() : null,
                BillingAddress = invoice.BillingAddress,
                PaymentAmount = invoice.PaymentItems.Sum(p => p.Amount),
                Total = invoice.Amount + invoice.GST,
                Amount = invoice.Amount,
                GST = invoice.GST,
                Paid = invoice.PaymentItems.Sum(p => p.Amount) == (invoice.Amount + invoice.GST) ? true : false,
                InvoiceType = invoice.InvoiceType,
                InvoiceDate = invoice.InvoiceDate,
                DueDate = invoice.DueDate,
                DebtorLinkInvoiceID = invoice.DebtorLinkInvoiceID,
                CreditorLinkInvoiceID = invoice.CreditorLinkInvoiceID,
                ClientID = invoice.ClientID,
                Client = invoice.Client != null? invoice.Client.Name : null,
                CompanyID = invoice.CompanyID,
                Company = invoice.Company != null ? invoice.Company.Name : null,
                Concurrency = invoice.Concurrency,
                CreateDate = invoice.CreateDate,
                ChangeDate = invoice.ChangeDate,
                InvoiceItems = invoice.InvoiceItems != null ? invoice.InvoiceItems.Select(add => add.ToDTO()).ToList() : new List<InvoiceItemDTO>(),
                PaymentItems = invoice.PaymentItems != null ? invoice.PaymentItems.Select(pay => pay.ToDTO()).ToList() : new List<PaymentItemDTO>()
            };
        }

       



     
    }
}