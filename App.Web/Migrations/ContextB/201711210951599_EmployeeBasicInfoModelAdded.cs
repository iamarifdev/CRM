namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeBasicInfoModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeBasicInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.String(nullable: false, maxLength: 20),
                        EmployeeName = c.String(maxLength: 100),
                        FatherName = c.String(maxLength: 100),
                        MotherName = c.String(maxLength: 100),
                        Dob = c.DateTime(),
                        Gender = c.Int(nullable: false),
                        MaritalStatus = c.Int(nullable: false),
                        SpouseName = c.String(maxLength: 100),
                        NidNo = c.String(maxLength: 50),
                        EmployeeDesignationId = c.Int(nullable: false),
                        ImageUrl = c.String(maxLength: 255),
                        BasicSalary = c.Double(),
                        OtherAllowance = c.Double(),
                        DateOfJoining = c.DateTime(),
                        IncrementDate = c.DateTime(),
                        BloodGroup = c.Int(),
                        LandLineNumber = c.String(maxLength: 16),
                        MobileNumber = c.String(nullable: false, maxLength: 15),
                        ContactPerson = c.String(maxLength: 100),
                        ContactNumber = c.String(maxLength: 15),
                        Address = c.String(unicode: false, storeType: "text"),
                        UserLevel = c.Int(),
                        ZoneId = c.String(maxLength: 20),
                        TerritoryId = c.String(maxLength: 20),
                        WarehouseId = c.String(maxLength: 20),
                        Status = c.Int(nullable: false),
                        DelStatus = c.Boolean(nullable: false),
                        EntryBy = c.Int(nullable: false),
                        EntryDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmployeeDesignations", t => t.EmployeeDesignationId, cascadeDelete: true)
                .Index(t => t.EmployeeDesignationId);
            
            CreateIndex("dbo.Users", "EmployeeId");
            AddForeignKey("dbo.Users", "EmployeeId", "dbo.EmployeeBasicInfoes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "EmployeeId", "dbo.EmployeeBasicInfoes");
            DropForeignKey("dbo.EmployeeBasicInfoes", "EmployeeDesignationId", "dbo.EmployeeDesignations");
            DropIndex("dbo.EmployeeBasicInfoes", new[] { "EmployeeDesignationId" });
            DropIndex("dbo.Users", new[] { "EmployeeId" });
            DropTable("dbo.EmployeeBasicInfoes");
        }
    }
}
