using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccBroker.WebAPI
{
    public class InvoiceItemDTO
    {
        public int ID { get; set; }

        public int? SequenceNo { get; set; }

        public int? ProductID { get; set; }

        public decimal? GST { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Total { get; set; }

        public string Description { get; set; }

        public int InvoiceID { get; set; }

        public Guid? Concurrency { get; set; }

        public byte[] CreateDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        public decimal? Discount { get; set; }

        public int? InvoiceType { get; set; }

        //public virtual Invoice Invoice { get; set; }

        //public virtual Product Product { get; set; }

        public virtual JsonInvoiceItemDTO JsonInvoiceItem { get; set; }

        public virtual ProductInvoiceItemDTO ProductInvoiceItem { get; set; }
    }
}