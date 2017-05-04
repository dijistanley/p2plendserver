using DataAccess.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace App.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        // POST: api/Account
        [AllowAnonymous]
        public async Task<IHttpActionResult> Post(RegisterUser newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (AccountRepository repo = new AccountRepository())
            {
                var result = await repo.RegisterUserAsync(newUser);

                if(result == null)
                {
                    // error occured
                    return BadRequest();
                }

                if (result.Errors.Count() > 0)
                {
                    return BadRequest(result.Errors.FirstOrDefault());
                }
                
                return Ok("user created");
                
            }
        }
    }
}
