namespace AccBroker.Data3
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Company")]
    public partial class Company
    {
        public Company()
        {
            Addresses = new HashSet<Address>();
            Contacts = new HashSet<Contact>();
            Invoices = new HashSet<Invoice>();
            Payments = new HashSet<Payment>();
        }

        public int ID { get; set; }

        [StringLength(10)]
        public string Name { get; set; }

        [StringLength(10)]
        public string ABN { get; set; }

        public Guid? Concurrency { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] CreateDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
