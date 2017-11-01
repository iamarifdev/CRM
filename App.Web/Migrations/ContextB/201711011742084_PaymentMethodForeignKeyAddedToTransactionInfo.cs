namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentMethodForeignKeyAddedToTransactionInfo : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TransactionsInfoes", "MethodId");
            AddForeignKey("dbo.TransactionsInfoes", "MethodId", "dbo.PaymentMethods", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransactionsInfoes", "MethodId", "dbo.PaymentMethods");
            DropIndex("dbo.TransactionsInfoes", new[] { "MethodId" });
        }
    }
}
