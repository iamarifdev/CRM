namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SmsNoToNullableClientInfoModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClientInfoes", "SmsNo", c => c.String(maxLength: 25));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClientInfoes", "SmsNo", c => c.String(nullable: false, maxLength: 25));
        }
    }
}
