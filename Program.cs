using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSZ_Beadando_DLL;

namespace HSZ_Beadando {
    internal class Program {
        static List<Reaktor> reactor = new List<Reaktor>();
        static void Main(string[] args) {
            try {
                Random r = new Random();
                for (int i = 0; i < 60; i++) { //1 perc

                    r.NextDouble();
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Hiba történt: {ex.Message}");
            }
            Console.ReadKey();
        }
    }
}
