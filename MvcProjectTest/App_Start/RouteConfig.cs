using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcProjectTest
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "ErrorPage",
               url: "Shopping/ErrorPage/{error}",
               defaults: new { controller = "Shopping", action = "ErrorPage", error = MvcProjectTest.Services.ShoppingCartService.Error.otherError }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Search",
                url: "Book/{name}",
                defaults: new { controller = "Book", action = "Index", name = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "BookTypeSearch",
                url: "Book/{name}/{type}",
                defaults: new { controller = "Book", action = "Index", name = UrlParameter.Optional,types = UrlParameter.Optional }
            );
           
        }
    }
}
