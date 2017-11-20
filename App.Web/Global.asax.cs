using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using App.Web.Controllers;
using App.Web.Helper;

namespace App.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.DefaultBinder = new TrimModelBinder();
        }

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    var ex = Server.GetLastError().GetBaseException();
        //    var httpContext = ((MvcApplication)sender).Context;

        //    var routeData = new RouteData();
        //    routeData.Values.Add("controller", "Error");

        //    if (ex.GetType() == typeof(HttpException))
        //    {
        //        var httpException = (HttpException)ex;
        //        var code = httpException.GetHttpCode();
        //        switch (code)
        //        {
        //            case 400:
        //                routeData.Values.Add("action", "BadRequest");
        //                break;

        //            case 401:
        //                routeData.Values.Add("action", "Unauthorized");
        //                break;

        //            case 403:
        //                routeData.Values.Add("action", "Forbidden");
        //                break;

        //            case 404:
        //                routeData.Values.Add("action", "NotFound");
        //                break;

        //            default:
        //                routeData.Values.Add("action", "Unknown");
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        routeData.Values.Add("action", "InternalServerError");
        //    }
        //    //routeData.Values.Add("error", ex);
        //    httpContext.ClearError();
        //    httpContext.Response.Clear();
        //    httpContext.Response.ContentType = "text/html";
        //    IController controller = new ErrorController();
        //    controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        //}

        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;
            var currentController = " ";
            var currentAction = " ";
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !string.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null && !string.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            var ex = Server.GetLastError();
            var controller = new ErrorController();
            var routeData = new RouteData();
            var action = "Index";

            if (ex is HttpException)
            {
                var httpEx = ex as HttpException;

                switch (httpEx.GetHttpCode())
                {
                    case 400:
                        action = "BadRequest";
                        break;

                    case 401:
                        action = "Unauthorized";
                        break;

                    case 403:
                        action = "Forbidden";
                        break;

                    case 404:
                        action = "NotFound";
                        break;

                    default:
                        action = "Unknown";
                        break;
                }
            }

            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.ContentType = "text/html";
            var exception = ex as HttpException;
            httpContext.Response.StatusCode = exception != null ? exception.GetHttpCode() : 500;
            if (exception == null) action = "InternalServerError";
            httpContext.Response.TrySkipIisCustomErrors = true;

            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;

            controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //if (Context.Response.StatusCode == 401 || Context.Response.StatusCode == 403)
            //{
            //    // this is important, because the 401 is not an error by default!!!
            //    throw new HttpException(401, "You are not authorised");
            //}
        }
    }
}
