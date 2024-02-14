// Import der notwendigen Namespaces
using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;

namespace TorGuard
{
    // Definition der NetworkConnection-Klasse
    public class NetworkConnection : IDisposable
    {
        // Definition einer Klasse, die die NETRESOURCE-Struktur repräsentiert.
        // Diese Struktur wird verwendet, um eine Netzwerkressource zu definieren, zu der eine Verbindung hergestellt oder die getrennt werden soll.
        [StructLayout(LayoutKind.Sequential)]
        private class NETRESOURCE
        {
            public int dwScope = 0;
            public int dwType = 1; // Typ der Ressource ist Festplatte
            public int dwDisplayType = 0;
            public int dwUsage = 0;
            public string lpLocalName = ""; // Kein lokaler Laufwerksbuchstabe zugewiesen
            public string lpRemoteName; // Der Netzwerkpfad zur Freigabe
            public string lpComment = "";
            public string lpProvider = "";
        }

        // Import der WNetAddConnection2 Funktion aus der mpr.dll
        // Diese Funktion stellt eine Verbindung zu einer Netzwerkressource her.
        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NETRESOURCE lpNetResource, string lpPassword, string lpUsername, int dwFlags);

        // Import der WNetCancelConnection2 Funktion aus der mpr.dll
        // Diese Funktion trennt eine bestehende Netzwerkverbindung.
        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string lpName, int dwFlags, bool fForce);

        private readonly string _networkName; // Speichert den Netzwerkpfad

        // Konstruktor der NetworkConnection-Klasse, der versucht, eine Verbindung herzustellen
        public NetworkConnection(string networkName, NetworkCredential credentials)
        {
            _networkName = networkName; // Speichern des Netzwerknamens

            // Erstellen einer Instanz von NETRESOURCE und Festlegen der Eigenschaften
            var netResource = new NETRESOURCE
            {
                lpRemoteName = networkName // Der Netzwerkpfad zur Freigabe
            };

            // Versucht, eine Verbindung mit der Netzwerkfreigabe herzustellen
            var result = WNetAddConnection2(netResource, credentials.Password, credentials.UserName, 0);

            // Überprüft das Ergebnis des Verbindungsversuchs
            if (result != 0)
            {
                // Wenn ein Fehler aufgetreten ist, wird eine Ausnahme geworfen
                throw new Win32Exception(result, "Fehler beim Verbinden mit Netzwerkpfad");
            }
        }

        // Implementierung der Dispose-Methode, um die Netzwerkverbindung sicher zu trennen
        public void Dispose()
        {
            // Versucht, die Netzwerkverbindung zu trennen
            var result = WNetCancelConnection2(_networkName, 0, true);
            // Überprüft das Ergebnis des Trennungsversuchs
            if (result != 0)
            {
                // Wenn ein Fehler aufgetreten ist, wird eine Ausnahme geworfen
                throw new Win32Exception(result, "Fehler beim Trennen der Netzwerkverbindung");
            }
        }
    }
}

