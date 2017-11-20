using System.Web.Mvc;
using System.Web.Routing;

namespace App.Web.Helper
{
    public class CrmAuthorizeAttribute : AuthorizeAttribute
    {
        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    //if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
        //    //{
        //    //    filterContext.Result = new RedirectResult("~/Account/Login");
        //    //    return;
        //    //}

        //    if (filterContext.HttpContext.User.Identity.IsAuthenticated && filterContext.Result is HttpUnauthorizedResult)
        //    {
        //        filterContext.Result = new RedirectToRouteResult(
        //            new RouteValueDictionary 
        //            {
        //                { "action", "Index" },
        //                { "controller", "Error" },
        //                { "status", 401 }
        //            });
        //    }
        //    base.OnAuthorization(filterContext);
        //}

        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (context.HttpContext.Request.IsAuthenticated)
            {
                context.Result = new ViewResult
                {
                    ViewName = "~/Views/Error/Unauthorized.cshtml"
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(context);
            }
        }
    }
}