using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace App.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController: ApiController
    {
        // GET api/values
        public IHttpActionResult Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest("Unauthorized");
            }

            var userName = this.RequestContext.Principal.Identity.Name;
            return Ok(String.Format("Hello, {0}.", userName));
        }
    }
}