using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardLibrary.Functions
{
	/// <summary>
	/// 
	///		Use this for create Stepping Function
	///		Executing step by step
	/// 
	///		Crete instance in function, create steps by CreteStep() method
	///		and retunrn inctance as FunctionStepper<TResult>.IFunctionStepperExecutor
	///		then execute steps
	///	
	/// </summary>
	/// <typeparam name="TResult">
	/// 
	///		Type of function return value
	///		*Use StepperVoid type for void returned functions
	/// 
	/// </typeparam>
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

		/// <summary>
		/// 
		///		Main returned result of function
		///		
		/// </summary>
		public FunctionStepperStepResult Result =>
			mainReturned ? new FunctionStepperStepResult(mainResult) :
				new FunctionStepperStepResult();

		/// <summary>
		/// 
		///		If function has next step returns true
		///		else false
		///		
		/// </summary>
		public bool HasNextStep => steps.Count - 1 >= iterator;

		/// <summary>
		/// 
		///		Get executor 
		///		Return this from function
		///		
		/// </summary>
		public IFunctionStepperExecutor Executor => this;

		/// <summary>
		/// 
		///		Create new function step
		///		
		/// </summary>
		/// <param name="func">Step function</param>
		public void CreateStep(Func<TResult> func)
		{
			steps.Add(new StepInfo() { Step = func });
		}

		/// <summary>
		/// 
		///		Analog of CreateStep(Func<Tresult), but multiple
		///		
		/// </summary>
		/// <param name="funcs">Array of step functions</param>
		public void CreateSteps(params Func<TResult>[] funcs)
		{
			steps.AddRange(funcs.Select((s) => new StepInfo() { Step = s }).ToArray());
		}

		/// <summary>
		/// 
		///		Invoke in step if current step don't return value
		///		After invoke this method return any value, exemple: return default
		///		*If TResult is StepperVoid, don't need invoke this method 
		///		
		/// </summary>
		public void ReturnVoid()
		{
			voidReturned = true;
		}

		/// <summary>
		/// 
		///		Invoke in step
		///		Skip the next step
		///		
		/// </summary>
		public void SkipNextStep()
		{
			iterator++;
		}

		/// <summary>
		/// 
		///		Invoke in step
		///		Set function main result
		///		
		/// </summary>
		/// <param name="value">Main result</param>
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

		/// <summary>
		/// 
		///		Invoke in step
		///		Set next step as async
		///		
		/// </summary>
		public void ExecuteNextStepAsAsync()
		{
			steps[iterator + 1].IsAsync = true;
		}

		//Executor Methods

		/// <summary>
		/// 
		///		Executor Method
		///		Invoke and do next step in step list
		///		
		/// </summary>
		/// <returns>Result of execution</returns>
		public FunctionStepperStepResult DoNextStep()
		{
			var r = DoStep(iterator);
			iterator++;
			return r;
		}

		/// <summary>
		/// 
		///		Executor Method
		///		Executes all steps in list
		/// 
		/// </summary>
		/// <param name="results">Results of execution</param>
		/// <returns>Main result</returns>
		public FunctionStepperStepResult ExecuteAllSteps(out FunctionStepperStepResult[] results)
		{
			results = new FunctionStepperStepResult[steps.Count];
			for (; HasNextStep; iterator++)
			{
				results[iterator] = DoStep(iterator);

			}

			return Result;
		}

		/// <summary>
		/// 
		///		Executor Method
		///		Executes all steps in list,
		///		then wait all async steps	
		/// 
		/// </summary>
		/// <param name="results">Results of execution</param>
		/// <returns>Main result</returns>
		public FunctionStepperStepResult ExecuteAndWaitAllSteps(out FunctionStepperStepResult[] results)
		{
			var tmp = ExecuteAllSteps(out results);
			WaitAllStepsTasks();

			return tmp;
		}

		/// <summary>
		/// 
		/// 	Executor Method
		///		Executes all steps in list,
		///		then wait all async steps
		/// 
		/// </summary>
		/// <returns>Main result</returns>
		public FunctionStepperStepResult ExecuteAndWaitAllSteps() => ExecuteAndWaitAllSteps(out var _);

		/// <summary>
		/// 
		///		Executor Method
		///		Executes all steps in list
		/// 
		/// </summary>
		/// <returns>Main result</returns>
		public FunctionStepperStepResult ExecuteAllSteps() => ExecuteAllSteps(out var _);

		/// <summary>
		/// 
		///		Executor Method
		///		Wait previous step if is async
		///		else throw exception
		/// 
		/// </summary>
		/// <returns>Result of execution</returns>
		public FunctionStepperStepResult WaitPreviousStepTask()
		{
			if (steps[iterator - 1].IsAsync == false) 
				throw new InvalidOperationException(StepIsNotAsyncExecutableExceptionMessage);

			results[iterator - 1].AsyncResult.Wait();
			return results[iterator - 1];
		}

		/// <summary>
		/// 
		///		Executor Method
		///		Wait all executing async steps
		/// 
		/// </summary>
		/// <returns>Results of execution</returns>
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

		/// <summary>
		/// 
		///		Executor Method
		///		Wait acync step
		/// 
		/// </summary>
		/// <param name="index">Step index</param>
		/// <returns>Results of execution</returns>
		public FunctionStepperStepResult WaitStepTaskByIndex(int index)
		{
			if (steps[index].IsAsync == false)
				throw new InvalidOperationException(StepIsNotAsyncExecutableExceptionMessage);

			results[index].AsyncResult.Wait();
			return results[index];
		}

		/// <summary>
		/// 
		///		Executor Method
		///		Get step execution result
		/// 
		/// </summary>
		/// <param name="index">Step index</param>
		/// <returns>Requested Result</returns>
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

		/// <summary>
		///		
		///		Result model for Stepping Functions
		/// 
		/// </summary>
		public class FunctionStepperStepResult
		{
			public const string ReturnedVoidExceptionMessage =
				"Step returned a void result";


			/// <summary>
			///		
			///		Creates inctance for void result or async void result
			///		
			/// </summary>
			public FunctionStepperStepResult()
			{
				result = default;
				IsVoid = true;
				isAsync = false;
			}

			/// <summary>
			///	
			///		Creates inctance for non void result
			/// 
			/// </summary>
			/// <param name="result">Step executing result</param>
			public FunctionStepperStepResult(TResult result)
			{
				this.result = result;
				IsVoid = false;
				isAsync = true;
			}

			/// <summary>
			///	
			///		Creates inctance for async step with non void result
			/// 
			/// </summary>
			/// <param name="result">Step executing result</param>
			public FunctionStepperStepResult(Task<TResult> result)
			{
				asyncResult = result;
				IsVoid = false;
				isAsync = true;
			}


			/// <summary>
			/// 
			///		Gets the step result
			///		If result is void(void and async) throw exception
			///		If result is async wait task and return result
			/// 
			/// </summary>
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

			/// <summary>
			/// 
			///		Gets the async result task
			///		If result isn't async or void throw exception
			/// 
			/// </summary>
			public Task<TResult> AsyncResult 
			{ 
				get 
				{ 
					if (isAsync) return asyncResult;
					else throw new InvalidOperationException(StepIsNotAsyncExecutableExceptionMessage);
				} 
			}

			/// <summary>
			/// 
			///		If result void return true
			///		else false
			/// 
			/// </summary>
			public bool IsVoid { get; private set; }


			private readonly TResult result;
			private readonly Task<TResult> asyncResult;
			private readonly bool isAsync;


			/// <summary>
			/// 
			///		Implicit cast to TResult
			/// 
			/// </summary>
			/// <param name="value"></param>
			public static implicit operator TResult(FunctionStepperStepResult value) => value.Result;
		}

		public interface IFunctionStepperExecutor
		{
			/// <summary>
			/// 
			///		Executes all steps in list
			/// 
			/// </summary>
			/// <returns>Main result</returns>
			FunctionStepperStepResult ExecuteAllSteps();

			/// <summary>
			/// 
			///		Executes all steps in list
			/// 
			/// </summary>
			/// <returns>Main result</returns>
			FunctionStepperStepResult ExecuteAllSteps(out FunctionStepperStepResult[] results);

			/// <summary>
			/// 
			///		Invoke and do next step in step list
			///		
			/// </summary>
			/// <returns>Result of execution</returns>
			FunctionStepperStepResult DoNextStep();

			/// <summary>
			/// 
			///		Executes all steps in list,
			///		then wait all async steps
			/// 
			/// </summary>
			/// <returns>Main result</returns>
			FunctionStepperStepResult ExecuteAndWaitAllSteps(out FunctionStepperStepResult[] results);

			/// <summary>
			/// 
			///		Wait all executing async steps
			/// 
			/// </summary>
			/// <returns>Results of execution</returns>
			FunctionStepperStepResult ExecuteAndWaitAllSteps();

			/// <summary>
			/// 
			///		Wait previous step if is async
			///		else throw exception
			/// 
			/// </summary>
			/// <returns>Result of execution</returns>
			FunctionStepperStepResult WaitPreviousStepTask();

			/// <summary>
			/// 
			///		Wait all executing async steps
			/// 
			/// </summary>
			/// <returns>Results of execution</returns>
			FunctionStepperStepResult[] WaitAllStepsTasks();

			/// <summary>
			/// 
			///		Wait acync step
			/// 
			/// </summary>
			/// <param name="index">Step index</param>
			/// <returns>Results of execution</returns>
			FunctionStepperStepResult WaitStepTaskByIndex(int index);

			/// <summary>
			/// 
			///		Get step execution result
			/// 
			/// </summary>
			/// <param name="index">Step index</param>
			/// <returns>Requested Result</returns>
			FunctionStepperStepResult GetStepResult(int index);


			/// <summary>
			/// 
			///		Main returned result of function
			///		
			/// </summary>
			FunctionStepperStepResult Result { get; }

			/// <summary>
			/// 
			///		If function has next step returns true
			///		else false
			///		
			/// </summary>
			bool HasNextStep { get; }
		}
	}

	/// <summary>
	/// 
	///		Void type for FunctionStepper<TResult>
	/// 
	/// </summary>
	public sealed class StepperVoid { private StepperVoid() { } }
}
