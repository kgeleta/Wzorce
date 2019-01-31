using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLtest
{
    class ResultSaver
    {
        private SavingStrategy strategy;

        public ResultSaver(SavingStrategy _strategy)
        {
            this.strategy = _strategy;
        }

        public void SetStrategy(SavingStrategy _strategy)
        {
            this.strategy = _strategy;
        }

        public void SaveResult(QueryPerformanceResult result)
        {
            this.strategy.SaveResult(result);
        }

        public void SaveResult(LocalPerformanceResult result)
        {
            this.strategy.SaveResult(result);
        }
    }
}
