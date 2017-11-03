using System.IO;
using System.Reflection;

namespace App.Web.Migrations.ContextB
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class ConfigB : DbMigrationsConfiguration<App.Web.Context.CrmDbContext>
    {
        public ConfigB()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ContextB";
        }

        protected override void Seed(App.Web.Context.CrmDbContext context)
        {
            //var codebase = Assembly.GetExecutingAssembly().CodeBase;
            //var uri = new UriBuilder(codebase);
            //var path = Uri.UnescapeDataString(uri.Path);
            //var baseDir = Path.GetDirectoryName(path) + "\\Migrations\\ContextB\\V_TransactionInfo.sql";
            //context.Database.ExecuteSqlCommand("IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N\'[dbo].[V_TransactionInfo]\'))\t\r\nBEGIN\r\n\tDROP VIEW dbo.V_TransactionInfo\r\nEND\r\nGO\r\nCREATE VIEW V_TransactionInfo \r\nAS\r\nSELECT  \r\n\tti.Id, ti.TransactionId, \r\n\tCASE\r\n\t\tWHEN ti.TransactionType=1 THEN \'TRANSFER\'\r\n\t\tWHEN ti.TransactionType=2 THEN \'DEPOSIT\'\r\n\t\tWHEN ti.TransactionType=3 THEN \'EXPENSE\'\r\n\tEND TransactionType,\r\n\t(SELECT AccountName FROM BankAccounts WHERE Id=ti.AccountFrom) AccountFrom,\r\n\t(SELECT AccountName FROM BankAccounts WHERE Id=ti.AccountTo) AccountTo,\r\n\tti.Date,\r\n\t(SELECT MethodName FROM PaymentMethods WHERE Id=ti.MethodId) Payer,\r\n\tCASE WHEN ti.TransactionType=1 THEN ti.Amount ELSE 0.00 END TransferAmount,\r\n\tCASE WHEN ti.TransactionType=2 THEN ti.Amount ELSE 0.00 END DepositAmount,\r\n\tCASE WHEN ti.TransactionType=3 THEN ti.Amount ELSE 0.00 END ExpenseAmount,\r\n\tti.Description\r\nFROM TransactionsInfoes ti\r\n\t\r\n");


            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
