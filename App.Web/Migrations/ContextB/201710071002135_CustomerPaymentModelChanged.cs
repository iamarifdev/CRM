namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerPaymentModelChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CustomerPayments", "BranchId", c => c.Int(nullable: false));
            AlterColumn("dbo.CustomerPayments", "CustomerId", c => c.Int(nullable: false));
            AlterColumn("dbo.CustomerPayments", "PaymentAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.CustomerPayments", "MethodId", c => c.Int(nullable: false));
            AlterColumn("dbo.CustomerPayments", "DelStatus", c => c.Boolean(nullable: false));
            AlterColumn("dbo.CustomerPayments", "EntryBy", c => c.Int(nullable: false));
            AlterColumn("dbo.CustomerPayments", "EntryDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.CustomerPayments", "MethodId");
            CreateIndex("dbo.CustomerPayments", "EntryBy");
            AddForeignKey("dbo.CustomerPayments", "MethodId", "dbo.PaymentMethods", "Id", cascadeDelete: false);
            AddForeignKey("dbo.CustomerPayments", "EntryBy", "dbo.Users", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerPayments", "EntryBy", "dbo.Users");
            DropForeignKey("dbo.CustomerPayments", "MethodId", "dbo.PaymentMethods");
            DropIndex("dbo.CustomerPayments", new[] { "EntryBy" });
            DropIndex("dbo.CustomerPayments", new[] { "MethodId" });
            AlterColumn("dbo.CustomerPayments", "EntryDate", c => c.DateTime());
            AlterColumn("dbo.CustomerPayments", "EntryBy", c => c.String(maxLength: 20));
            AlterColumn("dbo.CustomerPayments", "DelStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.CustomerPayments", "MethodId", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.CustomerPayments", "PaymentAmount", c => c.Double());
            AlterColumn("dbo.CustomerPayments", "CustomerId", c => c.String(maxLength: 20));
            AlterColumn("dbo.CustomerPayments", "BranchId", c => c.String(maxLength: 20));
        }
    }
}
