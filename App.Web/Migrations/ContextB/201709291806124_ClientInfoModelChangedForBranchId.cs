namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientInfoModelChangedForBranchId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClientInfoes", "BranchId", "dbo.BranchInfoes");
            DropIndex("dbo.ClientInfoes", new[] { "BranchId" });
            AlterColumn("dbo.ClientInfoes", "BranchId", c => c.Int());
            CreateIndex("dbo.ClientInfoes", "BranchId");
            AddForeignKey("dbo.ClientInfoes", "BranchId", "dbo.BranchInfoes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientInfoes", "BranchId", "dbo.BranchInfoes");
            DropIndex("dbo.ClientInfoes", new[] { "BranchId" });
            AlterColumn("dbo.ClientInfoes", "BranchId", c => c.Int(nullable: false));
            CreateIndex("dbo.ClientInfoes", "BranchId");
            AddForeignKey("dbo.ClientInfoes", "BranchId", "dbo.BranchInfoes", "Id", cascadeDelete: true);
        }
    }
}
