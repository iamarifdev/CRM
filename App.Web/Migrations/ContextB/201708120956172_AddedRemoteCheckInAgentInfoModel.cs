namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRemoteCheckInAgentInfoModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AgentInfoes", "ResetStatus", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AgentInfoes", "ResetStatus", c => c.Int(nullable: false));
        }
    }
}
