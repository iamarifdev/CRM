namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserModelChangedAgain : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Active", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "Status");
        }
    }
}
