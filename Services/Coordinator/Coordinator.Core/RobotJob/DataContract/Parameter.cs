using Coordinator.Constants;
using System.Xml.Serialization;

namespace Coordinator.Core.RobotJob.DataContract
{
	public class Parameter
	{
		[XmlAttribute(AttributeName = Consts.Function.KeyAttribute)]
		public string Key { get; set; }

		[XmlAttribute(AttributeName = Consts.Function.ValueAttribute)]
		public string Value { get; set; }

		[XmlAttribute(AttributeName = Consts.Function.TypeAttribute)]
		public string Type { get; set; }
	}
}
