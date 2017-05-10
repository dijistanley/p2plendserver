using Microsoft.AspNet.Identity.EntityFramework;
using Model.Models;

namespace DataAccess.Core
{
    class ApplicationUserStore: UserStore<ApplicationUser>
    {
        public ApplicationUserStore(DatabaseContext dbContext = null): base(dbContext ?? new DatabaseContext())
		{
        }
    }
}
