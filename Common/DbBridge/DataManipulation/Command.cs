using Constants;
using DbBridge.Attributes;
using DbBridge.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using DbBridge.Classes;

namespace DbBridge.DataManipulation
{
	public abstract class Command<T> where T : class, IEntity
	{
		public SQL<T> Sql { get; protected set; }
		protected Db Database { get; private set; }
		protected List<MySqlParameter> Parameters { get; private set; }

		protected Command(Db mariaDb)
		{
			Database = mariaDb;

			Sql = new SQL<T>(this);
			Parameters = new List<MySqlParameter>();
		}

		protected void FindDbColumns(Func<object, bool> func, Action<object, PropertyInfo> action)
		{
			foreach (var property in typeof(T).GetProperties())
			{
				var columnAttribute = property.GetCustomAttributes(true).SingleOrDefault(func);

				if (columnAttribute != null)
					action.Invoke(columnAttribute, property);
			}
		}

		protected string GetTableName()
		{
			return typeof(T).GetField(Globals.DbEntity.TableField)?.GetValue(null).ToString();
		}

		protected MySqlCommand GetDbBridgeCommand(string sql)
		{
			MySqlCommand command = null;

			if (Database.InTransaction)
				command = new MySqlCommand(sql, Database.Connection, Database.Transaction);
			else
				command = new MySqlCommand(sql, Database.Connection);

			foreach (var parameter in Parameters)
				command.Parameters.Add(parameter);

			return command;
		}

		protected void AddDbParamater(DbColumn column, object value)
		{
			MySqlParameter parameter = new MySqlParameter()
			{
				ParameterName = column.Parameter,
				MySqlDbType = column.Type,
				IsNullable = column.IsNullable,
				Value = value
			};

			Parameters.Add(parameter);
		}

		public DbColumn AddDbParamater(string columnName, object value)
		{
			DbColumn column = GetDbColumn(columnName);

			if (column != null)
				AddDbParamater(column, value);

			return column;
		}

		public DbColumn GetDbColumn(string columnName)
		{
			foreach (var property in typeof(T).GetProperties())
			{
				var columnAttribute = property.GetCustomAttributes(true).SingleOrDefault(attribute => attribute is DbColumn);

				if (columnAttribute != null)
				{
					DbColumn column = columnAttribute as DbColumn;

					if (column.Name.Equals(columnName))
						return column;
				}
			}

			return null;
		}

		protected abstract MySqlCommand BuildCommand();
	}
}
