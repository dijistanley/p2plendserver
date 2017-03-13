using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace p2plend
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			ConfigureCORS(config);

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
			var cors = new EnableCorsAttribute("*", "*", "*");
			config.EnableCors(cors);
			config.MessageHandlers.Add(new PreflightRequestsHandler());
		}
}
}
