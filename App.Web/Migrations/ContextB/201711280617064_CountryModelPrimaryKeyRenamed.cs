namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CountryModelPrimaryKeyRenamed : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClientInfoes", "CountryId", "dbo.Countries");
            DropIndex("dbo.ClientInfoes", new[] { "CountryId" });
            RenameColumn("dbo.Countries", "CountryId", "Id");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.Countries", "Id", "CountryId");
            CreateIndex("dbo.ClientInfoes", "CountryId");
            AddForeignKey("dbo.ClientInfoes", "CountryId", "dbo.Countries", "CountryId");
        }
    }
}
