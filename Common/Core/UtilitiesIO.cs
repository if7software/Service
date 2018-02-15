using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Core
{
	public static class UtilitiesIO
	{
		public static void AppendText(string message, string path)
		{
			using (StreamWriter sw = File.AppendText(path))
			{
				sw.Write(message);
			}
		}

		public static void AppendLineText(string message, string path)
		{
			using (StreamWriter sw = File.AppendText(path))
			{
				sw.WriteLine(message);
			}
		}

		public static string ReadFile(string fileName, Encoding encoding)
		{
			using (TextReader reader = new StreamReader(fileName, encoding))
			{
				return reader.ReadToEnd();
			}
		}

		public static bool ReadAccess(string filePath)
		{
			try
			{
				using (FileStream fs = File.OpenRead(filePath))
				{
					return true;
				}
			}
			catch { }

			return false;
		}

		public static void CheckOrCreateDirectory(string path)
		{
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
		}

		public static void DeleteFile(string path)
		{
			if (!string.IsNullOrEmpty(path) && File.Exists(path))
				File.Delete(path);
		}

		public static string GetRelativePath(string filePath)
		{
			return filePath.Replace(Path.GetPathRoot(filePath), "").TrimStart('\\');
		}
	}
}
