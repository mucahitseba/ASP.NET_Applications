namespace Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Invoices", "Quantity", c => c.Decimal(nullable: false, precision: 8, scale: 4));
            AlterColumn("dbo.Invoices", "Price", c => c.Decimal(nullable: false, precision: 9, scale: 3));
            AlterColumn("dbo.Invoices", "Discount", c => c.Decimal(nullable: false, precision: 3, scale: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Invoices", "Discount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Invoices", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Invoices", "Quantity", c => c.Double(nullable: false));
        }
    }
}
