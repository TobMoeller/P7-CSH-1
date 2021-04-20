using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P7_CSH_1 {
    class Day4 : Day {
        public Day4() {
            addAufgabe("Mitschrift 01: Thread", Transcript01);
            addAufgabe("Mitschrift 02: ThreadPool", Transcript02);
        }

        static Stopwatch s1 = new Stopwatch();
        static Stopwatch s2 = new Stopwatch();
        static Stopwatch s3 = new Stopwatch();
        static TimeSpan ts1;
        static TimeSpan ts2;
        static TimeSpan ts3;
        static bool t1Fertig = false;
        static bool t2Fertig = false;

        /*
         *  Mitschrift 01: Thread
         */

        public void Transcript01() {
            int x = 10000;
            // Platz für Programmlogik
            //synchrone Ausführung erst order1, dann order2
            //s3.Start();
            //order1(x);
            //order2();
            //s3.Stop();
            //ts3 = s3.Elapsed;
            //Console.WriteLine("\nGESAMTZEIT: ++++ {0}s {1}ms +++++\n", ts3.Seconds, ts3.Milliseconds);
            // asynchrone Ausführung von 2 nebenläufigen Threads
            //Thread.Sleep(200);
            Thread t1 = new Thread(new ParameterizedThreadStart(order1));
            //t1.Priority = ThreadPriority.Highest;
            // t1 Methodenverweis auf Methode mit Parameterübergabe
            Thread t2 = new Thread(order2);
            s3.Start();
            t2.Priority = ThreadPriority.Highest;
            t1.Start(x);
            t2.Start();
            while (!t1Fertig || !t2Fertig) ; // bei t1Fertig = true und t2Fertig = true wird while-Schleife abgebrochen
            s3.Stop();
            ts3 = s3.Elapsed;
            Console.WriteLine("\nGESAMTZEIT: ++++ {0}s {1}ms +++++\n", ts3.Seconds, ts3.Milliseconds);
        }

        public void order1(object args) {
            Thread.Sleep(200);  // wenn Hintergrund-Thread im Running Modus
            int x = (int)args;
            s1.Start();

            for (int i = 0; i < x; i++) {
                if (i % 1000 == 0) Thread.Sleep(20); // Zustandswechsel 
                Console.ForegroundColor = ConsoleColor.Green;
                if (i % 10 != 0)
                    Console.Write(i.ToString() + " ORDER1 aktiv ");
                else
                    Console.Write(i.ToString() + " ORDER1 passiv\n");
            }
            s1.Stop();
            ts1 = s1.Elapsed;
            t1Fertig = true;
            Console.WriteLine("\nZeit ORDER1: {0}s {1}ms\n", ts1.Seconds, ts1.Milliseconds);
        }

        public void order2() {

            s2.Start();
            for (int i = 10000; i > 0; i--) {
                if (i % 1000 == 0) Thread.Sleep(20);
                Console.ForegroundColor = ConsoleColor.Red;
                if (i % 10 != 0)
                    Console.Write(i.ToString() + " order2 aktiv ");
                else
                    Console.Write(i.ToString() + " order2 passiv\n");
            }
            s2.Stop();
            ts2 = s2.Elapsed;
            Console.WriteLine("\nZeit order2: {0}s {1}ms\n", ts2.Seconds, ts2.Milliseconds);
            t2Fertig = true;
        }


        /*
         *  Mitschrift 02: Thread
         */

        static int verfuegbar;

        public void Transcript02() {
            int maxThreads;
            int asyncThreads;

            // Platz für Programmlogik

            ThreadPool.GetMaxThreads(out maxThreads, out asyncThreads);
            ThreadPool.GetAvailableThreads(out verfuegbar, out asyncThreads);
            Console.WriteLine("Max. Anzahl: {0} davon E/A-Threads: {1}", maxThreads, asyncThreads);
            Console.WriteLine("Pool true/false: {0}  Thread-ID: {1} ---> {2} \n Anzahl verfügbare Threads: {3}",
                       Thread.CurrentThread.IsThreadPoolThread.ToString(),
                       Thread.CurrentThread.ManagedThreadId,
                       System.Reflection.MethodBase.GetCurrentMethod().Name,
                       verfuegbar);

            AutoResetEvent fortsetzen1 = new AutoResetEvent(false);  // das Event ist nicht aktiv
            AutoResetEvent fortsetzen2 = new AutoResetEvent(false);  // das Event ist nicht aktiv
            ThreadPool.GetAvailableThreads(out verfuegbar, out asyncThreads);
            s3.Start();
            ThreadPool.QueueUserWorkItem(new WaitCallback(order12), fortsetzen1);

            ThreadPool.QueueUserWorkItem(new WaitCallback(order22), fortsetzen2);
            ThreadPool.GetAvailableThreads(out verfuegbar, out asyncThreads);

            if (fortsetzen1.WaitOne() && fortsetzen2.WaitOne()) {
                Console.WriteLine("ALLE Threads sind beendet");
            }
            s3.Stop();
            ts3 = s3.Elapsed;
            Console.WriteLine("\nGESAMT: {0}m {1}s {2}ms\n", ts3.Minutes, ts3.Seconds, ts3.Milliseconds);
            fortsetzen1.Reset();
            fortsetzen2.Reset();
            ThreadPool.QueueUserWorkItem(new WaitCallback(order12), fortsetzen1);
            ThreadPool.GetAvailableThreads(out verfuegbar, out asyncThreads);
            fortsetzen1.WaitOne();
        }

        static void order12(object obj) {
            Console.WriteLine("Pool true/false: {0}  Thread-ID: {1} ---> {2} \n Anzahl verfügbare Threads: {3}",
                       Thread.CurrentThread.IsThreadPoolThread.ToString(),
                       Thread.CurrentThread.ManagedThreadId,
                       System.Reflection.MethodBase.GetCurrentMethod().Name,
                       verfuegbar);
            s1.Start();
            int anz = 1000;
            //Console.WriteLine(anz.ToString());
            for (int i = anz; i > 0; i--) {
                if (i % 1000 == 0) Thread.Sleep(20);
                Console.ForegroundColor = ConsoleColor.Red;
                if (i % 10 != 0)
                    Console.Write(i.ToString() + " order12 aktiv ");
                else
                    Console.Write(i.ToString() + " order12 passiv\n");
            }
            s1.Stop();
            ts1 = s1.Elapsed;
            Console.WriteLine("\nZeit order2: {0}m {1}s {2}ms\n", ts1.Minutes, ts1.Seconds, ts1.Milliseconds);
            ((AutoResetEvent)obj).Set();
            Thread.CurrentThread.Abort();
        }

        public void order22(object obj) {
            Console.WriteLine("Pool true/false: {0}  Thread-ID: {1} ---> {2} \n Anzahl verfügbare Threads: {3}",
                      Thread.CurrentThread.IsThreadPoolThread.ToString(),
                      Thread.CurrentThread.ManagedThreadId,
                      System.Reflection.MethodBase.GetCurrentMethod().Name,
                      verfuegbar);
            s2.Start();
            int anz = 1000;
            //Console.WriteLine(anz.ToString());
            for (int i = anz; i > 0; i--) {
                if (i % 1000 == 0) Thread.Sleep(20);
                Console.ForegroundColor = ConsoleColor.White;
                if (i % 10 != 0)
                    Console.Write(i.ToString() + " order22 aktiv ");
                else
                    Console.Write(i.ToString() + " order22 passiv\n");
            }
            s2.Stop();
            ts2 = s2.Elapsed;
            Console.WriteLine("\nZeit order3: {0}m {1}s {2}ms\n", ts2.Minutes, ts2.Seconds, ts2.Milliseconds);
            ((AutoResetEvent)obj).Set();
            Thread.CurrentThread.Abort();

        }
    }
}
