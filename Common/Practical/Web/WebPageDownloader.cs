using System.Net;

namespace Practical.Web
{
	public static class WebPageDownloader
	{
		public static string GetString(string url)
		{
			string htmlCode = null;

			using (WebClient client = new WebClient())
			{
				htmlCode = client.DownloadString(url);
			}

			return htmlCode;
		}

		public static void GetFile(string url, string filePath)
		{
			using (WebClient client = new WebClient())
			{
				client.DownloadFile(url, filePath);
			}
		}
	}
}
