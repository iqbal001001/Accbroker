using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccBroker.WebAPI
{
    public class ClientDTO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ABN { get; set; }
        public Guid? Concurrency { get; set; }
        public int? CompanyID { get; set; }
        public int? LinkCompanyID { get; set; }
        public byte[] CreateDate { get; set; }
        public DateTime? ChangeDate { get; set; }
        public bool? AutoApplyGST { get; set; }

        public decimal? AppliedGST { get; set; }

        public virtual ICollection<AddressDTO> Addresses { get; set; }

        public virtual ICollection<ContactDTO> Contacts { get; set; }

        public virtual ICollection<InvoiceDTO> Invoices { get; set; }

        public virtual ICollection<PaymentDTO> Payments { get; set; }
    }
}