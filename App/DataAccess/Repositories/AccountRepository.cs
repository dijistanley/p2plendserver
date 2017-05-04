using DataAccess.Core;
using DataAccess.enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Security;

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

            IdentityResult result = await userManager.CreateAsync(iUser, user.Password);

            if(result.Succeeded)
            {
                // add user role
                await userManager.AddToRoleAsync(iUser.Id, "User");
            }

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
        
        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await userManager.AddLoginAsync(userId, login);

            return result;
        }

        public void CreateDefaultRolesAndUser()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            
            // In Startup iam creating first Admin Role and creating a default Admin User  
            if (!roleManager.RoleExists(UserRoles.Admin.ToString()))
            {

                // first we create Admin rool 
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = UserRoles.Admin.ToString();
                roleManager.CreateAsync(role);
                
                //Here we create a Admin super user who will maintain the website                 
                var user = new ApplicationUser();
                user.UserName = "voiddigits";
                user.Email = "dijistanley@voiddigits.com";
                string userPWD = "123456";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin 
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, UserRoles.Admin.ToString());

                }
            }

            // creating Creating Manager role  
            if (!roleManager.RoleExists(UserRoles.User.ToString()))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = UserRoles.User.ToString();
                roleManager.Create(role);
            }
        }

        public void Dispose()
        {
            context.Dispose();
            userManager.Dispose();

        }
    }
}