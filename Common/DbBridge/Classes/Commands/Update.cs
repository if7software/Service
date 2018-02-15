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
	public class Update<T> : CommandExec<T> where T : class, IEntity
	{
		private string SqlSet { get; set; }
		private string SqlFrom { get; set; }
		private string SqlWhere { get; set; }
		private string SqlEnd { get; set; }

		public Update(Db mariaDb, T entity, params string[] columns)
			: base(mariaDb)
		{
			Initialize();

			SqlFrom = GetTableName();

			FindDbColumns(
				attribute => attribute is DbColumn || attribute is DbPrimaryKeyColumn,
				(attribute, property) =>
				{
					if (attribute is DbPrimaryKeyColumn)
					{
						var column = attribute as DbPrimaryKeyColumn;

						SqlWhere = string.Concat(column.Name, " = ", property.GetValue(entity));
					}
					else if (attribute is DbColumn)
					{
						var column = attribute as DbColumn;

						if (columns.Length == 0 || columns.ToList().Contains(column.Name))
						{
							var separator = string.IsNullOrEmpty(SqlSet) ? string.Empty : ", ";

							SqlSet = string.Concat(SqlSet, separator, column.Name, " = ", column.Parameter);

							AddDbParamater(column, property.GetValue(entity));
						}
					}
				}
			);
		}

		private void Initialize()
		{
			SqlSet = string.Empty;
			SqlWhere = string.Empty;
			SqlEnd = string.Empty;
		}

		protected override MySqlCommand BuildCommand()
		{
			var sql = new StringBuilder();
			sql.Append("UPDATE ");
			sql.Append(SqlFrom);
			sql.Append(" SET ");
			sql.Append(SqlSet);

			if (!string.IsNullOrWhiteSpace(SqlWhere))
				sql.AppendFormat(" WHERE {0}", SqlWhere);

			if (!string.IsNullOrWhiteSpace(SqlEnd))
				sql.Append(" ").Append(SqlEnd);

			return GetDbBridgeCommand(sql.ToString());
		}

		public Update<T> Where(SQL<T> sql)
		{
			return this;
		}
	}
}
