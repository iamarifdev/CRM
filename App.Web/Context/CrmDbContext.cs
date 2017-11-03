using System.Data.Entity;
using System.Reflection;
using App.Entity.Models;

namespace App.Web.Context
{
    public class CrmDbContext : DbContext
    {
        public CrmDbContext() : base("CrmDbContext") { }
        public DbSet<AgentInfo> AgentInfos { get; set; }
        public DbSet<AirLineInfo> AirLineInfos { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BranchInfo> BranchInfos { get; set; }
        public DbSet<ClientInfo> ClientInfos { get; set; }
        public DbSet<CountryList> CountryLists { get; set; }
        public DbSet<CustomerPayment> CustomerPayments { get; set; }
        public DbSet<EmployeeBasicInfo> EmployeeBasicInfos { get; set; }
        public DbSet<EmployeeDesignation> EmployeeDesignations { get; set; }
        public DbSet<GeneralSetting> GeneralSettings { get; set; }
        public DbSet<LoginAttempts> LoginAttemptses { get; set; }
        public DbSet<Meta> Metas { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<SectorInfo> SectorInfos { get; set; }
        public DbSet<ServiceInfo> ServiceInfos { get; set; }
        public DbSet<SmsSentHistory> SmsSentHistories { get; set; }
        public DbSet<SuppliersInfo> SuppliersInfos { get; set; }
        public DbSet<TransactionsInfo> TransactionsInfos { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<TransactionView> TransactionView { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
