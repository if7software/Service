using System;
using System.Windows.Forms;
using Core;
using Coordinator.Robot;
using Coordinator.Service;
using Self = System.Windows.Forms.Application;

namespace Coordinator.UI
{
	public partial class Tray : ApplicationContext
	{
		private ServiceRest _rest;
		private ServiceCore _core;
		private NotifyIcon _trayIcon;

		public Tray()
		{
			InitializeComponent();

			try
			{
				General.Initialize();
			}
			catch { }

			_core = new ServiceCore();
			_core.StartService();

			_rest = new ServiceRest();
			_rest.OpenHost();
		}

		private void OnRestart(object sender, EventArgs e)
		{
			_core.EndService();
			_core.StartService();
		}

		private void OnExit(object sender, EventArgs e)
		{
			_core.EndService();

			_trayIcon.Visible = false;
			Self.Exit();
		}
	}
}
