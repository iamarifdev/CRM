using App.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace App.Web.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("CrmDbContext", throwIfV1Schema: false) { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}