namespace AccBroker.Data2
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AccountDB : DbContext
    {
        public AccountDB()
            : base("name=AccountDB")
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentItem> PaymentItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }

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

            modelBuilder.Entity<InvoiceItem>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<InvoiceItem>()
                .Property(e => e.CreateDate)
                .IsFixedLength();

            modelBuilder.Entity<Payment>()
                .Property(e => e.PaymentNo)
                .IsFixedLength();

            modelBuilder.Entity<Payment>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<Payment>()
                .Property(e => e.CreateDate)
                .IsFixedLength();

            modelBuilder.Entity<Payment>()
                .HasMany(e => e.PaymentItems)
                .WithRequired(e => e.Payment)
                .WillCascadeOnDelete(false);

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
                .Property(e => e.ProductName)
                .IsFixedLength();

            modelBuilder.Entity<Product>()
                .Property(e => e.ProductDescription)
                .IsFixedLength();
        }
    }
}
