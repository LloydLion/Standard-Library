using Microsoft.VisualStudio.TestTools.UnitTesting;
using StandardLibrary.ProcessManagment;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace TestProject.ProcessManagment
{
	[TestClass]
	public class ConsoleProcessEngineTest
	{
		private const string ExePath = @"..\..\..\..\TestConsoleApp\bin\Debug\netcoreapp3.1\TestConsoleApp.exe";
		private const string FileName = "TestConsoleApp";

		private ProcessStartInfo startInfo = new ProcessStartInfo()
		{
			FileName = ExePath,
			UseShellExecute = false,
			RedirectStandardError = true,
			RedirectStandardInput = true,
			RedirectStandardOutput = true
		};

		[TestMethod]
		public void BasicTest()
		{
			
			var c1 = Process.GetProcessesByName(FileName).Length;

			var engine = new ConsoleProcessEngine(startInfo);

			var c2 = Process.GetProcessesByName(FileName).Length;

			engine.Start();
			var c3 = Process.GetProcessesByName(FileName).Length;
			engine.StopAsync("Close").GetResult();

			var c4 = Process.GetProcessesByName(FileName).Length;

			Assert.IsTrue(c1 == c2 && c1 == c3 - 1 && c1 == c4, $"c1:{c1}, c2:{c2}, c3:{c3}, c4:{c4}");
		}

		[TestMethod]
		public void CommandWriteTest()
		{
			var engine = new ConsoleProcessEngine(startInfo);
			engine.Start();

			//Wait process start(see TestConsoleApp.Program.Main in TestConsoleApp project)
			Thread.Sleep(1500);

			engine.WriteCommand("TestMSG1");
			var e = engine.ReadConsoleSegment("Hello World!", "Started!\r\nHello World!");

			Assert.AreEqual(e, "\r\nHelloMSG1\r\n", $"e:{e}");

			engine.StopAsync("Close");
		}

		[TestMethod]
		public void StartAsyncTest()
		{
			var engine = new ConsoleProcessEngine(startInfo);
			var waiter = engine.StartAsync("Started!");

			for (int i = 0; i < 6000; i++)
			{
				Thread.Sleep(1);
				if (waiter.IsCompleted == true) return;
			}

			throw new TimeoutException("Start wait time is out");
		}
	}
}
