using System.Data.Entity.Migrations;

namespace DataAccess.Core
{
    public class DBConfig : DbMigrationsConfiguration<DatabaseContext>
    {
        public DBConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;


            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
            SetHistoryContextFactory("MySql.Data.MySqlClient", MySqlHistoryContext.GetContext);

        }

        
    }
}
