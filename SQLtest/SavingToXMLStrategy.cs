using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLtest
{
    class SavingToXMLStrategy : SavingStrategy
    {
        void SavingStrategy.SaveResult(QueryPerformanceResult result)
        {
            Console.WriteLine("Not implemented");
        }

        void SavingStrategy.SaveResult(LocalPerformanceResult result)
        {
            Console.WriteLine("Not implemented");
        }
    }
}
