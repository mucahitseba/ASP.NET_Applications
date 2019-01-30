namespace Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "TaxRate", c => c.Decimal(nullable: false, precision: 4, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categories", "TaxRate", c => c.Decimal(nullable: false, precision: 3, scale: 2));
        }
    }
}
