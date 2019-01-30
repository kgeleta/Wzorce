using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLtest
{
    class SavingToConsoleStrategy : SavingStrategy
    {
        void SavingStrategy.SaveResult(QueryPerformanceResult result)
        {
            Console.WriteLine("Data Base query performance result:");
            Console.WriteLine("Line number: " + result.LineNumber.ToString());
            Console.WriteLine("Cpu time: " + result.CpuTime.ToString());
            Console.WriteLine("Elapsed time: " + result.ElapsedTime.ToString());
            Console.WriteLine("Bytes received: " + result.BytesReceived.ToString());
            Console.WriteLine("Rows selected: " + result.SelectRows.ToString() + "\n");
        }
    }
}
