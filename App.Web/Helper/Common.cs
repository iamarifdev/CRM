using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using App.Entity.Models;
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

        public static string GetDisplayName<T>(this T enumValue) where T : IComparable, IFormattable, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Argument must be of type Enum");
            try
            {
                return enumValue.GetType() // GetType causes exception if DisplayAttribute.Name is not set
                                .GetMember(enumValue.ToString())
                                .First()
                                .GetCustomAttribute<DisplayAttribute>()
                                .GetName();
            }
            catch // If there's no DisplayAttribute.Name set, just return the ToString value
            {
                return enumValue.ToString();
            }
        }

        public static string GetEnumDisplayName(this Enum enumType)
        {
            return enumType.GetType().GetMember(enumType.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>()
                           .Name;
        }

        public static DisplayAttribute GetDisplayAttributesFrom(this Enum enumValue, Type enumType)
        {
            return enumType.GetMember(enumValue.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>();
        }

        public static SelectList ToSelectList<T>(object selectedvalue = null)
        {
            var t = typeof(T);
            if (!t.IsEnum) return null;
            var list = Enum.GetValues(t).Cast<T>().Select(v => new
                SelectListItem { Text = v.ToString(), Value = Convert.ToInt32(v).ToString() }).ToList();
            return selectedvalue == null ? new SelectList(list, "Value", "Text") : new SelectList(list, "Value", "Text", selectedvalue);
        }

        public static List<SelectListItem> StatusList
        {
            get
            {
                return Enum.GetValues(typeof(Status)).Cast<Status>().Select(v => new
                       SelectListItem { Text = v.ToString(), Value = ((int)v).ToString() }).ToList();
            }
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

        public static string HasPassword(string newPassword)
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
            //IN ORDER TO INVOKE SIGNALR FUNCTIONALITY DIRECTLY FROM SERVER SIDE WE MUST USE THIS
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();

            //CALCULATING PERCENTAGE BASED ON THE PARAMETERS SENT
            var percentage = (progressCount * 100) / totalItems;

            //PUSHING DATA TO ALL CLIENTS
            hubContext.Clients.All.AddProgress(progressMessage, percentage + "%");
        }
    }
}