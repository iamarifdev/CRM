namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentMethodModelChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentMethods", "DelStatus", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentMethods", "EntryDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PaymentMethods", "EntryBy", c => c.Int(nullable: false));
            CreateIndex("dbo.PaymentMethods", "EntryBy");
            AddForeignKey("dbo.PaymentMethods", "EntryBy", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentMethods", "EntryBy", "dbo.Users");
            DropIndex("dbo.PaymentMethods", new[] { "EntryBy" });
            DropColumn("dbo.PaymentMethods", "EntryBy");
            DropColumn("dbo.PaymentMethods", "EntryDate");
            DropColumn("dbo.PaymentMethods", "DelStatus");
        }
    }
}
