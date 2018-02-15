using DbBridge.DataManipulation;
using DbBridge.Interfaces;
using System.Collections.Generic;

namespace DbBridge.Classes
{
	public class SQL<T> where T : class, IEntity
	{
		private Command<T> _command;

		private HashSet<string> _list;
		public HashSet<string> List
		{
			get { return _list; }
		}

		public SQL(Command<T> command)
		{
			_command = command;

			_list = new HashSet<string>();
		}

		private void And()
		{
			string query = $"({ string.Join(" AND ", _list) })";
			MergeQuery(query);
		}

		private void Or()
		{
			string query = $"({ string.Join(" OR ", _list) })";
			MergeQuery(query);
		}

		private void MergeQuery(string query)
		{
			_list.Clear();
			_list.Add(query);
		}

		public SQL<T> And(SQL<T> sql1, SQL<T> sql2)
		{
			And();
			return this;
		}

		public SQL<T> And(SQL<T> sql1, SQL<T> sql2, SQL<T> sql3)
		{
			And();
			return this;
		}

		public SQL<T> And(SQL<T> sql1, SQL<T> sql2, SQL<T> sql3, SQL<T> sql4)
		{
			And();
			return this;
		}

		public SQL<T> And(SQL<T> sql1, SQL<T> sql2, SQL<T> sql3, SQL<T> sql4, SQL<T> sql5)
		{
			And();
			return this;
		}

		public SQL<T> Or(SQL<T> sql1, SQL<T> sql2)
		{
			Or();
			return this;
		}

		public SQL<T> Or(SQL<T> sql1, SQL<T> sql2, SQL<T> sql3)
		{
			Or();
			return this;
		}

		public SQL<T> Or(SQL<T> sql1, SQL<T> sql2, SQL<T> sql3, SQL<T> sql4)
		{
			Or();
			return this;
		}

		public SQL<T> Or(SQL<T> sql1, SQL<T> sql2, SQL<T> sql3, SQL<T> sql4, SQL<T> sql5)
		{
			Or();
			return this;
		}

		public SQL<T> Not(string columnName, object value, string alias = null)
		{
			return this;
		}

		public SQL<T> IsNull(string columnName, string alias = null)
		{
			if (string.IsNullOrWhiteSpace(alias))
				_list.Add($"{ columnName } IS NULL");
			else
				_list.Add($"{ alias }.{ columnName } IS NULL");

			return this;
		}

		public SQL<T> IsNotNull(string columnName, string alias = null)
		{
			if (string.IsNullOrWhiteSpace(alias))
				_list.Add($"{ columnName } IS NOT NULL");
			else
				_list.Add($"{ alias }.{ columnName } IS NOT NULL");

			return this;
		}

		public SQL<T> Equal(string columnName, object value, string alias = null)
		{
			var column = _command.AddDbParamater(columnName, value);

			if (string.IsNullOrWhiteSpace(alias))
				_list.Add($"{ column.Name } = { column.Parameter }");
			else
				_list.Add($"{ alias }.{ column.Name } = { column.Parameter }");

			return this;
		}

		public SQL<T> GreaterThan(string columnName, object value, string alias = null)
		{
			return this;
		}

		public SQL<T> LessThan(string columnName, object value, string alias = null)
		{
			return this;
		}

		public SQL<T> GreaterThanOrEqual(string columnName, object value, string alias = null)
		{
			return this;
		}

		public SQL<T> LessThanOrEqual(string columnName, object value, string alias = null)
		{
			return this;
		}

		public SQL<T> Like(string columnName, object value, string alias = null)
		{
			return this;
		}

		public SQL<T> ILike(string columnName, object value, string alias = null)
		{
			return this;
		}

		public SQL<T> Between(string columnName, object value, string alias = null)
		{
			return this;
		}

		public SQL<T> SimilarTo(string columnName, object value, string alias = null)
		{
			return this;
		}

		public SQL<T> Overlaps(string columnName, object value, string alias = null)
		{
			return this;
		}
	}
}
