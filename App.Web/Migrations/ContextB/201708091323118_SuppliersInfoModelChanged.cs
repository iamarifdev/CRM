namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SuppliersInfoModelChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SuppliersInfoes", "DelStatus", c => c.Boolean(nullable: false));
            AddColumn("dbo.SuppliersInfoes", "EntryDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SuppliersInfoes", "EntryBy", c => c.Int(nullable: false));
            AlterColumn("dbo.SuppliersInfoes", "SupplierEmail", c => c.String(maxLength: 100));
            AlterColumn("dbo.SuppliersInfoes", "SupplierPhone", c => c.String(maxLength: 50));
            AlterColumn("dbo.SuppliersInfoes", "SupplierAddress", c => c.String(maxLength: 250));
            CreateIndex("dbo.SuppliersInfoes", "EntryBy");
            AddForeignKey("dbo.SuppliersInfoes", "EntryBy", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SuppliersInfoes", "EntryBy", "dbo.Users");
            DropIndex("dbo.SuppliersInfoes", new[] { "EntryBy" });
            AlterColumn("dbo.SuppliersInfoes", "SupplierAddress", c => c.String(nullable: false, unicode: false, storeType: "text"));
            AlterColumn("dbo.SuppliersInfoes", "SupplierPhone", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.SuppliersInfoes", "SupplierEmail", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.SuppliersInfoes", "EntryBy");
            DropColumn("dbo.SuppliersInfoes", "EntryDate");
            DropColumn("dbo.SuppliersInfoes", "DelStatus");
        }
    }
}
