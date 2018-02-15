using Constants;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace AlertMessage
{
	public class MailAlertMessages
	{
		public class WarningMessage : IMessage
		{
			public WarningMessage(string message)
				: base(Globals.AlertMessageFiles.Message)
			{
				ReplaceMacro(Globals.AlertMessageMacros.Message, message);
			}

			public WarningMessage(Exception exception)
				: base(Globals.AlertMessageFiles.MessageStackTrace)
			{
				ReplaceMacro(Globals.AlertMessageMacros.Message, exception.Message);
				ReplaceMacro(Globals.AlertMessageMacros.StackTrace, exception.StackTrace);
			}

			public WarningMessage(string message, Exception exception)
				: base(Globals.AlertMessageFiles.MessageStackTrace)
			{
				ReplaceMacro(Globals.AlertMessageMacros.Message, $"{ message } - { exception.Message }");
				ReplaceMacro(Globals.AlertMessageMacros.StackTrace, exception.StackTrace);
			}

			protected override string GetSubject()
			{
				return $"Warning [{ ConfigurationSettings.AppSettings["AppName"] }] - { DateTime.Now }";
			}
		}

		public class ErrorMessage : IMessage
		{
			public ErrorMessage(string message)
				: base(Globals.AlertMessageFiles.Message)
			{
				ReplaceMacro(Globals.AlertMessageMacros.Message, message);
			}

			public ErrorMessage(Exception exception)
				: base(Globals.AlertMessageFiles.MessageStackTrace)
			{
				ReplaceMacro(Globals.AlertMessageMacros.Message, exception.Message);
				ReplaceMacro(Globals.AlertMessageMacros.StackTrace, exception.StackTrace);
			}

			public ErrorMessage(string message, Exception exception)
				: base(Globals.AlertMessageFiles.MessageStackTrace)
			{
				ReplaceMacro(Globals.AlertMessageMacros.Message, $"{ message } - { exception.Message }");
				ReplaceMacro(Globals.AlertMessageMacros.StackTrace, exception.StackTrace);
			}

			protected override string GetSubject()
			{
				return $"Error [{ ConfigurationSettings.AppSettings["AppName"] }] - { DateTime.Now }";
			}
		}

		public class FatalMessage : IMessage
		{
			public FatalMessage(string message)
				: base(Globals.AlertMessageFiles.Message)
			{
				ReplaceMacro(Globals.AlertMessageMacros.Message, message);
			}

			public FatalMessage(Exception exception)
				: base(Globals.AlertMessageFiles.MessageStackTrace)
			{
				ReplaceMacro(Globals.AlertMessageMacros.Message, exception.Message);
				ReplaceMacro(Globals.AlertMessageMacros.StackTrace, exception.StackTrace);
			}

			public FatalMessage(string message, Exception exception)
				: base(Globals.AlertMessageFiles.MessageStackTrace)
			{
				ReplaceMacro(Globals.AlertMessageMacros.Message, $"{ message } - { exception.Message }");
				ReplaceMacro(Globals.AlertMessageMacros.StackTrace, exception.StackTrace);
			}

			protected override string GetSubject()
			{
				return $"Fatal [{ ConfigurationSettings.AppSettings["AppName"] }] - { DateTime.Now }";
			}
		}

		public abstract class IMessage
		{
			public string Subject { get; set; }
			public string Body { get; set; }

			public IMessage(string fileName)
			{
				Subject = GetSubject();

				LoadFromResources(fileName);
				ReplaceMacro(Globals.AlertMessageMacros.Title, Subject);
			}

			private void LoadFromResources(string fileName)
			{
				var executingAssembly = Assembly.GetExecutingAssembly();
				var resource = $"{ GetType().Namespace }.Templates.{ fileName }";
				
				using (Stream stream = executingAssembly.GetManifestResourceStream(resource))
				using (StreamReader reader = new StreamReader(stream))
				{
					Body = reader.ReadToEnd();
				}
			}

			protected void ReplaceMacro(string macro, string value)
			{
				if (!string.IsNullOrWhiteSpace(Body))
					Body = Body.Replace(macro, value);
			}

			protected abstract string GetSubject();
		}
	}
}
