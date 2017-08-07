namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeDesignationAndBranchInfoModelChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BranchInfoes", "EntryDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EmployeeDesignations", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.EmployeeDesignations", "DelStatus", c => c.Boolean(nullable: false));
            AlterColumn("dbo.EmployeeDesignations", "EntryDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EmployeeDesignations", "EntryBy", c => c.Int(nullable: false));
            CreateIndex("dbo.EmployeeDesignations", "EntryBy");
            AddForeignKey("dbo.EmployeeDesignations", "EntryBy", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeDesignations", "EntryBy", "dbo.Users");
            DropIndex("dbo.EmployeeDesignations", new[] { "EntryBy" });
            AlterColumn("dbo.EmployeeDesignations", "EntryBy", c => c.String(maxLength: 20));
            AlterColumn("dbo.EmployeeDesignations", "EntryDate", c => c.DateTime());
            AlterColumn("dbo.EmployeeDesignations", "DelStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.EmployeeDesignations", "Status", c => c.String(maxLength: 10));
            AlterColumn("dbo.BranchInfoes", "EntryDate", c => c.DateTime());
        }
    }
}
