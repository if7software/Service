using DbBridge.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DbBridge.DataManipulation
{
	public abstract class CommandQuery<T> : Command<T> where T : class, IEntity, new()
	{
		protected CommandQuery(Db mariaDb)
			: base(mariaDb)
		{
		}

		public T ReturnUnique()
		{
			MySqlCommand command = null;
			
			try
			{
				command = BuildCommand();

				List<T> result = new List<T>();

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						if (result.Count > 0)
							throw new Exception("");

						T entity = new T();

						for (int i = 0; i < reader.FieldCount; i++)
							entity.Fill(reader.GetName(i), reader.GetValue(i), reader.IsDBNull(i));

						result.Add(entity);
					}

					reader.Close();
				}

				return result.First();
			}
			catch (MySqlException ex)
			{
				throw ex;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (command != null)
					command.Dispose();
			}
		}

		public List<T> ReturnList()
		{
			MySqlCommand command = null;

			try
			{
				command = BuildCommand();

				List<T> result = new List<T>();

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						T entity = new T();

						for (int i = 0; i < reader.FieldCount; i++)
							entity.Fill(reader.GetName(i), reader.GetValue(i), reader.IsDBNull(i));

						result.Add(entity);
					}

					reader.Close();
				}

				return result;
			}
			catch (MySqlException ex)
			{
				throw ex;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (command != null)
					command.Dispose();
			}
		}

		protected override abstract MySqlCommand BuildCommand();
	}
}
