using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P7_CSH_1 {
    class Day5 : Day {
        public Day5() {
            addAufgabe("Mitschrift 1: Synchronisation + Monitor", Transcript01);
            addAufgabe("Mitschrift 2: Semaphore / Nachtwache", Transcript02);
        }

        public void Transcript01() {
            Demo obj = new Demo();
            Thread thread1, thread2;
            thread1 = new Thread(new ThreadStart(obj.Worker));

            thread1.Name = "Master";
            thread1.Priority = ThreadPriority.Highest;
            thread2 = new Thread(new ThreadStart(obj.Worker));
            //thread2.Priority = ThreadPriority.Highest;
            thread2.Name = "Slave";
            thread1.Start();
            thread2.Start();

            Console.ReadKey();
        }

        class Demo {
            private int value;

            public void Worker() {
                //while (true)
                //{
                //    Aufruf erfolgt async, die Anweisungsfolge dementsprechend
                //   value++;
                //    if (value > 250) break;
                //    Console.WriteLine("Zahl = {0,5} Thread = {1}", value, Thread.CurrentThread.GetHashCode().ToString());
                //}
                //while (true)
                //{
                //    //Aufruf erfolgt ebenfalls asynchron, aber die Folge der Anweisungen wird, was ZErgebnisse von Rechenoperationen
                //    //betrifft, zwischengespeichert
                //    Monitor.Enter(this);
                //    value++;
                //    if (value > 250) break;

                //    Console.WriteLine("Zahl = {0,5} Thread = {1}", value, Thread.CurrentThread.Name.ToString());
                //    //Thread.Sleep(10);

                //    Monitor.Exit(this);
                //}

                while (true) {
                    //Aufruf erfolgt ebenfalls asynchron, aber die Folge der Anweisungen wird, was ZErgebnisse von Rechenoperationen
                    //betrifft, zwischengespeichert
                    lock (this) {
                        value++;
                        if (value > 250) break;

                        Console.WriteLine("Zahl = {0,5} Thread = {1}", value, Thread.CurrentThread.Name.ToString());
                        //Thread.Sleep(10);
                    }

                }
            }
        }


        /*
         *  Mitschrift 2: Semaphore / Nachtwächter
         */
        private static Thread w1, w2, w3;
        static Semaphore einsatzplan = new Semaphore(1, 10);
        static int stunden = 0;
        public void Transcript02() {
            int wachAnzahl = 12;
            Console.WriteLine("Synchronisierte Threads mit Semaphore");
            do {
                w1 = new Thread(waechter01);
                w1.Start();
                wachAnzahl--;
                Thread.Sleep(2000);
                w2 = new Thread(waechter02);
                w2.Start();
                wachAnzahl--;
                Thread.Sleep(2000);
                w3 = new Thread(waechter03);
                w3.Start();
                wachAnzahl--;
                Thread.Sleep(2000);
            } while (wachAnzahl > 0);
            w1.Join();
            w2.Join();
            w3.Join();
            Console.WriteLine("Nachtwache beendet");
            Console.ReadKey();
        }

        private static void waechter01() {
            einsatzplan.WaitOne();
            lock (" ") {
                Console.WriteLine("W1 wacht von " + stunden.ToString("0.##") + " Uhr bis " + (stunden + 2).ToString("0.##") + " Uhr, W2 und W3 ruhen");
                Thread.Sleep(5000);
                stunden += 2;
                einsatzplan.Release();
            }
        }


        private static void waechter02() {
            einsatzplan.WaitOne();
            lock (" ") {
                Console.WriteLine("W2 wacht von " + stunden.ToString("0.##") + " Uhr bis " + (stunden + 2).ToString("0.##") + " Uhr, W1 und W3 ruhen");
                Thread.Sleep(5000);
                stunden += 2;
                einsatzplan.Release();
            }
        }


        private static void waechter03() {
            lock (" ") {
                Console.WriteLine("W3 wacht von " + stunden.ToString("0.##") + " Uhr bis " + (stunden + 2).ToString("0.##") + " Uhr, W1 und W2 ruhen");
                Thread.Sleep(5000);
                stunden += 2;
                einsatzplan.Release();

            }
        }
    }
}
