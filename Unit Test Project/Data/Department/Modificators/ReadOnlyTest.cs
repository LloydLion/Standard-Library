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
	public class ReadOnlyTest
	{
		[TestMethod]
		public void BasicTest()
		{
			var obj = new TestClass();

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
		public void BasicTestSecondPart()
		{
			var obj = new TestClass();
			var val1 = obj.DepartmentStore.GetPropertyValue(TestClass.testProperty);
			obj.SetVar();
			var val2 = obj.DepartmentStore.GetPropertyValue(TestClass.testProperty);

			if(val1 == val2) throw new Exception("Property value hasn't changed");
		}


		private class TestClass : IDepartmentPropertiesSupport
		{
			public static readonly DepartmentPropertyInfo<string> testProperty = new DepartmentPropertyInfo<string>("Test", token).AddModificator(new ReadonlyProperty(), token);
			private static readonly AccessToken token = new AccessToken();


			public LocalDepartmentPropertiesValuesStore DepartmentStore { get; } = new LocalDepartmentPropertiesValuesStore(token).PutProperty(testProperty);


			public void SetVar()
			{
				DepartmentStore.SetPropertyValue(testProperty, "123", token);
			}
		}
	}
}
