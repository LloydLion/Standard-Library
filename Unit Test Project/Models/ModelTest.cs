using Microsoft.VisualStudio.TestTools.UnitTesting;
using StandardLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Models
{
	[TestClass]
	public class ModelTest
	{
		[TestMethod]
		public void CloneTest()
		{
			var obj = new ModelClass()
			{
				Property = 12,
				PropertyLink = "asd"
			};

			var cobj = (ModelClass)obj.Clone();
			cobj.PropertyLink += "aa";
			cobj.Property += 2;


			Assert.AreEqual(obj.Property, 12, $"cobj.Pv:{cobj.Property}");
			Assert.AreEqual(cobj.Property, 14, $"obj.Pv:{obj.Property}");

			Assert.AreEqual(obj.PropertyLink, "asd", $"cobj.Pl:{cobj.PropertyLink}");
			Assert.AreEqual(cobj.PropertyLink, "asdaa", $"obj.Pl:{obj.PropertyLink}");
		}

		[TestMethod]
		public void PropertyEventTest()
		{
			var PropertyChangingCount = 0;
			var PropertyChangedCount = 0;

			var obj = new ModelClass()
			{
				Property = 12,
				PropertyLink = "asd"
			};

			obj.PropertyChanging += (s, a) => 
			{
				PropertyChangingCount++;

				Assert.AreEqual(a.PropertyName, "Property", $"a.PropertyName:{a.PropertyName}");
			};

			obj.PropertyChanged += (s, a) =>
			{
				PropertyChangedCount++;

				Assert.AreEqual(a.PropertyName, "Property", $"a.PropertyName:{a.PropertyName}");
			};


			obj.Property = 14;


			Assert.AreEqual(obj.Property, 14, $"obj.Property:{obj.Property}");

			Assert.AreEqual(PropertyChangingCount, 1, $"PropertyChangingCount:{PropertyChangingCount}");
			Assert.AreEqual(PropertyChangedCount, 1, $"PropertyChangedCount:{PropertyChangedCount}");
		}

		[TestMethod]
		public void EventNullTest()
		{
			var obj = new ModelClass()
			{
				Property = 12,
				PropertyLink = "asd"
			};

			try
			{
				obj.Property = 14;
				obj.Initialize();
				obj.FinalizeObject();
				obj.Dispose();
			}
			catch(NullReferenceException)
			{
				throw new Exception("TestFailed.. NullReferenceException");
			}
		}

		[TestMethod]
		[Obsolete]
		public void DisposeTest()
		{
			var obj = new ModelClass()
			{
				Property = 12,
				PropertyLink = "asd"
			};

			void Test(bool resource, bool disposed, string prefix)
			{
				Assert.AreEqual(obj.Resource, resource, $"({prefix}) Resource:{obj.Resource}");
				Assert.AreEqual(obj.IsDisposed, disposed, $"({prefix}) Disposed:{obj.IsDisposed}");
			}

			Test(true, false, "1");

			obj.Disposing += (q, w) => Test(true, false, "2/1");
			obj.NewDisposing += (q, w) => Test(true, false, "2/1");

			obj.Disposed += (q, w) => Test(false, true, "3/1");
			obj.NewDisposed += (q, w) => Test(false, true, "3/2");

			obj.Dispose();

			Test(false, true, "4");
		}

		[TestMethod]
		[Obsolete]
		public void InitializeTest()
		{
			var obj = new ModelClass();

			void Test(int value, string prefix, bool init)
			{
				Assert.AreEqual(obj.Property, value, $"({prefix}) Property:{obj.Property}");
				Assert.AreEqual(obj.IsInitialized, init, $"({prefix}) IsInitialized:{obj.IsInitialized}");
			}

			Test(0, "1", false);

			obj.NewInitializing += (q, w) => Test(0, "2", false);

			obj.Initialized += (q, w) => Test(213, "3/1", true);
			obj.NewInitialized += (q, w) => Test(213, "3/2", true);

			obj.Initialize();

			Test(213, "4", true);
		}

		[TestMethod]
		public void FinalizeTest()
		{
			var obj = new ModelClass
			{
				Property = 1231
			};

			obj.FinalizeObject();

			try
			{
				obj.Property = 12314;
			}
			catch(Exception)
			{
				Assert.AreEqual(obj.Property, 1231, $"Property value changed");

				return;
			}

			throw new Exception("Exception hasn't throwed");
		}


		class ModelClass : Model
		{
			private int property = 0;
			private string propertyLink = "";
			private bool resource = true;

			public int Property
			{ get => property; set { OnPropertyChanging(); property = value; OnPropertyChanged(); } }

			public string PropertyLink
			{ get => propertyLink; set { OnPropertyChanging(); propertyLink = value; OnPropertyChanged(); } }

			public bool Resource
			{ get => resource; private set { OnPropertyChanging(); resource = value; OnPropertyChanged(); } }


			public override object Clone()
			{
				var d = new ModelClass()
				{
					Property = this.Property,
					PropertyLink = (string)this.PropertyLink?.Clone()
				};

				return d;
			}

			protected override void InitializeE()
			{
				Property = 213;
			}

			public override string ToString()
			{
				throw new NotImplementedException();
			}

			protected override void DisposeE()
			{
				using (new ModelOpenHandler(this))
				{
					Resource = false;
				}
			}
		}
	}
}
