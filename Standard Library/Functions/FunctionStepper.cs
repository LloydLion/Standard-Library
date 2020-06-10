using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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


        private readonly List<StepInfo> steps;
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

        public FunctionStepperStepResult ExecuteAllSteps() => ExecuteAllSteps(out var _);

        private FunctionStepperStepResult DoStep(int i)
        {
            if (steps.Count - 1 < i) throw new InvalidOperationException(AllStepsExecutedExceptionMessage);

            FunctionStepperStepResult r;
            var val = steps[i].Step.Invoke();
            if (voidReturned == true)
                r = new FunctionStepperStepResult();
            else
                r = new FunctionStepperStepResult(val);

            voidReturned = typeof(TResult) == typeof(StepperVoid);

            return r;
        }


        private class StepInfo
        {
            public Func<TResult> Step { get; set; }
            public bool IsVoidReturn { get; set; }
        }

        public class FunctionStepperStepResult
        {
            /// <summary>
            /// Invoke if returned a void
            /// </summary>
            public FunctionStepperStepResult()
            {
                Result = default;
                IsVoid = true;
            }

            public FunctionStepperStepResult(TResult result)
            {
                Result = result;
                IsVoid = false;
            }

            public TResult Result
            {
                get
                {
                    if (IsVoid == true) throw new InvalidOperationException();
                    else return result;
                }
                private set => result = value;
            }
            private TResult result;

            public bool IsVoid { get; private set; }


            public static implicit operator TResult(FunctionStepperStepResult value) => value.Result;
        }

        public interface IFunctionStepperExecutor
        {
            FunctionStepperStepResult ExecuteAllSteps();
            FunctionStepperStepResult ExecuteAllSteps(out FunctionStepperStepResult[] results);
            FunctionStepperStepResult DoNextStep();

            FunctionStepperStepResult Result { get; }
            bool HasNextStep { get; }
        }
    }

    public sealed class StepperVoid { private StepperVoid() { } }
}
