using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Standard_Library.ProcessManagment
{
    public class ConsoleProcessEngine
    {
        private ProcessStartInfo processStartInfo;

        public ConsoleProcessEngine(ProcessStartInfo processStartInfo)
        {
            this.processStartInfo = processStartInfo;
        }

        public string ReadConsoleSegment(string start, string end)
        {

        }
    }
}
