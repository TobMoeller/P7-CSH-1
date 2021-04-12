using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_CSH_1 {
    class Day1 : Day {
        public Day1() {
            addAufgabe("Mitschrift 1: Operatorenüberladung", Transcript01);
            addAufgabe("Mitschrift 2: Indexer", Transcript02);
        }

        /*
         *  Mitschrift 1: Operatorenüberladung
         */
        public void Transcript01() {
            Zeichen zeichen1 = new Zeichen('-');
            Zeichen zeichen2 = new Zeichen('>');
            Console.WriteLine(zeichen1 + zeichen2);
            Console.WriteLine('-' + '>');
            Console.WriteLine(20 * zeichen1);
            Console.WriteLine(zeichen1 == zeichen2);
            Console.WriteLine('-' == '>');
        }

        class Zeichen {
            private char wert; public Zeichen(char c) { wert = c; }
            public static string operator +(Zeichen c1, Zeichen c2) { 
                return "" + c1.wert + c2.wert; 
            }
            public static string operator *(int anzahl, Zeichen c) { 
                string s = ""; 
                for (int i = 0; i < anzahl; i++) { 
                    s += c.wert; 
                } 
                return s; 
            }

            public static bool operator ==(Zeichen c1, Zeichen c2) {
                return (c1.wert != c2.wert);
            }

            public static bool operator !=(Zeichen c1, Zeichen c2) {
                return (c1.wert == c2.wert);
            }
        }

        /*
         *  Mitschrift 2: Indexer
         */
        public void Transcript02() {
            Person person = new Person("Günther", "Manfred", 123456);
            Console.WriteLine(person["Name"]);
            person["telefon"] = "56789";
            Console.WriteLine(person["Telefon"]);
            Console.WriteLine(person[1]);
            person[2] = "12345";
            Console.WriteLine(person[2]);
        }

        class Person {
            private string name;
            private string vorname;
            private int telefon;

            public Person(string name, string vorname, int telefon) {
                this.name = name;
                this.vorname = vorname;
                this.telefon = telefon;
            }

            public string this[string index] {
                get {
                    switch (index.ToLower()) {
                        case "name":
                            return name;
                        case "vorname":
                            return vorname;
                        case "telefon":
                            return telefon.ToString();
                    }
                    return "";
                }
                set {
                    switch (index.ToLower()) {
                        case "name":
                            name = value; break;
                        case "vorname":
                            vorname = value; break;
                        case "telefon":
                            telefon = Convert.ToInt32(value); break;
                    }
                }
            }

            public string this[int index] {
                get {
                    switch (index) {
                        case 0:
                            return name;
                        case 1:
                            return vorname;
                        case 2:
                            return telefon.ToString();
                    }
                    return "";
                }
                set {
                    switch (index) {
                        case 0:
                            name = value; break;
                        case 1:
                            vorname = value; break;
                        case 2:
                            telefon = Convert.ToInt32(value); break;
                    }
                }
            }
        }
    }
}
