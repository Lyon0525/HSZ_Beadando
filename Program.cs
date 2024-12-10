using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSZ_Beadando_DLL;

namespace HSZ_Beadando {
    
    internal class Program {
        static Reaktor reactor = new Reaktor(0,0,0); 
        
        static void Main(string[] args) {
            try {
                //Random szám feltöltés: Kristóf
                Random r = new Random();
                reactor = new Reaktor(r.Next(5, 11), r.Next(4, 6), r.Next(550, 651));
                for (int i = 0; i < 24; i++) { //1 perc
                    //Delegált: Zoli
                    if (i % 3 == 0) {
                        reactor.Delegalt(true);
                    }
                    else {
                        reactor.Delegalt(false);
                    }
                    reactor.Kiiras();
                }
                //Linq: Zoli
                var atlagok = reactor.HomersekletLog.Skip(1) //első elem az alap elem
                           .Select((value, index) => new { Value = value, Group = index / 6 })
                           .GroupBy(x => x.Group)
                           .Select(group => group.Average(x => x.Value));
                Console.WriteLine($"A reaktor kezdőhőmérséklete: {reactor.HomersekletLog[0]}");
                Console.WriteLine("Minden negyed nap átlaghőmérséklete:");
                foreach (var item in atlagok) Console.WriteLine(item);
                var minmax = reactor.HomersekletLog.Skip(1)
                                 .Select((value, index) => new { Value = value, Group = index / 6 })
                                 .GroupBy(x => x.Group)
                                 .Select(group => new
                                 {
                                     Minimum = group.Min(x => x.Value),
                                     Maximum = group.Max(x => x.Value)
                                 });
                Console.WriteLine("Minden 6 elem minimuma és maximuma (0 kihagyva):");
                foreach (var item in minmax) Console.WriteLine($"Minimum: {item.Minimum}, Maximum: {item.Maximum}");
                var energiaAtlag = reactor.MegtermeltEnergiaLog.Average();
                var energiaAtlagFelettiIdopontok = reactor.MegtermeltEnergiaLog
                    .Select((value, index) => new { Ertek = value, Ora = index })
                    .Where(x => x.Ertek > energiaAtlag)
                    .ToList();
                Console.WriteLine($"Átlag feletti megtermelt energia értékek száma: {energiaAtlagFelettiIdopontok.Count}");
                Console.WriteLine("Átlag feletti értékek és az időpontjaik:");
                foreach (var item in energiaAtlagFelettiIdopontok) Console.WriteLine($"Érték: {item.Ertek} MW, Időpont: {item.Ora}. óra");
                

            }
            catch (Exception ex) {
                Console.WriteLine($"Hiba történt: {ex.Message}");
            }
            Console.ReadKey();
            /*
            • eseménykezelés(legalább egy eseményé)
            • a mérési adatok elhelyezése adatbázisban
            • a mérési adatok kiírása JSON fájlba
            • legalább 3 LINQ lekérdezés megvalósítása*/
        }
    }
}
