using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccBroker.WebAPI
{
    public class CompanyDTO
    {
        public CompanyDTO()
        {
            //Addresses = new HashSet<Address>();
            //Contacts = new HashSet<Contact>();
            //Invoices = new HashSet<Invoice>();
            //Payments = new HashSet<Payment>();
        }

        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ABN { get; set; }

        public Guid? Concurrency { get; set; }
        public byte[] CreateDate { get; set; }
        public DateTime? ChangeDate { get; set; }

        public ICollection<AddressDTO> Addresses { get; set; }

        //[Serializable]
        public virtual ICollection<ContactDTO> Contacts { get; set; }

        //[Serializable]
        //public virtual ICollection<Invoice> Invoices { get; set; }

        //[Serializable]
        //public virtual ICollection<Payment> Payments { get; set; }
    }
}