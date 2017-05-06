using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using App.App_Start;
using System.Linq;

namespace App
{
    public class WebApiApplication :HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest()
        {
            //if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
            //{
            //    Response.Flush();
            //}
        }
    }
}
