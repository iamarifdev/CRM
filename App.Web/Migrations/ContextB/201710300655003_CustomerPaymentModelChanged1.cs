namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerPaymentModelChanged1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CustomerPayments", "Channel", c => c.Int(nullable: false));
            AlterColumn("dbo.CustomerPayments", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CustomerPayments", "Status", c => c.String(maxLength: 10));
            AlterColumn("dbo.CustomerPayments", "Channel", c => c.String(maxLength: 10));
        }
    }
}
