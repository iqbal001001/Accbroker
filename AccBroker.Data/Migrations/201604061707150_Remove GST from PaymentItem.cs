namespace AccBroker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveGSTfromPaymentItem : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PaymentItem", "GST");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentItem", "GST", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
