using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace P2PLend.Core
{
	public class P2PUserManager: UserManager<IdentityUser>
	{
		public P2PUserManager(): base(new P2PUserStore())
		{
		}
	}
}
