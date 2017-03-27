using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using App.Identity;
using System;
using System.Configuration;
using App.Providers;

namespace App
{
    public partial class Startup
	{
		public void ConfigureOAuth(IAppBuilder app)
		{
			var issuer = ConfigurationManager.AppSettings["issuer"];
			var secret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["secret"]);

			app.CreatePerOwinContext(() => new DataAccess.Core.DatabaseContext());
			app.CreatePerOwinContext(() => new DataAccess.Core.ApplicationUserManager());

            //// Token Generation
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
				AccessTokenFormat = new CustomJwtFormat(issuer),
                RefreshTokenProvider = new CustomRefreshTokenProvider()
            });

            //use a cookie to temporarily store information about a user logging in with a third party login provider
            //app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            
            //Configure Google External Login
            //googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = ConfigManager.Instance.GoogleClientID,
            //    ClientSecret = ConfigManager.Instance.GoogleClientSecret,
            //    Provider = new GoogleAuthProvider()
            //};
            //app.UseGoogleAuthentication(googleAuthOptions);

            //Configure Facebook External Login
            //facebookAuthOptions = new FacebookAuthenticationOptions()
            //{
            //    AppId = "xxxxxx",
            //    AppSecret = "xxxxxx",
            //    Provider = new FacebookAuthProvider()
            //};
            //app.UseFacebookAuthentication(facebookAuthOptions);
        }
    }
}