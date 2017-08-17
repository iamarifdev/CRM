namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupplierIdMadeNullableToClientInfoModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClientInfoes", "SupplierId", "dbo.SuppliersInfoes");
            DropIndex("dbo.ClientInfoes", new[] { "SupplierId" });
            AlterColumn("dbo.ClientInfoes", "SupplierId", c => c.Int());
            CreateIndex("dbo.ClientInfoes", "SupplierId");
            AddForeignKey("dbo.ClientInfoes", "SupplierId", "dbo.SuppliersInfoes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientInfoes", "SupplierId", "dbo.SuppliersInfoes");
            DropIndex("dbo.ClientInfoes", new[] { "SupplierId" });
            AlterColumn("dbo.ClientInfoes", "SupplierId", c => c.Int(nullable: false));
            CreateIndex("dbo.ClientInfoes", "SupplierId");
            AddForeignKey("dbo.ClientInfoes", "SupplierId", "dbo.SuppliersInfoes", "Id", cascadeDelete: true);
        }
    }
}
