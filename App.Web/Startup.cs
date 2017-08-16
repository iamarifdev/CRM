using System;
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
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login   
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
                        roleManager.Create(role);

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
                            CreatedOn = DateTime.Now.Ticks,
                            Active = Status.Active,
                            Level = "Head Office",
                            Uid = uId
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
    }
}
