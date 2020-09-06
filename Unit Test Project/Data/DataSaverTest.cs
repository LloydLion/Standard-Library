using Microsoft.VisualStudio.TestTools.UnitTesting;
using StandardLibrary.Data;
using StandardLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject.Data
{
	[TestClass]
	public class DataSaverTest
	{
		[TestInitialize]
		public void Begin()
		{
			DataSaver.SetApplicationName("TestApp");
		}

		[TestMethod]
		public void BasicTest()
		{
			var saver = new DataSaver(DataSaver.DataLocation.UserData);

			saver.Save("alpha", 321);
			Assert.AreEqual(saver.GetSavedNumber("alpha"), 321);
		}

		[TestMethod]
		public void SharedTypeTest()
		{
			var saver = new DataSaver(DataSaver.DataLocation.UserData);
			saver.Save("alpha_1", 123);
			saver.Save("alpha_2", 321);
			saver.Save("alpha_3", new string[] { "sadas,", "asdsad::" });
			saver.Save("alpha_4", "asdasdas");
			saver.Save("alpha_5", new TestModel("asd", 1232));

			Assert.AreEqual(saver.GetSavedObject<int>("alpha_1"), 123, "(test 1 - number)");
			Assert.AreEqual(saver.GetSavedObject<int>("alpha_2"), 321, "(test 2 - number)");
			Assert.IsTrue(saver.GetSavedArray<string>("alpha_3").SequenceEqual(new string[] { "sadas,", "asdsad::" }), "(test 3 - array)");
			Assert.AreEqual(saver.GetSavedObject<string>("alpha_4"), "asdasdas", "(test 4 - string)");
			Assert.AreEqual(saver.GetSavedObject<TestModel>("alpha_5"), new TestModel("asd", 1232), "(test 5 - class)");
		}

		[TestMethod]
		public void HasKeyTest()
		{
			var saver = new DataSaver(DataSaver.DataLocation.UserData);
			saver.Save("alpha", 321);
			saver.Save("alpha2", "asd");

			Assert.AreEqual(saver.HasKey("alpha"), true);
			Assert.AreEqual(saver.HasKey("alpha2"), true);
			Assert.AreEqual(saver.HasKey("alpha3"), false);
		}


		private class TestModel : Model
		{
			public string Value { get; set; }
			public int Value2 { get; set; }


			public TestModel(string value, int value2)
			{
				Value = value;
				Value2 = value2;
			}


			public override bool Equals(object obj)
			{
				var model = (TestModel)obj;
				return model.Value == Value && model.Value2 == Value2;
			}

			public override string ToString()
			{
				throw new NotImplementedException();
			}

			public override int GetHashCode()
			{
				return HashCode.Combine(Id, IsDisposed, IsInitialized, IsFinalized, Value, Value2);
			}
		}
	}
}
