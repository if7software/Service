using System;
using System.Linq;
using System.Text;
using DbBridge.Attributes;
using DbBridge.Interfaces;
using DbBridge.DataManipulation;
using MySql.Data.MySqlClient;
using DbBridge.Classes;

namespace DbBridge
{
	public class Delete<T> : CommandExec<T> where T : class, IEntity
	{
		private string SqlFrom { get; set; }
		private string SqlWhere { get; set; }
		private string SqlEnd { get; set; }

		public Delete(Db mariaDb)
			: base(mariaDb)
		{
			Initialize();

			SqlFrom = GetTableName();
		}

		public Delete(Db mariaDb, T entity)
			: base(mariaDb)
		{
			Initialize();

			SqlFrom = GetTableName();

			FindDbColumns(
				attribute => attribute is DbPrimaryKeyColumn,
				(attribute, property) =>
				{
					DbColumn column = attribute as DbColumn;

					SqlWhere = string.Concat(column.Name, " = ", property.GetValue(entity));
				}
			);
		}

		private void Initialize()
		{
			SqlWhere = string.Empty;
			SqlEnd = string.Empty;
		}

		protected override MySqlCommand BuildCommand()
		{
			var sql = new StringBuilder();
			sql.Append("DELETE FROM ");
			sql.Append(SqlFrom);

			if (!string.IsNullOrWhiteSpace(SqlWhere))
				sql.AppendFormat(" WHERE {0}", SqlWhere);

			if (!string.IsNullOrWhiteSpace(SqlEnd))
				sql.Append(" ").Append(SqlEnd);

			return GetDbBridgeCommand(sql.ToString());
		}

		public Delete<T> Where(SQL<T> sql)
		{
			return this;
		}
	}
}
