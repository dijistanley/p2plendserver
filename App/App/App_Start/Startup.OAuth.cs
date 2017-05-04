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
using DataAccess.Repositories;

namespace App
{
    public partial class Startup
	{
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public void ConfigureOAuth(IAppBuilder app)
		{
            CreateRolesAndAdminUsers();

            var issuer = ConfigurationManager.AppSettings["issuer"];
			var secret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["secret"]);
            
			app.CreatePerOwinContext(() => new DataAccess.Core.DatabaseContext());
			app.CreatePerOwinContext(() => new DataAccess.Core.ApplicationUserManager());
            
			OAuthOptions = new OAuthAuthorizationServerOptions
            {
#if DEBUG
                AllowInsecureHttp = true,
#endif
                TokenEndpointPath = new PathString("/oauth2/token"),
                //AuthorizeEndpointPath = new PathString("/account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(5),
				Provider = new CustomOAuthProvider(),
				AccessTokenFormat = new CustomJwtFormat(issuer),
                RefreshTokenProvider = new CustomRefreshTokenProvider(),
                AuthenticationType = OAuthDefaults.AuthenticationType
            };
            
            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(
                    new OAuthBearerAuthenticationOptions() {
                        AccessTokenFormat = new CustomJwtFormat(issuer)
                    });


            //// Token Generation
            //app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            //{
            //    AuthenticationMode = AuthenticationMode.Active,
            //    AllowedAudiences = new[] { "Any" },
            //    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
            //    {
            //        new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
            //    },
                
            //});

            //use a cookie to temporarily store information about a user logging in with a third party login provider
            //app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);

            //configure google external login
            //googleauthoptions = new googleoauth2authenticationoptions()
            //{
            //    clientid = configmanager.instance.googleclientid,
            //    clientsecret = configmanager.instance.googleclientsecret,
            //    provider = new googleauthprovider()
            //};
            //app.usegoogleauthentication(googleauthoptions);

            //Configure Facebook External Login
            //facebookAuthOptions = new FacebookAuthenticationOptions()
            //{
            //    AppId = "xxxxxx",
            //    AppSecret = "xxxxxx",
            //    Provider = new FacebookAuthProvider()
            //};
            //app.UseFacebookAuthentication(facebookAuthOptions);
        }

        public void CreateRolesAndAdminUsers()
        {
            using (AccountRepository accRepo = new AccountRepository())
            {
                accRepo.CreateDefaultRolesAndUser();
            }
            
        }
    }
}