using Microsoft.VisualStudio.TestTools.UnitTesting;
using StandardLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Collections
{
	[TestClass]
	public class SingleUseCollectionTest
	{
		[TestMethod]
		public void BasicTest()
		{
			var list = new SingleUseCollection<int>
			{
				3,
				12
			};

			if (list.Contains(3) == true)
			{
				_ = list[0];

				if(list.Contains(3) == false && list.Contains(12) == true)
				{
					using(var enumerator = list.GetEnumerator())
					{
						enumerator.MoveNext();
					}

					if(list.Contains(12) == false) return;
				}
			}

			throw new Exception("SingleUseCollection don't work right");
		}
	}
}
