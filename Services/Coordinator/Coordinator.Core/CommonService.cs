using AlertMessage;
using Core;
using Coordinator.Constants;
using Coordinator.Core.Providers;
using Logger;
using System;
using System.IO;
using System.Configuration;

namespace Coordinator.Core
{
	public static class CommonService
	{
		private static Configuration _config;
		private static ServiceProperties _serviceProperties;
		private static EventProperties _eventProperties;

		public static void Initialize()
		{
			// W pierszej kolejności!!!
			_config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

			LoggingEvent.CreateLogger(LogsDirectory, AppName);

			_serviceProperties = new ServiceProperties(ServiceConfigDirectory);
			_serviceProperties.Initialize();

			_eventProperties = new EventProperties(ServiceConfigDirectory);
			_eventProperties.Initialize();
		}

		#region Logger
		public static void LogDebug(string message)
		{
			LoggingEvent.Debug(message);
		}

		public static void LogDebug(string message, Exception exception)
		{
			LoggingEvent.Debug(message, exception);
		}

		public static void Log(string message)
		{
			LoggingEvent.Information(message);
		}

		public static void Log(string message, Exception exception)
		{
			LoggingEvent.Information(message, exception);
		}

		public static void LogWarning(string message, bool sendMail = false)
		{
			LoggingEvent.Warning(message);

			if (sendMail)
				SendAlertMessage(MailAlertType.Warning, message);
		}

		public static void LogWarning(string message, Exception exception, bool sendMail = false)
		{
			LoggingEvent.Warning(message, exception);

			if (sendMail)
				SendAlertMessage(MailAlertType.Warning, message, exception);
		}

		public static void LogError(string message, bool sendMail = false)
		{
			LoggingEvent.Error(message);

			if (sendMail)
				SendAlertMessage(MailAlertType.Error, message);
		}

		public static void LogError(string message, Exception exception, bool sendMail = false)
		{
			LoggingEvent.Error(message);

			if (sendMail)
				SendAlertMessage(MailAlertType.Error, message, exception);
		}

		public static void LogFatal(string message, bool sendMail = false)
		{
			LoggingEvent.Fatal(message);

			if (sendMail)
				SendAlertMessage(MailAlertType.Fatal, message);
		}

		public static void LogFatal(string message, Exception exception, bool sendMail = false)
		{
			LoggingEvent.Fatal(message);

			if (sendMail)
				SendAlertMessage(MailAlertType.Fatal, message, exception);
		}
		#endregion // Logger

		#region Send Alert Message
		public static void SendAlertMessage(MailAlertType type, string message)
		{
			MailAlert mailAlert = new MailAlert(AlertMessageHost, AlertMessagePort)
			{
				Username = AlertMessageUsername,
				Password = AlertMessagePassword,
				Recipient = AlertMessageTo
			};

			mailAlert.Send(type, message);
		}

		public static void SendAlertMessage(MailAlertType type, Exception exception)
		{
			MailAlert mailAlert = new MailAlert(AlertMessageHost, AlertMessagePort)
			{
				Username = AlertMessageUsername,
				Password = AlertMessagePassword,
				Recipient = AlertMessageTo
			};

			mailAlert.Send(type, exception);
		}

		public static void SendAlertMessage(MailAlertType type, string message, Exception exception)
		{
			MailAlert mailAlert = new MailAlert(AlertMessageHost, AlertMessagePort)
			{
				Username = AlertMessageUsername,
				Password = AlertMessagePassword,
				Recipient = AlertMessageTo
			};

			mailAlert.Send(type, message, exception);
		}
		#endregion // Send Alert Message

		public static string AppName
		{
			get { return _config.AppSettings.Settings["AppName"].Value; }
		}

		public static string ServiceConfigDirectory
		{
			get
			{
				string appName = General.GetConfigurationSetting("AppName");

				if (string.IsNullOrWhiteSpace(appName))
					throw new Exception("The service should have a name in app.config");

				return Path.Combine(General.ConfigDirectory, appName);
			}
		}

		public static string JobsDirectory
		{
			get
			{
				string path = Path.Combine(ServiceConfigDirectory, "Jobs");
				UtilitiesIO.CheckOrCreateDirectory(path);
				return path;
			}
		}

		public static string LogsDirectory
		{
			get
			{
				string path = Path.Combine(ServiceConfigDirectory, "Logs");
				UtilitiesIO.CheckOrCreateDirectory(path);
				return path;
			}
		}

		#region Alert Message
		public static string AlertMessageHost
		{
			get
			{
				if (_serviceProperties.PropertyExist(ServicePropertyParameters.AlertMessageHost))
					return _serviceProperties.GetProperty<string>(ServicePropertyParameters.AlertMessageHost);

				return string.Empty;
			}
		}

		public static int AlertMessagePort
		{
			get
			{
				if (_serviceProperties.PropertyExist(ServicePropertyParameters.AlertMessagePort))
					return _serviceProperties.GetProperty<int>(ServicePropertyParameters.AlertMessagePort);

				return 0;
			}
		}

		public static string AlertMessageUsername
		{
			get
			{
				if (_serviceProperties.PropertyExist(ServicePropertyParameters.AlertMessageUsername))
					return _serviceProperties.GetProperty<string>(ServicePropertyParameters.AlertMessageUsername);

				return string.Empty;
			}
		}

		public static string AlertMessagePassword
		{
			get
			{
				if (_serviceProperties.PropertyExist(ServicePropertyParameters.AlertMessagePassword))
					return _serviceProperties.GetProperty<string>(ServicePropertyParameters.AlertMessagePassword);

				return string.Empty;
			}
		}

		public static string AlertMessageTo
		{
			get
			{
				if (_serviceProperties.PropertyExist(ServicePropertyParameters.AlertMessageTo))
					return _serviceProperties.GetProperty<string>(ServicePropertyParameters.AlertMessageTo);

				return string.Empty;
			}
		}
		#endregion // Alert Message
	}
}
