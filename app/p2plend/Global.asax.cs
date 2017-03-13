using System.Web;
using System.Web.Routing;
using System.Web.Http;
using System.Data.Entity;
using P2PLend.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace p2plend
{
	public class Global : HttpApplication
	{
		protected void Application_Start()
		{
			//AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);

			// initialise database
			InitializeDB();

			RouteConfig.RegisterRoutes(RouteTable.Routes);

			// Configure JSON to use camel case
			ConfigureJSON();

		}

		void InitializeDB()
		{
			Database.SetInitializer(new DBInitializer());
		}

		void ConfigureJSON()
		{
			var formatters = GlobalConfiguration.Configuration.Formatters;
			var jsonFormatter = formatters.JsonFormatter;
			var settings = jsonFormatter.SerializerSettings;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
		}
	}
}
