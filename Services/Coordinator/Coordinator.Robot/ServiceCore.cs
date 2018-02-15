using Coordinator.Core;
using Coordinator.Core.Automation;
using DbBridge;
using DbEntities.Test;
using Logger;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Coordinator.Robot
{
	public class ServiceCore
    {
		//private Thread thread;

		public ServiceCore()
		{
			CommonService.Initialize();
		}

		public void StartService()
		{
			CommonService.Log("Start service");

			//CommonService.LogWarning("Test systemu", new Exception("Testinho"), true);

			using (var session = DbBridgeHelper.GetDatabaseForTest())
			{
				Select<TestEntity> testInsert = new Select<TestEntity>(session);
				//testInsert.Where(query => query.Or(query.And(query.Equal(TestEntity.COLUMN_ID, 2), query.Equal(TestEntity.COLUMN_NAME, "test444")), query.Equal(TestEntity.COLUMN_ID, 5)));
				var lol = testInsert
					.Where(query => query.And(query.Equal(TestEntity.COLUMN_ID, 2), query.Equal(TestEntity.COLUMN_NAME, "test02")))
					.ReturnList();
			}

			//RobotJobCache cache = new RobotJobCache();

			//string guid = cache.Create(Core.RobotJob.RobotJobType.TEST, DateTime.Now.AddMinutes(1), new Dictionary<string, object>());

			//cache.AddAttempt(guid);

			//cache.AddAttempt(guid, new Exception("Test"));

			//cache.Delete(guid);

			//thread = new Thread(ThreadMethod);
			//thread.IsBackground = true;
			//thread.Start();
		}

		public void EndService()
		{

		}

		private void ThreadMethod()
		{
			LoggingEvent.Debug("Create cooperation thread");

			//FunctionCache functionCache = new FunctionCache();

			while (true)
			{
				Thread.Sleep(10000);
			}
		}
	}
}
