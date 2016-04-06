using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccBroker.WebAPI
{
    public class AddressDTO
    {
        public int ID { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }

        public int? ClientID { get; set; }

        public int? CompanyID { get; set; }

        public Guid? Concurrency { get; set; }

        public int? AddressType { get; set; }
        public byte[] CreateDate { get; set; }

        public DateTime? ChangeDate { get; set; }
    }
}