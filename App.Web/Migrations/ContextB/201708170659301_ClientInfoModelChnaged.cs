namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientInfoModelChnaged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClientInfoes", "CustomerId", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.ClientInfoes", "BranchId", c => c.Int(nullable: false));
            AlterColumn("dbo.ClientInfoes", "Sn", c => c.String(maxLength: 255));
            AlterColumn("dbo.ClientInfoes", "ReferralType", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.ClientInfoes", "AgentId", c => c.Int());
            AlterColumn("dbo.ClientInfoes", "SupplierId", c => c.Int(nullable: false));
            AlterColumn("dbo.ClientInfoes", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.ClientInfoes", "ServiceId", c => c.Int(nullable: false));
            AlterColumn("dbo.ClientInfoes", "AirLineId", c => c.Int(nullable: false));
            AlterColumn("dbo.ClientInfoes", "CollageName", c => c.String(maxLength: 150));
            AlterColumn("dbo.ClientInfoes", "CourseName", c => c.String(maxLength: 150));
            AlterColumn("dbo.ClientInfoes", "EmailAddress", c => c.String(maxLength: 100));
            AlterColumn("dbo.ClientInfoes", "ServedBy", c => c.Int());
            AlterColumn("dbo.ClientInfoes", "DoneBy", c => c.Int());
            AlterColumn("dbo.ClientInfoes", "WorkingStatus", c => c.Int());
            AlterColumn("dbo.ClientInfoes", "DeliveryStatus", c => c.Int());
            AlterColumn("dbo.ClientInfoes", "InfoStatus", c => c.Int());
            AlterColumn("dbo.ClientInfoes", "Status", c => c.Int());
            AlterColumn("dbo.ClientInfoes", "EntryBy", c => c.Int(nullable: false));
            AlterColumn("dbo.ClientInfoes", "EntryDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ClientInfoes", "VenueFromId", c => c.Int());
            AlterColumn("dbo.ClientInfoes", "VenueToId", c => c.Int());
            AlterColumn("dbo.ClientInfoes", "CountryId", c => c.Int());
            AlterColumn("dbo.ClientInfoes", "Finger", c => c.String(maxLength: 10));
            AlterColumn("dbo.ClientInfoes", "Manpower", c => c.String(maxLength: 10));
            AlterColumn("dbo.ClientInfoes", "TicketIssue", c => c.String(maxLength: 10));
            AlterColumn("dbo.ClientInfoes", "FlightStatus", c => c.String(maxLength: 10));
            CreateIndex("dbo.ClientInfoes", "AgentId");
            CreateIndex("dbo.ClientInfoes", "SupplierId");
            CreateIndex("dbo.ClientInfoes", "ServiceId");
            CreateIndex("dbo.ClientInfoes", "AirLineId");
            CreateIndex("dbo.ClientInfoes", "ServedBy");
            CreateIndex("dbo.ClientInfoes", "DoneBy");
            CreateIndex("dbo.ClientInfoes", "EntryBy");
            CreateIndex("dbo.ClientInfoes", "VenueFromId");
            CreateIndex("dbo.ClientInfoes", "VenueToId");
            CreateIndex("dbo.ClientInfoes", "CountryId");
            AddForeignKey("dbo.ClientInfoes", "AgentId", "dbo.AgentInfoes", "Id");
            AddForeignKey("dbo.ClientInfoes", "AirLineId", "dbo.AirLineInfoes", "Id", cascadeDelete: false);
            AddForeignKey("dbo.ClientInfoes", "CountryId", "dbo.CountryLists", "CountryId");
            AddForeignKey("dbo.ClientInfoes", "VenueFromId", "dbo.SectorInfoes", "Id");
            AddForeignKey("dbo.ClientInfoes", "VenueToId", "dbo.SectorInfoes", "Id");
            AddForeignKey("dbo.ClientInfoes", "ServiceId", "dbo.ServiceInfoes", "Id", cascadeDelete: false);
            AddForeignKey("dbo.ClientInfoes", "SupplierId", "dbo.SuppliersInfoes", "Id", cascadeDelete: false);
            AddForeignKey("dbo.ClientInfoes", "DoneBy", "dbo.Users", "Id");
            AddForeignKey("dbo.ClientInfoes", "EntryBy", "dbo.Users", "Id", cascadeDelete: false);
            AddForeignKey("dbo.ClientInfoes", "ServedBy", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientInfoes", "ServedBy", "dbo.Users");
            DropForeignKey("dbo.ClientInfoes", "EntryBy", "dbo.Users");
            DropForeignKey("dbo.ClientInfoes", "DoneBy", "dbo.Users");
            DropForeignKey("dbo.ClientInfoes", "SupplierId", "dbo.SuppliersInfoes");
            DropForeignKey("dbo.ClientInfoes", "ServiceId", "dbo.ServiceInfoes");
            DropForeignKey("dbo.ClientInfoes", "VenueToId", "dbo.SectorInfoes");
            DropForeignKey("dbo.ClientInfoes", "VenueFromId", "dbo.SectorInfoes");
            DropForeignKey("dbo.ClientInfoes", "CountryId", "dbo.CountryLists");
            DropForeignKey("dbo.ClientInfoes", "AirLineId", "dbo.AirLineInfoes");
            DropForeignKey("dbo.ClientInfoes", "AgentId", "dbo.AgentInfoes");
            DropIndex("dbo.ClientInfoes", new[] { "CountryId" });
            DropIndex("dbo.ClientInfoes", new[] { "VenueToId" });
            DropIndex("dbo.ClientInfoes", new[] { "VenueFromId" });
            DropIndex("dbo.ClientInfoes", new[] { "EntryBy" });
            DropIndex("dbo.ClientInfoes", new[] { "DoneBy" });
            DropIndex("dbo.ClientInfoes", new[] { "ServedBy" });
            DropIndex("dbo.ClientInfoes", new[] { "AirLineId" });
            DropIndex("dbo.ClientInfoes", new[] { "ServiceId" });
            DropIndex("dbo.ClientInfoes", new[] { "SupplierId" });
            DropIndex("dbo.ClientInfoes", new[] { "AgentId" });
            AlterColumn("dbo.ClientInfoes", "FlightStatus", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.ClientInfoes", "TicketIssue", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.ClientInfoes", "Manpower", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.ClientInfoes", "Finger", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.ClientInfoes", "CountryId", c => c.String(nullable: false, maxLength: 5));
            AlterColumn("dbo.ClientInfoes", "VenueToId", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.ClientInfoes", "VenueFromId", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.ClientInfoes", "EntryDate", c => c.DateTime());
            AlterColumn("dbo.ClientInfoes", "EntryBy", c => c.String(maxLength: 20));
            AlterColumn("dbo.ClientInfoes", "Status", c => c.String(maxLength: 10));
            AlterColumn("dbo.ClientInfoes", "InfoStatus", c => c.String(maxLength: 20));
            AlterColumn("dbo.ClientInfoes", "DeliveryStatus", c => c.String(maxLength: 20));
            AlterColumn("dbo.ClientInfoes", "WorkingStatus", c => c.String(maxLength: 20));
            AlterColumn("dbo.ClientInfoes", "DoneBy", c => c.String(maxLength: 100));
            AlterColumn("dbo.ClientInfoes", "ServedBy", c => c.String(maxLength: 20));
            AlterColumn("dbo.ClientInfoes", "EmailAddress", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.ClientInfoes", "CourseName", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.ClientInfoes", "CollageName", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.ClientInfoes", "AirLineId", c => c.String(maxLength: 20));
            AlterColumn("dbo.ClientInfoes", "ServiceId", c => c.String(maxLength: 20));
            AlterColumn("dbo.ClientInfoes", "FirstName", c => c.String(maxLength: 50));
            AlterColumn("dbo.ClientInfoes", "SupplierId", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.ClientInfoes", "AgentId", c => c.String(maxLength: 20));
            AlterColumn("dbo.ClientInfoes", "ReferralType", c => c.String(maxLength: 10));
            AlterColumn("dbo.ClientInfoes", "Sn", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.ClientInfoes", "BranchId", c => c.String(maxLength: 20));
            AlterColumn("dbo.ClientInfoes", "CustomerId", c => c.String(maxLength: 20));
        }
    }
}
