using Coordinator.Constants;
using System;
using System.Xml.Serialization;

namespace Coordinator.Core.RobotJob.DataContract
{
	public class Attempt
	{
		[XmlElement(ElementName = Consts.Function.DateElement)]
		public DateTime Date { get; set; }

		[XmlElement(ElementName = Consts.Function.ErrorMessageElement)]
		public string ErrorMessage { get; set; }
	}
}
