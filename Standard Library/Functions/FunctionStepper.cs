using System;
using System.Collections.Generic;
using System.Text;

namespace Standard_Library.Functions
{
    public class FunctionStepper<TResult>
    {
        List<Func<TResult>> steps = new List<Func<TResult>>();
        int iterator = 0;
        bool voidReturned = typeof(TResult) == typeof(StepperVoid);

        public void CreateStep(Func<TResult> func)
        {
            steps.Add(func);
        }

        public void CreateSteps(params Func<TResult>[] funcs)
        {
            steps.AddRange(funcs);
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

        public FunctionStepperStepResult<TResult> DoNextStep()
        {
            var r = DoStep(iterator);
            iterator++;
            return r;
        }

        public bool HasNextStep() => steps.Count - 1 > iterator;

        private FunctionStepperStepResult<TResult> DoStep(int i)
        {
            if (steps.Count - 1 < i) throw new InvalidOperationException("Executed all steps");

            FunctionStepperStepResult<TResult> r;
            var val = steps[i].Invoke();
            if (voidReturned == true)
                r = new FunctionStepperStepResult<TResult>();
            else
                r = new FunctionStepperStepResult<TResult>(val);

            if (typeof(TResult) == typeof(StepperVoid)) voidReturned = true;
            else voidReturned = false;

            return r;
        }

        public class FunctionStepperStepResult<T>
        {
            /// <summary>
            /// Invoke if returned a void
            /// </summary>
            public FunctionStepperStepResult()
            {
                Result = default;
                IsVoid = true;
            }

            public FunctionStepperStepResult(T result)
            {
                Result = result;
                IsVoid = false;
            }

            public T Result { get { if (IsVoid == true) throw new InvalidOperationException("Step returns void result");
                            else return result; } private set => result = value; }
            private T result;

            public bool IsVoid { get; private set; }


            public static implicit operator T(FunctionStepperStepResult<T> value) => value.Result;
        }
    }

    public class StepperVoid { }
}
