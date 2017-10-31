namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionsInfoModelChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransactionsInfoes", "AccountFrom", c => c.Int());
            AlterColumn("dbo.TransactionsInfoes", "TransactionType", c => c.Int(nullable: false));
            AlterColumn("dbo.TransactionsInfoes", "AccountTo", c => c.Int());
            AlterColumn("dbo.TransactionsInfoes", "Date", c => c.DateTime());
            AlterColumn("dbo.TransactionsInfoes", "PayerType", c => c.Int(nullable: false));
            AlterColumn("dbo.TransactionsInfoes", "PayerId", c => c.Int());
            AlterColumn("dbo.TransactionsInfoes", "MethodId", c => c.Int(nullable: false));
            CreateIndex("dbo.TransactionsInfoes", "AccountFrom");
            CreateIndex("dbo.TransactionsInfoes", "AccountTo");
            AddForeignKey("dbo.TransactionsInfoes", "AccountFrom", "dbo.BankAccounts", "Id");
            AddForeignKey("dbo.TransactionsInfoes", "AccountTo", "dbo.BankAccounts", "Id");
            DropColumn("dbo.TransactionsInfoes", "Account");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TransactionsInfoes", "Account", c => c.String(nullable: false, maxLength: 10));
            DropForeignKey("dbo.TransactionsInfoes", "AccountTo", "dbo.BankAccounts");
            DropForeignKey("dbo.TransactionsInfoes", "AccountFrom", "dbo.BankAccounts");
            DropIndex("dbo.TransactionsInfoes", new[] { "AccountTo" });
            DropIndex("dbo.TransactionsInfoes", new[] { "AccountFrom" });
            AlterColumn("dbo.TransactionsInfoes", "MethodId", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.TransactionsInfoes", "PayerId", c => c.String(maxLength: 10));
            AlterColumn("dbo.TransactionsInfoes", "PayerType", c => c.String(maxLength: 8));
            AlterColumn("dbo.TransactionsInfoes", "Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TransactionsInfoes", "AccountTo", c => c.String(maxLength: 10));
            AlterColumn("dbo.TransactionsInfoes", "TransactionType", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.TransactionsInfoes", "AccountFrom");
        }
    }
}
