﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SignalRSever
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RouteTable.Routes.MapHttpRoute(
                    name: "QueueApi",
                    routeTemplate: "api/{controller}/{action}/id",
                    defaults: new { id = System.Web.Http.RouteParameter.Optional }
                    );

            RouteTable.Routes.MapHttpRoute(
                     name: "DefaultApi",
                     routeTemplate: "api/{controller}/{id}",
                     defaults: new { id = System.Web.Http.RouteParameter.Optional }
                     );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
