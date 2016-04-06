namespace AccBroker.Data2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            InvoiceItems = new HashSet<InvoiceItem>();
        }

        public int ID { get; set; }

        public Guid? Concurrency { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] CreateDate { get; set; }

        [StringLength(10)]
        public string ProductName { get; set; }

        [StringLength(10)]
        public string ProductDescription { get; set; }

        public decimal? CostPrice { get; set; }

        public decimal? SellPrice { get; set; }

        public DateTime? ChangeDate { get; set; }

        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
