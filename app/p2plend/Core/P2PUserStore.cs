using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace P2PLend.Core
{
	public class P2PUserStore: UserStore<IdentityUser>
	{
		public P2PUserStore(): base(new P2PDBContext())
		{
		}
	}
}
