namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BankAccountModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.String(nullable: false),
                        AccountName = c.String(nullable: false, maxLength: 100),
                        AccountNumber = c.String(maxLength: 50),
                        BankName = c.String(maxLength: 50),
                        BranchName = c.String(maxLength: 50),
                        Status = c.Int(nullable: false),
                        DelStatus = c.Boolean(nullable: false),
                        EntryDate = c.DateTime(nullable: false),
                        EntryBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.EntryBy, cascadeDelete: true)
                .Index(t => t.EntryBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BankAccounts", "EntryBy", "dbo.Users");
            DropIndex("dbo.BankAccounts", new[] { "EntryBy" });
            DropTable("dbo.BankAccounts");
        }
    }
}
