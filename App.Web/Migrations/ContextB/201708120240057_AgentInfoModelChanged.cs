namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgentInfoModelChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AgentInfoes", "AgentId", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.AgentInfoes", "OfficeName", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.AgentInfoes", "AgentName", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.AgentInfoes", "Email", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.AgentInfoes", "AgentPhoto", c => c.String(maxLength: 30));
            AlterColumn("dbo.AgentInfoes", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.AgentInfoes", "DelStatus", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AgentInfoes", "EntryBy", c => c.Int(nullable: false));
            AlterColumn("dbo.AgentInfoes", "EntryDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.AgentInfoes", "EntryBy");
            AddForeignKey("dbo.AgentInfoes", "EntryBy", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AgentInfoes", "EntryBy", "dbo.Users");
            DropIndex("dbo.AgentInfoes", new[] { "EntryBy" });
            AlterColumn("dbo.AgentInfoes", "EntryDate", c => c.DateTime());
            AlterColumn("dbo.AgentInfoes", "EntryBy", c => c.String(maxLength: 20));
            AlterColumn("dbo.AgentInfoes", "DelStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.AgentInfoes", "Status", c => c.String(maxLength: 10));
            AlterColumn("dbo.AgentInfoes", "AgentPhoto", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.AgentInfoes", "Email", c => c.String(maxLength: 100));
            AlterColumn("dbo.AgentInfoes", "AgentName", c => c.String(maxLength: 150));
            AlterColumn("dbo.AgentInfoes", "OfficeName", c => c.String(maxLength: 150));
            AlterColumn("dbo.AgentInfoes", "AgentId", c => c.String(maxLength: 20));
        }
    }
}
