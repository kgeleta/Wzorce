using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLtest
{
    class QueryPerformanceResult
    {
        private int lineNumber;
        private long cpuTime;
        private long elapsedTime;
        private long bytesReceived;
        private long selectRows;

        public int LineNumber { get => lineNumber; set => lineNumber = value; }
        public long BytesReceived { get => bytesReceived; set => bytesReceived = value; }
        public long SelectRows { get => selectRows; set => selectRows = value; }
        public long CpuTime { get => cpuTime; set => cpuTime = value; }
        public long ElapsedTime { get => elapsedTime; set => elapsedTime = value; }

        public void setTime(String message)
        {
            string pattern = @"(\d+)";
            Match result = Regex.Match(message, pattern);
            if (result.Success)
            {
                CpuTime = Int64.Parse(result.Value);
                result = result.NextMatch();
                ElapsedTime = Int64.Parse(result.Value);
            }
        }

        // FOR TESTING:
        public override string ToString()
        {
            return "line: " + LineNumber.ToString() + "\ncpu: " + CpuTime.ToString() +
                "\nelapsed: " + ElapsedTime.ToString() + "\nbytes: " + BytesReceived.ToString() + "\nrows: " + SelectRows.ToString();
        }


    }
}
