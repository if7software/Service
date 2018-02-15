using Microsoft.Win32.TaskScheduler;

namespace Practical.Scheduler
{
	public class ShedulerWin
	{
		public void CreateTask()
		{
			using (TaskService ts = new TaskService())
			{
				TaskDefinition td = ts.NewTask();
				td.RegistrationInfo.Description = "Does something";
				td.Triggers.Add(new DailyTrigger { DaysInterval = 2 });
				td.Actions.Add(new ExecAction("notepad.exe", "c:\\test.log", null));
				ts.RootFolder.RegisterTaskDefinition(@"Test", td);
				ts.RootFolder.DeleteTask("Test");
			}
		}
	}
}
