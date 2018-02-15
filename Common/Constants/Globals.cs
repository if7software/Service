namespace Constants
{
	public static partial class Globals
    {
		public static class ExitCode
		{
			public const int ERROR_SUCCESS = 0;
			public const int ERROR_BAD_ARGUMENTS = 0xA0;
			public const int ERROR_ARITHMETIC_OVERFLOW = 0x216;
			public const int ERROR_INVALID_COMMAND_LINE = 0x667;
		}

		public static class SolutionConfig
		{
			public const string Debug = "Debug";
			public const string Release = "Release";
		}

		public static class Database
		{
			public const string Test = "Test";
			public const string Coordinator = "Coordinator";
		}

		public static class DbEntity
		{
			public const string TableField = "TABLE";

			public const string FillMethod = "Fill";
		}

		public static class ConfigurationFiles
		{
			public const string ServiceProperties = "ServiceProperties.xml";
			public const string EventProperties = "EventProperties.xml";
		}

		public static class AlertMessageFiles
		{
			public const string Message = "Message.txt";
			public const string MessageStackTrace = "Message2.txt";
		}

		public static class AlertMessageMacros
		{
			public const string Message = "{{ Message }}";
			public const string Title = "{{ Title }}";
			public const string StackTrace = "{{ StackTrace }}";
		}
	}
}
