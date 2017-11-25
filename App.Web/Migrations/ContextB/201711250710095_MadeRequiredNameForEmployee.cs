namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeRequiredNameForEmployee : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EmployeeBasicInfoes", "EmployeeName", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EmployeeBasicInfoes", "EmployeeName", c => c.String(maxLength: 100));
        }
    }
}
