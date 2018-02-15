using System;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using Coordinator.Constants;
using Constants;

namespace Coordinator.Core.RobotJob.DataContract
{
	[XmlRoot(Consts.Function.JobElement)]
	public class Job
	{
		[XmlAttribute(AttributeName = Consts.Function.IdAttribute)]
		public string Id { get; set; }

		[XmlAttribute(AttributeName = Consts.Function.TypeAttribute)]
		public Globals.BackgroundAppType Type { get; set; }

		[XmlAttribute(AttributeName = Consts.Function.StatusAttribute)]
		public RobotJobStatus Status { get; set; }

		[XmlAttribute(AttributeName = Consts.Function.CreationDateAttribute)]
		public DateTime CreationDate { get; set; }

		[XmlAttribute(AttributeName = Consts.Function.ExecutionDateAttribute)]
		public DateTime ExecutionDate { get; set; }

		[XmlArray(Consts.Function.AttemptsElement)]
		public Attempt[] Attempts { get; set; }

		[XmlArray(Consts.Function.ParametersElement)]
		public Parameter[] Parameters { get; set; }

		public Job()
		{
			Attempts = new Attempt[0];
			Parameters = new Parameter[0];
		}

		public void SetParameters(Dictionary<string, object> parameters)
		{
			Parameters = new List<Parameter>(
				parameters.Select(parameter => new Parameter()
				{
					Key = parameter.Key,
					Value = parameter.Value.ToString(),
					Type = parameter.Value.GetType().FullName
				}))
				.ToArray();
		}
	}
}
