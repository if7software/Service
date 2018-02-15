using System;
using System.Xml.Linq;
using Coordinator.Constants;

namespace Coordinator.Core.RobotJob.FunctionFileModifiers
{
	public class AddAttemptRobotJob : IModifyJob
	{
		public string ErrorMessage { get; set; }

		public void Execute(ref XDocument document)
		{
			XElement attempts = document.Root.Element(Consts.Function.AttemptsElement);

			if (attempts == null)
				throw new Exception(string.Format("Missing {0} element.", Consts.Function.AttemptsElement));

			attempts.Add(new XElement(Consts.Function.AttemptElement,
				new XElement(Consts.Function.DateElement, DateTime.Now),
				new XElement(Consts.Function.DateElement, ErrorMessage)
			));
		}
	}
}
