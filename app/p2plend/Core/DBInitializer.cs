using System;
using System.Data.Entity;

namespace P2PLend.Core
{
	public class DBInitializer: MigrateDatabaseToLatestVersion<P2PDBContext, DBConfig>
	{
	}
}
