namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoneByAsDateTimeAddedToClientInfoModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClientInfoes", "DoneBy", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClientInfoes", "DoneBy", c => c.String(maxLength: 100));
        }
    }
}
