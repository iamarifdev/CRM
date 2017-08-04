namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupModelChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Groups", "Crm", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Groups", "Billing", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Groups", "Accounts", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Groups", "Report", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Groups", "Hrm", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Groups", "Setup", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Groups", "Setup", c => c.String(nullable: false, maxLength: 5));
            AlterColumn("dbo.Groups", "Hrm", c => c.String(nullable: false, maxLength: 5));
            AlterColumn("dbo.Groups", "Report", c => c.String(nullable: false, maxLength: 5));
            AlterColumn("dbo.Groups", "Accounts", c => c.String(nullable: false, maxLength: 5));
            AlterColumn("dbo.Groups", "Billing", c => c.String(nullable: false, maxLength: 5));
            AlterColumn("dbo.Groups", "Crm", c => c.String(nullable: false, maxLength: 5));
        }
    }
}
