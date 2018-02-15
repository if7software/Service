using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace AlertMessage
{
	public class MailAlert
	{
		public int Port { get; set; }
		public string Host { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Recipient { get; set; }

		private string AppName
		{
			get { return ConfigurationSettings.AppSettings["AppName"]; }
		}

		public MailAlert(string host, int port)
		{
			Host = host;
			Port = port;
		}

		public void Send(MailAlertType type, string message)
		{
			MailAlertMessages.IMessage alertMessage = null;

			SwitchAlertMessage(
				type,
				() => { alertMessage = new MailAlertMessages.WarningMessage(message); },
				() => { alertMessage = new MailAlertMessages.ErrorMessage(message); },
				() => { alertMessage = new MailAlertMessages.FatalMessage(message); }
			);

			Send(alertMessage);
		}

		public void Send(MailAlertType type, Exception exception)
		{
			MailAlertMessages.IMessage alertMessage = null;

			SwitchAlertMessage(
				type,
				() => { alertMessage = new MailAlertMessages.WarningMessage(exception); },
				() => { alertMessage = new MailAlertMessages.ErrorMessage(exception); },
				() => { alertMessage = new MailAlertMessages.FatalMessage(exception); }
			);

			Send(alertMessage);
		}

		public void Send(MailAlertType type, string message, Exception exception)
		{
			MailAlertMessages.IMessage alertMessage = null;

			SwitchAlertMessage(
				type,
				() => { alertMessage = new MailAlertMessages.WarningMessage(message, exception); },
				() => { alertMessage = new MailAlertMessages.ErrorMessage(message, exception); },
				() => { alertMessage = new MailAlertMessages.FatalMessage(message, exception); }
			);

			Send(alertMessage);
		}

		private void SwitchAlertMessage(MailAlertType type, Action warning, Action error, Action fatal)
		{
			switch (type)
			{
				case MailAlertType.Warning:
					warning.Invoke();
					break;

				case MailAlertType.Error:
					error.Invoke();
					break;

				case MailAlertType.Fatal:
					fatal.Invoke();
					break;
			}
		}

		private bool Send(MailAlertMessages.IMessage instance)
		{
			if (instance == null || string.IsNullOrWhiteSpace(instance.Subject))
				return false;

			MailMessage message = new MailMessage();
			message.From = new MailAddress(Username, AppName);
			message.To.Add(new MailAddress(Recipient));
			message.Subject = instance.Subject;
			message.Body = !string.IsNullOrWhiteSpace(instance.Body)
				? instance.Body : string.Empty;
			message.IsBodyHtml = true;

			try
			{
				SmtpClient client = new SmtpClient(Host, Port);
				client.Credentials = new NetworkCredential(Username, Password);
				client.UseDefaultCredentials = true;
				client.EnableSsl = true;
				client.Timeout = 60000;
				client.Send(message);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
