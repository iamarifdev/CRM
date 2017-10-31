namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionInfoModelChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransactionsInfoes", "Description", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransactionsInfoes", "Description", c => c.String(nullable: false, unicode: false, storeType: "text"));
        }
    }
}
