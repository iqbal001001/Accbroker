﻿namespace AccBroker.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ProductInvoiceItem")]
    public partial class ProductInvoiceItem
    {
        public int ID { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InvoiceItemID { get; set; }

        public int InvoiceID { get; set; }

        public int ProductID { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(6)]
        public string ProductCode { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual InvoiceItem InvoiceItem { get; set; }

        public virtual Product Product { get; set; }
    }
}
