namespace AccBroker.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("InvoiceItem")]
    public partial class InvoiceItem
    {
        public int ID { get; set; }

        public int? SequenceNo { get; set; }

        public decimal? GST { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public int InvoiceID { get; set; }

        public Guid? Concurrency { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] CreateDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        [StringLength(20)]
        public string CreateUser { get; set; }

        [StringLength(20)]
        public string ChangeUser { get; set; }

        public decimal? Discount { get; set; }

        public int? InvocieType { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual JsonInvoiceItem JsonInvoiceItem { get; set; }

        public virtual ProductInvoiceItem ProductInvoiceItem { get; set; }
    }
}
