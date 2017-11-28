namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CoutryIdForeignKeyAddedToClientInfo : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ClientInfoes", "CountryId");
            AddForeignKey("dbo.ClientInfoes", "CountryId", "dbo.Countries", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientInfoes", "CountryId", "dbo.Countries");
            DropIndex("dbo.ClientInfoes", new[] { "CountryId" });
        }
    }
}
