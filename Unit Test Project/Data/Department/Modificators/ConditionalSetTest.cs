using Microsoft.VisualStudio.TestTools.UnitTesting;
using StandardLibrary.Data.Department;
using StandardLibrary.Data.Department.Modificators;
using StandardLibrary.TypeManagment.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Data.Department.Modificators
{
	[TestClass]
	public class ConditionalSetTest
	{
		private static bool Func(string arg)
		{
			return arg?.Contains("AED") ?? false;
		}

		[TestMethod]
		public void BasicTest()
		{
			var obj = new TestClass();

			var val1 = obj.DepartmentStore.GetPropertyValue(TestClass.testProperty);
			obj.DepartmentStore.SetPropertyValue(TestClass.testProperty, "Dfasd");
			Assert.AreEqual(val1, obj.DepartmentStore.GetPropertyValue(TestClass.testProperty));

			obj.DepartmentStore.SetPropertyValue(TestClass.testProperty, "\n\rAED!@");
			Assert.AreEqual("\n\rAED!@", obj.DepartmentStore.GetPropertyValue(TestClass.testProperty));
		}


		private class TestClass : IDepartmentPropertiesSupport
		{
			public static readonly DepartmentPropertyInfo<string> testProperty = new DepartmentPropertyInfo<string>("Test", token).AddModificator(new ConditionalSet((s) => Func((string)s)), token);
			private static readonly AccessToken token = new AccessToken();


			public LocalDepartmentPropertiesValuesStore DepartmentStore { get; } = new LocalDepartmentPropertiesValuesStore(token).PutProperty(testProperty);
		}
	}
}
