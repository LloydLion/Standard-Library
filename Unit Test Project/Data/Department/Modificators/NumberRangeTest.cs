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
	public class NumberRangeTest
	{
		[TestMethod]
		public void BasicTest()
		{
			var obj = new TestClass();

			obj.DepartmentStore.SetPropertyValue(TestClass.testProperty, 3123);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(TestClass.testProperty), 53);
			
			obj.DepartmentStore.SetPropertyValue(TestClass.testProperty, -412);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(TestClass.testProperty), 11);
			
			obj.DepartmentStore.SetPropertyValue(TestClass.testProperty, 43);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(TestClass.testProperty), 43);
		}

		[TestMethod]
		public void DecimalTest()
		{
			var obj = new TestClass();

			obj.DepartmentStore.SetPropertyValue(TestClass.testDecimalProperty, 31248971237543232847123974990m);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(TestClass.testDecimalProperty), 31248971237543232847123974983m);

			obj.DepartmentStore.SetPropertyValue(TestClass.testDecimalProperty, -31248971237543232847123999999m);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(TestClass.testDecimalProperty), -31248971237543232847123974983m);

			obj.DepartmentStore.SetPropertyValue(TestClass.testDecimalProperty, 31248971237543232847123.3123123m);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(TestClass.testDecimalProperty), 31248971237543232847123.3123123m);
		}

		[TestMethod]
		public void DoubleTest()
		{
			var obj = new TestClass();

			obj.DepartmentStore.SetPropertyValue(TestClass.testDoubleProperty, 533.312);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(TestClass.testDoubleProperty), 53.312);

			obj.DepartmentStore.SetPropertyValue(TestClass.testDoubleProperty, -11.34);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(TestClass.testDoubleProperty), 11.34);

			obj.DepartmentStore.SetPropertyValue(TestClass.testDoubleProperty, 20.34);
			Assert.AreEqual(obj.DepartmentStore.GetPropertyValue(TestClass.testDoubleProperty), 20.34);
		}


		private class TestClass : IDepartmentPropertiesSupport
		{
			public static readonly DepartmentPropertyInfo<int> testProperty = new DepartmentPropertyInfo<int>("Test", token).AddModificator(new NumberRange(11, 53), token);
			public static readonly DepartmentPropertyInfo<decimal> testDecimalProperty =
				new DepartmentPropertyInfo<decimal>("TestDecimal", token).AddModificator(new NumberRange(-31248971237543232847123974983m, 31248971237543232847123974983m), token);
			public static readonly DepartmentPropertyInfo<double> testDoubleProperty = new DepartmentPropertyInfo<double>("TestDouble", token).AddModificator(new NumberRange(11.34m, 53.312m), token);
			private static readonly AccessToken token = new AccessToken();


			public LocalDepartmentPropertiesValuesStore DepartmentStore { get; } = new LocalDepartmentPropertiesValuesStore(token).PutProperty(testProperty, testDecimalProperty, testDoubleProperty);
		}
	}
}
