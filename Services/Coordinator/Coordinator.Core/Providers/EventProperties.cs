using Constants;
using Coordinator.Constants;
using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;

namespace Coordinator.Core.Providers
{
	public class EventProperties
	{
		public enum RecurringEvent
		{
			EveryWeek,
			EverySecondWeek,
			EveryFourthWeek,
			EveryEighthWeek
		}

		public enum DaysOfWeek
		{
			Monday,
			Tuesday,
			Wednesday,
			Thursday,
			Friday,
			Saturday,
			Sunday
		}

		private static object _syncObject = new object();

		private string _filePath;
		private bool _initialized;
		private FileSystemWatcher _watcher;

		private Dictionary<string, EventProperty> _events;
		public Dictionary<string, EventProperty> Events
		{
			get { return _events; }
		}

		public EventProperties(string dirPath)
		{
			_initialized = false;
			_filePath = Path.Combine(dirPath, Globals.ConfigurationFiles.EventProperties);

			_events = new Dictionary<string, EventProperty>();

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

			ParseEventProperties();
		}

		public void Initialize()
		{
			ParseEventProperties();
		}

		private void ParseEventProperties()
		{
			XmlDocument doc = new XmlDocument();

			lock (_syncObject)
			{
				try
				{
					List<string> propertiesToRemove = _events.Keys.ToList();

					UtilitiesIO.WaitForReadAccess(_filePath, new TimeSpan(0, 0, 15), new TimeSpan(0, 0, 0, 0, 500));

					doc.Load(_filePath);
					XmlNode paramsNode = doc.SelectSingleNode(Consts.EventProperties.Root);

					if (paramsNode == null)
						throw new Exception("Root param have not child nodes.");

					foreach (XmlNode node in paramsNode.ChildNodes)
					{
						if (node.NodeType != XmlNodeType.Element)
							continue;

						if (node.Name != Consts.EventProperties.Element)
							continue;

						string key = null;
						EventProperty eventProperty = new EventProperty();

						try
						{
							key = node.Attributes[Consts.EventProperties.KeyAttribute].Value;

							RecurringEvent recurring;
							List<TimeSpan> times = new List<TimeSpan>();
							List<DaysOfWeek> daysOfWeek = new List<DaysOfWeek>();

							if (!Enum.TryParse(node.Attributes[Consts.EventProperties.RecurringEventAttribute].Value, out recurring))
								throw new Exception("Recurring is not enum");

							foreach (string day in node
								.Attributes[Consts.EventProperties.DaysOfWeekAttribute].Value.Split(',').Select(d => d.Trim()))
							{
								DaysOfWeek dayEnum;

								if (!Enum.TryParse(day, out dayEnum))
									throw new Exception("DayOfWeek is not enum");

								daysOfWeek.Add(dayEnum);
							}

							foreach (string time in node
								.Attributes[Consts.EventProperties.TimeAttribute].Value.Split(',').Select(t => t.Trim()))
							{
								TimeSpan timeEnum;

								if (!Enum.TryParse(time, out timeEnum))
									throw new Exception("Times is not enum");

								times.Add(timeEnum);
							}

							eventProperty.Name = key;
							eventProperty.Recurring = recurring;
							eventProperty.Times = times.ToArray();
							eventProperty.DaysOfWeeks = daysOfWeek.ToArray();

							if (_events.ContainsKey(key))
							{
								propertiesToRemove.Remove(key);

								_events.Remove(key);
								_events.Add(key, eventProperty);
							}
							else
								_events.Add(key, eventProperty);
						}
						catch { }
					}

					if (propertiesToRemove.Count > 0)
					{
						foreach (string pName in propertiesToRemove)
							_events.Remove(pName);
					}

					if (!_initialized)
						_initialized = true;
				}
				catch { }
			}
		}

		public void Dispose()
		{
			_watcher.EnableRaisingEvents = false;
			_watcher.Dispose();
			_watcher = null;
		}

		public class EventProperty
		{
			public string Name { get; set; }
			public RecurringEvent Recurring { get; set; }
			public DaysOfWeek[] DaysOfWeeks { get; set; }
			public TimeSpan[] Times { get; set; }
		}
	}
}
