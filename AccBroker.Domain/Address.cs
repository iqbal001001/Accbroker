namespace AccBroker.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("Address")]
    [Serializable]
    public partial class Address
    {
        public int ID { get; set; }

        [StringLength(10)]
        public string AddressLine1 { get; set; }

        [StringLength(10)]
        public string AddressLine2 { get; set; }

        [StringLength(10)]
        public string Suburb { get; set; }

        [StringLength(10)]
        public string State { get; set; }

        [StringLength(10)]
        public string PostCode { get; set; }

        public int? ClientID { get; set; }

        public int? CompanyID { get; set; }

        public Guid? Concurrency { get; set; }

        public int? AddressType { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] CreateDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        public virtual Client Client { get; set; }

        public virtual Company Company { get; set; }
    }
}
