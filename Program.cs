﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSZ_Beadando_DLL;
using Newtonsoft.Json;
using System.IO;

namespace HSZ_Beadando {
    //JSON osztály
    public class MeresiAdat {
        public double VizNyomas { get; set; }
        public double MegtermeltEnergia { get; set; } 
        public double Homerseklet { get; set; }
        public int Idopont { get; set; } 
    }
    internal class Program {
        static Reaktor reactor = new Reaktor(0,0,0); 
        static new List<MeresiAdat> meresiAdatok = new List<MeresiAdat>(); //JSON lista
        static void Main(string[] args) {
            try
            {
                //Event hozzáadás
                reactor.HomersekletTullepes += (homerseklet) =>
                {
                    Console.WriteLine($"Figyelem! A hőmérséklet elérte a kritikus szintet: {homerseklet} °C");
                };
                //Random szám feltöltés: Kristóf
                Random r = new Random();
                reactor = new Reaktor(r.Next(5, 11), r.Next(4, 6), r.Next(550, 651));
                AdatBeszuras(reactor.VizNyomas, reactor.MegtermeltEnergia, reactor.Homerseklet); //SQL-be szúrás 
                meresiAdatok.Add(new MeresiAdat
                {
                    VizNyomas = reactor.VizNyomas,
                    MegtermeltEnergia = reactor.MegtermeltEnergia,
                    Homerseklet = reactor.Homerseklet,
                    Idopont = 0
                }); //JSON listához adás
                for (int i = 0; i < 24; i++)
                { //1 perc
                    //Delegált: Zoli
                    if (i % 3 == 0)
                    {
                        reactor.Delegalt(true);
                    }
                    else
                    {
                        reactor.Delegalt(false);
                    }
                    reactor.Kiiras();
                    AdatBeszuras(reactor.VizNyomas, reactor.MegtermeltEnergia, reactor.Homerseklet);
                    meresiAdatok.Add(new MeresiAdat
                    {
                        VizNyomas = reactor.VizNyomas,
                        MegtermeltEnergia = reactor.MegtermeltEnergia,
                        Homerseklet = reactor.Homerseklet,
                        Idopont = i + 1
                    });
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

                //Adatbázis Kristóf
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS MeresiAdatok (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        VizNyomas REAL,
                        MegtermeltEnergia REAL,
                        Homerseklet REAL,
                        Idopont DATETIME DEFAULT CURRENT_TIMESTAMP
                    );
                ";
                    command.ExecuteNonQuery();
                }

                //JSON Kristóf
                string jsonString = JsonConvert.SerializeObject(meresiAdatok, Formatting.Indented);
                StreamWriter writer = new StreamWriter("Json.txt");
                writer.WriteLine(jsonString);
                writer.Flush();
                writer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt: {ex.Message}");
            }
            Console.ReadKey();
        }
        static string connectionString = "Data Source=reaktor_adatok.db;";
        static void AdatBeszuras(double vizNyomas, double megtermeltEnergia, double homerseklet)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                INSERT INTO MeresiAdatok (VizNyomas, MegtermeltEnergia, Homerseklet) 
                VALUES (@vizNyomas, @megtermeltEnergia, @homerseklet);
            ";
                command.Parameters.AddWithValue("@vizNyomas", vizNyomas);
                command.Parameters.AddWithValue("@megtermeltEnergia", megtermeltEnergia);
                command.Parameters.AddWithValue("@homerseklet", homerseklet);
                command.ExecuteNonQuery();
            }
        }
        static void AdatokLekerdezese()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM MeresiAdatok;";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Id: {reader["Id"]}, Nyomás: {reader["VizNyomas"]} MPa, Energia: {reader["MegtermeltEnergia"]} MW, Hőmérséklet: {reader["Homerseklet"]} °C, Időpont: {reader["Idopont"]}");
                    }
                }
            }
        }
    }
}
