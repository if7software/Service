namespace DbBridge.Interfaces
{
	public interface IEntity
	{
		void Fill(string name, object value, bool isNull);
	}
}
