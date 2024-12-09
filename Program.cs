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
                Random r = new Random();
                reactor = new Reaktor(r.Next(5, 11), 0, r.Next(550, 651));
                for (int i = 0; i < 60; i++) { //1 perc
                    if (i % 3 == 0) {
                        reactor.NyomasAllitas(0 - r.NextDouble());
                        reactor.EnergiaValtozas(0 - r.NextDouble());
                        reactor.Homersekletallitas(0 - r.NextDouble());
                    }
                    else {
                        reactor.NyomasAllitas(r.NextDouble());
                        reactor.EnergiaValtozas(r.NextDouble());
                        reactor.Homersekletallitas(r.NextDouble());
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Hiba történt: {ex.Message}");
            }
            Console.ReadKey();
        }
    }
}
