using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using System.Web.Http.Cors;

namespace App.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //ConfigureCORS(config);
            ConfigureOAuthTokenHandler(config);
            ConfigureRoutes(config);
        }

        static void ConfigureRoutes(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        static void ConfigureCORS(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("http://localhost", "*", "*");
            config.EnableCors(cors);
            config.MessageHandlers.Add(new PreflightRequestsHandler());
        }

        static void ConfigureOAuthTokenHandler(HttpConfiguration config)
        {
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationAttribute(OAuthDefaults.AuthenticationType));
        }
    }
}
