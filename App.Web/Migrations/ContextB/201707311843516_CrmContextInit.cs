namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CrmContextInit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgentInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AgentId = c.String(maxLength: 20),
                        OfficeName = c.String(maxLength: 150),
                        AgentName = c.String(maxLength: 150),
                        ContactName = c.String(maxLength: 50),
                        MobileNo = c.String(maxLength: 25),
                        Address = c.String(maxLength: 255),
                        OfficeNo = c.String(maxLength: 15),
                        FaxNo = c.String(maxLength: 15),
                        Email = c.String(maxLength: 100),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                        ResetStatus = c.Int(nullable: false),
                        Channel = c.String(maxLength: 10),
                        AgentPhoto = c.String(nullable: false, maxLength: 30),
                        Status = c.String(maxLength: 10),
                        DelStatus = c.Int(nullable: false),
                        EntryBy = c.String(maxLength: 20),
                        EntryDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AirLineInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AirLineId = c.String(nullable: false, maxLength: 20),
                        AirLineName = c.String(maxLength: 100),
                        Description = c.String(maxLength: 250),
                        Status = c.String(maxLength: 10),
                        DelStatus = c.Int(nullable: false),
                        EntryDate = c.DateTime(),
                        EntryBy = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BranchInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchId = c.String(nullable: false, maxLength: 20),
                        BranchName = c.String(maxLength: 100),
                        BranchCode = c.String(maxLength: 100),
                        Status = c.String(maxLength: 10),
                        DelStatus = c.Int(nullable: false),
                        EntryDate = c.DateTime(),
                        EntryBy = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClientInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.String(maxLength: 20),
                        BranchId = c.String(maxLength: 20),
                        Sn = c.String(nullable: false, maxLength: 255),
                        ReferralType = c.String(maxLength: 10),
                        AgentId = c.String(maxLength: 20),
                        SupplierId = c.String(nullable: false, maxLength: 20),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        ContactNo = c.String(maxLength: 25),
                        Referral = c.String(maxLength: 100),
                        ReferralContactNo = c.String(maxLength: 25),
                        ServiceId = c.String(maxLength: 20),
                        AirLineId = c.String(maxLength: 20),
                        OldFlightDate = c.DateTime(),
                        ChangeFlightDate = c.DateTime(),
                        AirLinePnr = c.String(maxLength: 50),
                        GdsPnr = c.String(maxLength: 50),
                        NewFlightDate = c.DateTime(nullable: false),
                        CollageName = c.String(nullable: false, maxLength: 150),
                        CourseName = c.String(nullable: false, maxLength: 150),
                        EmailAddress = c.String(nullable: false, maxLength: 100),
                        ServiceCharge = c.Double(),
                        Cost = c.Double(),
                        Profit = c.Double(),
                        Discount = c.Double(nullable: false),
                        ServedBy = c.String(maxLength: 20),
                        DoneBy = c.String(maxLength: 100),
                        WorkingStatus = c.String(maxLength: 20),
                        DeliveryStatus = c.String(maxLength: 20),
                        InfoStatus = c.String(maxLength: 20),
                        Remark = c.String(maxLength: 255),
                        Status = c.String(maxLength: 10),
                        DelStatus = c.Boolean(),
                        EntryBy = c.String(maxLength: 20),
                        EntryDate = c.DateTime(),
                        VenueFromId = c.String(nullable: false, maxLength: 20),
                        VenueToId = c.String(nullable: false, maxLength: 20),
                        SmsNo = c.String(nullable: false, maxLength: 25),
                        CountryId = c.String(nullable: false, maxLength: 5),
                        Finger = c.String(nullable: false, maxLength: 10),
                        Manpower = c.String(nullable: false, maxLength: 10),
                        TicketIssue = c.String(nullable: false, maxLength: 10),
                        FlightStatus = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CountryLists",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        CountryCode = c.String(nullable: false, maxLength: 2),
                        CountryName = c.String(nullable: false, maxLength: 100),
                        DelStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.CustomerPayments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchId = c.String(maxLength: 20),
                        CustomerId = c.String(maxLength: 20),
                        PaymentDate = c.DateTime(),
                        PaymentAmount = c.Double(),
                        MethodId = c.String(nullable: false, maxLength: 20),
                        Channel = c.String(maxLength: 10),
                        Status = c.String(maxLength: 10),
                        DelStatus = c.Int(nullable: false),
                        EntryBy = c.String(maxLength: 20),
                        EntryDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.EmployeeDesignations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DesignationId = c.String(nullable: false, maxLength: 20),
                        DesignationTitleEn = c.String(maxLength: 100),
                        DesignationTitleBn = c.String(maxLength: 100),
                        DesignationDepertment = c.String(maxLength: 20),
                        Status = c.String(maxLength: 10),
                        DelStatus = c.Int(nullable: false),
                        EntryDate = c.DateTime(),
                        EntryBy = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GeneralSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SettingName = c.String(nullable: false, maxLength: 20),
                        SettingValue = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Description = c.String(nullable: false, maxLength: 100),
                        Crm = c.String(nullable: false, maxLength: 5),
                        Billing = c.String(nullable: false, maxLength: 5),
                        Accounts = c.String(nullable: false, maxLength: 5),
                        Report = c.String(nullable: false, maxLength: 5),
                        Hrm = c.String(nullable: false, maxLength: 5),
                        Setup = c.String(nullable: false, maxLength: 5),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Uid = c.String(nullable: false, maxLength: 20),
                        GroupId = c.Int(nullable: false),
                        IpAddress = c.String(nullable: false, maxLength: 45),
                        Username = c.String(maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        CreatedOn = c.Long(nullable: false),
                        LastLogin = c.Long(),
                        Active = c.Boolean(),
                        EmployeeId = c.String(maxLength: 20),
                        BranchId = c.String(maxLength: 20),
                        Level = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LoginAttempts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IpAddress = c.String(nullable: false, maxLength: 15),
                        Login = c.String(nullable: false, maxLength: 100),
                        Time = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Metas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        BirthDate = c.DateTime(nullable: false, storeType: "date"),
                        Phone = c.String(maxLength: 20),
                        CreditGiven = c.String(nullable: false, maxLength: 10),
                        CreditAvailable = c.String(nullable: false, maxLength: 10),
                        Commands = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaymentMethods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MethodId = c.String(nullable: false, maxLength: 50),
                        MethodName = c.String(nullable: false, maxLength: 50),
                        CurrentValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SectorInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SectorId = c.String(nullable: false, maxLength: 20),
                        SectorName = c.String(maxLength: 100),
                        SectorCode = c.String(maxLength: 100),
                        Status = c.String(maxLength: 10),
                        DelStatus = c.Int(nullable: false),
                        EntryDate = c.DateTime(),
                        EntryBy = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServiceInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceId = c.String(nullable: false, maxLength: 20),
                        ServiceName = c.String(maxLength: 100),
                        Description = c.String(maxLength: 250),
                        Status = c.String(maxLength: 10),
                        DelStatus = c.Int(nullable: false),
                        EntryDate = c.DateTime(),
                        EntryBy = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SmsSentHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SmsSendId = c.String(maxLength: 50),
                        CustomerId = c.String(maxLength: 20),
                        MobileNumber = c.String(maxLength: 20),
                        SmsBody = c.String(maxLength: 250),
                        SendStatus = c.String(maxLength: 20),
                        Status = c.String(maxLength: 10),
                        DelStatus = c.Int(nullable: false),
                        EntryBy = c.String(maxLength: 20),
                        EntryDate = c.DateTime(),
                        CurrentTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SuppliersInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SupplierId = c.String(nullable: false, maxLength: 100),
                        SupplierName = c.String(nullable: false, maxLength: 100),
                        SupplierEmail = c.String(nullable: false, maxLength: 100),
                        SupplierPhone = c.String(nullable: false, maxLength: 50),
                        SupplierAddress = c.String(nullable: false, unicode: false, storeType: "text"),
                        SupplierMobileNo = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransactionsInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionId = c.String(nullable: false, maxLength: 10),
                        TransactionType = c.String(nullable: false, maxLength: 20),
                        Account = c.String(nullable: false, maxLength: 10),
                        AccountTo = c.String(maxLength: 10),
                        Date = c.DateTime(nullable: false),
                        PayerType = c.String(maxLength: 8),
                        PayerId = c.String(maxLength: 10),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MethodId = c.String(nullable: false, maxLength: 10),
                        Description = c.String(nullable: false, unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VenueInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VenueId = c.String(nullable: false, maxLength: 20),
                        VenueName = c.String(maxLength: 100),
                        VenueCode = c.String(maxLength: 100),
                        Status = c.String(maxLength: 10),
                        DelStatus = c.Int(nullable: false),
                        EntryDate = c.DateTime(),
                        EntryBy = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGroups", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserGroups", "GroupId", "dbo.Groups");
            DropIndex("dbo.UserGroups", new[] { "GroupId" });
            DropIndex("dbo.UserGroups", new[] { "UserId" });
            DropTable("dbo.VenueInfoes");
            DropTable("dbo.TransactionsInfoes");
            DropTable("dbo.SuppliersInfoes");
            DropTable("dbo.SmsSentHistories");
            DropTable("dbo.ServiceInfoes");
            DropTable("dbo.SectorInfoes");
            DropTable("dbo.PaymentMethods");
            DropTable("dbo.Metas");
            DropTable("dbo.LoginAttempts");
            DropTable("dbo.Users");
            DropTable("dbo.UserGroups");
            DropTable("dbo.Groups");
            DropTable("dbo.GeneralSettings");
            DropTable("dbo.EmployeeDesignations");
            DropTable("dbo.EmployeeBasicInfoes");
            DropTable("dbo.CustomerPayments");
            DropTable("dbo.CountryLists");
            DropTable("dbo.ClientInfoes");
            DropTable("dbo.BranchInfoes");
            DropTable("dbo.AirLineInfoes");
            DropTable("dbo.AgentInfoes");
        }
    }
}
