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
				Assert.IsTrue(sr.IsVoid || sr.Result == srr, 
					$"iteration:{i}, srr:{srr}, sr.R:{(sr.IsVoid ? null : (object)sr.Result)}");			
			}
		}

		[TestMethod]
		public void StepperVoidTest()
		{
			var r = Foo2(924, "asdFsa");

			var req = new string[]
			{
				"925asdFsa",
				"926asdfsaasdfsaasdfsa926",
				"926ASDFSAASDFSAASDFSA926"
			};

			for (int i = 0; r.HasNextStep; i++)
			{
				var srr = req[i];
				r.DoNextStep();
				Assert.IsTrue(testResource == srr, $"iteration:{i}, srr:{srr}, testResource:{testResource}");
			}
		}

		private string testResource;


		private FunctionStepper<(int, string)>.IFunctionStepperExecutor Foo(int s, string a)
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

		private FunctionStepper<StepperVoid>.IFunctionStepperExecutor Foo2(int s, string a)
		{
			var step = new FunctionStepper<StepperVoid>();

			step.CreateSteps(() =>
			{
				testResource = ++s + a;
				a = a.ToLower();

				return default;
			},
			
			() =>
			{
				testResource = ++s + a + a + a + s++;

				return default;
			},
			
			() =>
			{
				testResource = testResource.ToUpper();

				return default;
			});

			return step;
		}
	}
}
