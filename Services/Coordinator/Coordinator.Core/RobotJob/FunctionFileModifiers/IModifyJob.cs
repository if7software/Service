using System.Xml.Linq;

namespace Coordinator.Core.RobotJob.FunctionFileModifiers
{
	public interface IModifyJob
	{
		void Execute(ref XDocument document);
	}
}
