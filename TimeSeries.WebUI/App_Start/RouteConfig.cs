using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TimeSeries.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Index",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "About",
                url: "about",
                defaults: new { controller = "Home", action = "About" }
            );
            
            routes.MapRoute(
                name: "TimeSerie_List",
                url: "TimeSerie",
                defaults: new { controller = "TimeSerie", action = "List" }
            );
            routes.MapRoute(
                name: "TimeSerie_Visualize",
                url: "TimeSerie/Visualize",
                defaults: new { controller = "TimeSerie", action = "Visualize" }
            );

            routes.MapRoute(
                name: "TimeSerie_Create",
                url: "TimeSerie/Create",
                defaults: new { controller = "TimeSerie", action = "Create" }
            );

            routes.MapRoute(
                name: "TimeSerie_Phase",
                url: "TimeSerie/Phase/{timeSerieId}",
                defaults: new { controller = "TimeSerie", action = "Phase", timeSerieId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "TimeSerie_Edit",
                url: "TimeSerie/Edit/{timeSerieId}",
                defaults: new { controller = "TimeSerie", action = "Edit", timeSerieId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "TimeSerie_Delete",
                url: "TimeSerie/Delete/{timeSerieId}",
                defaults: new { controller = "TimeSerie", action = "Delete", timeSerieId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "TimeSerie_DownloadFile",
                url: "TimeSerie/DownloadFile/{data}",
                defaults: new { controller = "TimeSerie", action = "DownloadFile", data = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Login",
                url: "Account/Login/{returnUrl}",
                defaults: new { controller = "Account", action = "Login", returnUrl = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Register",
                url: "Account/Register",
                defaults: new { controller = "Account", action = "Register" }
            );

            routes.MapRoute(
                name: "LogOut",
                url: "Account/LogOut",
                defaults: new { controller = "Account", action = "LogOut" }
            );

            routes.MapRoute(
                name: "404-catch-all",
                url: "{*catchall}",
                defaults: new { controller = "Error", action = "NotFound" }
            );
        }
    }
}
