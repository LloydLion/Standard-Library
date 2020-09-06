using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using StandardLibrary.Other;

namespace TestProject.Other
{
	[TestClass]
	public class ExtensionsTest
	{
		[TestMethod]
		public void IndexOfTest()
		{
			int[] array = new int[] { 0, 2, 10, 222, 12 };
			Assert.AreEqual(array.IndexOf(222), 3);
		}

		[TestMethod]
		public void InvokeForAllTest()
		{
			int invokeCount = 0;
			int sum = 0;
			int[] array = new int[] { 0, 2, 10, 222, 12 };

			array.InvokeForAll((s) => { invokeCount++; sum += s; });

			Assert.AreEqual(invokeCount, array.Length, $"invokeCount:{invokeCount}");
			Assert.AreEqual(sum, 246, $"invokeCount:{invokeCount}");
		}
	}
}
