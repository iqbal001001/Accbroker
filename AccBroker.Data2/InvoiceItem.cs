namespace AccBroker.Data2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvoiceItem")]
    public partial class InvoiceItem
    {
        public int ID { get; set; }

        public int? ProductID { get; set; }

        public decimal? GST { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(10)]
        public string Description { get; set; }

        public int? InvoiceID { get; set; }

        public Guid? Concurrency { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] CreateDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual Product Product { get; set; }
    }
}
