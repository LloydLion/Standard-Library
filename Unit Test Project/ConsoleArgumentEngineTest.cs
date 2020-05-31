using Microsoft.VisualStudio.TestTools.UnitTesting;
using StandardLibrary;
using StandardLibrary.Console;
using StandardLibrary.Functions;
using System.Linq;

namespace Unit_Test_Project
{
    [TestClass]
    public class ConsoleArgumentEngineTest
    {
        [TestMethod]
        public void BasicTest()
        {
            var args = "121 true 421,311 --flag1 --param AAA --param12 123".Split(' ');
            var result = new ConsoleArgumentEngine
            (
                new ConsoleArgumentEngine.PositionParameter() 
                    { Name = "pos1int", ParseFunc = (s) => int.Parse(s), Position = 0 },

                new ConsoleArgumentEngine.PositionParameter() 
                    { Name = "pos2bool", ParseFunc = (s) => bool.Parse(s), Position = 1 },

                new ConsoleArgumentEngine.PositionParameter() 
                    { Name = "pos3float", ParseFunc = (s) => float.Parse(s), Position = 2 },
                
                new ConsoleArgumentEngine.FlagParameter() 
                    { Name = "flag1", Key = "flag1" },

                new ConsoleArgumentEngine.FlagParameter() 
                    { Name = "flag2", Key = "flag2" },

                new ConsoleArgumentEngine.KeyParameter()
                    { Name = "param1str", ParseFunc = (s) => s, Key = "param" },

                new ConsoleArgumentEngine.KeyParameter()
                    { Name = "param2int", ParseFunc = (s) => int.Parse(s), Key = "param12" }

            ).Calculate(args);



            Assert.AreEqual(121, (int)result["pos1int"], $"res:{(int)result["pos1int"]}, act:121");
            Assert.AreEqual(true, (bool)result["pos2bool"], $"res:{(bool)result["pos2bool"]}, act:true");
            Assert.AreEqual(421.311, (float)result["pos3float"], 0.001, $"res:{(float)result["pos3float"]}, act:421,311");

            Assert.AreEqual(true, (bool)result["flag1"], $"res:{(bool)result["flag1"]}, act:true");
            Assert.AreEqual(false, (bool)result["flag2"], $"res:{(bool)result["flag2"]}, act:false");

            Assert.AreEqual("AAA", (string)result["param1str"], $"res:{(string)result["param1str"]}, act:AAA");
            Assert.AreEqual(123, (int)result["param2int"], $"res:{(int)result["param2int"]}, act:123");
        }
    }
}