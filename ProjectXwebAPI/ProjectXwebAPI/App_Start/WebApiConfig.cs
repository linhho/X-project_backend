using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;

namespace ProjectXwebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "RangeApi",
                routeTemplate: "api/{controller}/range/{start}/{end}"
            );

            config.Routes.MapHttpRoute(
                name: "UserApi",
                routeTemplate: "api/{controller}/user/{userId}/{start}/{end}"
            );

            config.Routes.MapHttpRoute(
                name: "RankingApi",
                routeTemplate: "api/{controller}/rank/{top}"
            );

            config.Routes.MapHttpRoute(
                name: "SearchApi",
                routeTemplate: "api/{controller}/search/{name}/{start}/{end}"
            );

            config.Routes.MapHttpRoute(
                name: "NameApi",
                routeTemplate: "api/{controller}/name/{slug}"
            );

            config.Routes.MapHttpRoute(
                name: "StatusApi",
                routeTemplate: "api/{controller}/{status}/{start}/{end}"
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }
    }
}
