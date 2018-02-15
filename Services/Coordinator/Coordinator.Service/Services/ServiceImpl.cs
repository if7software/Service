using Coordinator.Service.Interfaces;
using Logger;

namespace Coordinator.Service.Services
{
	public class ServiceImpl : IService
	{
		public string SomeOperation(string input)
		{
			LoggingEvent.Debug("Polecenie restowe");

			return "Testinho";
		}
	}
}
