using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using P2PLend.Core;
using P2PLend.Identity;
using System;
using System.Configuration;

namespace P2PLend
{
    public partial class Startup
	{
		public void ConfigureOAuth(IAppBuilder app)
		{
			var issuer = ConfigurationManager.AppSettings["issuer"];
			var secret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["secret"]);

			app.CreatePerOwinContext(() => new P2PDBContext());
			app.CreatePerOwinContext(() => new P2PUserManager());

			app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
			{
				AuthenticationMode = AuthenticationMode.Active,
				AllowedAudiences = new[] { "Any" },
				IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
				{
					new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
				}
			});
		
			app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
			{
				AllowInsecureHttp = true,
				TokenEndpointPath = new PathString("/oauth2/token"),
				AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
				Provider = new CustomOAuthProvider(),
				AccessTokenFormat = new CustomJwtFormat(issuer)
			});
		}
	}
}