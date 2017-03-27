using Common.Helpers;
using DataAccess.Core;
using DataAccess.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Model.Models;
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

			//ApplicationUser user = context.OwinContext.Get<DatabaseContext>().Users.FirstOrDefault(u => u.UserName == context.UserName);
			//if (!context.OwinContext.Get<ApplicationUserManager>().CheckPassword(user, context.Password))
			//{
			//	context.SetError("invalid_grant", "The user name or password is incorrect");
			//	context.Rejected();
   //             return;
			//}

            ApplicationUser user;
            using (AccountRepository _repo = new AccountRepository())
            {
                user = await _repo.FindUser(context.UserName, context.Password);
            }

            if (user == null)
            {
                context.Validated();
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                //context.Rejected();
                return;
            }

            ClaimsIdentity claimsIdentity = SetClaimsIdentity(context, user);
            AuthenticationTicket ticket = new AuthenticationTicket(claimsIdentity, new AuthenticationProperties());
			context.Validated(ticket);
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

		private static ClaimsIdentity SetClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context, IdentityUser user)
		{
			var identity = new ClaimsIdentity("JWT");
			identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
			identity.AddClaim(new Claim("sub", context.UserName));

			var userRoles = context.OwinContext.Get<DataAccess.Core.ApplicationUserManager>().GetRoles(user.Id);
			foreach (var role in userRoles)
			{
				identity.AddClaim(new Claim(ClaimTypes.Role, role));
			}

			return identity;
		}
	}
}
