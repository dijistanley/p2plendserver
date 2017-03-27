using Microsoft.AspNet.Identity.EntityFramework;
using Model.Models;
using System.Data.Entity;

namespace DataAccess.Core
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {

        public DatabaseContext() : base("DefaultConnection") {
            Database.SetInitializer(new Core.DatabaseInitializer());
            //Database.SetInitializer<DatabaseContext>(new CreateDatabaseIfNotExists<DatabaseContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // rename aspnetusers to users
            modelBuilder.Entity<IdentityUser>().ToTable("Users");
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
        }

        #region DBSets
        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        #endregion

    }
}
