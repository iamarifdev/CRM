namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateMadeToRequiredInTransactionInfo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransactionsInfoes", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransactionsInfoes", "Date", c => c.DateTime());
        }
    }
}
