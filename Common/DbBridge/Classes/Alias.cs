using System;

namespace DbBridge.Classes
{
	public class Alias
	{
		private string _alias;
		private string _column;

		public Alias(string column, string alias)
		{
			if (string.IsNullOrEmpty(alias))
				throw new ArgumentException("Alias can not be empty.");

			_alias = alias;
			_column = column;
		}

		public override string ToString()
		{
			return $"{ _alias }.{ _column }";
		}
	}
}
