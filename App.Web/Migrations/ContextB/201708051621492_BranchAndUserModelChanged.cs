namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BranchAndUserModelChanged : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserGroups", "UserId", "dbo.Users");
            DropIndex("dbo.UserGroups", new[] { "UserId" });
            DropPrimaryKey("dbo.Users");
            AddColumn("dbo.UserGroups", "User_Id", c => c.Int());
            AlterColumn("dbo.BranchInfoes", "BranchName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.BranchInfoes", "BranchCode", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.BranchInfoes", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.BranchInfoes", "DelStatus", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BranchInfoes", "EntryBy", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Users", "Id");
            CreateIndex("dbo.BranchInfoes", "EntryBy");
            CreateIndex("dbo.UserGroups", "User_Id");
            AddForeignKey("dbo.BranchInfoes", "EntryBy", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserGroups", "User_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGroups", "User_Id", "dbo.Users");
            DropForeignKey("dbo.BranchInfoes", "EntryBy", "dbo.Users");
            DropIndex("dbo.UserGroups", new[] { "User_Id" });
            DropIndex("dbo.BranchInfoes", new[] { "EntryBy" });
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.BranchInfoes", "EntryBy", c => c.String(maxLength: 20));
            AlterColumn("dbo.BranchInfoes", "DelStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.BranchInfoes", "Status", c => c.String(maxLength: 10));
            AlterColumn("dbo.BranchInfoes", "BranchCode", c => c.String(maxLength: 100));
            AlterColumn("dbo.BranchInfoes", "BranchName", c => c.String(maxLength: 100));
            DropColumn("dbo.UserGroups", "User_Id");
            AddPrimaryKey("dbo.Users", "Id");
            CreateIndex("dbo.UserGroups", "UserId");
            AddForeignKey("dbo.UserGroups", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
