namespace SQLtest
{
    using System;
    using System.Diagnostics;

    public class PerformanceTester{
        private static Process process = Process.GetCurrentProcess();
        private static PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private const long TicksPerMillisecond = 10000;

        private long elapsed;
        private long startTimeStamp;
        private long elapsedCPU;
        private long startCPUTime;
        private CounterSample startCounterSample;
        private int avgCPUUsage;

        private TesterState state;
        private ResultSaver resultSaver;

        internal ResultSaver ResultSaver { get => resultSaver; set => resultSaver = value; }

        public PerformanceTester()
        {
            Reset();
        }

        public void changeState(TesterState state)
        {
            this.state = state;
        }

        public void Start()
        {
            state.Start();
        }

        public void Stop(int lineNumber)
        {
            state.Stop(lineNumber);
        }

        public void startTest()
        {
            // Calling start on a running PerformanceTester is a no-op.
            startTimeStamp = GetTimestamp();
            startCPUTime = GetCPUTime();
            startCounterSample = GetNextSample();
        }

        public void stopTest(int lineNumber)
        {
            // Calling stop on a stopped is a no-op
            long endTimeStamp = GetTimestamp();
            long elapsedThisPeriod = endTimeStamp - startTimeStamp;
            Elapsed1 += elapsedThisPeriod;

            long endCPUTime = GetCPUTime();
            long elapsedThisPeriodCPU = endCPUTime - startCPUTime;
            ElapsedCPU1 += elapsedThisPeriodCPU;

            CounterSample counterSampleCur = GetNextSample();
            AvgCPUUsage = (int)CounterSample.Calculate(startCounterSample, counterSampleCur);

            if (Elapsed1 < 0)
            {
                // When measuring small time periods the PerformanceTester.Elapsed* 
                // properties can return negative values.  This is due to 
                // bugs in the basic input/output system (BIOS) or the hardware
                // abstraction layer (HAL) on machines with variable-speed CPUs
                // (e.g. Intel SpeedStep).

                Elapsed1 = 0;
            }
            if (ElapsedCPU1 < 0)
            {
                ElapsedCPU1 = 0;
            }
            if (AvgCPUUsage < 0)
            {
                ElapsedCPU1 = 0;
            }

            Save(lineNumber);
            Reset();
        }

        private void Save(int lineNumber)
        {
            LocalPerformanceResult performanceResult = new LocalPerformanceResult();
            performanceResult.LineNumber = lineNumber;
            performanceResult.CpuTime = ElapsedCPU1;
            performanceResult.CpuUsage = AvgCPUUsage;
            performanceResult.ElapsedTime = (long)Elapsed1/TicksPerMillisecond;
            resultSaver.SaveResult(performanceResult);
        }

        public void Reset()
        {
            Elapsed1 = 0;
            ElapsedCPU1 = 0;
            AvgCPUUsage = 0;
            startTimeStamp = 0;
            startCPUTime = 0;
            state = new NotRunningState(this);
        }

        public TimeSpan Elapsed
        {
            get { return new TimeSpan(state.GetElapsedDateTimeTicks()); }
        }

        public TimeSpan ElapsedCPU
        {
            get { return new TimeSpan(state.GetElapsedCPUTime()); }
        }

        public int AvgCPUUsage { get => avgCPUUsage; set => avgCPUUsage = value; }
        public long ElapsedCPU1 { get => elapsedCPU; set => elapsedCPU = value; }
        public long Elapsed1 { get => elapsed; set => elapsed = value; }

        public static long GetTimestamp()
        {
            return DateTime.UtcNow.Ticks;
        }

        public long GetElapsedDateTimeTicks()
        {
            long timeElapsed = Elapsed1;
            // If the PerformanceTester is running, add elapsed time since
            // the PerformanceTester is started last time
            long currentTimeStamp = GetTimestamp();
            long elapsedUntilNow = currentTimeStamp - startTimeStamp;
            timeElapsed += elapsedUntilNow;
            return timeElapsed;
        }

        public static long GetCPUTime()
        {
            return (long) process.UserProcessorTime.TotalMilliseconds;
        }

        public long GetElapsedCPUTime()
        {
            long timeElapsedCPU = ElapsedCPU1;
            long currentCPUTime = GetCPUTime();
            long elapsedUntilNow = currentCPUTime - startCPUTime;
            timeElapsedCPU += elapsedUntilNow;
            return timeElapsedCPU;
        }

        private static CounterSample GetNextSample()
        {
            return cpuCounter.NextSample();
        }

        public int GetAvgCPUUsage()
        {
            CounterSample counterSampleCur = GetNextSample();
            int finalCPUPrcnt = (int)CounterSample.Calculate(startCounterSample, counterSampleCur);
            return finalCPUPrcnt;

        }
    }
}
