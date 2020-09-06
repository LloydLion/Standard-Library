using Microsoft.VisualStudio.TestTools.UnitTesting;
using StandardLibrary.Other.Stade;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Other.Stade
{
	[TestClass]
	public class StadeMachineTest
	{
		[TestMethod]
		public void BasicTest()
		{
			var obj = new Container<string>() { Value = "asd" };
			var counter = 0;

			var stade = new StadeMachine<Stades, Container<string>, StandardStade<Stades, Container<string>>>(new StandardStade<Stades, Container<string>>[]
			{
				new StandardStade<Stades, Container<string>>(Stades.First, (s, d) => s.Value = Stades.First.ToString() + " + " + d.LastStade.ToString(), (s, d) => counter += (int)d.NewStade),
				new StandardStade<Stades, Container<string>>(Stades.Second, (s, d) => s.Value = Stades.Second.ToString() + " + " + d.LastStade.ToString(), (s, d) => counter += (int)d.NewStade),
				new StandardStade<Stades, Container<string>>(Stades.Third, (s, d) => s.Value = Stades.Third.ToString() + " + " + d.LastStade.ToString(), (s, d) => counter += (int)d.NewStade),
			}, obj);

			stade.SetStade(Stades.First);
			Assert.AreEqual(obj.Value, "First + First", "(test 1/1)");
			Assert.AreEqual(counter, 0, "(test 1/2)");

			stade.SetStade(Stades.Second);
			Assert.AreEqual(obj.Value, "Second + First", "(test 2/1)");
			Assert.AreEqual(counter, 1, "(test 2/2)");

			stade.SetStade(Stades.Third);
			Assert.AreEqual(obj.Value, "Third + Second", "(test 3/1)");
			Assert.AreEqual(counter, 3, "(test 3/2)");

			stade.SetStade(Stades.First);
			Assert.AreEqual(obj.Value, "First + Third", "(test 4/1)");
			Assert.AreEqual(counter, 3, "(test 4/2)");
		}


		private enum Stades
		{
			First, 
			Second,
			Third
		}

		private class Container<T>
		{
			public T Value { get; set; }
		}
	}
}
