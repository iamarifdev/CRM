using System.Web.Mvc;
using System.Web.Routing;
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
            }
        }
    }
}