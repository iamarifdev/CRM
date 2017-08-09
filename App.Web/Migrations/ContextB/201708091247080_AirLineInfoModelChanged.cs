namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AirLineInfoModelChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AirLineInfoes", "AirLineName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.AirLineInfoes", "Description", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.AirLineInfoes", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.AirLineInfoes", "DelStatus", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AirLineInfoes", "EntryDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AirLineInfoes", "EntryBy", c => c.Int(nullable: false));
            CreateIndex("dbo.AirLineInfoes", "EntryBy");
            AddForeignKey("dbo.AirLineInfoes", "EntryBy", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AirLineInfoes", "EntryBy", "dbo.Users");
            DropIndex("dbo.AirLineInfoes", new[] { "EntryBy" });
            AlterColumn("dbo.AirLineInfoes", "EntryBy", c => c.String(maxLength: 20));
            AlterColumn("dbo.AirLineInfoes", "EntryDate", c => c.DateTime());
            AlterColumn("dbo.AirLineInfoes", "DelStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.AirLineInfoes", "Status", c => c.String(maxLength: 10));
            AlterColumn("dbo.AirLineInfoes", "Description", c => c.String(maxLength: 250));
            AlterColumn("dbo.AirLineInfoes", "AirLineName", c => c.String(maxLength: 100));
        }
    }
}
