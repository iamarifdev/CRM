namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AirLineToNullableClientInfoModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClientInfoes", "AirLineId", "dbo.AirLineInfoes");
            DropIndex("dbo.ClientInfoes", new[] { "AirLineId" });
            AlterColumn("dbo.ClientInfoes", "AirLineId", c => c.Int());
            CreateIndex("dbo.ClientInfoes", "AirLineId");
            AddForeignKey("dbo.ClientInfoes", "AirLineId", "dbo.AirLineInfoes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientInfoes", "AirLineId", "dbo.AirLineInfoes");
            DropIndex("dbo.ClientInfoes", new[] { "AirLineId" });
            AlterColumn("dbo.ClientInfoes", "AirLineId", c => c.Int(nullable: false));
            CreateIndex("dbo.ClientInfoes", "AirLineId");
            AddForeignKey("dbo.ClientInfoes", "AirLineId", "dbo.AirLineInfoes", "Id", cascadeDelete: true);
        }
    }
}
