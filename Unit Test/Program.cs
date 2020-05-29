using System;
using System.Text;
using Standard_Library.Functions;

namespace Unit_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Unit Test. Testing class #Standart_Library.Functions.FunctionStepper");

            for (int i = 0; i < 4; i++)
            {
                var buffer = new byte[128];
                new Random().NextBytes(buffer);

                var r = Foo(new Random().Next(), Encoding.Default.GetString(buffer));

                while (r.HasNextStep())
                {
                    var sr = r.DoNextStep();

                    if (sr.IsVoid)
                    {
                        Console.WriteLine("@@void@@");
                    }
                    else
                    {
                        Console.WriteLine(sr.Result);
                    }
                }
            }
        }

        static FunctionStepper<(int, string)> Foo(int s, string q)
        {
            var stepper = new FunctionStepper<(int, string)>();

            stepper.CreateSteps(() =>
            {

                return (s, q);
            },
            
            () =>
            {
                var t = (++s + 12, q + --s);
                return t;
            },
            
            () =>
            {
                stepper.ReturnVoid();
                return default;
            });

            for (int i = 0; i < new Random().Next(20); i++)
            {
                stepper.CreateStep(() => (new Random(i + 142).Next(150), 
                    $"{new Random(i).NextDouble()} ---- execute ---- {i + 44}"));
            }

            return stepper;
        }
    }
}
