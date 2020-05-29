using System;
using System.Collections.Generic;
using System.Text;

namespace Standard_Library.Functions
{
    public class FunctionStepper<TResult>
    {
        List<Func<TResult>> steps = new List<Func<TResult>>();
        int iterator = 0;
        bool voidReturned = true;

        public void CreateStep(Func<TResult> func)
        {
            steps.Add(func);
        }

        /// <summary>
        /// Don't return value from step
        /// *After invoke this method return any value
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


        private FunctionStepperStepResult<TResult> DoStep(int i)
        {
            FunctionStepperStepResult<TResult> r;
            var val = steps[i].Invoke();
            if (voidReturned == true)
                r = new FunctionStepperStepResult<TResult>();
            else
                r = new FunctionStepperStepResult<TResult>(val);

            voidReturned = false;
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

            public T Result { get; private set; }
            public bool IsVoid { get; private set; }
        }

        public class StepperVoid { }
    }
}
