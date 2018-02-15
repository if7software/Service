using System;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using DbBridge.Attributes;
using DbBridge.Interfaces;
using DbBridge.DataManipulation;
using MySql.Data.MySqlClient;
using DbBridge.Classes;

namespace DbBridge
{
	public class Select<T> : CommandQuery<T> where T : class, IEntity, new()
	{
		private string SqlSelectFormat { get; set; }
		private string SqlColumns { get; set; }
		private string SqlFrom { get; set; }
		private string SqlWhere { get; set; }
		private string SqlOrderBy { get; set; }
		private string SqlHaving { get; set; }
		private string SqlGroupBy { get; set; }
		private string SqlEnd { get; set; }

		public Select(Db mariaDb)
			: base(mariaDb)
		{
			Construnctor(null, null);
		}

		public Select(Db mariaDb, params string[] columns)
			: base(mariaDb)
		{
			Construnctor(null, columns);
		}

		public Select(Db mariaDb, params Alias[] aliases)
			: base(mariaDb)
		{
			string[] columns = aliases.Select(a => a.ToString()).ToArray();

			Construnctor(null, columns);
		}

		public Select(Db mariaDb, string alias, params string[] columns)
			: base(mariaDb)
		{
			Construnctor(alias, columns);
		}

		public Select(Db mariaDb, string alias, params Alias[] aliases)
			: base(mariaDb)
		{
			string[] columns = aliases.Select(a => a.ToString()).ToArray();

			Construnctor(alias, columns);
		}

		private void Construnctor(string alias, params string[] columns)
		{
			Initialize();

			SqlFrom = !string.IsNullOrWhiteSpace(alias) ?
				$"{ alias } { GetTableName() }" :
				GetTableName();

			FindDbColumns(
				attribute => attribute is DbColumn,
				(attribute, property) =>
				{
					DbColumn column = attribute as DbColumn;

					if (columns == null || columns.ToList().Contains(column.Name))
					{
						var separator = string.IsNullOrEmpty(SqlColumns) ? string.Empty : ", ";

						SqlColumns = string.Concat(SqlColumns, separator, column.Name);
					}
				}
			);
		}

		private void Initialize()
		{
			SqlSelectFormat = "{0}";
			SqlColumns = string.Empty;
			SqlWhere = string.Empty;
			SqlGroupBy = string.Empty;
			SqlOrderBy = string.Empty;
			SqlEnd = string.Empty;
		}

		protected override MySqlCommand BuildCommand()
		{
			var sql = new StringBuilder();
			sql.Append("SELECT ");
			sql.AppendFormat(SqlSelectFormat, SqlColumns);
			sql.Append(" FROM ");
			sql.Append(SqlFrom);

			if (!string.IsNullOrWhiteSpace(SqlWhere))
				sql.AppendFormat(" WHERE {0}", SqlWhere);

			if (!string.IsNullOrWhiteSpace(SqlGroupBy))
				sql.AppendFormat(" GROUP BY {0}", SqlGroupBy);

			if (!string.IsNullOrWhiteSpace(SqlHaving))
				sql.AppendFormat(" HAVING {0}", SqlHaving);

			if (!string.IsNullOrWhiteSpace(SqlOrderBy))
				sql.AppendFormat(" ORDER BY {0}", SqlOrderBy);

			if (!string.IsNullOrWhiteSpace(SqlEnd))
				sql.Append(" ").Append(SqlEnd);

			return GetDbBridgeCommand(sql.ToString());
		}

		public Select<T> Top(int count)
		{
			SqlEnd = string.Concat("LIMIT ", count);

			return this;
		}

		public Select<T> Count()
		{
			SqlSelectFormat = "COUNT({0})";

			return this;
		}

		public Select<T> Where(Func<SQL<T>, SQL<T>> func)
		{
			Sql = func.Invoke(Sql);
			SqlWhere = Sql.List.FirstOrDefault();

			//SqlWhere = string.IsNullOrEmpty(SqlWhere) ? query : string.Concat("(", SqlWhere, ") AND (", query, ")");

			return this;
		}

		public Select<T> Join<TJoined>(Expression<Func<T, TJoined, bool>> clause)
			where TJoined : IEntity
		{
			//var query = GetQueryFromClause(clause);

			//var tableDefinition = new TableDefinition(typeof(TJoined));

			//var joinedTableName = _translator.GetSelectTableName(tableDefinition);
			//SqlFrom = string.Concat(SqlFrom, " INNER JOIN ", joinedTableName, " ON ", query);

			return this;
		}

		public Select<T> Join<TJoinedLeft, TJoinedRight>(Expression<Func<TJoinedLeft, TJoinedRight, bool>> clause)
		{
			//var query = GetQueryFromClause(clause);

			//var tableDefinition = new TableDefinition(typeof(TJoinedRight));

			//var joinedTableName = _translator.GetSelectTableName(tableDefinition);
			//SqlFrom = string.Concat(SqlFrom, " INNER JOIN ", joinedTableName, " ON ", query);

			return this;
		}

		public Select<T> LeftJoin<TJoined>(Expression<Func<T, TJoined, bool>> clause)
		{
			//var query = GetQueryFromClause(clause);

			//var tableDefinition = new TableDefinition(typeof(TJoined));

			//var joinedTableName = _translator.GetSelectTableName(tableDefinition);
			//SqlFrom = string.Concat(SqlFrom, " LEFT JOIN ", joinedTableName, " ON ", query);

			return this;
		}

		public Select<T> LeftJoin<TJoinedLeft, TJoinedRight>(Expression<Func<TJoinedLeft, TJoinedRight, bool>> clause)
		{
			//var query = GetQueryFromClause(clause);

			//var tableDefinition = new TableDefinition(typeof(TJoinedRight));

			//var joinedTableName = _translator.GetSelectTableName(tableDefinition);
			//SqlFrom = string.Concat(SqlFrom, " LEFT JOIN ", joinedTableName, " ON ", query);

			return this;
		}

		public Select<T> RightJoin<TJoined>(Expression<Func<T, TJoined, bool>> clause)
		{
			//var query = GetQueryFromClause(clause);

			//var tableDefinition = new TableDefinition(typeof(TJoined));

			//var joinedTableName = _translator.GetSelectTableName(tableDefinition);
			//SqlFrom = string.Concat(SqlFrom, " RIGHT JOIN ", joinedTableName, " ON ", query);

			return this;
		}

		public Select<T> RightJoin<TJoinedLeft, TJoinedRight>(Expression<Func<TJoinedLeft, TJoinedRight, bool>> clause)
		{
			//var query = GetQueryFromClause(clause);

			//var tableDefinition = new TableDefinition(typeof(TJoinedRight));

			//var joinedTableName = _translator.GetSelectTableName(tableDefinition);
			//SqlFrom = string.Concat(SqlFrom, " RIGHT JOIN ", joinedTableName, " ON ", query);

			return this;
		}

		public Select<T> OrderBy(Expression<Func<T, dynamic>> clause)
		{
			//SqlOrderBy = ExpressionHelper.FormatStringFromArguments(_translator, clause, SqlOrderBy, Parameters);

			return this;
		}

		public Select<T> OrderByDescending(Expression<Func<T, dynamic>> clause)
		{
			//SqlOrderBy = ExpressionHelper.FormatStringFromArguments(_translator, clause, SqlOrderBy, Parameters, _translator.GetOrderByDescendingFormat());

			return this;
		}

		public Select<T> Having(Expression<Func<T, bool>> clause)
		{
			//var query = GetQueryFromClause(clause);

			//SqlHaving = string.IsNullOrEmpty(SqlHaving) ? query : string.Concat("(", SqlHaving, ") AND (", query, ")");

			return this;
		}

		public Select<T> GroupBy(Expression<Func<T, dynamic>> clause)
		{
			//SqlGroupBy = ExpressionHelper.FormatStringFromArguments(_translator, clause, SqlGroupBy, Parameters);

			return this;
		}
	}
}
