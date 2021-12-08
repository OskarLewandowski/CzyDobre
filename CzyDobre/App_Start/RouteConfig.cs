﻿using System;
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
                name: "Default",
                url: "{controller}/{action}/{id}/{filtr1}/{filtr2}/filtr{3}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, filtr1 = UrlParameter.Optional, filtr2 =UrlParameter.Optional, filtr3=UrlParameter.Optional}
            );
        }
    }
}
