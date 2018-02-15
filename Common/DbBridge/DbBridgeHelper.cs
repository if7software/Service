using Constants;
using Core;
using System;

namespace DbBridge
{
	public static class DbBridgeHelper
	{
		public static Db GetDatabaseForTest()
		{
			try
			{
				string connectionString = General.ConnectionStringDict[Globals.Database.Test];

				return new Db(connectionString);
			}
			catch (Exception ex)
			{
				throw new ArgumentException("Database 'test' is not define.", ex);
			}
		}

		public static Db GetDatabaseForCoordinator()
		{
			try
			{
				string connectionString = General.ConnectionStringDict[Globals.Database.Coordinator];

				return new Db(connectionString);
			}
			catch (Exception ex)
			{
				throw new ArgumentException("Database 'coordinator' is not define.", ex);
			}
		}
	}
}
