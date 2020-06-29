using Microsoft.VisualStudio.TestTools.UnitTesting;
using StandardLibrary;
using StandardLibrary.Functions;
using System.Linq;

namespace TestProject.Functions
{
	[TestClass]
	public class FunctionStepperTest
	{
		[TestMethod]
		public void BasicTest()
		{
			var r = Foo(3, "fad123as");
			var requestedValues = new (int, string)[]
			{
				(3, "fad123as"),
				(4, "FAD123AS4"),
				default
			};

			for (int i = 0; r.HasNextStep; i++)
			{
				var srr = requestedValues[i];
				var sr = r.DoNextStep();
				Assert.IsTrue(sr.IsVoid ? srr == default : sr.Result == srr, $"iteration:{i}, srr:{srr}, sr.R:{sr.Result}");
			}
		}

		[TestMethod]
		public void StepperVoidTest()
		{
			var str = "asdFsa";
			var r = Foo2(924, "asdFsa");

			var req = new string[]
			{
				"asdFsa",
				"asdFsa"
			};

			for (int i = 0; r.HasNextStep; i++)
			{
				var srr = req[i];
				var sr = r.DoNextStep();
				Assert.IsTrue(str == srr, $"iteration:{i}, srr:{srr}, str:{str}");
			}
		}



		private FunctionStepper<(int, string)> Foo(int s, string a)
		{
			var step = new FunctionStepper<(int, string)>();

			step.CreateSteps(() =>
			{
				return (s, a);
			},

			() =>
			{
				return (++s, (a + s).ToUpper());
			},

			() =>
			{
				step.ReturnVoid();
				return default;            
			});

			return step;
		}

		private FunctionStepper<StepperVoid> Foo2(int s, string a)
		{
			var step = new FunctionStepper<StepperVoid>();

			step.CreateSteps(() =>
			{
				a += (a);
				a += s;
				s += 102;
				return default;
			},
			
			() =>
			{
				s++;
				a += s;
				a += a;
				return default;
			});

			return step;
		}
	}
}
