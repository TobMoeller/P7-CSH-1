using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P7_CSH_1 {
    class Day6 : Day {
        public Day6() {
            addAufgabe("Mitschrift 1: Task", Transcript01);
            addAufgabe("Übung 1: Tasks vs Threads", Exercise1);
        }

        public void Transcript01() {
            // Parallel.Invoke(Task1, Task2, Task3, Task4, Task5, Task6, Task7, Task8, Task9);
            // Parallel.Invoke(Task1, Task2, Task3, Task4);
            Parallel.Invoke(Task1, Task2, Task3);
        }
        static void Task1() {
            for (int i = 0; i < 100; i++) {
                Thread.Sleep(50);
                Console.WriteLine(" #1 ");
            }
        }
        static void Task2() {
            for (int i = 0; i < 100; i++) {
                Thread.Sleep(50);
                Console.Write(" #2 ");
            }
        }
        static void Task3() {
            for (int i = 0; i < 100; i++) {
                Thread.Sleep(50);
                Console.Write(" #3 ");
            }

        }

        static void Task4() {
            for (int i = 0; i < 100; i++) {
                Thread.Sleep(50);
                Console.WriteLine(" #4 ");
            }

        }

        static void Task5() {
            for (int i = 0; i < 100; i++) {
                Thread.Sleep(50);
                Console.Write(" #5 ");
            }

        }

        static void Task6() {
            for (int i = 0; i < 100; i++) {
                Thread.Sleep(50);
                Console.Write(" #6 ");
            }

        }

        static void Task7() {
            for (int i = 0; i < 100; i++) {
                Thread.Sleep(50);
                Console.Write(" #7 ");
            }

        }

        static void Task8() {
            for (int i = 0; i < 100; i++) {
                Thread.Sleep(50);
                Console.Write(" #8 ");
            }

        }

        static void Task9() {
            for (int i = 0; i < 100; i++) {
                Thread.Sleep(50);
                Console.Write(" #9 ");
            }

        }


        public void Exercise1() {
            string path = @"C:\Users\tmide\Documents\dev\P7-CSH\P7-Dateien";
            StrassenErweitert strassen = new StrassenErweitert(path);
            strassen.ZusammenThreadPool();
            strassen.ZusammenThread();
            strassen.ZusammenTask();
        }

        class StrassenErweitert {
            private string path;
            StreamReader streamReader;
            private string dateiInhalt = "";
            Stopwatch s1 = new Stopwatch();
            Stopwatch s2 = new Stopwatch();
            Stopwatch s3 = new Stopwatch();
            TimeSpan ts1;
            TimeSpan ts2;
            TimeSpan ts3;

            public StrassenErweitert(string path) {
                this.path = path;
                streamReader = new StreamReader(this.path + @"\strassen_osm.txt");
                dateiInhalt += streamReader.ReadToEnd();
                streamReader.Close();
            }

            public void Zusammenfuegen(object obj) {
                string name = null;
                AutoResetEvent autoResetEvent = null;
                if (obj is ZusammenArgs) {
                    name = (obj as ZusammenArgs).name;
                    autoResetEvent = (obj as ZusammenArgs).autoResetEvent;
                } else if (obj is string) {
                    name = (string)obj;
                }


                StreamWriter streamWriter = new StreamWriter(path + $@"\strassen_{(string)name}.txt", false);
                for (int i = 0; i < 4; i++) {
                    streamWriter.Write(dateiInhalt);
                }
                streamWriter.Flush();
                streamWriter.Close();


                autoResetEvent?.Set();
            }

            public void ZusammenTask() {
                s1.Start();
                Task temp = Task.Factory.StartNew(Zusammenfuegen, "Task");
                //Task temp = new Task(Zusammenfuegen, "Task");
                //temp.Start();
                temp.Wait();
                s1.Stop();
                ts1 = s1.Elapsed;
                Console.WriteLine("Task benötigt: " + s1.Elapsed.TotalMilliseconds);
            }

            public void ZusammenThread() {
                s2.Start();
                Thread thread = new Thread(new ParameterizedThreadStart(Zusammenfuegen));
                thread.Start("Thread");
                thread.Join();
                s2.Stop();
                ts2 = s2.Elapsed;
                Console.WriteLine("Thread benötigt: " + s2.Elapsed.TotalMilliseconds);
            }

            public void ZusammenThreadPool() {
                s3.Start();
                AutoResetEvent aRE = new AutoResetEvent(false);
                ThreadPool.QueueUserWorkItem(Zusammenfuegen, new ZusammenArgs() { name = "Pool", autoResetEvent = aRE });

                aRE.WaitOne();
                s3.Stop();
                ts3 = s3.Elapsed;
                Console.WriteLine("ThreadPool benötigt: " + s3.Elapsed.TotalMilliseconds);
            }
            class ZusammenArgs {
                public string name;
                public AutoResetEvent autoResetEvent;
            }
        }
    }
}
