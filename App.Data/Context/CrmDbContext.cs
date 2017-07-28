using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entity.Models;

namespace App.Data.Context
{
    public class CrmDbContext : DbContext
    {
        public CrmDbContext() : base("CrmDbContext")
        {

        }
        public DbSet<AgentInfo> AgentInfos { get; set; }
        public DbSet<AirLineInfo> AirLineInfos { get; set; }
        public DbSet<BranchInfo> BranchInfos { get; set; }
        public DbSet<ClientInfo> ClientInfos { get; set; }
        public DbSet<CountryList> CountryLists { get; set; }
        public DbSet<CustomerPayment> CustomerPayments { get; set; }
        public DbSet<EmployeeBasicInfo> EmployeeBasicInfos { get; set; }
        public DbSet<EmployeeDesignation> EmployeeDesignations { get; set; }
        public DbSet<GeneralSetting> GeneralSettings { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<LoginAttempts> LoginAttemptses { get; set; }
        public DbSet<Meta> Metas { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<SectorInfo> SectorInfos { get; set; }
        public DbSet<ServiceInfo> ServiceInfos { get; set; }
        public DbSet<SmsSentHistory> SmsSentHistories { get; set; }
        public DbSet<SuppliersInfo> SuppliersInfos { get; set; }
        public DbSet<TransactionsInfo> TransactionsInfos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<VenueInfo> VenueInfos { get; set; }
    }
}
