using MySql.Data.MySqlClient;
using System;

namespace DbBridge.Attributes
{
	public class DbColumn : Attribute
	{
		public string Name { get; set; }
		public bool IsNullable { get; set; }
		public MySqlDbType Type { get; set; }

		public string Parameter
		{
			get { return $"@{ Name }"; }
		}

		public DbColumn()
		{
			Name = string.Empty;
			IsNullable = false;
			Type = MySqlDbType.String;
		}
	}
}
