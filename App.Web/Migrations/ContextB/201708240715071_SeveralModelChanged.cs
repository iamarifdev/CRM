namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeveralModelChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SmsSentHistories", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.SmsSentHistories", "DelStatus", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SmsSentHistories", "EntryBy", c => c.Int(nullable: false));
            AlterColumn("dbo.SmsSentHistories", "EntryDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.SmsSentHistories", "EntryBy");
            AddForeignKey("dbo.SmsSentHistories", "EntryBy", "dbo.Users", "Id", cascadeDelete: true);
            DropTable("dbo.VenueInfoes");
        }
        
        public override void Down()
        {
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
            
            DropForeignKey("dbo.SmsSentHistories", "EntryBy", "dbo.Users");
            DropIndex("dbo.SmsSentHistories", new[] { "EntryBy" });
            AlterColumn("dbo.SmsSentHistories", "EntryDate", c => c.DateTime());
            AlterColumn("dbo.SmsSentHistories", "EntryBy", c => c.String(maxLength: 20));
            AlterColumn("dbo.SmsSentHistories", "DelStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.SmsSentHistories", "Status", c => c.String(maxLength: 10));
        }
    }
}
