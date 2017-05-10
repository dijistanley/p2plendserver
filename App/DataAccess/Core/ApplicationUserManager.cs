using Microsoft.AspNet.Identity;
using Model.Models;

namespace DataAccess.Core
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(DatabaseContext dbContext = null) : base(new ApplicationUserStore(dbContext))
        {
        }
    }
}
