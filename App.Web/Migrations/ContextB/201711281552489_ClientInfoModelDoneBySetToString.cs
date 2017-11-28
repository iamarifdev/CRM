namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientInfoModelDoneBySetToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClientInfoes", "DoneBy", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClientInfoes", "DoneBy", c => c.DateTime());
        }
    }
}
