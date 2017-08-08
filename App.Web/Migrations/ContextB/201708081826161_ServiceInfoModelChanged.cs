namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceInfoModelChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ServiceInfoes", "ServiceName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.ServiceInfoes", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceInfoes", "DelStatus", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ServiceInfoes", "EntryDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ServiceInfoes", "EntryBy", c => c.Int(nullable: false));
            CreateIndex("dbo.ServiceInfoes", "EntryBy");
            AddForeignKey("dbo.ServiceInfoes", "EntryBy", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceInfoes", "EntryBy", "dbo.Users");
            DropIndex("dbo.ServiceInfoes", new[] { "EntryBy" });
            AlterColumn("dbo.ServiceInfoes", "EntryBy", c => c.String(maxLength: 20));
            AlterColumn("dbo.ServiceInfoes", "EntryDate", c => c.DateTime());
            AlterColumn("dbo.ServiceInfoes", "DelStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceInfoes", "Status", c => c.String(maxLength: 10));
            AlterColumn("dbo.ServiceInfoes", "ServiceName", c => c.String(maxLength: 100));
        }
    }
}
