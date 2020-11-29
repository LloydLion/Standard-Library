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
	public class MultipleModificatorsTest
	{
		[TestMethod]
		public void ConditionalSetAndNumberRangeTest()
		{
			var obj = new ConditionalSetAndNumberRangeTestClass();

			obj.DepartmentStore.SetPropertyValue(ConditionalSetAndNumberRangeTestClass.departmentProperty, 34);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(ConditionalSetAndNumberRangeTestClass.departmentProperty), 30);

			obj.DepartmentStore.SetPropertyValue(ConditionalSetAndNumberRangeTestClass.departmentProperty, 50);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(ConditionalSetAndNumberRangeTestClass.departmentProperty), 50);

			obj.DepartmentStore.SetPropertyValue(ConditionalSetAndNumberRangeTestClass.departmentProperty, 1234);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(ConditionalSetAndNumberRangeTestClass.departmentProperty), 123);

			obj.DepartmentStore.SetPropertyValue(ConditionalSetAndNumberRangeTestClass.departmentProperty, 53);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(ConditionalSetAndNumberRangeTestClass.departmentProperty), 53);
		}

		[TestMethod]
		public void ReadonlyAndAllowOnlyOneSetTest()
		{
			var obj = new ReadonlyAndAllowOnlyOneSetTestClass();

			try
			{
				obj.DepartmentStore.SetPropertyValue(ReadonlyAndAllowOnlyOneSetTestClass.departmentProperty, 123);
			}
			catch(Exception) { return; }

			throw new Exception("Exception hasn't thowed but must");
		}

		[TestMethod]
		public void RedirecdSetMethodAndConditionalSetTest()
		{
			var obj = new RedirecdSetMethodAndConditionalSetTestClass();

			obj.DepartmentStore.SetPropertyValue(RedirecdSetMethodAndConditionalSetTestClass.departmentProperty, -3);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(RedirecdSetMethodAndConditionalSetTestClass.departmentProperty), 0);
			Assert.AreEqual(RedirecdSetMethodAndConditionalSetTestClass.val, 0);

			obj.DepartmentStore.SetPropertyValue(RedirecdSetMethodAndConditionalSetTestClass.departmentProperty, 3);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(RedirecdSetMethodAndConditionalSetTestClass.departmentProperty), 3);
			Assert.AreEqual(RedirecdSetMethodAndConditionalSetTestClass.val, 3);

			obj.DepartmentStore.SetPropertyValue(RedirecdSetMethodAndConditionalSetTestClass.departmentProperty, 0);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(RedirecdSetMethodAndConditionalSetTestClass.departmentProperty), 3);
			Assert.AreEqual(RedirecdSetMethodAndConditionalSetTestClass.val, 3);

			obj.DepartmentStore.SetPropertyValue(RedirecdSetMethodAndConditionalSetTestClass.departmentProperty, 32);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(RedirecdSetMethodAndConditionalSetTestClass.departmentProperty), 32);
			Assert.AreEqual(RedirecdSetMethodAndConditionalSetTestClass.val, 32);
		}


		class ConditionalSetAndNumberRangeTestClass : IDepartmentPropertiesSupport
		{
			private static readonly AccessToken token = new AccessToken();
			public static readonly DepartmentPropertyInfo<int> departmentProperty = new DepartmentPropertyInfo<int>("Department")
				.AddModificator(new ConditionalSet((s) => ((int)s) >= 50), token).AddModificator(new NumberRange(30, 123), token);


			public LocalDepartmentPropertiesValuesStore DepartmentStore { get; } = new LocalDepartmentPropertiesValuesStore(token).PutProperty(departmentProperty);
		}

		class ReadonlyAndAllowOnlyOneSetTestClass : IDepartmentPropertiesSupport
		{
			private static readonly AccessToken token = new AccessToken();
			public static readonly DepartmentPropertyInfo<int> departmentProperty = new DepartmentPropertyInfo<int>("Department")
				.AddModificator(new ReadonlyProperty(), token).AddModificator(new AllowOnlyOneSet(), token);


			public LocalDepartmentPropertiesValuesStore DepartmentStore { get; } = new LocalDepartmentPropertiesValuesStore(token).PutProperty(departmentProperty);
		}

		class RedirecdSetMethodAndConditionalSetTestClass : IDepartmentPropertiesSupport
		{
			private static readonly AccessToken token = new AccessToken();
			public static readonly DepartmentPropertyInfo<int> departmentProperty = new DepartmentPropertyInfo<int>("Department")
				.AddModificator(new ConditionalSet((s) => (int)s >= val), token).AddModificator(new RedirectSetMethod((s) => val = (int)s), token).AddModificator(new RedirectGetMethod(() => val));
			public static int val = 0;


			public LocalDepartmentPropertiesValuesStore DepartmentStore { get; } = new LocalDepartmentPropertiesValuesStore(token)
				.PutProperty(departmentProperty)
				.InitPropertyWithValue(departmentProperty, 0, token);
		}
	}
}
