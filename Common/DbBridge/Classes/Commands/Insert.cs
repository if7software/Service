using System.Linq;
using System.Text;
using DbBridge.Attributes;
using DbBridge.Interfaces;
using DbBridge.DataManipulation;
using MySql.Data.MySqlClient;

namespace DbBridge
{
	public class Insert<T> : CommandExec<T> where T : class, IEntity
	{
		private string SqlFrom { get; set; }
		private string SqlColumns { get; set; }
		private string SqlValues { get; set; }

		public Insert(Db mariaDb, T entity)
			: base(mariaDb)
		{
			Initialize();

			SqlFrom = GetTableName();

			FindDbColumns(
				attribute => attribute is DbColumn,
				(attribute, property) =>
				{
					DbColumn column = attribute as DbColumn;

					var separator = string.IsNullOrEmpty(SqlColumns) ? string.Empty : ", ";

					SqlColumns = string.Concat(SqlColumns, separator, column.Name);
					SqlValues = string.Concat(SqlValues, separator, column.Parameter);

					AddDbParamater(column, property.GetValue(entity));
				}
			);
		}

		private void Initialize()
		{
			SqlColumns = string.Empty;
			SqlValues = string.Empty;
		}

		protected override MySqlCommand BuildCommand()
		{
			var sql = new StringBuilder();
			sql.Append("INSERT INTO ");
			sql.Append(SqlFrom);

			if (!string.IsNullOrWhiteSpace(SqlColumns))
				sql.AppendFormat(" ({0})", SqlColumns);

			sql.Append(" VALUES ");

			if (!string.IsNullOrWhiteSpace(SqlValues))
				sql.AppendFormat("({0})", SqlValues);

			return GetDbBridgeCommand(sql.ToString());
		}
	}
}
