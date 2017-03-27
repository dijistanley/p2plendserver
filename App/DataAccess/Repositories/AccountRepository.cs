using DataAccess.Core;
using Microsoft.AspNet.Identity;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class AccountRepository: IDisposable
    {
        private DatabaseContext context;
        private UserManager<ApplicationUser> userManager;

        public AccountRepository()
        {
            context = new DatabaseContext();
            userManager = new ApplicationUserManager(context);
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterUser user)
        {
            ApplicationUser iUser = new ApplicationUser
            {
                UserName = user.UserName,
                Email = user.Email
            };

            var result = await userManager.CreateAsync(iUser, user.Password);

            return result;
        }

        public async Task<ApplicationUser> FindUser(string username, string password)
        {
            ApplicationUser user = await userManager.FindAsync(username, password);
            return user;
        }

        public Client FindClient(string clientId)
        {
            var client = context.Clients.Find(clientId);

            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = context.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            context.RefreshTokens.Add(token);

            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await context.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                context.RefreshTokens.Remove(refreshToken);
                return await context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            context.RefreshTokens.Remove(refreshToken);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await context.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return context.RefreshTokens.ToList();
        }

        public async Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo)
        {
            ApplicationUser user = await userManager.FindAsync(loginInfo);

            return user;
        }

        //public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        //{
        //    var result = await userManager.CreateAsync(user);

        //    return result;
        //}

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await userManager.AddLoginAsync(userId, login);

            return result;
        }

        public void Dispose()
        {
            context.Dispose();
            userManager.Dispose();

        }
    }
}