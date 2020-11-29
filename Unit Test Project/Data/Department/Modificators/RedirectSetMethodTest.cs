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
	public class RedirectSetMethodTest
	{
		private static int i;


		private static void RedirectMethod(object obj)
		{
			i = (int)obj;
		}


		[TestMethod]
		public void BasicTest()
		{
			var obj = new TestClass();
			var origin = obj.DepartmentStore.GetPropertyValue(TestClass.testProperty);

			obj.DepartmentStore.SetPropertyValue(TestClass.testProperty, 10);
			Assert.AreEqual(10, i);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(TestClass.testProperty), origin);

			obj.DepartmentStore.SetPropertyValue(TestClass.testProperty, 132130);
			Assert.AreEqual(132130, i);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(TestClass.testProperty), origin);

			obj.DepartmentStore.SetPropertyValue(TestClass.testProperty, 9929390);
			Assert.AreEqual(9929390, i);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(TestClass.testProperty), origin);
		}


		private class TestClass : IDepartmentPropertiesSupport
		{
			public static readonly DepartmentPropertyInfo<int> testProperty = new DepartmentPropertyInfo<int>("Test", token).AddModificator(new RedirectSetMethod(RedirectMethod), token);
			private static readonly AccessToken token = new AccessToken();


			public LocalDepartmentPropertiesValuesStore DepartmentStore { get; } = new LocalDepartmentPropertiesValuesStore(token).PutProperty(testProperty);
		}
	}
}
