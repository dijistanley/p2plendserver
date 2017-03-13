using System;
using System.Data.Entity.Migrations;
namespace P2PLend.Core
{
	public class DBConfig: DbMigrationsConfiguration<P2PDBContext>
	{
		public DBConfig()
		{
			AutomaticMigrationsEnabled = true;
			AutomaticMigrationDataLossAllowed = false;
		}
	}
}
