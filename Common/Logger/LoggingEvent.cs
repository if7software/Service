using Core;
using System;

namespace Logger
{
	public class LoggingEvent
	{
		private static string _log4NetName = typeof(LoggingEvent).FullName;
		private static object _syncObject = new Object();
		private static string _loggerPath;
		private static string _serviceName;

		private static Log4NetHelper _instance;
		public static Log4NetHelper Instance
		{
			get
			{
				DateTime dt = DateTime.Now;

				if (_instance != null && dt.Day != _instance.CreationInstanceDate.Day)
				{
					_instance.Dispose();
					_instance = null;
				}

				if (_instance == null)
					lock (_syncObject)
					{
						if (_instance == null)
							_instance = new Log4NetHelper(_log4NetName, _loggerPath, _serviceName);
					}

				return _instance;
			}
		}

		public static void CreateLogger(string loggerPath, string serviceName)
		{
			_loggerPath = loggerPath;
			_serviceName = serviceName;
		}

		private static string LogMessage(string msgEx)
		{
			return string.Format("[{1}] - {0}", msgEx, General.GetMethodName(3));
		}

		public static void Debug(string msgEx, Exception ex = null)
		{
			string message = LogMessage(msgEx);

			if (ex == null)
				Instance.Log4Net.Debug(message);
			else
				Instance.Log4Net.Debug(message, ex);
		}

		public static void Information(string msgEx, Exception ex = null)
		{
			string message = LogMessage(msgEx);

			if (ex == null)
				Instance.Log4Net.Info(message);
			else
				Instance.Log4Net.Info(message, ex);
		}

		public static void Warning(string msgEx, Exception ex = null)
		{
			string message = LogMessage(msgEx);

			if (ex == null)
				Instance.Log4Net.Warn(message);
			else
				Instance.Log4Net.Warn(message, ex);
		}

		public static void Error(string msgEx, Exception ex = null)
		{
			string message = LogMessage(msgEx);

			if (ex == null)
				Instance.Log4Net.Error(message);
			else
				Instance.Log4Net.Error(message, ex);
		}

		public static void Fatal(string msgEx, Exception ex = null)
		{
			string message = LogMessage(msgEx);

			if (ex == null)
				Instance.Log4Net.Fatal(message);
			else
				Instance.Log4Net.Fatal(message, ex);
		}
	}
}
