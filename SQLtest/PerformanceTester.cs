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
        private float avgCPUUsage;

        private bool isRunning;
        private ResultSaver resultSaver;

        internal ResultSaver ResultSaver { get => resultSaver; set => resultSaver = value; }

        public PerformanceTester()
        {
            Reset();
        }

        public void Start()
        {
            // Calling start on a running PerformanceTester is a no-op.
            if (!isRunning)
            {
                startTimeStamp = GetTimestamp();
                startCPUTime = GetCPUTime();
                startCounterSample = GetNextSample();
                isRunning = true;
            }
        }

        public void Stop()
        {
            // Calling stop on a stopped is a no-op
            if (isRunning)
            {
                long endTimeStamp = GetTimestamp();
                long elapsedThisPeriod = endTimeStamp - startTimeStamp;
                elapsed += elapsedThisPeriod;

                long endCPUTime = GetCPUTime();
                long elapsedThisPeriodCPU = endCPUTime - startCPUTime;
                elapsedCPU += elapsedThisPeriodCPU;

                CounterSample counterSampleCur = GetNextSample();
                avgCPUUsage = CounterSample.Calculate(startCounterSample, counterSampleCur);

                isRunning = false;

                if(elapsed < 0)
                {
                    // When measuring small time periods the PerformanceTester.Elapsed* 
                    // properties can return negative values.  This is due to 
                    // bugs in the basic input/output system (BIOS) or the hardware
                    // abstraction layer (HAL) on machines with variable-speed CPUs
                    // (e.g. Intel SpeedStep).

                    elapsed = 0;
                }
                if(elapsedCPU < 0)
                {
                    elapsedCPU = 0;
                }
                if (avgCPUUsage < 0)
                {
                    elapsedCPU = 0;
                }

                Save();
                Reset();
            }
        }

        private void Save()
        {
            LocalPerformanceResult performanceResult = new LocalPerformanceResult();
            performanceResult.CpuTime = elapsedCPU;
            performanceResult.CpuUsage = avgCPUUsage;
            performanceResult.ElapsedTime = elapsed/TicksPerMillisecond;
            resultSaver.SaveResult(performanceResult);
        }

        public void Reset()
        {
            elapsed = 0;
            elapsedCPU = 0;
            avgCPUUsage = 0;
            isRunning = false;
            startTimeStamp = 0;
            startCPUTime = 0;
        }


        public bool IsRunning
        {
            get { return isRunning; }
        }

        public TimeSpan Elapsed
        {
            get { return new TimeSpan(GetElapsedDateTimeTicks()); }
        }

        public TimeSpan ElapsedCPU
        {
            get { return new TimeSpan(GetElapsedCPUTime()); }
        }

        public double AverageCPUUsage
        {
            get { return GetAvgCPUUsage(); }
        }

        public long ElapsedMilliseconds
        {
            get { return GetElapsedDateTimeTicks() / TicksPerMillisecond; }
        }

        public double ElapsedCPUMilliseconds
        {
            get { return GetElapsedCPUTime() / TicksPerMillisecond; }
        }

       
        public static long GetTimestamp()
        {
            return DateTime.UtcNow.Ticks;
        }

        private long GetElapsedDateTimeTicks()
        {
            long timeElapsed = elapsed;

            if (isRunning)
            {
                // If the PerformanceTester is running, add elapsed time since
                // the PerformanceTester is started last time
                long currentTimeStamp = GetTimestamp();
                long elapsedUntilNow = currentTimeStamp - startTimeStamp;
                timeElapsed += elapsedUntilNow;
            }
            return timeElapsed;
        }

        public static long GetCPUTime()
        {
            return (long) process.UserProcessorTime.TotalMilliseconds;
        }

        private long GetElapsedCPUTime()
        {
            double timeElapsedCPU = elapsedCPU;

            if (isRunning)
            {
                double currentCPUTime = GetCPUTime();
                double elapsedUntilNow = currentCPUTime - startCPUTime;
                timeElapsedCPU += elapsedUntilNow;
            }
            return (long)timeElapsedCPU;
        }

        private static CounterSample GetNextSample()
        {
            return cpuCounter.NextSample();
        }

        private float GetAvgCPUUsage()
        {
            float finalCPUPrcnt = avgCPUUsage;

            if (isRunning)
            {
                CounterSample counterSampleCur = GetNextSample();
                finalCPUPrcnt = CounterSample.Calculate(startCounterSample, counterSampleCur);
            }
            return finalCPUPrcnt;

        }
    }
}
