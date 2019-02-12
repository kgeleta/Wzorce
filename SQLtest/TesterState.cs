using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLtest
{
    public abstract class TesterState
    {
        protected PerformanceTester performanceTester;

        public TesterState(PerformanceTester tester)
        {
            Tester = tester;
        }

        public PerformanceTester Tester
        {
            get
            {
                return performanceTester;
            }

            set
            {
                performanceTester = value;
            }
        }

        public abstract void Start();
        public abstract void Stop(int lineNumber);
        public abstract long GetElapsedDateTimeTicks();
        public abstract long GetElapsedCPUTime();
        public abstract int GetAvgCPUUsage();
    }

    public class RunningState : TesterState
    {
        public RunningState(PerformanceTester tester) 
            : base(tester) { }

        public override void Start()
        {
            Console.WriteLine("Tester is already running");
        }

        public override void Stop(int lineNumber)
        {
            performanceTester.stopTest(lineNumber);
            performanceTester.changeState(new NotRunningState(Tester));
        }

        public override int GetAvgCPUUsage()
        {
            return Tester.GetAvgCPUUsage();
        }

        public override long GetElapsedCPUTime()
        {
            return Tester.GetElapsedCPUTime();
        }

        public override long GetElapsedDateTimeTicks()
        {
            return Tester.GetElapsedDateTimeTicks();
        }
    }

    public class NotRunningState : TesterState
    {
        public NotRunningState(PerformanceTester tester)
            : base(tester) { }

        public override void Start()
        {
            performanceTester.startTest();
            performanceTester.changeState(new RunningState(Tester));
        }

        public override void Stop(int lineNumber)
        {
            Console.WriteLine("Tester is not running");
        }

        public override int GetAvgCPUUsage()
        {
            return Tester.AvgCPUUsage;
        }

        public override long GetElapsedCPUTime()
        {
            return Tester.ElapsedCPU1;
        }

        public override long GetElapsedDateTimeTicks()
        {
            return Tester.Elapsed1;
        }
    }
}
