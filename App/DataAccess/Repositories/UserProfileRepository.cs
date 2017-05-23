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

        public async Task<bool> UpdateAddress(string userID, Address newAddress)
        {
            UserProfile up = (await DBContext.UserProfile.Where(x => x.UserId == userID).ToListAsync()).FirstOrDefault();

            if (up != null)
            {
                up.Address.State = newAddress.State;
                up.Address.PostalCode = newAddress.PostalCode;
                up.Address.Line = newAddress.Line;
                up.Address.District = newAddress.District;
                up.Address.City = newAddress.City;
                up.Address.Country = newAddress.Country;

                up.Address.Text = string.Format("{0}, {1}, {2}, {3}. {4}. {5}", up.Address.Line, up.Address.District, up.Address.City, up.Address.State, up.Address.Country, up.Address.PostalCode);

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

        public async Task<bool> UpdateName(string userID, string firstname = null, string lastname = null)
        {
            UserProfile up = (await DBContext.UserProfile.Where(x => x.UserId == userID).ToListAsync()).FirstOrDefault();

            if(up != null)
            {
                if(!string.IsNullOrEmpty(firstname))
                    up.FirstName = firstname;

                if (!string.IsNullOrEmpty(lastname))
                    up.LastName = lastname;

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

        public override async Task<bool> Update(UserProfile t)
        {
            return false;
        }

        public override Task<bool> Delete(UserProfile t)
        {
            throw new NotImplementedException();
        }

    }
}
