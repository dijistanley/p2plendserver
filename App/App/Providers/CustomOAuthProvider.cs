using Common.Helpers;
using DataAccess.Core;
using DataAccess.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Model.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace App.Identity
{
    public class CustomOAuthProvider: OAuthAuthorizationServerProvider
	{
		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            
            ApplicationUser user;
            using (AccountRepository _repo = new AccountRepository())
            {
                user = await _repo.FindUser(context.UserName, context.Password);
            }

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }
            
            AuthenticationProperties properties = CreateProperties(user.UserName);
            ClaimsIdentity oAuthIdentity = GetOAuthClaimsIdentity(context, user);
            ClaimsIdentity cookiesIdentity = GetCookiesClaimsIdentity(context, user);

            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
			context.Validated(ticket);
            //context.Request.Context.Authentication.SignIn(cookiesIdentity);


		}

		public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            Client client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Validated();
                //context.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            using (AccountRepository _repo = new AccountRepository())
            {
                client = _repo.FindClient(context.ClientId);
            }

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != Helper.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

		private static ClaimsIdentity GetOAuthClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context, IdentityUser user)
		{
            // "JWT"
            var identity = new ClaimsIdentity(GetClaims(context, user), OAuthDefaults.AuthenticationType);

			return identity;
		}

        private static ClaimsIdentity GetCookiesClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context, IdentityUser user)
        {
            // "Cookies"
            var identity = new ClaimsIdentity(GetClaims(context, user), CookieAuthenticationDefaults.AuthenticationType);
            return identity;
        }

        private static IEnumerable<Claim> GetClaims(OAuthGrantResourceOwnerCredentialsContext context, IdentityUser user)
        {
            List<Claim> claims = new List<Claim>();
            yield return new Claim(ClaimTypes.Name, context.UserName);
            yield return new Claim(JwtRegisteredClaimNames.Sub, context.UserName);
            yield return new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            //yield return new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64);

            var userRoles = context.OwinContext.Get<DataAccess.Core.ApplicationUserManager>().GetRoles(user.Id);
            foreach (var role in userRoles)
            {
                yield return new Claim(ClaimTypes.Role, role);
            }
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}
