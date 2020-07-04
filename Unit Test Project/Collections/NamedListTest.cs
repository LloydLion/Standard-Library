using Microsoft.VisualStudio.TestTools.UnitTesting;
using StandardLibrary.Collections;
using StandardLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Collections
{
    [TestClass]
    public class NamedListTest
    {
        [TestMethod]
        public void BasicTest()
		{
            var list = new NamedList<NamedElement>();

			var el = new NamedElement();
			list.Add(el);

			var el2 = new NamedElement();
			el2.Rename("asd");
			list.Add(el2);

			//Test
			
			//Get by indexer
			Assert.AreEqual(el, list[null], $"el:{el}, list[null]:{list[null]}");
			Assert.AreEqual(el2, list["asd"], $"el2:{el2}, list[\"asd\"]:{list["asd"]}");

			//Get by method
			Assert.AreEqual(el, list.GetElementByName(null), $"el:{el}, " +
				$"list.GetElementByName(null){list.GetElementByName(null)}");

			Assert.AreEqual(el2, list.GetElementByName("asd"), $"el2:{el2}, " +
				$"list.GetElementByName(\"asd\"){list.GetElementByName("asd")}");

			//Set
			var el3 = new NamedElement();
			el3.Rename("qwe");
			list[null] = el3;

			Assert.IsTrue(!list.Contains(el));
			Assert.AreEqual(el3, list["qwe"], $"el3:{el3}, list[\"qwe\"]:{list["qwe"]}");
			Assert.AreEqual(el2, list["asd"], $"el2:{el2}, list[\"asd\"]:{list["asd"]}");
		}

		[TestMethod]
		public void UnicleNameTest()
		{
			var t = new NamedList<NamedElement>();

			var el = new NamedElement();
			el.Rename("dsa");
			t.Add(el);

			try
			{
				t.Add(el);
			}
			catch(InvalidOperationException)
			{
				return;
			}

			throw new Exception("Exception has not throwed by NamedList");
		}

		private class NamedElement : IPrivateNamedObject
		{
			public string Name { get; private set; }

			public void Rename(string newName)
			{
				Name = newName;
			}

			public override string ToString()
			{
				return "NamedElement: " + Name;
			}
		}
	}
}
