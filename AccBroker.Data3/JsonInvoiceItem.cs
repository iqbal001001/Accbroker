namespace AccBroker.Data3
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

        [Column(TypeName = "ntext")]
        [Required]
        public string JsonString { get; set; }

        public virtual InvoiceItem InvoiceItem { get; set; }
    }
}
