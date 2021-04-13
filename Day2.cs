using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P7_CSH_1 {
    class Day2 : Day {
        public Day2() {
            addAufgabe("Mitschrift 1: Regex IsMatch", Transcript01);
            addAufgabe("Mitschrift 2: Regex Match und Matches", Transcript02);
            addAufgabe("Aufgabe 1: CSV Datei einlesen und auswerten", Exercise01);
        }

        // Beispiel zur Regex Erzeugung
        public void Transcript01() {
            //Teststring erzeugen
            string wbsString = "WBS Training AG";
            //regulären Ausdruck erstellen
            Regex r = new Regex("(WBS)+");
            //Teststring überprüfen
            Console.WriteLine(r.IsMatch(wbsString));
            
        }

        public void Transcript02() {
            Regex rex = new Regex(@"\d{5,}");
            string rexMatches = @"\d{2,}";
            string matchText = "heute ist der 13.04.2021";
            // Prüfmuster: mindestens 5 Ziffern
            Console.WriteLine("Suche von mind. 5 Ziffern in Folge");
            Match m = rex.Match(matchText);
            Console.WriteLine("********************************** ");
            if (m.Success) Console.WriteLine(m.Value + " an Position: " + m.Index + 1);
            else Console.WriteLine("Suche ohne Erfolg");
            Console.WriteLine("********************************** ");
            // Prüfmuster mc2 Mind. 2 Ziffern
            Console.WriteLine("Suche von mind. 2 Ziffern in Folge");
            Console.WriteLine("********************************** ");
            MatchCollection mc2 = Regex.Matches(matchText, rexMatches);
            foreach (Match ma2 in mc2) {
                Console.WriteLine(Environment.NewLine + "******************** ");
                Console.WriteLine("Inhalt: " + ma2.Value);
                Console.WriteLine("Index: " + ma2.Index + 1);
                Console.WriteLine("Zeichenlänge: " + ma2.Length);
            }

            

            //string text = "Heute kümmern wir uns um a Regulären .NEUES Ausdruck";
            //string regex = @"\w+";

            //MatchCollection mc = Regex.Matches(text, regex);
            //Console.WriteLine("Anzahl gefunden: " + mc.Count);
            //foreach (Match ma in mc)
            //{
            //    Console.WriteLine("******************** ");
            //    Console.WriteLine("Inhalt: " + ma.Value);
            //    Console.WriteLine("Index: " + (ma.Index + 1));
            //    Console.WriteLine("Zeichenlänge: " + ma.Length);
            //}
            //string ausgabeText;
            //Console.WriteLine(text);
            //ausgabeText = Regex.Replace(text, "N.V.S", "not");
            //Console.WriteLine(ausgabeText);
        }

        public void Exercise01() {
            string filepath = @"C:\Users\tmide\Documents\dev\P7-CSH\P7-Dateien";
            string regex = "";
            //BLZ.readFile(filepath);
            //BLZ.filterFile(regex);
            BLZ.readWholeFile(filepath);
        }

        class BLZ {
            public static List<string> fileLines = new List<string>();
            public static string firstLine;

            public static void readFile(string filePath) {
                StreamReader streamReader = new StreamReader(filePath + @"\blz-aktuell-xls-data.csv");
                firstLine = streamReader.ReadLine();
                while (streamReader.Peek() != -1) {
                    fileLines.Add(streamReader.ReadLine());
                }
                streamReader.Close();
                Console.WriteLine("Listlänge: " + fileLines.Count);
            }

            public static void readWholeFile(string filePath) {
                StreamReader streamReader = new StreamReader(filePath + @"\blz-aktuell-xls-data.csv");
                string wholeFile = streamReader.ReadToEnd();
                streamReader.Close();

                Console.WriteLine("BLZ Liste zu welcher Stadt?");
                string town = Console.ReadLine();
                StreamWriter streamWriter = new StreamWriter(filePath + $@"\{Regex.Replace(town, @"\s", "")}.csv", false);

                //Regex regex = new Regex(@"[\d]{8};\d;" + "Mainz");
                // 10010010;1;Postbank Ndl der Deutsche Bank;10559;Berlin;
                MatchCollection matchCollection = Regex.Matches(wholeFile, @"[\d]{8};\d;(.*?);\d*;" + town);
                foreach (Match match in matchCollection) {
                    Console.WriteLine(match.Value);
                    string temp = match.Value.Remove(8);
                    Console.WriteLine(temp);
                    streamWriter.WriteLine(temp);
                }
                streamWriter.Close();
            }

            public static void filterFile(string regex) {
                Regex regex1 = new Regex(regex);
            }
        }
    }
}
