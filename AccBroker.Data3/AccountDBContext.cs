namespace AccBroker.Data3
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AccountDBContext : DbContext
    {
        public AccountDBContext()
            : base("name=AccountDBContext")
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }
        public virtual DbSet<JsonInvoiceItem> JsonInvoiceItems { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentItem> PaymentItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductInvoiceItem> ProductInvoiceItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .Property(e => e.AddressLine1)
                .IsFixedLength();

            modelBuilder.Entity<Address>()
                .Property(e => e.AddressLine2)
                .IsFixedLength();

            modelBuilder.Entity<Address>()
                .Property(e => e.Suburb)
                .IsFixedLength();

            modelBuilder.Entity<Address>()
                .Property(e => e.State)
                .IsFixedLength();

            modelBuilder.Entity<Address>()
                .Property(e => e.PostCode)
                .IsFixedLength();

            modelBuilder.Entity<Address>()
                .Property(e => e.CreateDate)
                .IsFixedLength();

            modelBuilder.Entity<Client>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Client>()
                .Property(e => e.ABN)
                .IsFixedLength();

            modelBuilder.Entity<Client>()
                .Property(e => e.CreateDate)
                .IsFixedLength();

            modelBuilder.Entity<Company>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Company>()
                .Property(e => e.ABN)
                .IsFixedLength();

            modelBuilder.Entity<Company>()
                .Property(e => e.CreateDate)
                .IsFixedLength();

            modelBuilder.Entity<Contact>()
                .Property(e => e.ContactType)
                .IsFixedLength();

            modelBuilder.Entity<Contact>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Contact>()
                .Property(e => e.Position)
                .IsFixedLength();

            modelBuilder.Entity<Contact>()
                .Property(e => e.HomePhone)
                .IsFixedLength();

            modelBuilder.Entity<Contact>()
                .Property(e => e.Mobile)
                .IsFixedLength();

            modelBuilder.Entity<Contact>()
                .Property(e => e.WorkPhone)
                .IsFixedLength();

            modelBuilder.Entity<Contact>()
                .Property(e => e.CreateDate)
                .IsFixedLength();

            modelBuilder.Entity<Invoice>()
                .Property(e => e.InvoiceNo)
                .IsFixedLength();

            modelBuilder.Entity<Invoice>()
                .Property(e => e.InvoiceDescription)
                .IsFixedLength();

            modelBuilder.Entity<Invoice>()
                .Property(e => e.CreateDate)
                .IsFixedLength();

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.InvoiceItems)
                .WithRequired(e => e.Invoice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InvoiceItem>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<InvoiceItem>()
                .Property(e => e.CreateDate)
                .IsFixedLength();

            modelBuilder.Entity<InvoiceItem>()
                .HasOptional(e => e.JsonInvoiceItem)
                .WithRequired(e => e.InvoiceItem);

            modelBuilder.Entity<InvoiceItem>()
                .HasOptional(e => e.ProductInvoiceItem)
                .WithRequired(e => e.InvoiceItem);

            modelBuilder.Entity<Payment>()
                .Property(e => e.PaymentNo)
                .IsFixedLength();

            modelBuilder.Entity<Payment>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<Payment>()
                .Property(e => e.CreateDate)
                .IsFixedLength();

            modelBuilder.Entity<PaymentItem>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<PaymentItem>()
                .Property(e => e.InvoiceNo)
                .IsFixedLength();

            modelBuilder.Entity<PaymentItem>()
                .Property(e => e.CreateDate)
                .IsFixedLength();

            modelBuilder.Entity<Product>()
                .Property(e => e.CreateDate)
                .IsFixedLength();

            modelBuilder.Entity<Product>()
                .Property(e => e.ProductCode)
                .IsFixedLength();

            modelBuilder.Entity<Product>()
                .Property(e => e.ProductDescription)
                .IsFixedLength();

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductInvoiceItems)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductInvoiceItem>()
                .Property(e => e.ProductName)
                .IsFixedLength();

            modelBuilder.Entity<ProductInvoiceItem>()
                .Property(e => e.ProductCode)
                .IsFixedLength();
        }
    }
}
