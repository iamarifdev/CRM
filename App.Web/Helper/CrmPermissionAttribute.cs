using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using App.Entity.Models;
using App.Web.Models;

namespace App.Web.Helper
{
    public class CrmPermissionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext.HttpContext.Session.Get<AppData>("AppData") == null)
            {
                filterContext.HttpContext.Session.RemoveAll();
                filterContext.HttpContext.Session.Abandon();
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Account",
                    action = "Login"
                }));
                return;
            }

            var actionName = filterContext.ActionDescriptor.ActionName;
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var appData = filterContext.HttpContext.Session.Get<AppData>("AppData");

            if (string.Equals(controllerName, "Home", StringComparison.CurrentCultureIgnoreCase)) return;
            if (string.Equals(controllerName, "Navigation", StringComparison.CurrentCultureIgnoreCase)) return;
            if (!appData.MenuList.Any(x => 
                string.Equals(x.ActionName, actionName, StringComparison.CurrentCultureIgnoreCase) 
                && string.Equals(x.ControllerName, controllerName, StringComparison.CurrentCultureIgnoreCase) 
                && x.Status == Status.Active)
            )
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Error",
                    action = "Unauthorized"
                }));
            }
            
        }
    }
}