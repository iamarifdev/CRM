namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CountryModelChangedForCountryCodeLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Countries", "CountryCode", c => c.String(nullable: false, maxLength: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Countries", "CountryCode", c => c.String(nullable: false, maxLength: 2));
        }
    }
}
