using System;
using System.IO;
using System.Text.Json;
using TorGuard.Backend;

namespace TorGuard
{
    public class HC_AufnahmeKonfig
    {
        public double StundenProDatei { get; set; }
        public double BackupDays { get; set; }
        public double AktualisierungServerDatei { get; set; }

        // Singleton-Instanz
        private static HC_AufnahmeKonfig instance;

        // Privater Konstruktor, um direkte Instanziierung zu verhindernA
        //public HC_AufnahmeKonfig(double stundenProDatei, double backupDays, double aktualisierungServerDatei) 
        //{
        //    stundenProDatei = StundenProDatei;
        //    backupDays = BackupDays;
        //    aktualisierungServerDatei = AktualisierungServerDatei;
        //}

        public HC_AufnahmeKonfig()
        {
        }

        // Öffentliche Methode zum Zugriff auf die Singleton-Instanz
        public static HC_AufnahmeKonfig Instance
        {
            get
            {
                if (instance == null)
                {
                    Speicherpfade speicherpfade = new Speicherpfade();

                    string filePath = Path.Combine(speicherpfade.Speicherpfad_Konfig, speicherpfade.Speichername_Aufnahemekonfig);
                    if (File.Exists(filePath))
                    {
                        string json = File.ReadAllText(filePath);
                        instance = JsonSerializer.Deserialize<HC_AufnahmeKonfig>(json);
                    }
                    else
                    {
                        // Erstellen einer neuen Instanz, falls die Konfigurationsdatei nicht existiert
                        instance = new HC_AufnahmeKonfig();
                    }
                }
                return instance;
            }
        }
    }
}
