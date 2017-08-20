namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SmsConfirmationAddedToClientInfoModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientInfoes", "SmsConfirmation", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientInfoes", "SmsConfirmation");
        }
    }
}
