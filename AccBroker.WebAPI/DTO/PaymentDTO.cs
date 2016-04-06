using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccBroker.WebAPI
{
    public class PaymentDTO
    {
        public int ID { get; set; }

        public string PaymentNo { get; set; }

        public string Description { get; set; }

        public decimal? GST { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Total { get; set; }

        public int? PaymentType { get; set; }

        public Guid? Concurrency { get; set; }

        public DateTime? PaymentDate { get; set; }

        public byte[] CreateDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        public int? CompanyID { get; set; }

        public string CompanyName { get; set; }

        public int? ClientID { get; set; }

        public string ClientName { get; set; }

        //public virtual Client Client { get; set; }

        //public virtual Company Company { get; set; }

        public virtual ICollection<PaymentItemDTO> PaymentItems { get; set; }
    }
}