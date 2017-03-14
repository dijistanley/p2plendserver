using P2PLend.Core;
using P2PLend.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace p2plend.Controllers
{
    public class AuthController : ApiController
    {
		public AuthController() 
		{ 
			// initialize repositories or models here
		}

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            using (var context = new P2PDBContext())
            {
                IDictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("Stannley", "Diji");
                return Ok(dic.ToList());
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> Register([FromBody] NewAccountRequest request)
        {
            return Ok();
        }

        //[HttpPost]
        //public async Task<IHttpActionResult> Login([FromBody] LoginRequest request)
        //{
        //    return Ok();
        //}
    }
}