using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccBroker.WebAPI
{
    public class JsonInvoiceItemDTO
    {
        public int ID { get; set; }

        public int InvoiceItemID { get; set; }

        public string JsonString { get; set; }

        public virtual InvoiceItemDTO InvoiceItem { get; set; }
    }
}