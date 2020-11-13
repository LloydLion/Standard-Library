using Microsoft.VisualStudio.TestTools.UnitTesting;
using StandardLibrary.Data.Department;
using StandardLibrary.Data.Department.Modificators;
using StandardLibrary.Models;
using StandardLibrary.TypeManagment.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Data.Department
{
	[TestClass]
	public class LocalDepartmentPropertiesValuesStoreTest
	{
		[TestMethod]
		public void BasicTest()
		{
			var obj = new TestClass();
			Assert.AreEqual(obj.TestStringProperty, "321");
			Assert.AreEqual(obj.TestStringProperty2, "#321");

			obj.TestStringProperty2 = "#123";
			Assert.AreEqual(obj.TestStringProperty2, "#123");
		}

		[TestMethod]
		public void AccsessTest()
		{
			var obj = new TestClass();

			try
			{
				obj.DepartmentStore.SetPropertyValue(TestClass.testStringPropertyInfo, "123");
			}
			catch(MemberAccessException)
			{
				Assert.AreEqual(obj.TestStringProperty, "321");
				obj.SetValue();
				Assert.AreEqual(obj.TestStringProperty, "123");

				try
				{
					obj.DepartmentStore.GetToken();
				}
				catch(MemberAccessException)
				{
					return;
				}
			}

			throw new Exception("Exception hasn't catched and throwed, but must");
		}

		[TestMethod]
		public void EventsTest()
		{
			bool changed = false;

			var obj = new TestClass();
			obj.DepartmentStore.PropertyChanged += (e, s) => { changed = true; };

			obj.TestStringProperty2 = "321";

			Assert.IsTrue(changed);
		}


		public class TestClass : Model, IDepartmentPropertiesSupport
		{
			public static readonly DepartmentPropertyInfo<string> testStringPropertyInfo;
			public static readonly DepartmentPropertyInfo<string> testStringProperty2Info;
			private readonly AcsessToken token;


			static TestClass()
			{
				testStringPropertyInfo = new DepartmentPropertyInfo<string>("TestStringProperty", typeof(TestClass)).AddModificator(new ReadonlyProperty());
				testStringProperty2Info = new DepartmentPropertyInfo<string>("TestStringProperty", typeof(TestClass));
			}

			public TestClass()
			{
				DepartmentStore = new LocalDepartmentPropertiesValuesStore();
				DepartmentStore.PutProperty(testStringPropertyInfo);
				DepartmentStore.PutProperty(testStringProperty2Info);

				token = DepartmentStore.GetToken();
				TestStringProperty = "321";
				TestStringProperty2 = "#321";
			}


			public LocalDepartmentPropertiesValuesStore DepartmentStore { get; }
			public string TestStringProperty { get => DepartmentStore.GetPropertyValue(testStringPropertyInfo); private set => DepartmentStore.SetPropertyValue(testStringPropertyInfo, value, token); }
			public string TestStringProperty2 { get => DepartmentStore.GetPropertyValue(testStringProperty2Info); set => DepartmentStore.SetPropertyValue(testStringProperty2Info, value); }

			
			public override string ToString()
			{
				return $"TestClass #{Id}";
			}


			public void SetValue()
			{
				TestStringProperty = "123";
			}
		}
	}
}
