using Coordinator.UI;
using System.Windows.Forms;

namespace Coordinator
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Tray());
		}
	}
}
