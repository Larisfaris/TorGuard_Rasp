using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Windows;
using TorGuard.Backend;

using System.ComponentModel;
using System.Runtime.InteropServices;

namespace TorGuard
{
    public partial class ServerZugriff 
    {
        // Ersetzen Sie 'benutzername' und 'passwort' durch Ihre tatsächlichen Anmeldedaten
        string benutzername = "lm";
        string passwort = "";

        string Speicherpfade = @"C:\Users\lemen\Desktop\Test_TorGuard";
        public ServerZugriff()
        {        
        }      

        //In der Konsole Tore Auflisten
        private void ListFilesInSmbShare()
        {
            try
            { 
                // Pfad zur SMB-Freigabe auf dem Raspberry Pi
                string smbPath1 = Speicherpfade;

                // Die NetworkCredential-Klasse wird verwendet, um die Anmeldedaten für den Zugriff auf die SMB-Freigabe anzugeben
                NetworkCredential credentials = new NetworkCredential(benutzername, passwort);

                // CredentialCache wird verwendet, um die Anmeldedaten zusammen mit dem Pfad der Freigabe zu speichern
                CredentialCache cache = new CredentialCache();
                cache.Add(new Uri(Speicherpfade), "Basic", credentials);

                // NetworkConnection stellt eine Verbindung zur SMB-Freigabe her, unter Verwendung der oben definierten Anmeldedaten
                using (new NetworkConnection(Speicherpfade, credentials))
                {
                    // Directory.GetFiles wird verwendet, um alle Dateien im angegebenen Pfad der SMB-Freigabe aufzulisten
                    foreach (var file in Directory.GetFiles(Speicherpfade))
                    {
                        // Hier können Sie mit den Dateien arbeiten, z.B. sie auflisten oder ihren Inhalt lesen
                        Console.WriteLine(file);
                    }
                }
                
            }
            catch (Exception ex)
            {
                // Im Falle eines Fehlers wird eine Nachrichtenbox mit der Fehlermeldung angezeigt
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}", "Fehler");
            }
        }
    
        //// Generische Methode zum Laden einer Datei vom Server
        //public T LadeDateiVomServer<T>(string dateiPfad, string dateiName) where T : class
        //{
        //    string vollerPfad = Path.Combine(dateiPfad, dateiName);
        //    try
        //    {   //using (var netzwerkVerbindung = new NetworkConnection(dateiPfad, new NetworkCredential("benutzername", "passwort")))
                
        //            using (var netzwerkVerbindung = new NetworkConnection(vollerPfad, new NetworkCredential(benutzername, passwort)))
        //        {
        //            if (File.Exists(vollerPfad))
        //            {
        //                string json = File.ReadAllText(vollerPfad);
        //                var configData = JsonSerializer.Deserialize<T>(json);
        //                return configData;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Fehler beim Laden der Datei: {ex.Message}", "Fehler");
        //    }
        //    return null;
        //}

        // Methode zum Speichern der Konfigurationsdatei auf dem Server
        public void SpeichereDateiAufServer( string localDestinationPath)
        {
            try
            {
                using (var netzwerkVerbindung = new NetworkConnection(Speicherpfade, new NetworkCredential(benutzername, passwort)))
                {
                    {
                        // Kopieren der Datei von der Netzwerkfreigabe auf den lokalen PC
                        File.Copy(Speicherpfade, localDestinationPath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern der Datei: {ex.Message}", "Fehler");
            }
        }
    }
}