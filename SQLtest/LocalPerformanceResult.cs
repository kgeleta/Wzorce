using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLtest
{
    class LocalPerformanceResult
    {
        private long cpuTime;
        private long elapsedTime;
        private float cpuUsage;
        
        public long CpuTime { get => cpuTime; set => cpuTime = value; }
        public long ElapsedTime { get => elapsedTime; set => elapsedTime = value; }
        public float CpuUsage { get => cpuUsage; set => cpuUsage = value; }

        // FOR TESTING:
        public override string ToString()
        {
            return "\ncpu: " + CpuTime.ToString() + "\nelapsed: " + 
                ElapsedTime.ToString() + "\ncpuusage: " + CpuUsage.ToString();
        }
    }
}
