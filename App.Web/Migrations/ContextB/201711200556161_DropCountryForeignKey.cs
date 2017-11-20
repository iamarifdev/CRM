namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropCountryForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClientInfoes", "CountryId", "dbo.CountryLists");
            DropIndex("dbo.ClientInfoes", new[] { "CountryId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.ClientInfoes", "CountryId");
            AddForeignKey("dbo.ClientInfoes", "CountryId", "dbo.CountryLists", "CountryId");
        }
    }
}
