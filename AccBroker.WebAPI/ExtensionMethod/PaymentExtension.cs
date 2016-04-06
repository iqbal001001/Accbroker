using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Reflection;
using System.Web;
using AccBroker.Domain;

namespace AccBroker.WebAPI
{
    public static class PaymentExtension
    {
        public static PaymentDTO ToDTO(this Payment payment)
        {
            return new PaymentDTO
            {
                ID = payment.ID,
                PaymentNo = payment.PaymentNo,
                Description = payment.Description,
                GST = payment.GST,
                Amount = payment.Amount,
                Total = payment.Amount + payment.GST,
                PaymentType = payment.PaymentType,
                ClientID = payment.ClientID,
                ClientName = payment.Client != null ? payment.Client.Name : "",
                CompanyID = payment.CompanyID,
                CompanyName = payment.Company != null ? payment.Company.Name : "",
                Concurrency = payment.Concurrency,
                PaymentDate = payment.PaymentDate,
                CreateDate = payment.CreateDate,
                ChangeDate = payment.ChangeDate,
                PaymentItems = payment.PaymentItems != null ? payment.PaymentItems.Select(add => add.ToDTO()).ToList() : new List<PaymentItemDTO>()
            };
        }

        public static object ToDataShapeObject(this PaymentDTO payment, List<string> lstOfFields)
        {
            if (!lstOfFields.Any())
            {
                return payment;
            }
            else
            {
                ExpandoObject objectToReturn = new ExpandoObject();
                foreach (var field in lstOfFields)
                {
                    var fieldValue = payment.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(payment, null);

                    ((IDictionary<string, object>)objectToReturn).Add(field, fieldValue);

                }
                return objectToReturn;
            }

        }

       

        public static Payment ToDomain(this PaymentDTO payment, Payment originalPayment = null)
        {
            if (originalPayment != null && originalPayment.ID == payment.ID)
            {
                originalPayment.PaymentNo = payment.PaymentNo;
                originalPayment.Description = payment.Description;
                originalPayment.GST = payment.GST;
                originalPayment.Amount = payment.Amount;
                originalPayment.PaymentType = payment.PaymentType;
                originalPayment.ClientID = payment.ClientID;
                originalPayment.CompanyID = payment.CompanyID;
                originalPayment.CreateDate = payment.CreateDate;
                originalPayment.ChangeDate = payment.ChangeDate;
                originalPayment.PaymentDate = payment.PaymentDate;
                originalPayment.PaymentItems = payment.PaymentItems != null ?
    payment.PaymentItems.Select(add => add.ToDomain(originalPayment.PaymentItems.FirstOrDefault(oi => oi.ID == add.ID))).ToList() : new List<PaymentItem>();

                return originalPayment;
            }

            return new Payment()
            {
                ID = payment.ID,
                PaymentNo = payment.PaymentNo,
                Description = payment.Description,
                GST = payment.GST,
                Amount = payment.Amount,
                PaymentType = payment.PaymentType,
                ClientID = payment.ClientID,
                CompanyID = payment.CompanyID,
                Concurrency = payment.Concurrency,
                CreateDate = payment.CreateDate,
                ChangeDate = payment.ChangeDate,
                PaymentDate = payment.PaymentDate,
                PaymentItems = payment.PaymentItems.Select(ii => ii.ToDomain()).ToList()
            };

        }
    }
}