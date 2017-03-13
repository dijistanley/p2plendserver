using System;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(P2PLend.Startup))]
namespace P2PLend
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureOAuth(app);
		}
	}
}
