using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Reflection;
using System.Web;
using AccBroker.Domain;

namespace AccBroker.WebAPI
{
    public static class PaymentItemExtension
    {
        public static PaymentItemDTO ToDTO(this PaymentItem paymentItem)
        {
            return new PaymentItemDTO
            {
                ID = paymentItem.ID,
                SequenceNo = paymentItem.SequenceNo,
                PaymentID = paymentItem.PaymentID,
                Description = paymentItem.Description,
                GST = paymentItem.GST,
                Amount = paymentItem.Amount,
                InvoiceID = paymentItem.InvoiceID,
                InvoiceNo = paymentItem.InvoiceNo,
                Concurrency = paymentItem.Concurrency,
                CreateDate = paymentItem.CreateDate,
                ChangeDate = paymentItem.ChangeDate,
                PaymentNo = paymentItem.Payment != null ? paymentItem.Payment.PaymentNo : null,
                PaymentType = paymentItem.Payment != null ? paymentItem.Payment.PaymentType : null,
                PaymentDate = paymentItem.Payment != null ? paymentItem.Payment.PaymentDate : null,

            };
        }

        public static object ToDataShapeObject(this PaymentItemDTO paymentItem, List<string> lstOfFields)
        {
            if (!lstOfFields.Any())
            {
                return paymentItem;
            }
            else
            {
                ExpandoObject objectToReturn = new ExpandoObject();
                foreach (var field in lstOfFields)
                {
                    var fieldValue = paymentItem.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(paymentItem, null);

                    ((IDictionary<string, object>)objectToReturn).Add(field, fieldValue);

                }
                return objectToReturn;
            }

        }



        public static PaymentItem ToDomain(this PaymentItemDTO paymentItem, PaymentItem originalPaymentItem = null)
        {
            if (originalPaymentItem != null && originalPaymentItem.ID == paymentItem.ID)
            {
                originalPaymentItem.SequenceNo = paymentItem.SequenceNo;
                originalPaymentItem.PaymentID = paymentItem.PaymentID;
                originalPaymentItem.Description = paymentItem.Description;
                originalPaymentItem.GST = paymentItem.GST;
                originalPaymentItem.Amount = paymentItem.Amount;
                originalPaymentItem.InvoiceID = paymentItem.InvoiceID;
                originalPaymentItem.InvoiceNo = paymentItem.InvoiceNo;
                originalPaymentItem.CreateDate = paymentItem.CreateDate;
                originalPaymentItem.ChangeDate = paymentItem.ChangeDate;

                return originalPaymentItem;
            }

            return new PaymentItem()
            {
                ID = paymentItem.ID,
                SequenceNo = paymentItem.SequenceNo,
                PaymentID = paymentItem.PaymentID,
                Description = paymentItem.Description,
                GST = paymentItem.GST,
                Amount = paymentItem.Amount,
                InvoiceID = paymentItem.InvoiceID,
                InvoiceNo = paymentItem.InvoiceNo,
                Concurrency = paymentItem.Concurrency,
                CreateDate = paymentItem.CreateDate,
                ChangeDate = paymentItem.ChangeDate
            };

        }
    }
}