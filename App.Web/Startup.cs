using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using App.Web.Context;
using App.Entity.Models;
using App.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(App.Web.Startup))]

namespace App.Web
{
    public partial class Startup
    {
        private void CreateRolesandUsers()
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var context = new ApplicationDbContext();

                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var db = new CrmDbContext();


                    // In Startup iam creating first Admin Role and creating a default Admin User    
                    if (!roleManager.RoleExists("Admin"))
                    {

                        // first we create Admin role   
                        var role = new IdentityRole { Name = "Admin" };
                        var ack = roleManager.Create(role);
                        var group = new Group();
                        if (ack.Succeeded)
                        {
                            group = new Group
                            {
                                Name = "Admin",
                                Description = "Administrator",
                                Account = Flag.IsOn,
                                Billing = Flag.IsOn,
                                Crm = Flag.IsOn,
                                Hrm = Flag.IsOn,
                                Report = Flag.IsOn,
                                Setup = Flag.IsOn
                            };
                            db.Groups.Add(group);
                            db.SaveChanges();
                        }

                        //Here we create a Admin super user who will maintain the website                  

                        const string userName = "admin";
                        const string email = "arif.basis.net@gmail.com";
                        var uId = string.Format("UI-{0:000000}", db.Users.Count() + 1);

                        var user = new ApplicationUser
                        {
                            UserName = userName,
                            Email = email
                        };
                        var crmUser = new User
                        {
                            UserName = userName,
                            Email = email,
                            IpAddress = "127.0.0.1",
                            CreatedOn = DateTime.Now,
                            Status = Status.Active,
                            Level = UserLevel.HeadOffice,
                            Uid = uId,
                            GroupId = group.Id
                        };

                        const string userPwd = "112233";

                        var checkUser = userManager.Create(user, userPwd);
                        db.Users.Add(crmUser);
                        var crmCheckUser = db.SaveChanges() > 0;

                        //Add default User to Role Admin   
                        if (checkUser.Succeeded && crmCheckUser)
                        {
                            userManager.AddToRole(user.Id, "Admin");
                        }
                    }

                    // creating Creating Manager role    
                    if (!roleManager.RoleExists("Agent"))
                    {
                        var role = new IdentityRole { Name = "Agent" };
                        roleManager.Create(role);
                    }

                    scope.Complete();
                }
                catch (Exception)
                {
                    Transaction.Current.Rollback();
                    throw;
                }

            }

        }
        private static void AddServices()
        {
            var db = new CrmDbContext();
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var serviceList = new List<ServiceInfo>
                    {
                        new ServiceInfo{ServiceId = "DI-000001",ServiceName = "CONFIRM", Status = Status.Active, EntryDate = DateTime.Now, EntryBy = 1},
                        new ServiceInfo{ServiceId = "DI-000002",ServiceName = "DATE CHANGE", Status = Status.Active, EntryDate = DateTime.Now, EntryBy = 1},
                        new ServiceInfo{ServiceId = "DI-000003",ServiceName = "VISA CHECK", Status = Status.Active, EntryDate = DateTime.Now, EntryBy = 1},
                        new ServiceInfo{ServiceId = "DI-000004",ServiceName = "OTHERS", Status = Status.Active, EntryDate = DateTime.Now, EntryBy = 1},
                        new ServiceInfo{ServiceId = "DI-000005",ServiceName = "FORM FILLUP", Status = Status.Active, EntryDate = DateTime.Now, EntryBy = 1},
                        new ServiceInfo{ServiceId = "DI-000006",ServiceName = "E-MAIL", Status = Status.Active, EntryDate = DateTime.Now, EntryBy = 1},
                        new ServiceInfo{ServiceId = "DI-000007",ServiceName = "STUDENT VISA", Status = Status.Active, EntryDate = DateTime.Now, EntryBy = 1},
                        new ServiceInfo{ServiceId = "DI-000008",ServiceName = "TOURIST VISA", Status = Status.Active, EntryDate = DateTime.Now, EntryBy = 1},
                        new ServiceInfo{ServiceId = "DI-000009",ServiceName = "TKT+MP", Status = Status.Active, EntryDate = DateTime.Now, EntryBy = 1},
                        new ServiceInfo{ServiceId = "DI-000010",ServiceName = "NEW TICKET", Status = Status.Active, EntryDate = DateTime.Now, EntryBy = 1},
                        new ServiceInfo{ServiceId = "DI-000011",ServiceName = "WP VISA", Status = Status.Active, EntryDate = DateTime.Now, EntryBy = 1},
                        new ServiceInfo{ServiceId = "DI-000012",ServiceName = "MANPOWER", Status = Status.Active, EntryDate = DateTime.Now, EntryBy = 1},
                        new ServiceInfo{ServiceId = "DI-000013",ServiceName = "RE-CONFIRM", Status = Status.Active, EntryDate = DateTime.Now, EntryBy = 1}
                    };

                    foreach (var service in serviceList)
                    {
                        if (db.ServiceInfos.Any(x => x.ServiceId == service.ServiceId)) continue;
                        db.ServiceInfos.Add(service);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }
        private static void AddDefaultBranch()
        {
            var db = new CrmDbContext();
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (!db.BranchInfos.Any(x => x.BranchName == "Head" && x.BranchCode == "main"))
                    {
                        db.BranchInfos.Add(new BranchInfo
                        {
                            BranchId = "BI-000001",
                            BranchName = "Head",
                            BranchCode = "main",
                            Status = Status.Active,
                            EntryBy = 1,
                            EntryDate = DateTime.Now
                        });
                        db.SaveChanges();
                        transaction.Commit();
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
                
            }
        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
            AddServices();
            AddDefaultBranch();
            app.MapSignalR();
        }

        // In this method we will create default User roles and Admin user for login   


    }
}
