using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TorGuard.Backend;

namespace TorGuard
{
    public class Controller
    {
        private Tor_Verwaltungsstelle torVerwaltung;
        private C_GPIOpins gpioPins;
        private C_EmailService emailService;
        private Aufnahme aufnahme;

        Speicherpfade speicherpfade = new Speicherpfade(
               @"\\RASPBERRYPI\MeineFreigabe",//speicherpfadTor,
               @"\\RASPBERRYPI\MeineFreigabe",//speicherpfadKonfig,
               "Tor_config.json",//speichernameTorKonfig,
               "Netzwerk_config.json",//speichernameNetzwerkKonfig,
               "Empfängerlist_config.json",//speichernameEmailKonfig,
               "AufnahmeKonfig.json"//speichernameAufnahmeKonfig
            );
        public Controller()
        {
            torVerwaltung = new Tor_Verwaltungsstelle();
            gpioPins = new C_GPIOpins(); // Stellen Sie sicher, dass dies funktioniert
            emailService = new C_EmailService();
            aufnahme = new Aufnahme();

            gpioPins.OnCrashDetected += GpioPins_OnCrashDetected;

            torVerwaltung.LoadAllTore();
        }



        public async Task StartSystemAsync()
        {
            // Starte Aufnahmen für alle Tore
            foreach (var tor in torVerwaltung.GetAllTore())
            {
                await aufnahme.ContinuousRecordingAsync(tor);
            }
            // Überwache alle Pins - Implementierung unten
            await CheckPinStateAsync();
        }

        public async Task CheckPinStateAsync()
        {
            while (true) // Endlosschleife, um kontinuierlich den Pin-Zustand zu überprüfen
            {
                foreach (var tor in torVerwaltung.GetAllTore())
                {
                    gpioPins.ÜberprüfePin(tor.GpioPinIndoor, true);
                    gpioPins.ÜberprüfePin(tor.GpioPinOutdoor, false);
                }

                await Task.Delay(1000); // Wartezeit von 1 Sekunde (oder nach Bedarf anpassen) zwischen den Überprüfungen
            }
        }
        public void GpioPins_OnCrashDetected(string torName, int pinNummer, DateTime crashTime, bool isIndoor)
        {
            aufnahme.HandleCrashDetection( pinNummer,isIndoor);
            string speicherortCrash = torVerwaltung.GetCrashSpeicherortByName(torName);

            if (speicherortCrash == null)
            {
                Console.WriteLine($"Kein Speicherort für {torName} gefunden.");
                return;
            }
            emailService.Emailtext(torName, crashTime, isIndoor,speicherortCrash);
            emailService.Emailssenden();
        }

    }
}
