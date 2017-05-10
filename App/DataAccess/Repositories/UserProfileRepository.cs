using Model.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserProfileRepository: Repository<UserProfile>
    {
        public UserProfileRepository(): base()
        {
        }

        public override async Task<bool> Create(UserProfile t)
        {
            // validate userID is not null
            if (string.IsNullOrEmpty(t.UserId))
                return false;

            // validate userID is valid
            using (AccountRepository ar = new AccountRepository())
            {
                if (await ar.FindByIDAsync(t.UserId) == null)
                    return false;
            }

            // validate no userprofile with same userID exists
            if (await FindByUserId(t.UserId) != null)
                return false;

            DBContext.UserProfile.Add(t);

            await DBContext.SaveChangesAsync();

            return true;
        }

        public override async Task<UserProfile> Find(string id)
        {
            var data = await DBContext.UserProfile.Where(x => x.Id == int.Parse(id)).ToListAsync();
            return data.FirstOrDefault();
        }

        public async Task<UserProfile> FindByUserId(string id)
        {
            var data = await DBContext.UserProfile.Where(x => x.UserId == id).ToListAsync();
            return data.FirstOrDefault();
        }

        public override async Task<bool> Update(UserProfile t)
        {
            UserProfile up = await Find(t.Id.ToString());
                
            if(up == null && !string.IsNullOrEmpty(t.UserId))
            {
                up = await FindByUserId(t.UserId);
            }

            if(up != null)
            {
                up.FirstName = t.FirstName;
                up.LastName = t.LastName;
                up.Address = t.Address;

                try
                {
                    await DBContext.SaveChangesAsync();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public override Task<bool> Delete(UserProfile t)
        {
            throw new NotImplementedException();
        }

    }
}
