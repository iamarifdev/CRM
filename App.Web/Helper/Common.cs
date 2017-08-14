using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using App.Entity.Models;
using App.Web.Context;
using Owin;
using Microsoft.Owin;
using System.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace App.Web.Helper
{
    public static class Common
    {
        public static string GetUserName(this int id)
        {
            using (var db = new CrmDbContext())
            {
                return db.Users.First(u => u.Id == id).Username;
            }
        }

        public static List<SelectListItem> StatusList
        {
            get
            {
                return Enum.GetValues(typeof(Status)).Cast<Status>().Select(v => new
                       SelectListItem { Text = v.ToString(), Value = ((int)v).ToString() }).ToList();
            }
        }

        public static bool ChangePassword(string newPassword)
        {
            using (var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>()))
            {
                var httpContext = HttpContext.Current;
                var userId = httpContext.User.Identity.GetUserId();
                var token = userManager.GeneratePasswordResetToken(userId);
                var result = userManager.ResetPassword(userId, token, newPassword);

                return result.Succeeded;
            }
        }

        public static string HasPassword(string newPassword)
        {
            using (var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>()))
            {
                return userManager.PasswordHasher.HashPassword(newPassword);
            }
        }
    }
}