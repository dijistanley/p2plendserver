﻿using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAccess.Core
{
    class ApplicationUserStore: UserStore<ApplicationUser>
    {
        public ApplicationUserStore(DatabaseContext dbContext = null): base(dbContext ?? new DatabaseContext())
		{
        }
    }
}
