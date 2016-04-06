namespace AccBroker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AddressLine1 = c.String(maxLength: 10, fixedLength: true),
                        AddressLine2 = c.String(maxLength: 10, fixedLength: true),
                        Suburb = c.String(maxLength: 10, fixedLength: true),
                        State = c.String(maxLength: 10, fixedLength: true),
                        PostCode = c.String(maxLength: 10, fixedLength: true),
                        ClientID = c.Int(),
                        CompanyID = c.Int(),
                        Concurrency = c.Guid(),
                        AddressType = c.Int(),
                        CreateDate = c.Binary(fixedLength: true, timestamp: true, storeType: "timestamp"),
                        ChangeDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Client", t => t.ClientID)
                .ForeignKey("dbo.Company", t => t.CompanyID)
                .Index(t => t.ClientID)
                .Index(t => t.CompanyID);
            
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 3, fixedLength: true),
                        Name = c.String(maxLength: 10, fixedLength: true),
                        ABN = c.String(maxLength: 10, fixedLength: true),
                        Concurrency = c.Guid(),
                        CompanyID = c.Int(),
                        LinkCompanyID = c.Int(),
                        CreateDate = c.Binary(fixedLength: true, timestamp: true, storeType: "timestamp"),
                        ChangeDate = c.DateTime(),
                        AutoApplyGST = c.Boolean(),
                        AppliedGST = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ContactType = c.String(maxLength: 10, fixedLength: true),
                        Name = c.String(maxLength: 10, fixedLength: true),
                        Position = c.String(maxLength: 10, fixedLength: true),
                        HomePhone = c.String(maxLength: 10, fixedLength: true),
                        Mobile = c.String(maxLength: 10, fixedLength: true),
                        WorkPhone = c.String(maxLength: 10, fixedLength: true),
                        Concurrency = c.Guid(),
                        ClientID = c.Int(),
                        CompanyID = c.Int(),
                        CreateDate = c.Binary(fixedLength: true, timestamp: true, storeType: "timestamp"),
                        ChangeDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Client", t => t.ClientID)
                .ForeignKey("dbo.Company", t => t.CompanyID)
                .Index(t => t.ClientID)
                .Index(t => t.CompanyID);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 3, fixedLength: true),
                        Name = c.String(maxLength: 10, fixedLength: true),
                        ABN = c.String(maxLength: 10, fixedLength: true),
                        Concurrency = c.Guid(),
                        CreateDate = c.Binary(fixedLength: true, timestamp: true, storeType: "timestamp"),
                        ChangeDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Invoice",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        InvoiceNo = c.String(maxLength: 10, fixedLength: true),
                        InvoiceDescription = c.String(maxLength: 10, fixedLength: true),
                        BillingAddress = c.Int(),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        CompanyID = c.Int(),
                        ClientID = c.Int(),
                        GST = c.Decimal(precision: 18, scale: 2),
                        Concurrency = c.Guid(),
                        InvoiceType = c.Int(),
                        DebtorLinkInvoiceID = c.Int(),
                        CreditorLinkInvoiceID = c.Int(),
                        CreateDate = c.Binary(fixedLength: true, timestamp: true, storeType: "timestamp"),
                        ChangeDate = c.DateTime(),
                        InvoiceDate = c.DateTime(storeType: "date"),
                        DueDate = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Client", t => t.ClientID)
                .ForeignKey("dbo.Company", t => t.CompanyID)
                .Index(t => t.CompanyID)
                .Index(t => t.ClientID);
            
            CreateTable(
                "dbo.InvoiceItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        InvoiceID = c.Int(nullable: false),
                        SequenceNo = c.Int(),
                        GST = c.Decimal(precision: 18, scale: 2),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        Description = c.String(maxLength: 10, fixedLength: true),
                        Concurrency = c.Guid(),
                        CreateDate = c.Binary(fixedLength: true, timestamp: true, storeType: "timestamp"),
                        ChangeDate = c.DateTime(),
                        Discount = c.Decimal(precision: 18, scale: 2),
                        InvocieType = c.Int(),
                    })
                .PrimaryKey(t => new { t.ID, t.InvoiceID })
                .ForeignKey("dbo.Invoice", t => t.InvoiceID, cascadeDelete: true)
                .Index(t => t.InvoiceID);
            
            CreateTable(
                "dbo.JsonInvoiceItem",
                c => new
                    {
                        InvoiceItemID = c.Int(nullable: false),
                        InvoiceID = c.Int(nullable: false),
                        ID = c.Int(nullable: false, identity: true),
                        JsonString = c.String(nullable: false, storeType: "ntext"),
                    })
                .PrimaryKey(t => new { t.InvoiceItemID, t.InvoiceID })
                .ForeignKey("dbo.InvoiceItem", t => new { t.InvoiceItemID, t.InvoiceID })
                .ForeignKey("dbo.Invoice", t => t.InvoiceID)
                .Index(t => new { t.InvoiceItemID, t.InvoiceID });
            
            CreateTable(
                "dbo.ProductInvoiceItem",
                c => new
                    {
                        InvoiceItemID = c.Int(nullable: false),
                        InvoiceID = c.Int(nullable: false),
                        ID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        ProductName = c.String(nullable: false, maxLength: 100, fixedLength: true),
                        ProductCode = c.String(nullable: false, maxLength: 10, fixedLength: true),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.InvoiceItemID, t.InvoiceID })
                .ForeignKey("dbo.Product", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.InvoiceItem", t => new { t.InvoiceItemID, t.InvoiceID })
                .ForeignKey("dbo.Invoice", t => t.InvoiceID)
                .Index(t => new { t.InvoiceItemID, t.InvoiceID })
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Concurrency = c.Guid(),
                        CreateDate = c.Binary(fixedLength: true, timestamp: true, storeType: "timestamp"),
                        ProductCode = c.String(nullable: false, maxLength: 10),
                        ProductName = c.String(nullable: false, storeType: "ntext"),
                        ProductDescription = c.String(maxLength: 10, fixedLength: true),
                        CostPrice = c.Decimal(precision: 18, scale: 2),
                        SellPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ChangeDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PaymentItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PaymentID = c.Int(nullable: false),
                        SequenceNo = c.Int(),
                        Description = c.String(maxLength: 10, fixedLength: true),
                        GST = c.Decimal(precision: 18, scale: 2),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        InvoiceID = c.Int(),
                        InvoiceNo = c.String(maxLength: 10, fixedLength: true),
                        Concurrency = c.Guid(),
                        CreateDate = c.Binary(fixedLength: true, timestamp: true, storeType: "timestamp"),
                        ChangeDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.ID, t.PaymentID })
                .ForeignKey("dbo.Invoice", t => t.InvoiceID)
                .ForeignKey("dbo.Payment", t => t.PaymentID, cascadeDelete: true)
                .Index(t => t.PaymentID)
                .Index(t => t.InvoiceID);
            
            CreateTable(
                "dbo.Payment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PaymentNo = c.String(maxLength: 10, fixedLength: true),
                        Description = c.String(maxLength: 10, fixedLength: true),
                        GST = c.Decimal(precision: 18, scale: 2),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        PaymentType = c.Int(),
                        Concurrency = c.Guid(),
                        PaymentDate = c.DateTime(storeType: "date"),
                        CreateDate = c.Binary(fixedLength: true, timestamp: true, storeType: "timestamp"),
                        ChangeDate = c.DateTime(),
                        CompanyID = c.Int(),
                        ClientID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Client", t => t.ClientID)
                .ForeignKey("dbo.Company", t => t.CompanyID)
                .Index(t => t.CompanyID)
                .Index(t => t.ClientID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductInvoiceItem", "InvoiceID", "dbo.Invoice");
            DropForeignKey("dbo.PaymentItem", "PaymentID", "dbo.Payment");
            DropForeignKey("dbo.Payment", "CompanyID", "dbo.Company");
            DropForeignKey("dbo.Payment", "ClientID", "dbo.Client");
            DropForeignKey("dbo.PaymentItem", "InvoiceID", "dbo.Invoice");
            DropForeignKey("dbo.JsonInvoiceItem", "InvoiceID", "dbo.Invoice");
            DropForeignKey("dbo.InvoiceItem", "InvoiceID", "dbo.Invoice");
            DropForeignKey("dbo.ProductInvoiceItem", new[] { "InvoiceItemID", "InvoiceID" }, "dbo.InvoiceItem");
            DropForeignKey("dbo.ProductInvoiceItem", "ProductID", "dbo.Product");
            DropForeignKey("dbo.JsonInvoiceItem", new[] { "InvoiceItemID", "InvoiceID" }, "dbo.InvoiceItem");
            DropForeignKey("dbo.Invoice", "CompanyID", "dbo.Company");
            DropForeignKey("dbo.Invoice", "ClientID", "dbo.Client");
            DropForeignKey("dbo.Contact", "CompanyID", "dbo.Company");
            DropForeignKey("dbo.Address", "CompanyID", "dbo.Company");
            DropForeignKey("dbo.Contact", "ClientID", "dbo.Client");
            DropForeignKey("dbo.Address", "ClientID", "dbo.Client");
            DropIndex("dbo.Payment", new[] { "ClientID" });
            DropIndex("dbo.Payment", new[] { "CompanyID" });
            DropIndex("dbo.PaymentItem", new[] { "InvoiceID" });
            DropIndex("dbo.PaymentItem", new[] { "PaymentID" });
            DropIndex("dbo.ProductInvoiceItem", new[] { "ProductID" });
            DropIndex("dbo.ProductInvoiceItem", new[] { "InvoiceItemID", "InvoiceID" });
            DropIndex("dbo.JsonInvoiceItem", new[] { "InvoiceItemID", "InvoiceID" });
            DropIndex("dbo.InvoiceItem", new[] { "InvoiceID" });
            DropIndex("dbo.Invoice", new[] { "ClientID" });
            DropIndex("dbo.Invoice", new[] { "CompanyID" });
            DropIndex("dbo.Contact", new[] { "CompanyID" });
            DropIndex("dbo.Contact", new[] { "ClientID" });
            DropIndex("dbo.Address", new[] { "CompanyID" });
            DropIndex("dbo.Address", new[] { "ClientID" });
            DropTable("dbo.Payment");
            DropTable("dbo.PaymentItem");
            DropTable("dbo.Product");
            DropTable("dbo.ProductInvoiceItem");
            DropTable("dbo.JsonInvoiceItem");
            DropTable("dbo.InvoiceItem");
            DropTable("dbo.Invoice");
            DropTable("dbo.Company");
            DropTable("dbo.Contact");
            DropTable("dbo.Client");
            DropTable("dbo.Address");
        }
    }
}
