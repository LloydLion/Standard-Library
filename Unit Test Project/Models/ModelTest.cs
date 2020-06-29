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
        public void EventTest()
        {
            var t1 = 0;
            var t2 = 0;

            var obj = new ModelClass()
            {
                Property = 12,
                PropertyLink = "asd"
            };

            obj.PropertyChanging += (s, a) => 
            {
                t1++;

                Assert.AreEqual(a.PropertyName, "Property", $"a.PropertyName:{a.PropertyName}");
            };

            obj.PropertyChanged += (s, a) =>
            {
                t2++;

                Assert.AreEqual(a.PropertyName, "Property", $"a.PropertyName:{a.PropertyName}");
            };

            obj.Property = 14;

            Assert.AreEqual(obj.Property, 14, $"obj.Property:{obj.Property}");

            Assert.AreEqual(t1, 1, $"t1:{t1}, t2:{t2}");
            Assert.AreEqual(t2, 1, $"t1:{t1}, t2:{t2}");
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
            }
            catch(NullReferenceException)
            {
                throw new Exception("TestFailed.. NullReferenceException");
            }
        }

        [TestMethod]
        public void DisposeTest()
        {
            var obj = new ModelClass()
            {
                Property = 12,
                PropertyLink = "asd"
            };

            Assert.AreEqual(obj.Resource, true, $"(1) Resource:{obj.Resource}");
            Assert.AreEqual(obj.IsDisposed, false, $"(1) Disposed:{obj.IsDisposed}");

            obj.Dispose();

            Assert.AreEqual(obj.Resource, false, $"(2) Resource:{obj.Resource}");
            Assert.AreEqual(obj.IsDisposed, true, $"(2) Disposed:{obj.IsDisposed}");
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

			public override string ToString()
			{
				throw new NotImplementedException();
			}

			protected override void DisposeE()
            {
                Resource = false;
                base.Dispose();
            }
        }
    }
}
