namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceBranchModelChanged : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ServiceInfoes", "ServiceId", unique: true, name: "IX_Service_Id");
            CreateIndex("dbo.ServiceInfoes", "ServiceName", unique: true, name: "IX_Service_Name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ServiceInfoes", "IX_Service_Name");
            DropIndex("dbo.ServiceInfoes", "IX_Service_Id");
        }
    }
}
