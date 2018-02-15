using System;
using System.IO;
using System.Collections.Generic;
using Coordinator.Core.RobotJob.DataContract;
using Coordinator.Core.RobotJob;
using Coordinator.Core.RobotJob.FunctionFileModifiers;
using System.Text.RegularExpressions;
using Constants;

namespace Coordinator.Core.Automation
{
	public class RobotJobCache
	{
		private static Dictionary<string, object> _syncFile;
		private FileSystemWatcher _watcher;

		private Dictionary<string, Job> _jobs;
		public Dictionary<string, Job> Jobs
		{
			get { return _jobs; }
		}

		public RobotJobCache()
		{
			_jobs = new Dictionary<string, Job>();
			_syncFile = new Dictionary<string, object>();

			foreach (string path in Directory.GetFiles(CommonService.JobsDirectory))
			{
				string name = Path.GetFileNameWithoutExtension(path);

				AddSynObject(name);
				CreateRobotJob(path);
			}

			_watcher = new FileSystemWatcher(CommonService.JobsDirectory, "*.xml");
			_watcher.EnableRaisingEvents = true;

			_watcher.Created += new FileSystemEventHandler(CreatedSomeFile);
			_watcher.Deleted += new FileSystemEventHandler(DeletedSomeFile);
			_watcher.Changed += new FileSystemEventHandler(ChangedSomeFile);
		}

		private void CreatedSomeFile(object sender, FileSystemEventArgs e)
		{
			CreateRobotJob(e.FullPath);
		}

		private void DeletedSomeFile(object sender, FileSystemEventArgs e)
		{
			DeleteRobotJob(e.FullPath);
		}

		private void ChangedSomeFile(object sender, FileSystemEventArgs e)
		{
			ChangedRobotJob(e.FullPath);
		}

		private void CreateRobotJob(string path)
		{
			string guid = GetIdOnRobotJobFile(path);

			lock (GetSyncObject(guid))
			{
				if (!File.Exists(path))
					throw new Exception("The job was not found, but it should be.");

				Job job = RobotJobManager.Parse(path);
				_jobs[guid] = job;
			}
		}

		private void DeleteRobotJob(string path)
		{
			string guid = GetIdOnRobotJobFile(path);

			lock (GetSyncObject(guid))
			{
				if (_jobs.ContainsKey(guid))
					_jobs.Remove(guid);
			}
		}

		private void ChangedRobotJob(string path)
		{
			string guid = GetIdOnRobotJobFile(path);

			lock (GetSyncObject(guid))
			{
				if (!File.Exists(path))
					throw new Exception("The job was not found, but it should be.");

				Job job = RobotJobManager.Parse(path);

				if (_jobs.ContainsKey(guid))
					_jobs[guid] = null;

				_jobs[guid] = job;
			}
		}

		private void AddSynObject(string str)
		{
			string guid = null;

			if (Regex.IsMatch(str, "^[a-f0-9]{32}$"))
				guid = str;
			else if (Regex.IsMatch(str, "^[a-z0-9]+_[a-f0-9]{32}$"))
				guid = GetIdOnRobotJobFile(str);
			else
				throw new Exception("String is not guid and file name.");

			if (!string.IsNullOrWhiteSpace(guid))
				_syncFile.Add(guid, new object());
		}

		private object GetSyncObject(string guid)
		{
			object sync = _syncFile[guid];

			return sync;
		}

		private string GetRobotJobFileName(Globals.BackgroundAppType type, out string guid)
		{
			Guid id = Guid.NewGuid();
			guid = id.ToString("N");

			return $"{ type.ToString().ToLower() }_{ guid }";
		}

		private string GetIdOnRobotJobFile(string path)
		{
			string name = Path.GetFileNameWithoutExtension(path);
			return name.Substring(name.IndexOf('_') + 1);
		}

		private Globals.BackgroundAppType GetTypeOnRobotJobFile(string path)
		{
			Globals.BackgroundAppType type;
			string name = Path.GetFileNameWithoutExtension(path);

			if (!Enum.TryParse(name.Substring(0, name.IndexOf('_')).ToUpper(), out type))
				throw new Exception($"Unable to convert type: { path }");

			return type;
		}

		public string Create(Globals.BackgroundAppType type, DateTime dt, Dictionary<string, object> parameters)
		{
			try
			{
				string guid = null;
				string name = GetRobotJobFileName(type, out guid);

				AddSynObject(guid);

				lock (GetSyncObject(guid))
				{
					string file = Path.Combine(CommonService.JobsDirectory, $"{ name }.xml");

					RobotJobManager.Save(file, name, type, dt, parameters);
				}

				return name;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Delete(string name)
		{
			try
			{
				string guid = GetIdOnRobotJobFile(name);

				if (!_jobs.ContainsKey(guid))
					throw new Exception("The job was not found, but it should be.");

				lock (GetSyncObject(guid))
				{
					string file = Path.Combine(CommonService.JobsDirectory, $"{ name }.xml");

					if (File.Exists(file))
						File.Delete(file);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void AddAttempt(string name, Exception exception = null)
		{
			try
			{
				string guid = GetIdOnRobotJobFile(name);

				if (!_jobs.ContainsKey(guid))
					throw new Exception("The job was not found, but it should be.");

				lock (GetSyncObject(guid))
				{
					string file = Path.Combine(CommonService.JobsDirectory, $"{ name }.xml");

					RobotJobManager.Save(file, new AddAttemptRobotJob()
					{
						ErrorMessage = exception == null ?
							null : $"{ exception.Message }\n{ exception.StackTrace }"
					});
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
