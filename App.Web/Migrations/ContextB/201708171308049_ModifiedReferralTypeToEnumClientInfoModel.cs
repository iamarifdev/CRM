namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedReferralTypeToEnumClientInfoModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClientInfoes", "ReferralType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClientInfoes", "ReferralType", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
