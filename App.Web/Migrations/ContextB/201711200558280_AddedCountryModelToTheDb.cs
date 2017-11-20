namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCountryModelToTheDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        CountryCode = c.String(nullable: false, maxLength: 2),
                        CountryName = c.String(nullable: false, maxLength: 100),
                        DelStatus = c.Boolean(nullable: false),
                        EntryDate = c.DateTime(nullable: false),
                        EntryBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CountryId)
                .ForeignKey("dbo.Users", t => t.EntryBy, cascadeDelete: true)
                .Index(t => t.EntryBy);
            
            CreateIndex("dbo.ClientInfoes", "CountryId");
            AddForeignKey("dbo.ClientInfoes", "CountryId", "dbo.Countries", "CountryId");
            DropTable("dbo.CountryLists");
        }
        
        public override void Down()
        {
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
            
            DropForeignKey("dbo.ClientInfoes", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Countries", "EntryBy", "dbo.Users");
            DropIndex("dbo.Countries", new[] { "EntryBy" });
            DropIndex("dbo.ClientInfoes", new[] { "CountryId" });
            DropTable("dbo.Countries");
        }
    }
}
