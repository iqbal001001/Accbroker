namespace AccBroker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldsUserCreateChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Address", "CreateUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Address", "ChangeUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Client", "CreateUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Client", "ChangeUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Contact", "CreateUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Contact", "ChangeUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Company", "CreateUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Company", "ChangeUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Invoice", "CreateUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Invoice", "ChangeUser", c => c.String(maxLength: 20));
            AddColumn("dbo.InvoiceItem", "CreateUser", c => c.String(maxLength: 20));
            AddColumn("dbo.InvoiceItem", "ChangeUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Product", "CreateUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Product", "ChangeUser", c => c.String(maxLength: 20));
            AddColumn("dbo.PaymentItem", "CreateUser", c => c.String(maxLength: 20));
            AddColumn("dbo.PaymentItem", "ChangeUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Payment", "CreateUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Payment", "ChangeUser", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payment", "ChangeUser");
            DropColumn("dbo.Payment", "CreateUser");
            DropColumn("dbo.PaymentItem", "ChangeUser");
            DropColumn("dbo.PaymentItem", "CreateUser");
            DropColumn("dbo.Product", "ChangeUser");
            DropColumn("dbo.Product", "CreateUser");
            DropColumn("dbo.InvoiceItem", "ChangeUser");
            DropColumn("dbo.InvoiceItem", "CreateUser");
            DropColumn("dbo.Invoice", "ChangeUser");
            DropColumn("dbo.Invoice", "CreateUser");
            DropColumn("dbo.Company", "ChangeUser");
            DropColumn("dbo.Company", "CreateUser");
            DropColumn("dbo.Contact", "ChangeUser");
            DropColumn("dbo.Contact", "CreateUser");
            DropColumn("dbo.Client", "ChangeUser");
            DropColumn("dbo.Client", "CreateUser");
            DropColumn("dbo.Address", "ChangeUser");
            DropColumn("dbo.Address", "CreateUser");
        }
    }
}
