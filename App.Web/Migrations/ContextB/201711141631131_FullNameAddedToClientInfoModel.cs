namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FullNameAddedToClientInfoModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientInfoes", "FullName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientInfoes", "FullName");
        }
    }
}
