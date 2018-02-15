using Core.Sections;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Generic;
using Constants;

namespace Core
{
	public static class General
	{
		private static Configuration _config;

		public static string BaseDirectory { get; private set; }

		public static Dictionary<string, string> ConnectionStringDict { get; private set; }

		public static void Initialize()
		{
			_config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

			BaseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

			SetConnectionString();
		}

		private static void SetConnectionString()
		{
			string sectionName = "DbBridgeSettings";

			DbBridgeSection section = (DbBridgeSection)ConfigurationManager.GetSection(sectionName);

			if (section != null)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();

				foreach (DbBridgeSection.Database variable in section.Databases)
					dictionary[variable.Key] = variable.Value;

				ConnectionStringDict = dictionary
					.Where(connectionString => connectionString.Key.StartsWith(SolutionConfig))
					.ToDictionary(k => GetDatabaseNameFromAppSettings(k.Key), v => v.Value);
			}
			else
			{
				throw new Exception($"Section name: { sectionName } not exist in app.config");
			}
		}

		private static string GetDatabaseNameFromAppSettings(string appSettingsConfig)
		{
			int count1 = $"{ SolutionConfig }.".Length;
			int count2 = appSettingsConfig.Length;

			return appSettingsConfig.Substring(count1, count2 - count1);
		}

		public static string SolutionConfig
		{
#if DEBUG
			get { return Globals.SolutionConfig.Debug; }
#else
			get { return Globals.SolutionConfig.Release; }
#endif
		}

		public static string ConfigDirectory
		{
			get { return Path.Combine(GetRootedPath("OneDrive"), SolutionConfig); }
		}

		public static string GetConfigurationSetting(string key, string defaultValue = null)
		{
			try
			{
				if (_config.AppSettings.Settings.AllKeys.Contains(key))
					return _config.AppSettings.Settings[key].Value;
				else
					return defaultValue;
			}
			catch
			{
				if (string.IsNullOrEmpty(defaultValue))
					return string.Empty;
				else
					return defaultValue;
			}
		}

		public static string GetRootedPath(string path)
		{
			if (Path.IsPathRooted(path))
				return path;

			return Path.Combine(BaseDirectory, path);
		}

		public static string GetRequiredDirectory(string dir)
		{
			dir = GetRootedPath(dir);
			UtilitiesIO.CheckOrCreateDirectory(dir);
			return dir;
		}

		public static string GetMethodName(int integer)
		{
			StackTrace st = new StackTrace();
			StackFrame sf = st.GetFrame(integer);
			MethodBase mb = sf.GetMethod();

			return mb.Name;
		}

		public static string GetMethodName()
		{
			StackTrace st = new StackTrace();
			StackFrame sf = st.GetFrame(1);
			MethodBase mb = sf.GetMethod();

			return mb.Name;
		}

		public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs)
			where T : class, IComparable<T>
		{
			List<T> objects = new List<T>();

			foreach (Type type in
				Assembly.GetAssembly(typeof(T)).GetTypes()
				.Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
			{
				objects.Add((T)Activator.CreateInstance(type, constructorArgs));
			}

			objects.Sort();
			return objects;
		}
	}
}
