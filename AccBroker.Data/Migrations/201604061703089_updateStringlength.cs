namespace AccBroker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateStringlength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Address", "AddressLine1", c => c.String(maxLength: 30, fixedLength: true));
            AlterColumn("dbo.Address", "AddressLine2", c => c.String(maxLength: 30, fixedLength: true));
            AlterColumn("dbo.Address", "Suburb", c => c.String(maxLength: 20, fixedLength: true));
            AlterColumn("dbo.Address", "State", c => c.String(maxLength: 3, fixedLength: true));
            AlterColumn("dbo.Address", "PostCode", c => c.String(maxLength: 4, fixedLength: true));
            AlterColumn("dbo.Client", "Name", c => c.String(maxLength: 50, fixedLength: true));
            AlterColumn("dbo.Contact", "Name", c => c.String(maxLength: 20, fixedLength: true));
            AlterColumn("dbo.Contact", "Position", c => c.String(maxLength: 20, fixedLength: true));
            AlterColumn("dbo.Company", "Name", c => c.String(maxLength: 50, fixedLength: true));
            AlterColumn("dbo.Invoice", "InvoiceDescription", c => c.String(maxLength: 100, fixedLength: true));
            AlterColumn("dbo.InvoiceItem", "Description", c => c.String(maxLength: 100, fixedLength: true));
            AlterColumn("dbo.ProductInvoiceItem", "ProductCode", c => c.String(nullable: false, maxLength: 6, fixedLength: true));
            AlterColumn("dbo.Product", "ProductDescription", c => c.String(maxLength: 100, fixedLength: true));
            AlterColumn("dbo.PaymentItem", "Description", c => c.String(maxLength: 100, fixedLength: true));
            AlterColumn("dbo.Payment", "Description", c => c.String(maxLength: 100, fixedLength: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payment", "Description", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.PaymentItem", "Description", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.Product", "ProductDescription", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.ProductInvoiceItem", "ProductCode", c => c.String(nullable: false, maxLength: 10, fixedLength: true));
            AlterColumn("dbo.InvoiceItem", "Description", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.Invoice", "InvoiceDescription", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.Company", "Name", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.Contact", "Position", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.Contact", "Name", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.Client", "Name", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.Address", "PostCode", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.Address", "State", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.Address", "Suburb", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.Address", "AddressLine2", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.Address", "AddressLine1", c => c.String(maxLength: 10, fixedLength: true));
        }
    }
}
