namespace AccBroker.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("Payment")]
    public partial class Payment
    {
        public Payment()
        {
            PaymentItems = new HashSet<PaymentItem>();
        }

        public int ID { get; set; }

        [StringLength(10)]
        public string PaymentNo { get; set; }

        [StringLength(10)]
        public string Description { get; set; }

        public decimal? GST { get; set; }

        public decimal? Amount { get; set; }

        public int? PaymentType { get; set; }

        public Guid? Concurrency { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PaymentDate { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] CreateDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        public int? CompanyID { get; set; }

        public int? ClientID { get; set; }

        public virtual Client Client { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<PaymentItem> PaymentItems { get; set; }
    }
}
