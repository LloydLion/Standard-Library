using Microsoft.VisualStudio.TestTools.UnitTesting;
using StandardLibrary.Data.Department;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Data.Department
{
	[TestClass]
	public class ExtendedDepartmentPropertiesValuesStoreTest
	{
		private static readonly DepartmentPropertyInfo<string> testStringProperty = new DepartmentPropertyInfo<string>("TestString", typeof(object));
		private static readonly DepartmentPropertyInfo<string> testString2Property = new DepartmentPropertyInfo<string>("TestString2", typeof(string));


		[TestMethod]
		public void BasicTest()
		{
			object obj = new object();
			var store = new ExtendedDepartmentPropertiesValuesStore();

			store.PutProperty(testStringProperty);
			store.FinalizeObject();
			store.AddObject(obj);

			store.GetObjectData(obj).SetPropertyValue(testStringProperty, "123");
			Assert.AreEqual(store.GetObjectData(obj).GetPropertyValue(testStringProperty), "123");
		}

		[TestMethod]
		public void PropertiesToObjectAddTest()
		{
			object obj1 = new object();
			object obj2 = "";
			var store = new ExtendedDepartmentPropertiesValuesStore();

			store.PutProperty(testStringProperty);
			store.PutProperty(testString2Property);
			store.FinalizeObject();

			store.AddObject(obj1);
			store.AddObject(obj2);

			try
			{
				store.GetObjectData(obj1).GetPropertyValue(testString2Property);
			}
			catch(Exception)
			{
				return;
			}

			throw new Exception("Exception hasn't throwed");
		}
	}
}
