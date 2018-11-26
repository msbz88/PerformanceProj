using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceProj {
    public static class RunTime {
        static Stopwatch Watch = new Stopwatch();

        public static long StopWatch() {
            Watch.Stop();
            var elapsedMs = Watch.ElapsedMilliseconds;
            return elapsedMs;
        }

        public static void StartWatch() {
            Watch.Reset();
            Watch.Start();
        }
    }
}
