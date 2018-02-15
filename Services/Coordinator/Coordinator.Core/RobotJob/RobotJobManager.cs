using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using Coordinator.Core.RobotJob.DataContract;
using Coordinator.Core.RobotJob.FunctionFileModifiers;
using Constants;

namespace Coordinator.Core.RobotJob
{
	public class RobotJobManager
	{
		private static XDocument GetXDocument(string filePath)
		{
			XDocument xDoc = null;

			using (StreamReader strReader = new StreamReader(filePath))
			using (XmlReader xmlReader = XmlReader.Create(strReader))
			{
				xDoc = XDocument.Load(xmlReader);

				xmlReader.Close();
				strReader.Close();
			}

			if (xDoc == null)
				return null;

			return xDoc;
		}

		private static void Save(XDocument xDoc, string path)
		{
			using (StreamWriter sw = new StreamWriter(path))
			{
				xDoc.Save(sw);
				sw.Close();
			}
		}

		public static Job Parse(string path)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Job));

			Job function;

			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			using (XmlReader reader = XmlReader.Create(fileStream))
			{
				function = (Job)serializer.Deserialize(reader);
				fileStream.Close();
			}

			return function;
		}

		public static void Save(string path, IModifyJob action)
		{
			try
			{
				XDocument xDoc = GetXDocument(path);
				action.Execute(ref xDoc);

				Save(xDoc, path);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Exception when saving package xml: {0}", ex.Message));
			}
		}

		public static void Save(string path, string guid, Globals.BackgroundAppType type, DateTime executionDate, Dictionary<string, object> parameters)
		{
			var job = new Job()
			{
				Id = guid,
				Type = type,
				Status = RobotJobStatus.ACTIVE,
				CreationDate = DateTime.Now,
				ExecutionDate = executionDate
			};

			job.SetParameters(parameters);

			XmlSerializer SerializerObj = new XmlSerializer(typeof(Job));
			using (TextWriter writer = new StreamWriter(path))
			{
				SerializerObj.Serialize(writer, job);
				writer.Close();
			}
		}
	}
}
