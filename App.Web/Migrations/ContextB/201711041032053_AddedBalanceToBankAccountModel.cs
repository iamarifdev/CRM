namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBalanceToBankAccountModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankAccounts", "Balance", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BankAccounts", "Balance");
        }
    }
}
