using DataAccess.Repositories;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace App.Controllers
{
    public class BaseController: ApiController
    {
        public BaseController() : base() { }

        public async Task<ApplicationUser> GetUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userName = this.RequestContext.Principal.Identity.Name;
            
            using (AccountRepository ar = new AccountRepository())
            {
                return await ar.FindByNameAsync(userName);
            }
            
        }
    }
}