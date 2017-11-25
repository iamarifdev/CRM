namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeRequiredNameForEmployeeSeveralField : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EmployeeBasicInfoes", "ContactNumber", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.EmployeeBasicInfoes", "UserLevel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EmployeeBasicInfoes", "UserLevel", c => c.Int());
            AlterColumn("dbo.EmployeeBasicInfoes", "ContactNumber", c => c.String(maxLength: 15));
        }
    }
}
