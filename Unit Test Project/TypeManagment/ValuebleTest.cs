using Microsoft.VisualStudio.TestTools.UnitTesting;
using StandardLibrary.TypeManagment;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace TestProject.TypeManagment
{
    [TestClass]
    public class ValuebleTest
    {
        [TestMethod]
        public void BasicTest()
        {
            var val = (Valueble<string>)"123";
            var r = Foo(val);

            Assert.AreEqual(r, "123321", $"val:{val} Foo(val):{r}");
            Assert.AreEqual((string)val, "123", $"val:{val} Foo(val):{r}");
        }

        private string Foo(string asd)
        {
            return asd + "321";
        }
    }
}
