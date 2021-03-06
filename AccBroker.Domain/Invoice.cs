namespace AccBroker.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("Invoice")]
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceItems = new HashSet<InvoiceItem>();
            JsonInvoiceItems = new HashSet<JsonInvoiceItem>();
            PaymentItems = new HashSet<PaymentItem>();
        }

        public int ID { get; set; }

        [StringLength(10)]
        public string InvoiceNo { get; set; }

        [StringLength(100)]
        public string InvoiceDescription { get; set; }

        public int? BillingAddress { get; set; }

        public decimal? Amount { get; set; }

        public int? CompanyID { get; set; }

        public int? ClientID { get; set; }

        public decimal? GST { get; set; }

        public Guid? Concurrency { get; set; }

        public int? InvoiceType { get; set; }

        public int? DebtorLinkInvoiceID { get; set; }

        public int? CreditorLinkInvoiceID { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] CreateDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        [StringLength(20)]
        public string CreateUser { get; set; }

        [StringLength(20)]
        public string ChangeUser { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InvoiceDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DueDate { get; set; }

        public virtual Client Client { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }

        public virtual ICollection<JsonInvoiceItem> JsonInvoiceItems { get; set; }

        public virtual ICollection<ProductInvoiceItem> ProductInvoiceItems { get; set; }

        public virtual ICollection<PaymentItem> PaymentItems { get; set; }
    }
}
