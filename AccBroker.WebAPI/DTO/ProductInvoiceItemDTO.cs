using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccBroker.WebAPI
{
    public class ProductInvoiceItemDTO
    {
        public int ID { get; set; }

        public int InvoiceItemID { get; set; }

        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public virtual InvoiceItemDTO InvoiceItem { get; set; }

        public virtual ProductDTO Product { get; set; }
    }
}