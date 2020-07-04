using System;
using System.Threading;

namespace TestConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Thread.Sleep(1000);
			Console.WriteLine("Started!");

			Console.WriteLine("Hello World!");
			var input1 = Console.ReadLine();

			switch (input1)
			{
				case "TestMSG1":
					Console.WriteLine("HelloMSG1");
					break;

				case "TestMSG2":
					Console.WriteLine("HelloMSG2");
					break;

				case "TestMSG3":
					Console.WriteLine("HelloMSG3");
					break;

				case "Close":
					return;

				default:
					Console.WriteLine("Invalid input");
					break;
			}

			Main(args);
		}
	}
}
