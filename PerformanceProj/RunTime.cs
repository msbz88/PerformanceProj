using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceProj {
    public static class RunTime {
        static Stopwatch Watch = new Stopwatch();

        public static void PrintStopwatchResult(string controlName) {
            Watch.Stop();
            var elapsedMs = Watch.ElapsedMilliseconds;
            Console.WriteLine("Runtime of [" + controlName + "]: " + elapsedMs + " ms (" + elapsedMs/1000 + " sec)");
        }

        public static void StartWatch() {
            Watch.Reset();
            Watch.Start();
        }
    }
}
