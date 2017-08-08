namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeDesignationModelChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EmployeeDesignations", "DesignationTitleEn", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EmployeeDesignations", "DesignationTitleEn", c => c.String(maxLength: 100));
        }
    }
}
