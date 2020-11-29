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
	public class AllowOnlyOneSetTest
	{
		[TestMethod]
		public void BasicTest()
		{
			var obj = new TestClass();
			obj.DepartmentStore.SetPropertyValue(TestClass.testProperty, "*value*");

			try
			{
				obj.DepartmentStore.SetPropertyValue(TestClass.testProperty, "342134");
			}
			catch (MemberAccessException)
			{
				obj.DepartmentStore.GetPropertyValue(TestClass.testProperty);
				return;
			}

			throw new Exception("Exception hasn't catched and throwed, but must");
		}

		[TestMethod]
		public void MultipleAccsessTest()
		{
			BasicTest();
			BasicTest();
			BasicTest();
			BasicTest();
		}

		[TestMethod]
		public void CombinedTest()
		{
			var obj = new TestClass();
			obj.DepartmentStore.SetPropertyValue(TestClass.testProperty, "*value*");

			try
			{
				obj.DepartmentStore.SetPropertyValue(TestClass.testProperty, "342134");
			}
			catch (MemberAccessException)
			{
				obj.DepartmentStore.GetPropertyValue(TestClass.testProperty);
				obj.SetVar();

				return;
			}

			throw new Exception("Exception hasn't catched and throwed, but must");
		}


		private class TestClass : IDepartmentPropertiesSupport
		{
			public static readonly DepartmentPropertyInfo<string> testProperty = new DepartmentPropertyInfo<string>("Test", token).AddModificator(new AllowOnlyOneSet(), token);
			private static readonly AccessToken token = new AccessToken();


			public LocalDepartmentPropertiesValuesStore DepartmentStore { get; } = new LocalDepartmentPropertiesValuesStore(token).PutProperty(testProperty);


			public void SetVar()
			{
				DepartmentStore.SetPropertyValue(testProperty, "123", token);
			}
		}
	}
}
