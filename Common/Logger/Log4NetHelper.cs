using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository;
using System;
using System.IO;

namespace Logger
{
	public class Log4NetHelper
	{
		//[%c]
		private const string PATTERN_LAYOUT = "%date{dd/MM/yyyy HH:mm:ss} %-5level - %message%newline";

		public DateTime CreationInstanceDate { get; set; }
		public ILog Log4Net { get; set; }

		public Log4NetHelper(string log4NetName, string loggerPath, string serviceName)
		{
			CreationInstanceDate = DateTime.Now;

			ILoggerRepository repository = LogManager.CreateRepository(Guid.NewGuid().ToString());

			RollingFileAppender fileAppender = new RollingFileAppender();
			fileAppender.Layout = new PatternLayout(PATTERN_LAYOUT);
			fileAppender.File = Log4NetFileName(loggerPath, serviceName);
			fileAppender.AppendToFile = true;
			fileAppender.StaticLogFileName = true;
			fileAppender.DatePattern = ".dd/MM/yyyy";
			fileAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
			fileAppender.LockingModel = new FileAppender.MinimalLock();
			fileAppender.ActivateOptions();

			ColoredConsoleAppender consoleAppender = new ColoredConsoleAppender();
			consoleAppender.AddMapping(GetConsoleLevel(Level.Fatal, ColoredConsoleAppender.Colors.Purple));
			consoleAppender.AddMapping(GetConsoleLevel(Level.Error, ColoredConsoleAppender.Colors.Red));
			consoleAppender.AddMapping(GetConsoleLevel(Level.Warn, ColoredConsoleAppender.Colors.Yellow));
			consoleAppender.AddMapping(GetConsoleLevel(Level.Info, ColoredConsoleAppender.Colors.White));
			consoleAppender.AddMapping(GetConsoleLevel(Level.Debug, ColoredConsoleAppender.Colors.HighIntensity));
			consoleAppender.Layout = new PatternLayout(PATTERN_LAYOUT);
			consoleAppender.ActivateOptions();

			BasicConfigurator.Configure(repository, fileAppender);
			BasicConfigurator.Configure(repository, consoleAppender);
			Log4Net = LogManager.GetLogger(repository.Name, log4NetName);
		}

		public void Dispose()
		{

		}

		private ColoredConsoleAppender.LevelColors GetConsoleLevel(Level level, ColoredConsoleAppender.Colors color)
		{
			return new ColoredConsoleAppender.LevelColors()
			{
				Level = level,
				ForeColor = color
			};
		}

		private string Log4NetFileName(string loggerPath, string serviceName)
		{
			return Path.Combine(
				loggerPath,
				serviceName,
				DateTime.Today.Year.ToString(),
				DateTime.Today.Month.ToString("d2"),
				string.Format("{0:yyyy_MM_dd}.log", DateTime.Today)
				);
		}
	}
}
