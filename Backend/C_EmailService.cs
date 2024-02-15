using MimeKit;
using MailKit.Net.Smtp;
using System.Text.Json;
using TorGuard.Backend;


namespace TorGuard
{ 

    public class C_EmailService
    {
        private string subject;
        private string textBody;

        private List<string> emailAddresses = new List<string>();

        public (string subject, string textBody) Emailtext(string torName, DateTime Zeitpunkt, bool isIndoor,string SpeicherortCrash)
        {
            string seite = isIndoor ? "Innenseite" : "Aussenseite";
            

            var subject = $"Unfallbericht Tor_Guard vom {Zeitpunkt:dd.MM.yyyy 'um' HH:mm:ss}";
            string textBody = $"Mit Bedauern müssen wir Ihnen mitteilen, dass in Ihrer Firma das Tor {torName} am {Zeitpunkt:dd.MM.yyyy 'um' HH:mm:ss} an der {seite} beschädigt wurde. Bitte überprüfen Sie die Videoaufzeichnung im besagten Zeitpunkt. Sie finden die Aufzeichnungen unter {SpeicherortCrash}.";

            return (subject, textBody);
        }

        public void Emailssenden() 
        {
            EmailsAsync();
        }



        // Methode zum asynchronen Versenden von E-Mails
        public async Task<bool> EmailsAsync()
        {
            //Speicherpfade speicherpfade = new Speicherpfade();  
            string filePath_Mail = Path.Combine(Speicherpfade.Instance.Speichername_Emailkonfig, Speicherpfade.Instance.Speichername_Emailkonfig);

           
                string json = File.ReadAllText(filePath_Mail);
                var configData = JsonSerializer.Deserialize<dynamic>(json);



            List<string> empfängeradressen = ((JsonElement)configData?.EmailAddresses).EnumerateArray().Select(email => email.GetString()).ToList();



            string senderEmail = configData?.SMTP?.Email;
            string password = configData?.SMTP?.Password;
            string addresse = configData?.SMTP?.Address;
            string port = configData?.SMTP?.Port;




                bool allSuccess = true;

            foreach (string empfängeradresse in empfängeradressen)
            {
                bool success = await SendEmailAsync(empfängeradresse, senderEmail, password, addresse,port);
                allSuccess &= success; // Setzt allSuccess auf false, wenn irgendein Aufruf von SendTestEmailAsync false zurückgibt
            }
            return allSuccess;
        }

        //Methode zum asynchronen Versenden einer E-Mail
        public async Task<bool> SendEmailAsync(string empfängeradresse, string senderEmail, string password, string addresse, string port) {

          
            try {   //Email Kopf angaben 

                MimeMessage email = new MimeMessage();

                //email.From.Add(new MailboxAddress("Absender", "Tor_Guard@web.de"));
                //email.To.Add(new MailboxAddress("Empfänger", "lementken@gmail.com"));

                email.From.Add(new MailboxAddress("Absender", senderEmail));
                email.To.Add(new MailboxAddress("Empfänger", empfängeradresse));

                email.Subject = subject;
                email.Body = new TextPart("plain")
                {
                    Text = textBody
                };

                using (var smtp = new SmtpClient())// Email versant 
                {
                    //smtp.Connect("smtp.web.de", 587, true); //smtp-serveradresse,Port Nr., gesicherteverbindung erzwinge = true#
                    //smtp.Connect("smtp.web.de", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    //smtp.Connect("smtp.web.de", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    smtp.Connect(addresse, Convert.ToInt16(port), MailKit.Security.SecureSocketOptions.SslOnConnect);

                    // smtp.Authenticate("Tor_Guard@web.de", "TorGuard");
                    smtp.Authenticate(senderEmail, password);//Daten aus der Maske  // Nur erforderlich, wenn der SMTP-Server eine Authentifizierung erfordert

                    await smtp.SendAsync(email);//await verwendet, um asynchrone Operationen zu kennzeichnen
                    smtp.Disconnect(true);

                    return true; // Erfolg
                }
            }
            catch (MailKit.Security.AuthenticationException)
            {
                Console.WriteLine($"Login fehlgeschlagen. Überprüfen Sie die ihre Anmelde Informationene für {addresse}.Wenn das nicht hilft überprüfe ob du dich Einloggen kannst, eventull wegen nicht vertrauenswürdig gesperrt.", "Authentifizierungsfehler");
                return false; // Authentifizierungsfehler
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Versenden der E-Mail an {empfängeradresse}: {ex.Message}", "Fehler");
                return false; // Sonstiger Fehler
            }
        }
    }
}
