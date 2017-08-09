namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SectorInfoModelChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SectorInfoes", "SectorName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.SectorInfoes", "SectorCode", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.SectorInfoes", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.SectorInfoes", "DelStatus", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SectorInfoes", "EntryDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SectorInfoes", "EntryBy", c => c.Int(nullable: false));
            CreateIndex("dbo.SectorInfoes", "EntryBy");
            AddForeignKey("dbo.SectorInfoes", "EntryBy", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SectorInfoes", "EntryBy", "dbo.Users");
            DropIndex("dbo.SectorInfoes", new[] { "EntryBy" });
            AlterColumn("dbo.SectorInfoes", "EntryBy", c => c.String(maxLength: 20));
            AlterColumn("dbo.SectorInfoes", "EntryDate", c => c.DateTime());
            AlterColumn("dbo.SectorInfoes", "DelStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.SectorInfoes", "Status", c => c.String(maxLength: 10));
            AlterColumn("dbo.SectorInfoes", "SectorCode", c => c.String(maxLength: 100));
            AlterColumn("dbo.SectorInfoes", "SectorName", c => c.String(maxLength: 100));
        }
    }
}
