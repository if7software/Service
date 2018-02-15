using System.Drawing;
using System.Windows.Forms;

namespace Coordinator.UI
{
	public partial class Tray
	{
		private void InitializeComponent()
		{
			_trayIcon = new NotifyIcon()
			{
				Icon = new Icon("Resources//icon.ico"),
				ContextMenu = new ContextMenu(new MenuItem[] {
					new MenuItem("Restart", OnRestart),
					new MenuItem("Exit", OnExit)
				}),
				Visible = true
			};
		}
	}
}
