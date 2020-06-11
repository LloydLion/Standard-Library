using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardLibrary.Functions
{
	public class FunctionStepper<TResult> : FunctionStepper<TResult>.IFunctionStepperExecutor
	{
		public const string StepReturnsVoidResultExceptionMessage = 
			"Step returns void result";
		
		public const string MainResultHasAlreadyReturnedExceptionMessage =
			"Main result has already returned";

		public const string AllStepsExecutedExceptionMessage =
			"All steps executed";

		public const string StepIsNotAsyncExecutableExceptionMessage =
			"Step isn't async executeble";



		private readonly List<StepInfo> steps = new List<StepInfo>();
		private readonly List<FunctionStepperStepResult> results = new List<FunctionStepperStepResult>();
		private int iterator = 0;
		private bool voidReturned = typeof(TResult) == typeof(StepperVoid);
		private TResult mainResult;
		private bool mainReturned;


		public FunctionStepperStepResult Result =>
			mainReturned ? new FunctionStepperStepResult(mainResult) :
				new FunctionStepperStepResult();

		public bool HasNextStep => steps.Count - 1 > iterator;

		public IFunctionStepperExecutor Executor => this;


		public void CreateStep(Func<TResult> func)
		{
			steps.Add(new StepInfo() { Step = func });
		}

		public void CreateSteps(params Func<TResult>[] funcs)
		{
			steps.AddRange(funcs.Select((s) => new StepInfo() { Step = s }).ToArray());
		}

		/// <summary>
		/// Don't return value from step
		/// *After invoke this method return any value
		/// *If TResult is StepperVoid, don't need invoke this method 
		/// </summary>
		public void ReturnVoid()
		{
			voidReturned = true;
		}

		public void SkipNextStep()
		{
			iterator++;
		}

		public void ReturnMainResult(TResult value)
		{
			if (!mainReturned)
			{
				mainResult = value;
				mainReturned = true;
			}
			else
				throw new InvalidOperationException(MainResultHasAlreadyReturnedExceptionMessage);
		}

		public void ExecuteNextStepAsAsync()
		{
			steps[iterator + 1].IsAsync = true;
		}

		//Executor Methods
		public FunctionStepperStepResult DoNextStep()
		{
			var r = DoStep(iterator);
			iterator++;
			return r;
		}

		public FunctionStepperStepResult ExecuteAllSteps(out FunctionStepperStepResult[] results)
		{
			results = new FunctionStepperStepResult[steps.Count];
			for (; HasNextStep; iterator++)
			{
				results[iterator] = DoStep(iterator);

			}

			return Result;
		}

		public FunctionStepperStepResult ExecuteAndWaitAllSteps(out FunctionStepperStepResult[] results)
		{
			var tmp = ExecuteAllSteps(out results);
			WaitAllStepsTasks();

			return tmp;
		}

		public FunctionStepperStepResult ExecuteAndWaitAllSteps() => ExecuteAndWaitAllSteps(out var _);

		public FunctionStepperStepResult ExecuteAllSteps() => ExecuteAllSteps(out var _);

		public FunctionStepperStepResult WaitPreviousStepTask()
		{
			if (steps[iterator - 1].IsAsync == false) 
				throw new InvalidOperationException(StepIsNotAsyncExecutableExceptionMessage);

			results[iterator - 1].AsyncResult.Wait();
			return results[iterator - 1];
		}

		public FunctionStepperStepResult[] WaitAllStepsTasks()
		{
			List<FunctionStepperStepResult> r = new List<FunctionStepperStepResult>();
			for (int i = 0; i < results.Count; i++)
			{
				if (steps[i].IsAsync == false) continue;
				results[i].AsyncResult.Wait();
				r.Add(results[i]);
			}

			return r.ToArray();
		}

		public FunctionStepperStepResult WaitStepTaskByIndex(int index)
		{
			if (steps[index].IsAsync == false)
				throw new InvalidOperationException(StepIsNotAsyncExecutableExceptionMessage);

			results[index].AsyncResult.Wait();
			return results[index];
		}

		public FunctionStepperStepResult GetStepResult(int index)
		{
			if (index < 0 || index > results.Count - 1) 
				throw new ArgumentOutOfRangeException(nameof(index));

			return results[index];
		}

		//Private Methods
		private FunctionStepperStepResult DoStep(int i)
		{
			if (steps.Count - 1 < i) throw new InvalidOperationException(AllStepsExecutedExceptionMessage);

			FunctionStepperStepResult r;
			if (steps[i].IsAsync == false)
			{
				var val = steps[i].Step.Invoke();
				if (voidReturned == true)
					r = new FunctionStepperStepResult();
				else
					r = new FunctionStepperStepResult(val);
			}
			else
			{
				var val = Task.Run(steps[i].Step.Invoke);
				if (voidReturned == true)
					r = new FunctionStepperStepResult();
				else
					r = new FunctionStepperStepResult(val);
			}

			voidReturned = typeof(TResult) == typeof(StepperVoid);

			results.Add(r);
			return r;
		}


		private class StepInfo
		{
			public Func<TResult> Step { get; set; }
			public bool IsVoidReturn { get; set; }
			public bool IsAsync { get; set; }
		}

		public class FunctionStepperStepResult
		{
			public const string ReturnedVoidExceptionMessage =
				"Step returned a void result";


			/// <summary>
			/// Invoke if returned a void
			/// </summary>
			public FunctionStepperStepResult()
			{
				result = default;
				IsVoid = true;
				isAsync = false;
			}

			public FunctionStepperStepResult(TResult result)
			{
				this.result = result;
				IsVoid = false;
				isAsync = true;
			}

			public FunctionStepperStepResult(Task<TResult> result)
			{
				asyncResult = result;
				IsVoid = false;
				isAsync = true;
			}


			public TResult Result
			{
				get
				{
					if (isAsync == false)
					{
						if (IsVoid == true) throw new InvalidOperationException(ReturnedVoidExceptionMessage);
						else return result;
					}
					else
					{
						if (IsVoid == true) throw new InvalidOperationException(ReturnedVoidExceptionMessage);
						else
						{
							asyncResult.Wait();
							return asyncResult.Result;
						}
					}
				}
			}

			public Task<TResult> AsyncResult 
			{ 
				get 
				{ 
					if (isAsync) return asyncResult;
					else throw new InvalidOperationException(StepIsNotAsyncExecutableExceptionMessage);
				} 
			}


			private readonly TResult result;
			private readonly Task<TResult> asyncResult;
			private readonly bool isAsync;

			public bool IsVoid { get; private set; }

			public static implicit operator TResult(FunctionStepperStepResult value) => value.Result;
		}

		public interface IFunctionStepperExecutor
		{
			FunctionStepperStepResult ExecuteAllSteps();

			FunctionStepperStepResult ExecuteAllSteps(out FunctionStepperStepResult[] results);

			FunctionStepperStepResult DoNextStep();

			FunctionStepperStepResult ExecuteAndWaitAllSteps(out FunctionStepperStepResult[] results);

			FunctionStepperStepResult ExecuteAndWaitAllSteps();

			FunctionStepperStepResult WaitPreviousStepTask();

			FunctionStepperStepResult[] WaitAllStepsTasks();

			FunctionStepperStepResult WaitStepTaskByIndex(int index);


			FunctionStepperStepResult Result { get; }

			bool HasNextStep { get; }
		}
	}

	public sealed class StepperVoid { private StepperVoid() { } }
}
