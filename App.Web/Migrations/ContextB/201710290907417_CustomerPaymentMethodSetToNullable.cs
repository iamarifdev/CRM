namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerPaymentMethodSetToNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomerPayments", "MethodId", "dbo.PaymentMethods");
            DropIndex("dbo.CustomerPayments", new[] { "MethodId" });
            AlterColumn("dbo.CustomerPayments", "MethodId", c => c.Int());
            CreateIndex("dbo.CustomerPayments", "MethodId");
            AddForeignKey("dbo.CustomerPayments", "MethodId", "dbo.PaymentMethods", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerPayments", "MethodId", "dbo.PaymentMethods");
            DropIndex("dbo.CustomerPayments", new[] { "MethodId" });
            AlterColumn("dbo.CustomerPayments", "MethodId", c => c.Int(nullable: false));
            CreateIndex("dbo.CustomerPayments", "MethodId");
            AddForeignKey("dbo.CustomerPayments", "MethodId", "dbo.PaymentMethods", "Id", cascadeDelete: true);
        }
    }
}
