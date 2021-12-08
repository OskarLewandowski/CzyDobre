using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CzyDobre
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
               name: "Opinion",
               url: "{controller}/{action}/{id}/{filter1}/{filter2}/filter{3}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, filter1 = UrlParameter.Optional, filter2 = UrlParameter.Optional, filter3 = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional}
            );
        }
    }
}
