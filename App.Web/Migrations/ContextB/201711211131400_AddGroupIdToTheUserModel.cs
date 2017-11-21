namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroupIdToTheUserModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "GroupId", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "GroupId");
            AddForeignKey("dbo.Users", "GroupId", "dbo.Groups", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "GroupId", "dbo.Groups");
            DropIndex("dbo.Users", new[] { "GroupId" });
            DropColumn("dbo.Users", "GroupId");
        }
    }
}
