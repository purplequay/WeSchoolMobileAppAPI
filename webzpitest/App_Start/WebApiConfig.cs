using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Http.Cors;

namespace webzpitest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Web API routes

            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*"); // Enable Cors (Cross Orgin Resource Sharing)
            config.EnableCors(cors);
            config.MapHttpAttributeRoutes();           

            config.Routes.MapHttpRoute(
            name: "studentsyllabus",
            routeTemplate: "api/{controller}/studentsyllabus/{studentcode}"
            );
            config.Routes.MapHttpRoute(
            name: "noticesdisplay",
            routeTemplate: "api/{controller}/noticesdisplay/{studentcode}/{requesttype}"
            ); 
                  
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}
