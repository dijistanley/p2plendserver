using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace P2PLend
{
	public class P2PUsermanager: UserManager<IdentityUser>
	{
		public P2PUsermanager(): base(new P2PUserStore())
		{
		}
	}
}
