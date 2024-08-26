using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace DomZdravljaWebApp.Models
{
    public static class Database
    {
        static Database()
        {
            InitializeDatabase();
        }

        private static void InitializeDatabase()
        {
            EnsureFileExists<Lekar>("~/App_Data/Lekari.json");
            EnsureFileExists<Pacijent>("~/App_Data/Pacijenti.json");
            EnsureFileExists<Administrator>("~/App_Data/Administratori.json");
            EnsureFileExists<Termin>("~/App_Data/Termini.json");
            EnsureFileExists<ZdravstveniKarton>("~/App_Data/ZdravstveniKartoni.json");

            Lekari = LoadFile<Lekar>(HostingEnvironment.MapPath("~/App_Data/Lekari.json"));
            Pacijenti = LoadFile<Pacijent>(HostingEnvironment.MapPath("~/App_Data/Pacijenti.json"));
            Administratori = LoadFile<Administrator>(HostingEnvironment.MapPath("~/App_Data/Administratori.json"));
            Termini = LoadFile<Termin>(HostingEnvironment.MapPath("~/App_Data/Termini.json"));
            ZdravstveniKartoni = LoadFile<ZdravstveniKarton>(HostingEnvironment.MapPath("~/App_Data/ZdravstveniKartoni.json"));
        }

        private static void EnsureFileExists<T>(string relativePath)
        {
            string path = HostingEnvironment.MapPath(relativePath);
            if (!File.Exists(path))
            {
                WriteFile(path, new List<T>());
            }
        }

        private static List<T> LoadFile<T>(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                return JsonConvert.DeserializeObject<List<T>>(sr.ReadToEnd());
            }
        }

        private static void WriteFile(string path, object data)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(JsonConvert.SerializeObject(data, jsonSerializerSettings));
            }
        }

        public static List<Lekar> Lekari { get; private set; }
        public static List<Pacijent> Pacijenti { get; private set; }
        public static List<Administrator> Administratori { get; private set; }
        public static List<Termin> Termini { get; private set; }
        public static List<ZdravstveniKarton> ZdravstveniKartoni { get; private set; }

        public static List<Osoba> Korisnici
        {
            get
            {
                return new List<Osoba>().Concat(Lekari).Concat(Pacijenti).Concat(Administratori).ToList();
            }
        }

        public static void Save()
        {
            WriteFile(HostingEnvironment.MapPath("~/App_Data/Lekari.json"), Lekari);
            WriteFile(HostingEnvironment.MapPath("~/App_Data/Pacijenti.json"), Pacijenti);
            WriteFile(HostingEnvironment.MapPath("~/App_Data/Administratori.json"), Administratori);
            WriteFile(HostingEnvironment.MapPath("~/App_Data/Termini.json"), Termini);
            WriteFile(HostingEnvironment.MapPath("~/App_Data/ZdravstveniKartoni.json"), ZdravstveniKartoni);
        }
    }
}
