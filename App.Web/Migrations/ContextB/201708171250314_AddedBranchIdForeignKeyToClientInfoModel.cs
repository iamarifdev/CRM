namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBranchIdForeignKeyToClientInfoModel : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ClientInfoes", "BranchId");
            AddForeignKey("dbo.ClientInfoes", "BranchId", "dbo.BranchInfoes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientInfoes", "BranchId", "dbo.BranchInfoes");
            DropIndex("dbo.ClientInfoes", new[] { "BranchId" });
        }
    }
}
