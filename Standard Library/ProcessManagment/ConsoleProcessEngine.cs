using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardLibrary.ProcessManagment
{
    public class ConsoleProcessEngine
    {
        public const string ProcessStartInfoValidationExceptionMessage = 
            "Please set $.UseShellExecute, $.RedirectStandardInput and $.RedirectStandardOutput to true";

        public const string ProcessHasNotStartedExceptionMessage =
            "Process has not started";


        private readonly Process process;

        private StreamWriter Writer { get => process.StandardInput; }
        private StreamReader Reader { get => process.StandardOutput; }

        public bool Running { get; private set; }

        public ConsoleProcessEngine(ProcessStartInfo processStartInfo)
        {

            if (processStartInfo.UseShellExecute == true &&
                processStartInfo.RedirectStandardInput == true &&
                processStartInfo.RedirectStandardOutput == true)
            {
                process = new Process();
                process.StartInfo = processStartInfo;
            }
            else
            {
                throw new ArgumentException(ProcessStartInfoValidationExceptionMessage, nameof(processStartInfo));
            }
        }

        public string ReadConsoleSegment(string startMark, string endMark)
        {
            CheckProcessStade();

            StringBuilder builder = new StringBuilder();
            StringBuilder alphaBuilder = new StringBuilder();
            char g;
            int startCount = 0;

            do
            {
                g = (char)Reader.Read();
                alphaBuilder.Append(g);

                if (alphaBuilder.ToString().EndsWith(startMark)) startCount++;
            }
            while (startCount < 1);

            do
            {
                g = (char)Reader.Read();

                if (startCount > 0)
                    builder.Append(g);

                if (builder.ToString().EndsWith(startMark)) startCount++;
                else if (builder.ToString().EndsWith(endMark)) startCount--;
            }
            while (startCount != 0);

            var s = builder.ToString();

            if (s.Length - s.Reverse().ToList().IndexOf('\n') - 4 < 2) return "";

            s = s.Substring(2, s.Length - s.Reverse().ToList().IndexOf('\n') - 4);


            return s;
        }

        public Task StartAsync(string startedMark)
        {
            Start();

            return Task.Run(() =>
            {
                StringBuilder builder = new StringBuilder();
                while (builder.ToString().EndsWith(startedMark))
                    builder.Append((char)Reader.Read());
            });
        }

        public void Start()
        {
            process.Start();
            Running = true;
        }

        public void KillProcess()
        {
            process.Kill();
            Running = false;
        }

        public Task StopAsync(string stopCommand, int waitTime = int.MaxValue)
        {
            WriteCommand(stopCommand);

            return Task.Run(() => { process.WaitForExit(waitTime); Running = false; });
        }
        
        public void Stop(string stopCommand, int waitTime = int.MaxValue)
        {
            StopAsync(stopCommand, waitTime).Wait();
        }

        public void WriteCommand(string command)
        {
            Writer.WriteLine(command);
        }

        private void CheckProcessStade()
        {
            if(!Running)
            {
                throw new InvalidOperationException("Process has not running");
            }
        }
    }
}
