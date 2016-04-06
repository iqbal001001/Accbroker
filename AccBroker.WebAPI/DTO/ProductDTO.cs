using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccBroker.WebAPI
{
    public class ProductDTO
    {
        public int ID { get; set; }

        public Guid? Concurrency { get; set; }

        public byte[] CreateDate { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal? CostPrice { get; set; }

        public decimal SellPrice { get; set; }

        public DateTime? ChangeDate { get; set; }

        public virtual ICollection<ProductInvoiceItemDTO> ProductInvoiceItems { get; set; }
    }
}