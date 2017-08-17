namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientInfoModelChangedToDropUserDoneBy : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClientInfoes", "DoneBy", "dbo.Users");
            DropIndex("dbo.ClientInfoes", new[] { "DoneBy" });
            AlterColumn("dbo.ClientInfoes", "DoneBy", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClientInfoes", "DoneBy", c => c.Int());
            CreateIndex("dbo.ClientInfoes", "DoneBy");
            AddForeignKey("dbo.ClientInfoes", "DoneBy", "dbo.Users", "Id");
        }
    }
}
