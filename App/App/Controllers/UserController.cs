using App.ResonseModels;
using DataAccess.Repositories;
using Microsoft.Owin.Security.OAuth;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace App.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController: ApiController
    {
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

        // GET api/values
        [HttpGet]
        public async Task<IHttpActionResult> Info()
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

            using (AccountRepository ar = new AccountRepository())
            {
                var result = await ar.FindByNameAsync(userName);

                if(result == null)
                {
                    return BadRequest();
                }

                using (UserProfileRepository upr = new UserProfileRepository())
                {
                    var upresult = await upr.FindByUserId(result.Id);

                    if (upresult != null)
                    {
                        UserProfileResponse response = new UserProfileResponse()
                        {
                            Email = result.Email,
                            EmailConfirmed = result.EmailConfirmed,
                            Username = result.UserName,
                            PhoneNumber = result.PhoneNumber,
                            PhoneNumberConfirmed = result.PhoneNumberConfirmed,
                            Firstname = upresult.FirstName,
                            Lastname = upresult.LastName,
                            Address = new AddressResponse(upresult.Address)
                        };

                        return Json(response);
                        //return Ok(response);
                    }
                }
            }

            return BadRequest("Could not fetch user profile");
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(UserProfileRequest request)
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
            ApplicationUser user;
            using (AccountRepository ar = new AccountRepository())
            {
                user = await ar.FindByNameAsync(userName);
                if (!string.IsNullOrEmpty(request.PhoneNumber))
                {
                    // update phone

                    if (user == null)
                    {
                        return BadRequest();
                    }

                    user.PhoneNumber = request.PhoneNumber;
                    user.PhoneNumberConfirmed = false;
                    await ar.UpdateUserAsync(user);
                }
            }

            using (UserProfileRepository uprepo = new UserProfileRepository())
            {
                UserProfile up = await uprepo.FindByUserId(user.Id);

                if (up != null)
                {
                    up.FirstName = string.IsNullOrEmpty( request.Firstname ) ? up.FirstName : request.Firstname;
                    up.LastName = string.IsNullOrEmpty(request.Lastname) ? up.LastName : request.Lastname;
                    
                    if(request.Address != null)
                    {
                        up.Address.Text = string.IsNullOrEmpty(request.Address.Text) ? up.Address.Text : request.Address.Text;
                        up.Address.Line = string.IsNullOrEmpty(request.Address.Line) ? up.Address.Line : request.Address.Line;
                        up.Address.District = string.IsNullOrEmpty(request.Address.District) ? up.Address.District : request.Address.District;
                        up.Address.City = string.IsNullOrEmpty(request.Address.City) ? up.Address.City : request.Address.City;
                        up.Address.State = string.IsNullOrEmpty(request.Address.State) ? up.Address.State : request.Address.State;
                        up.Address.Period = string.IsNullOrEmpty(request.Address.Period) ? up.Address.Period : request.Address.Period;
                        up.Address.PostalCode = string.IsNullOrEmpty(request.Address.PostalCode) ? up.Address.PostalCode : request.Address.PostalCode;
                        up.Address.Country = string.IsNullOrEmpty(request.Address.Country) ? up.Address.Country : request.Address.Country;
                    }

                    if (await uprepo.Update(up))
                        return Ok("User Profile Updated");
                }
            }

            return BadRequest("Could not update profile");
            
        }
    }
}