using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccBroker.WebAPI
{
    public class PaymentItemDTO
    {
        public int ID { get; set; }

        public int? SequenceNo { get; set; }

        public int PaymentID { get; set; }

        public string Description { get; set; }

        public decimal? GST { get; set; }

        public decimal? Amount { get; set; }

        public int? InvoiceID { get; set; }

        public string InvoiceNo { get; set; }

        public Guid? Concurrency { get; set; }

        public byte[] CreateDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        //public virtual Invoice Invoice { get; set; }

        //public virtual PaymentDTO Payment { get; set; }

        public string PaymentNo { get; set; }

        public int? PaymentType { get; set; }

        public DateTime? PaymentDate { get; set; }
    }
}