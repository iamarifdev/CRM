namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerPaymentModelChangedToAddForeignKey : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CustomerPayments", "CustomerId");
            AddForeignKey("dbo.CustomerPayments", "CustomerId", "dbo.ClientInfoes", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerPayments", "CustomerId", "dbo.ClientInfoes");
            DropIndex("dbo.CustomerPayments", new[] { "CustomerId" });
        }
    }
}
