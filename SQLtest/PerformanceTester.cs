namespace SQLtest
{
    using Microsoft.Win32;
    using System;
    #if FEATURE_NETCORE
        using System.Security;
    #endif

    public class PerformanceTester{
        private const long TicksPerMillisecond = 10000;

        private long elapsed;
        private long startTimeStamp;
        private bool isRunning;

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
                isRunning = true;
            }
        }

        public static PerformanceTester StartNew()
        {
            PerformanceTester p = new PerformanceTester();
            p.Start();
            return p;
        }

        public void Stop()
        {
            // Calling stop on a stopped is a no-op
            if (isRunning)
            {
                long endTimeStamp = GetTimestamp();
                long elapsedThisPeriod = endTimeStamp - startTimeStamp;
                elapsed += elapsedThisPeriod;
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
            }
        }

        public void Reset()
        {
            elapsed = 0;
            isRunning = false;
            startTimeStamp = 0;
        }

        // Convenience method for replacing {pt.Reset(); pt.Start();} with a single pt.Restart()
        public void Restart()
        {
            elapsed = 0;
            startTimeStamp = GetTimestamp();
            isRunning = true;
        }

        public bool IsRunning
        {
            get { return isRunning; }
        }

        public TimeSpan Elapsed
        {
            get { return new TimeSpan(GetElapsedDateTimeTicks()); }
        }

        public long ElapsedMilliseconds
        {
            get { return GetElapsedDateTimeTicks() / TicksPerMillisecond; }
        }

        public long ElapsedTicks
        {
            get { return GetRawElapsedTicks(); }
        }

#if FEATURE_NETCORE
        [SecuritySafeCritical]
#endif
        public static long GetTimestamp()
        {
            return DateTime.UtcNow.Ticks;
        }

        //Get the elapsed ticks
#if FEATURE_NETCORE
        public long GetRawElapsedTicks() {
#else
        private long GetRawElapsedTicks()
        {
#endif
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

        // Get the elapsed ticks.        
#if FEATURE_NETCORE
        public long GetElapsedDateTimeTicks() {
#else
        private long GetElapsedDateTimeTicks()
        {
#endif
            long rawTicks = GetRawElapsedTicks();
            return rawTicks;
            
        }
    }
}
