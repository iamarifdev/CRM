namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserTypeAddedToCustomerPaymentModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerPayments", "UserType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerPayments", "UserType");
        }
    }
}
