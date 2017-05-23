using App.ResonseModels;
using App.ResonseModels.RequestModels;
using DataAccess.Repositories;
using Microsoft.AspNet.Identity;
using Model.Models;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace App.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController: BaseController
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
        public async Task<IHttpActionResult> UpdateProfileNames(UpdateUserProfileRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await GetUser();

            if (user == null) return BadRequest(StringResource.UnauthorizedMessage);
            
            using (UserProfileRepository uprepo = new UserProfileRepository())
            {
                
                if (await uprepo.UpdateName(user.Id, request.Firstname, request.Lastname))
                    return Ok(string.Format(StringResource.SuccessUpdateMessage, "User names"));

            }

            return BadRequest(String.Format(StringResource.ErorUpdateMessage, "User profile"));
            
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateEmail(UpdateEmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = await GetUser();

            if (user == null) return BadRequest(StringResource.UnauthorizedMessage);

            if (request != null)
            {
                using (AccountRepository ar = new AccountRepository())
                {
                    if(await ar.FindUser(user.Email, request.Password)!= null)
                    {
                        if((await ar.ChangeUserEmail(user.Id, request.Email)).Succeeded)
                        {
                            return Ok();
                        }
                    }
                }
            }

            return BadRequest("Could not update email");
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdatePassword(UpdatePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = await GetUser();

            if(user == null) return BadRequest(StringResource.UnauthorizedMessage);

            if (request != null)
            {
                using (AccountRepository ar = new AccountRepository())
                {
                    IdentityResult ir = await ar.ChangeUserPassword(user.Id, request.CurrentPassword, request.NewPassword);
                    if(ir.Succeeded)
                    {
                        return Ok(String.Format(StringResource.SuccessUpdateMessage, "Password"));
                    }
                }
            }

            return BadRequest(string.Format(StringResource.ErorUpdateMessage, "user password"));
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdatePhoneNumber(UpdatePhoneRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await GetUser();

            if (user == null) return BadRequest(StringResource.UnauthorizedMessage);

            if (request != null)
            {
                using (AccountRepository ar = new AccountRepository())
                {
                    IdentityResult ir = await ar.ChangeUserPhonenumber(user.Id, request.NewPhoneNumber);
                    if (ir.Succeeded)
                    {
                        return Ok(String.Format(StringResource.SuccessUpdateMessage, "Phone number"));
                    }
                }
            }

            return BadRequest(string.Format(StringResource.ErorUpdateMessage , "Phone number"));
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateAddress(UpdateAddressRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await GetUser();

            if (user == null) return BadRequest(StringResource.UnauthorizedMessage);

            if (request != null)
            {
                using (UserProfileRepository uprepo = new UserProfileRepository())
                {

                    Address address = new Address();

                    address.Line = request.Line;
                    address.District = request.District;
                    address.City = request.City;
                    address.State = request.State;
                    address.PostalCode = request.PostalCode;
                    address.Country = request.Country;

                    if (await uprepo.UpdateAddress(user.Id, address))
                        return Ok(string.Format(StringResource.SuccessUpdateMessage, "User Address"));
                }
            }
            return BadRequest(string.Format(StringResource.ErorUpdateMessage, "Address"));

        }
    }
}