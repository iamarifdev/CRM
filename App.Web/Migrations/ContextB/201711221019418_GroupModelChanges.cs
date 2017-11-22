namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupModelChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Groups", "Crm", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Groups", "Billing", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Groups", "Account", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Groups", "Report", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Groups", "Hrm", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Groups", "Setup", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Groups", "Setup", c => c.Int(nullable: false));
            AlterColumn("dbo.Groups", "Hrm", c => c.Int(nullable: false));
            AlterColumn("dbo.Groups", "Report", c => c.Int(nullable: false));
            AlterColumn("dbo.Groups", "Account", c => c.Int(nullable: false));
            AlterColumn("dbo.Groups", "Billing", c => c.Int(nullable: false));
            AlterColumn("dbo.Groups", "Crm", c => c.Int(nullable: false));
        }
    }
}
