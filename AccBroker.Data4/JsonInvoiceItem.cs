namespace AccBroker.Data4
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("JsonInvoiceItem")]
    public partial class JsonInvoiceItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InvoiceItemID { get; set; }

        public int InvoiceID { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string JsonString { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual InvoiceItem InvoiceItem { get; set; }
    }
}
