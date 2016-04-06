using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccBroker.WebAPI
{
    public class ContactDTO
    {
        public int ID { get; set; }
        public string ContactType { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string HomePhone { get; set; }
        public string Mobile { get; set; }
        public string WorkPhone { get; set; }
        public Guid? Concurrency { get; set; }
        public int? ClientID { get; set; }
        public int? CompanyID { get; set; }
        public byte[] CreateDate { get; set; }
        public DateTime? ChangeDate { get; set; }
    }
}