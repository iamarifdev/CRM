namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelsAndCrmContextChanged : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.UserGroups", "User_Id", "dbo.Users");
            DropIndex("dbo.UserGroups", new[] { "GroupId" });
            DropIndex("dbo.UserGroups", new[] { "User_Id" });
            DropColumn("dbo.Users", "GroupId");
            DropTable("dbo.UserGroups");
            DropTable("dbo.Groups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Description = c.String(nullable: false, maxLength: 100),
                        Crm = c.Boolean(nullable: false),
                        Billing = c.Boolean(nullable: false),
                        Accounts = c.Boolean(nullable: false),
                        Report = c.Boolean(nullable: false),
                        Hrm = c.Boolean(nullable: false),
                        Setup = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        GroupId = c.Int(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "GroupId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserGroups", "User_Id");
            CreateIndex("dbo.UserGroups", "GroupId");
            AddForeignKey("dbo.UserGroups", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.UserGroups", "GroupId", "dbo.Groups", "Id", cascadeDelete: true);
        }
    }
}
