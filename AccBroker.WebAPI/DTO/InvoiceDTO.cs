using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccBroker.WebAPI
{
    public class InvoiceDTO
    {
        public int ID { get; set; }
        
        public string InvoiceNo { get; set; }
        
        public string InvoiceDescription { get; set; }
        
        public int? BillingAddress { get; set; }
        
        public decimal? Amount { get; set; }

        public decimal? Total { get; set; }

        public int? CompanyID { get; set; }
        public string CompanyName { get; set; }

        public int? ClientID { get; set; }
        public string ClientName { get; set; }

        public decimal? GST { get; set; }

        public Guid? Concurrency { get; set; }

        public int? InvoiceType { get; set; }

        public int? DebtorLinkInvoiceID { get; set; }

        public int? CreditorLinkInvoiceID { get; set; }

        public byte[] CreateDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? DueDate { get; set; }

        //public virtual Client Client { get; set; }

        //public virtual Company Company { get; set; }

        public virtual ICollection<InvoiceItemDTO> InvoiceItems { get; set; }

        public virtual ICollection<PaymentItemDTO> PaymentItems { get; set; }
    }
}