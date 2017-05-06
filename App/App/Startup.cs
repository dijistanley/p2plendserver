using App.App_Start;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using System.Web.Http;
using System.Web.Routing;

[assembly: OwinStartup(typeof(App.Startup))]
namespace App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            // uncomment the line below to require authorization for all resources except oauth2/token
            GlobalConfiguration.Configure(FilterConfig.Configure);
            ConfigureJSON(GlobalConfiguration.Configuration);

            ConfigureOAuth(app);
            
            app.UseWebApi(GlobalConfiguration.Configuration);
        }

        void ConfigureJSON(HttpConfiguration config)
        {
            var formatters = config.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}