using Core;
using Constants;
using Coordinator.Constants;
using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace Coordinator.Core.Providers
{
	public class ServiceProperties
	{
		private Dictionary<string, Tuple<string, string>> _currentProperties;

		private static object _syncObject = new object();

		private string _filePath;
		private bool _initialized;
		private FileSystemWatcher _watcher;

		public int PropertiesCount
		{
			get { return _currentProperties.Count; }
		}

		public ServiceProperties(string dirPath)
		{
			_initialized = false;
			_filePath = Path.Combine(dirPath, Globals.ConfigurationFiles.ServiceProperties);

			_currentProperties = new Dictionary<string, Tuple<string, string>>();

			string dir = Path.GetDirectoryName(_filePath);
			string fileName = Path.GetFileName(_filePath);

			_watcher = new FileSystemWatcher(dir, fileName);
			_watcher.EnableRaisingEvents = true;
			_watcher.NotifyFilter = NotifyFilters.LastWrite;
			_watcher.Changed += new FileSystemEventHandler(WatcherChanged);
		}

		void WatcherChanged(object sender, FileSystemEventArgs e)
		{
			Thread.Sleep(200);

			ParseExternalProperties();
		}

		public void Initialize()
		{
			ParseExternalProperties();
		}

		private void ParseExternalProperties()
		{
			XmlDocument doc = new XmlDocument();

			lock (_syncObject)
			{
				try
				{
					List<string> propertiesToRemove = _currentProperties.Keys.ToList();

					UtilitiesIO.WaitForReadAccess(_filePath, new TimeSpan(0, 0, 15), new TimeSpan(0, 0, 0, 0, 500));

					doc.Load(_filePath);
					XmlNode paramsNode = doc.SelectSingleNode(Consts.ServiceProperties.Root);

					if (paramsNode == null)
						throw new Exception("Root param have not child nodes.");

					foreach (XmlNode node in paramsNode.ChildNodes)
					{
						if (node.NodeType != XmlNodeType.Element)
							continue;

						if (node.Name != Consts.ServiceProperties.Element)
							continue;

						string name = null;
						string value = null;
						string type = null;

						try
						{
							name = node.Attributes[Consts.ServiceProperties.KeyAttribute].Value;
							value = node.Attributes[Consts.ServiceProperties.ValueAttribute].Value;
							type = node.Attributes[Consts.ServiceProperties.TypeAttribute].Value;

							if (!Consts.ServicePropertyTypes.Types.Contains(type))
								continue;

							if (_currentProperties.ContainsKey(name))
							{
								propertiesToRemove.Remove(name);

								_currentProperties.Remove(name);
								_currentProperties.Add(name, new Tuple<string, string>(value, type));
							}
							else
								_currentProperties.Add(name, new Tuple<string, string>(value, type));
						}
						catch { }
					}

					if (propertiesToRemove.Count > 0)
					{
						foreach (string pName in propertiesToRemove)
							_currentProperties.Remove(pName);
					}

					if (!_initialized)
						_initialized = true;
				}
				finally
				{
					
				}
			}
		}

		public bool PropertyExist(string propertyName)
		{
			lock (_syncObject)
			{
				if (!_initialized || PropertiesCount <= 0)
					return false;

				return _currentProperties.ContainsKey(propertyName);
			}
		}

		public T GetProperty<T>(string propertyName)
		{
			if (!_initialized)
				throw new InvalidOperationException("ExternalProperties wasn't initialized");

			Tuple<string, string> result = null;

			lock (_syncObject)
			{
				if (!_currentProperties.TryGetValue(propertyName, out result))
					throw new KeyNotFoundException(String.Format("Property: {0} was not declared in file: {1}", propertyName, _filePath));

				if (!typeof(T).FullName.Equals(result.Item2))
					throw new Exception("Wrong type in external property");

				return (T)Convert.ChangeType(result.Item1, typeof(T));
			}
		}

		public void Dispose()
		{
			_watcher.EnableRaisingEvents = false;
			_watcher.Dispose();
			_watcher = null;
		}
	}
}
