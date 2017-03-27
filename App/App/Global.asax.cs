using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using App.App_Start;

namespace App
{
    public class WebApiApplication :HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            // uncomment the line below to require authorization for all resources except oauth2/token
            //GlobalConfiguration.Configure(FilterConfig.Configure);
            
            // not needed. Database is initialized from the DataAccess DBContext constructor class
            // initialise database
            // DataAccess.Initializer.InitializeDB();

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Configure JSON to use camel case
            ConfigureJSON();

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
