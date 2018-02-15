namespace Constants.SocialMedia
{
	public static class Facebook
	{
		public const string AppId = "125928271436861";
		public const string AppSecret = "6d83e4b80bfb2c55d829fc038d6fa474";
		
		public class Permissions
		{
			public readonly static string[] Authorize =
			{
				"publish_actions",
				"manage_pages"
			};
		}

		public class Pages
		{
			public class If7Community
			{
				public const string PageId = "146622282729882";
				public const string UrlName = "If7-community-146622282729882";
				public const string Name = "If7 community";
			}
		}
	}
}
