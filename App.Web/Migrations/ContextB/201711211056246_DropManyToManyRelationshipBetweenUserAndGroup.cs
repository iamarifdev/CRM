namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropManyToManyRelationshipBetweenUserAndGroup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GroupUsers", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.GroupUsers", "User_Id", "dbo.Users");
            DropIndex("dbo.GroupUsers", new[] { "Group_Id" });
            DropIndex("dbo.GroupUsers", new[] { "User_Id" });
            AlterColumn("dbo.EmployeeBasicInfoes", "Address", c => c.String(maxLength: 500));
            CreateIndex("dbo.EmployeeBasicInfoes", "EntryBy");
            AddForeignKey("dbo.EmployeeBasicInfoes", "EntryBy", "dbo.Users", "Id", cascadeDelete: false);
            DropTable("dbo.GroupUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GroupUsers",
                c => new
                    {
                        Group_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.User_Id });
            
            DropForeignKey("dbo.EmployeeBasicInfoes", "EntryBy", "dbo.Users");
            DropIndex("dbo.EmployeeBasicInfoes", new[] { "EntryBy" });
            AlterColumn("dbo.EmployeeBasicInfoes", "Address", c => c.String(unicode: false, storeType: "text"));
            CreateIndex("dbo.GroupUsers", "User_Id");
            CreateIndex("dbo.GroupUsers", "Group_Id");
            AddForeignKey("dbo.GroupUsers", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GroupUsers", "Group_Id", "dbo.Groups", "Id", cascadeDelete: true);
        }
    }
}
