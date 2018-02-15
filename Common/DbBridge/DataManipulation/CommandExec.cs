using System;
using DbBridge.Interfaces;
using MySql.Data.MySqlClient;

namespace DbBridge.DataManipulation
{
	public abstract class CommandExec<T> : Command<T> where T : class, IEntity
	{
		protected CommandExec(Db mariaDb)
			: base(mariaDb)
		{
		}

		public bool Execute()
		{
			bool result = true;
			MySqlCommand command = null;

			try
			{
				command = BuildCommand();
				command.ExecuteNonQuery();
			}
			catch (MySqlException ex)
			{
				result = false;
			}
			catch (Exception ex)
			{
				result = false;
			}
			finally
			{
				if (command != null)
					command.Dispose();
			}

			return result;
		}

		protected override abstract MySqlCommand BuildCommand();
	}
}
