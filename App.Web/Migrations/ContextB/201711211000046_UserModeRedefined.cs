namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserModeRedefined : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "EmployeeId", "dbo.EmployeeBasicInfoes");
            DropIndex("dbo.Users", new[] { "EmployeeId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Users", "EmployeeId");
            AddForeignKey("dbo.Users", "EmployeeId", "dbo.EmployeeBasicInfoes", "Id");
        }
    }
}
