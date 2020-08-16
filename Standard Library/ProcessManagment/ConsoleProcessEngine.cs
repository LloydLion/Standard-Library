using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

        /// <summary>
        /// 
        ///     Returns process status
        /// 
        /// </summary>
        public bool Running { get; private set; }

        /// <summary>
        /// 
        ///     Creates a new inctance and bind his with process start info
        /// 
        /// </summary>
        /// <param name="processStartInfo">Process bind</param>
        public ConsoleProcessEngine(ProcessStartInfo processStartInfo)
        {

            if (processStartInfo.UseShellExecute == false &&
                processStartInfo.RedirectStandardInput == true &&
                processStartInfo.RedirectStandardOutput == true)
            {
				process = new Process
				{
					StartInfo = processStartInfo
				};
			}
            else
            {
                throw new ArgumentException(ProcessStartInfoValidationExceptionMessage, nameof(processStartInfo));
            }
        }

        /// <summary>
        /// 
        ///     Reads segment from application output
        ///     Only if process running
        /// 
        /// </summary>
        /// <param name="startMark">Start reading string</param>
        /// <param name="endMark">Stop reading string</param>
        /// <returns>Reading result, without start and end masks</returns>
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

                if (builder.ToString().EndsWith(endMark)) startCount--;
                else if (builder.ToString().EndsWith(startMark)) startCount++;
            }
            while (startCount != 0);

            var s = builder.ToString();

            s = s.Substring(0, s.Length - endMark.Length);

            return s;
        }

        /// <summary>
        /// 
        ///     Launch process !sync with main thread
        ///     but wait async for app init
        /// 
        /// </summary>
        /// <param name="startedMark">Init mask, if it will be found in app output, then
        ///     the application is considered to be running</param>
        ///     
        /// <returns>Awaiter</returns>
        public TaskAwaiter StartAsync(string startedMark)
        {
            Start();

            return Task.Run(() =>
            {
                StringBuilder builder = new StringBuilder();
                while (builder.ToString().EndsWith(startedMark))
                    builder.Append((char)Reader.Read());
            }).GetAwaiter();
        }

        /// <summary>
        /// 
        ///     Simple process start
        /// 
        /// </summary>
        public void Start()
        {
            process.Start();
            Running = true;
       }

        /// <summary>
        /// 
        ///     Kill the running process
        /// 
        /// </summary>
        public void KillProcess()
        {
            CheckProcessStade();
            process.Kill();
            Running = false;
        }

        /// <summary>
        /// 
        ///     Stops process with sending in input stream stop command and async wait his termenate
        /// 
        /// </summary>
        /// <param name="stopCommand">Command for stop</param>
        /// <param name="waitTime">Max process terminating </param>
        /// <returns>Process termenate awaiter</returns>
        public TaskAwaiter StopAsync(string stopCommand, int waitTime = int.MaxValue)
        {
            CheckProcessStade();
            WriteCommand(stopCommand);

           return Task.Run(() => { process.WaitForExit(waitTime); Running = false; }).GetAwaiter();
        }

        /// <summary>
        /// 
        ///     Write command in app input 
        /// 
        /// </summary>
        /// <param name="command">Command to write</param>
        public void WriteCommand(string command)
        {
            CheckProcessStade();
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
