namespace AccBroker.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("Contact")]
    public partial class Contact
    {
        public int ID { get; set; }

        [StringLength(10)]
        public string ContactType { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        [StringLength(20)]
        public string Position { get; set; }

        [StringLength(10)]
        public string HomePhone { get; set; }

        [StringLength(10)]
        public string Mobile { get; set; }

        [StringLength(10)]
        public string WorkPhone { get; set; }

        public Guid? Concurrency { get; set; }

        public int? ClientID { get; set; }

        public int? CompanyID { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] CreateDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        [StringLength(20)]
        public string CreateUser { get; set; }

        [StringLength(20)]
        public string ChangeUser { get; set; }

        public virtual Client Client { get; set; }

        public virtual Company Company { get; set; }
    }
}
