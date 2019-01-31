using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLtest
{
    interface SavingStrategy
    {
        void SaveResult(QueryPerformanceResult result);
        void SaveResult(LocalPerformanceResult result);
    }
}
