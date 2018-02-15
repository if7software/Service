using System.Configuration;

namespace Core.Sections
{
	public class DbBridgeSection : ConfigurationSection
	{
		[ConfigurationProperty("Databases")]
		public DatabaseCollection Databases
		{
			get { return ((DatabaseCollection)(base["Databases"])); }
		}

		[ConfigurationCollection(typeof(Database))]
		public class DatabaseCollection : ConfigurationElementCollection
		{
			protected override ConfigurationElement CreateNewElement()
			{
				return new Database();
			}

			protected override object GetElementKey(ConfigurationElement element)
			{
				return ((Database)(element)).Key;
			}

			public Database this[int idx]
			{
				get { return (Database)BaseGet(idx); }
			}
		}

		public class Database : ConfigurationElement
		{
			[ConfigurationProperty("key", DefaultValue = "", IsKey = true, IsRequired = true)]
			public string Key
			{
				get { return ((string)(base["key"])); }
				set { base["key"] = value; }
			}

			[ConfigurationProperty("value", DefaultValue = "", IsKey = false, IsRequired = true)]
			public string Value
			{
				get { return ((string)(base["value"])); }
				set { base["value"] = value; }
			}
		}
	}
}
