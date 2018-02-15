namespace Coordinator.Constants
{
	public static class Consts
	{
		public static class Function
		{
			public const string JobElement = "job";
			public const string DateElement = "date";
			public const string AttemptElement = "attempt";
			public const string AttemptsElement = "attempts";
			public const string ParameterElement = "parameter";
			public const string ParametersElement = "parameters";
			public const string ErrorMessageElement = "error_message";

			public const string IdAttribute = "id";
			public const string KeyAttribute = "key";
			public const string TypeAttribute = "type";
			public const string ValueAttribute = "value";
			public const string StatusAttribute = "status";
			public const string CreationDateAttribute = "creation_date";
			public const string ExecutionDateAttribute = "execution_date";
		}

		public static class ServiceProperties
		{
			public const string Root = "params";
			public const string Element = "param";

			public const string KeyAttribute = "key";
			public const string ValueAttribute = "value";
			public const string TypeAttribute = "type";
		}

		public static class ServicePropertyTypes
		{
			public const string String = "System.String";
			public const string Int = "System.Int32";
			public const string Long = "System.Int64";
			public const string TimeSpan = "System.TimeSpan";
			public const string DateTime = "System.DateTime";
			public const string Bool = "System.Boolean";

			public static readonly string[] Types = { String, Int, Long, TimeSpan, DateTime, Bool };
		}

		public static class EventProperties
		{
			public const string Root = "events";
			public const string Element = "event";

			public const string KeyAttribute = "key";
			public const string RecurringEventAttribute = "recurring";
			public const string DaysOfWeekAttribute = "days";
			public const string TimeAttribute = "time";
		}
	}
}
