namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayerTypeSetToNullableInTransactionInfoModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransactionsInfoes", "PayerType", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransactionsInfoes", "PayerType", c => c.Int(nullable: false));
        }
    }
}
