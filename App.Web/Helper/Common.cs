using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using App.Web.Context;
using App.Web.Hubs;
using App.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;

namespace App.Web.Helper
{
    public static class Common
    {
        public static string GetUserName(this int id)
        {
            using (var db = new CrmDbContext())
            {
                return db.Users.First(u => u.Id == id).UserName;
            }
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            return enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<TAttribute>();
        }

        public static string GetDescription(Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null) return null;
            var field = type.GetField(name);
            if (field == null) return null;
            var attr = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;
            return attr != null ? attr.Name : name;
        }


        public static SelectList ToSelectList<T>(object selectedvalue = null)
        {
            var t = typeof(T);
            if (!t.IsEnum) return null;

            var list = new List<SelectListItem>();

            foreach (T obj in Enum.GetValues(t))
            {
                var enumType = Enum.Parse(typeof(T),obj.ToString()) as Enum;
                if (enumType == null) return null;
                var text = GetDescription(enumType);
                var value = Convert.ToInt32(enumType).ToString();
                list.Add(new SelectListItem { Text = text, Value = value });
            }

            return selectedvalue == null ? new SelectList(list, "Value", "Text") : new SelectList(list, "Value", "Text", selectedvalue);
        }
        
        public static bool ChangePassword(ApplicationUser user, string newPassword)
        {
            using (var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>())
            {
                var token = userManager.GeneratePasswordResetToken(user.Id);
                var result = userManager.ResetPassword(user.Id, token, newPassword);

                return result.Succeeded;
            }
        }

        public static string HashPassword(string newPassword)
        {
            using (var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>()))
            {
                return userManager.PasswordHasher.HashPassword(newPassword);
            }
        }

        public static string RenderRazorViewToString(this Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public static void SendProgress(string progressMessage, int progressCount, int totalItems)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
            var percentage = (progressCount * 100) / totalItems;
            hubContext.Clients.All.AddProgress(progressMessage, percentage + "%");
        }

        public static string NullDateToString(this DateTime? date)
        {
            return date == null ? "" : string.Format("{0:yyyy-MM-dd}", date);
        }

        public static string QueryToDateString(DateTime date)
        {
            return SqlFunctions.DateName("year", date) + "-" +
                   SqlFunctions.Replicate("0", 2 - SqlFunctions.StringConvert((double) date.Month).TrimStart().Length) +
                   SqlFunctions.StringConvert((double) date.Month).TrimStart() + "-" +
                   SqlFunctions.Replicate("0", 2 - SqlFunctions.DateName("dd", date).Trim().Length) +
                   SqlFunctions.DateName("dd", date).Trim();
        }
    }
}