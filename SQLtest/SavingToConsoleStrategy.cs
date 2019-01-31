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
            Console.WriteLine("Data Base query performance result:" +
            "\nLine number: " + result.LineNumber.ToString() +
            "\nCpu time: " + result.CpuTime.ToString() +
            "\nElapsed time: " + result.ElapsedTime.ToString() +
            "\nBytes received: " + result.BytesReceived.ToString() +
            "\nRows selected: " + result.SelectRows.ToString() + "\n");
        }

        void SavingStrategy.SaveResult(LocalPerformanceResult result)
        {
            Console.WriteLine("Data Base query performance result:" +
            "\nCpu time: " + result.CpuTime.ToString() +
            "\nCpu usage: " + result.CpuUsage.ToString() +
            "\nElapsed time: " + result.ElapsedTime.ToString() + "\n");
        }
    }
}
