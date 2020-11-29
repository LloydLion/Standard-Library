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
	public class RedirecdGetMethodTest
	{
		private static int i = 0;


		private static object RedirectMethod()
		{
			return i;
		}


		[TestMethod]
		public void BasicTest()
		{
			var obj = new TestClass();

			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(TestClass.testProperty), i);
			i++;

			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(TestClass.testProperty), i);
			i = -3213;

			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(TestClass.testProperty), i);
		}


		private class TestClass : IDepartmentPropertiesSupport
		{
			public static readonly DepartmentPropertyInfo<int> testProperty = new DepartmentPropertyInfo<int>("Test", token).AddModificator(new RedirectGetMethod(RedirectMethod), token);
			private static readonly AccessToken token = new AccessToken();


			public LocalDepartmentPropertiesValuesStore DepartmentStore { get; } = new LocalDepartmentPropertiesValuesStore(token).PutProperty(testProperty);
		}
	}
}
