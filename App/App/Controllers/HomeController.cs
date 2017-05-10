using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace App.Controllers
{
    [RoutePrefix("api/home")]
    public class HomeController: ApiController
    {
        // GET api/values
        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult Index()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return Json("Uberlend API, OK");
        }

    }
}