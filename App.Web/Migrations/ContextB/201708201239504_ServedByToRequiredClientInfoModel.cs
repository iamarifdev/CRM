namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServedByToRequiredClientInfoModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClientInfoes", "ServedBy", "dbo.Users");
            DropIndex("dbo.ClientInfoes", new[] { "ServedBy" });
            AlterColumn("dbo.ClientInfoes", "ServedBy", c => c.Int(nullable: false));
            CreateIndex("dbo.ClientInfoes", "ServedBy");
            AddForeignKey("dbo.ClientInfoes", "ServedBy", "dbo.Users", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientInfoes", "ServedBy", "dbo.Users");
            DropIndex("dbo.ClientInfoes", new[] { "ServedBy" });
            AlterColumn("dbo.ClientInfoes", "ServedBy", c => c.Int());
            CreateIndex("dbo.ClientInfoes", "ServedBy");
            AddForeignKey("dbo.ClientInfoes", "ServedBy", "dbo.Users", "Id");
        }
    }
}
