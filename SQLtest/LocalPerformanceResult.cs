using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLtest
{
    class LocalPerformanceResult
    {
        private int lineNumber;
        private long cpuTime;
        private long elapsedTime;
        private long cpuUsage;

        public int LineNumber { get => lineNumber; set => lineNumber = value; }
        public long CpuTime { get => cpuTime; set => cpuTime = value; }
        public long ElapsedTime { get => elapsedTime; set => elapsedTime = value; }
        public long CpuUsage { get => cpuUsage; set => cpuUsage = value; }

        // FOR TESTING:
        public override string ToString()
        {
            return "line: " + LineNumber.ToString() + "\ncpu: " + CpuTime.ToString() + 
                "\nelapsed: " + ElapsedTime.ToString() + "\ncpuusage: " + CpuUsage.ToString();
        }
    }
}
