using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using Thinktecture.IdentityModel.Tokens;

namespace App.Identity
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
	{
		private static readonly byte[] _secret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["secret"]);
		private readonly string issuer;

        private readonly string algorithm;
        private readonly TokenValidationParameters validationParameters;
        
        public CustomJwtFormat(string issuer)
        {
            //this.validationParameters = tokenValidationParameters;
            //this.algorithm = hmacSha256;
            this.issuer = issuer;
            algorithm = "HS256"; //SecurityAlgorithms.HmacSha256Signature;
            this.validationParameters = new TokenValidationParameters() {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new InMemorySymmetricSecurityKey(_secret),
                IssuerSigningKeyValidator = (a) => {  },
                

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = issuer,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = false,
                ValidAudience = "ExampleAudience",

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };
        }

        public string Protect(AuthenticationTicket data)
		{
			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

            //Microsoft.IdentityModel.Tokens.SymmetricSecurityKey secretKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(_secret);
            //Microsoft.IdentityModel.Tokens.SigningCredentials signingKey = new Microsoft.IdentityModel.Tokens.SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);

   //         HmacSigningCredentials signingKey = new HmacSigningCredentials(_secret);
			//var issued = data.Properties.IssuedUtc;
			//var expires = data.Properties.ExpiresUtc;

            //return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(issuer, null, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey));
            return new JwtSecurityTokenHandler().WriteToken(GenerateToken(data));

        }

		public AuthenticationTicket Unprotect(string protectedText)
		{
            var handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = null;
            SecurityToken validToken = null;

            try
            {
                principal = handler.ValidateToken(protectedText, this.validationParameters, out validToken);

                var validJwt = validToken as JwtSecurityToken;

                if (validJwt == null)
                {
                    throw new ArgumentException("Invalid JWT");
                }

                if (!validJwt.Header.Alg.Equals(algorithm, StringComparison.Ordinal))
                {
                    throw new ArgumentException($"Algorithm must be '{algorithm}'");
                }

                // Additional custom validation of JWT claims here (if any)
            }
            catch (SecurityTokenValidationException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }

            //Validation passed. Return a valid AuthenticationTicket:
            return new AuthenticationTicket(principal.Identity as ClaimsIdentity, CustomOAuthProvider.CreateProperties(principal.Identity.Name));

            //throw new NotImplementedException();
        }

        private JwtSecurityToken GenerateToken(AuthenticationTicket context)
        {
            var now = DateTime.UtcNow;
            SigningCredentials sc = new SigningCredentials(new InMemorySymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);
            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: this.issuer,
                claims: context.Identity.Claims,
                notBefore: now,
                expires: context.Properties.ExpiresUtc.Value.UtcDateTime,
                signingCredentials: sc);

            return jwt;
        }
    }
}
