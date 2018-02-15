using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;

namespace Coordinator.Service.Interfaces
{
	[ServiceContract]
	public interface IService
	{
		[OperationContract]
		//[WebGet(UriTemplate = "test/{input}")]
		string SomeOperation(string input);
	}
}
