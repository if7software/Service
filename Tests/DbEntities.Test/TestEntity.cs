using DbBridge.Attributes;
using DbBridge.Interfaces;
using MySql.Data.MySqlClient;
using System;

namespace DbEntities.Test
{
	public class TestEntity : IEntity
	{
		public const string TABLE = "testinho";

		public const string COLUMN_ID = "id";
		public const string COLUMN_SIZE = "size";
		public const string COLUMN_NAME = "name";

		[DbPrimaryKeyColumn(IsNullable = false, Name = COLUMN_ID, Type = MySqlDbType.Int32)]
		public int Id { get; set; }

		[DbColumn(IsNullable = false, Name = COLUMN_NAME, Type = MySqlDbType.VarChar)]
		public string Name { get; set; }

		[DbColumn(IsNullable = true, Name = COLUMN_SIZE, Type = MySqlDbType.Int32)]
		public int? Size { get; set; }

		public void Fill(string name, object value, bool isNull)
		{
			switch (name)
			{
				case COLUMN_ID:
					Id = Convert.ToInt32(value);
					break;
				case COLUMN_NAME:
					Name = Convert.ToString(value);
					break;
				case COLUMN_SIZE:
					Size = isNull ? null : (int?)Convert.ToInt32(value);
					break;
			}
		}
	}
}
