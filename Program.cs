using System;
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
                    if (i % 3 == 0) {
                        reactor.Delegalt(true);
                    }
                    else {
                        reactor.Delegalt(false);
                    }
                    reactor.Kiiras();
                }
                //Delegált: Zoli

                //Linq: Zoli
            }
            catch (Exception ex) {
                Console.WriteLine($"Hiba történt: {ex.Message}");
            }
            Console.ReadKey();
            /*delegált használata
            • eseménykezelés(legalább egy eseményé)
            • a mérési adatok elhelyezése adatbázisban
            • a mérési adatok kiírása JSON fájlba
            • legalább 3 LINQ lekérdezés megvalósítása*/
        }
    }
}
