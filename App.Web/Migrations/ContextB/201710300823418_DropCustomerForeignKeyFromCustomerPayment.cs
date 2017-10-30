namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropCustomerForeignKeyFromCustomerPayment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomerPayments", "CustomerId", "dbo.ClientInfoes");
            DropIndex("dbo.CustomerPayments", new[] { "CustomerId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.CustomerPayments", "CustomerId");
            AddForeignKey("dbo.CustomerPayments", "CustomerId", "dbo.ClientInfoes", "Id", cascadeDelete: true);
        }
    }
}
