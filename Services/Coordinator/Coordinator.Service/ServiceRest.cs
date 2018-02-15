using Coordinator.Service.Interfaces;
using Coordinator.Service.Services;
using Logger;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Threading;

namespace Coordinator.Service
{
	public class ServiceRest
    {
		private static TimeSpan waitForOpenHostTimeout = new TimeSpan(23, 59, 0);
		private static TimeSpan waitSleepTime = new TimeSpan(0, 0, 5);

		private Thread thread;

		public bool CloseHost { get; set; }

		public ServiceRest()
		{
			CloseHost = false;
		}

		public void OpenHost()
		{
			LoggingEvent.Debug("Open host");

			thread = new Thread(ThreadMethod);
			thread.IsBackground = true;
			thread.Start();
		}

		public void ThreadMethod()
		{
			var start = DateTime.Now;
			int counter = 0;

			//while ((DateTime.Now - start) < waitForOpenHostTimeout)
			//{
				try
				{
					using (ServiceHost host = new ServiceHost(typeof(ServiceImpl), new Uri("http://localhost:8000/coordinator/")))
					{
						var smb = host.Description.Behaviors.Find<ServiceMetadataBehavior>() ?? new ServiceMetadataBehavior();
						smb.HttpGetEnabled = true;
						smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;

						host.Description.Behaviors.Add(smb);
						host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
						host.AddServiceEndpoint(typeof(IService), new BasicHttpBinding(), "api");
						host.Open();

						while (!CloseHost)
						{
							Thread.Sleep(10000);
						}

						host.Close();
					}
				}
				catch (AddressAccessDeniedException exAccessDenied)
				{
					throw exAccessDenied;
				}
				catch (Exception ex)
				{
					counter++;

					Thread.Sleep(waitSleepTime);
				}
			//}
		}
	}
}
