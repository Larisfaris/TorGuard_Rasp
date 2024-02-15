using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorGuard.Backend
{
    public class Speicherpfade
    {
        public string Speicherpfad_tor { get; set; }
        public string Speicherpfad_Konfig { get; set; }
        public string Speichername_Torkonfig { get; set; }
        public string Speichername_Netzwerkkonfig { get; set; }
        public string Speichername_Emailkonfig { get; set; }
        public string Speichername_Aufnahemekonfig { get; set; }

        private static readonly Speicherpfade instance = new Speicherpfade();

        public Speicherpfade()
        {

            Speicherpfad_tor = @"/home/Tor.Guard/home/Documents/tore/"; //speicherpfadTor,
            Speicherpfad_Konfig = @"/home/Tor.Guard/Documents/konfig";//speicherpfadKonfig,;
            Speichername_Torkonfig = "Tor_config.json";
            Speichername_Netzwerkkonfig = "Netzwerk_config.json";
            Speichername_Emailkonfig = "Empfängerlist_config.json";
            Speichername_Aufnahemekonfig = "AufnahmeKonfig.json";
            //    speicherpfade.Speicherpfad_tor = @"/home/Tor.Guard/home/Documents/tore/"; //speicherpfadTor,
            //    speicherpfade.Speicherpfad_Konfig = @"/home/Tor.Guard/Documents/konfig";//speicherpfadKonfig,;
            //    speicherpfade.Speichername_Torkonfig = "Tor_config.json";
            //    speicherpfade.Speichername_Netzwerkkonfig = "Netzwerk_config.json";
            //    speicherpfade.Speichername_Emailkonfig = "Empfängerlist_config.json";
            //    speicherpfade.Speichername_Aufnahemekonfig = "AufnahmeKonfig.json";
            //}
            //public Speicherpfade(string speicherpfad_tor, string speicherpfad_Konfig, string speichername_Torkonfig, string speichername_Netzwerkkonfig, string speichername_Emailkonfig, string speichername_Aufnahemekonfig)
            //{
            //    Speicherpfad_tor = speicherpfad_tor;
            //    Speicherpfad_Konfig = speicherpfad_Konfig;
            //    Speichername_Torkonfig = speichername_Torkonfig;
            //    Speichername_Netzwerkkonfig = speichername_Netzwerkkonfig;
            //    Speichername_Emailkonfig = speichername_Emailkonfig;
            //    Speichername_Aufnahemekonfig = speichername_Aufnahemekonfig;
            //}
        }

        public static Speicherpfade Instance => instance;

    }
}   
