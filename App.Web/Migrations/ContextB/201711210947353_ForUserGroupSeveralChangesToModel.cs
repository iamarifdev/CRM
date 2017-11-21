namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForUserGroupSeveralChangesToModel : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.EmployeeBasicInfoes", "IX_Employee_Id");
            CreateTable(
                "dbo.GroupUsers",
                c => new
                    {
                        GroupId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new {t.GroupId, t.UserId })
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.UserId);
            
            AlterColumn("dbo.Users", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "LastLogin", c => c.DateTime());
            AlterColumn("dbo.Users", "Active", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "EmployeeId", c => c.Int());
            AlterColumn("dbo.Users", "BranchId", c => c.Int());
            AlterColumn("dbo.Users", "Level", c => c.Int(nullable: false));
            DropTable("dbo.EmployeeBasicInfoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EmployeeBasicInfoes",
                c => new
                    {
                        RowId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.String(maxLength: 20),
                        EmployeeName = c.String(maxLength: 100),
                        FatherName = c.String(maxLength: 100),
                        MotherName = c.String(maxLength: 100),
                        Dob = c.DateTime(),
                        Gender = c.String(maxLength: 10),
                        MaritalStatus = c.String(maxLength: 10),
                        SpouseName = c.String(maxLength: 100),
                        NidNo = c.String(maxLength: 50),
                        EmployeeDesignation = c.String(maxLength: 20),
                        ImageUrl = c.String(maxLength: 25),
                        BasicSalary = c.Double(),
                        OtherAllowance = c.Double(),
                        DateOfJoining = c.DateTime(),
                        IncrementDate = c.DateTime(),
                        BloodGroup = c.String(maxLength: 15),
                        LandLineNumber = c.String(maxLength: 16),
                        MobileNumber = c.String(maxLength: 11),
                        ContactPerson = c.String(maxLength: 100),
                        ContactNumber = c.String(maxLength: 11),
                        Address = c.String(unicode: false, storeType: "text"),
                        UserLevel = c.String(maxLength: 20),
                        ZoneId = c.String(maxLength: 20),
                        TerritoryId = c.String(maxLength: 20),
                        WarehouseId = c.String(maxLength: 20),
                        Status = c.String(maxLength: 10),
                        DelStatus = c.Int(nullable: false),
                        EntryBy = c.String(maxLength: 20),
                        EntryDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RowId);
            
            DropForeignKey("dbo.GroupUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.GroupUsers", "Group_Id", "dbo.Groups");
            DropIndex("dbo.GroupUsers", new[] { "User_Id" });
            DropIndex("dbo.GroupUsers", new[] { "Group_Id" });
            AlterColumn("dbo.Users", "Level", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Users", "BranchId", c => c.String(maxLength: 20));
            AlterColumn("dbo.Users", "EmployeeId", c => c.String(maxLength: 20));
            AlterColumn("dbo.Users", "Active", c => c.Int());
            AlterColumn("dbo.Users", "LastLogin", c => c.Long());
            AlterColumn("dbo.Users", "CreatedOn", c => c.Long(nullable: false));
            DropTable("dbo.GroupUsers");
            CreateIndex("dbo.EmployeeBasicInfoes", "EmployeeId", unique: true, name: "IX_Employee_Id");
        }
    }
}
