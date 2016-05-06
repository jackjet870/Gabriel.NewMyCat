using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Gabriel.NewMyCat.GUI
{
    /// <summary>
    /// 代码性能测试
    /// </summary>
    public class CodeTimerHelper
    {
        public static void Initialize()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Time("", 1, () => { });
        }

        public static void Time(string name, int iteration, Action action)
        {
            if (String.IsNullOrEmpty(name)) return;

            // warm up
            action();

            // 1.
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);
            Console.ForegroundColor = ConsoleColor.White;
            // 2.
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            int[] gcCounts = new int[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            // 3.
            Stopwatch watch = new Stopwatch();
            watch.Start();
            ulong cycleCount = GetCycleCount();
            for (int i = 0; i < iteration; i++) action();
            ulong cpuCycles = GetCycleCount() - cycleCount;
            watch.Stop();

            // 4.
            //Console.ForegroundColor = currentForeColor;
            //Console.WriteLine("\tTime Elapsed:\t" + watch.ElapsedMilliseconds.ToString("N0") + "ms");
            //Console.WriteLine("\tCPU Cycles:\t" + cpuCycles.ToString("N0"));
            // 4.
            Console.ForegroundColor = currentForeColor;
            Console.WriteLine("\tTotal Time:\t" + iteration.ToString("N0"));
            Console.WriteLine("\tTime Elapsed:\t" + watch.ElapsedMilliseconds.ToString("N0") + "ms");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\t1 second Time:\t" + (iteration* 1000 / watch.ElapsedMilliseconds ).ToString("N0") + "times");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\tCPU Cycles:\t" + cpuCycles.ToString("N0"));
            // 5.
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                int count = GC.CollectionCount(i) - gcCounts[i];
                Console.WriteLine("\tGen " + i + ": \t\t" + count);
            }

            Console.WriteLine();
        }

        private static ulong GetCycleCount()
        {
            ulong cycleCount = 0;
            QueryThreadCycleTime(GetCurrentThread(), ref cycleCount);
            return cycleCount;
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThread();
    }
}