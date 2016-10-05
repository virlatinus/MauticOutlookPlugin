using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using DamienG.Security.Cryptography;
using Microsoft.Win32;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;

namespace MauticOutlookPlugin {
    public partial class ThisAddIn {

        private void ThisAddIn_Startup(object sender, System.EventArgs e) {
            Outlook.Application oOutlook = Globals.ThisAddIn.Application;
            oOutlook.OptionsPagesAdd += new Outlook.ApplicationEvents_11_OptionsPagesAddEventHandler(Application_OptionsPagesAdd);

            // Event handler to include the tracking gif when sending the email
            oOutlook.ItemSend += new Outlook.ApplicationEvents_11_ItemSendEventHandler(Application_ItemSend);

            Trackable = false;

            // Retrieve the endpoint URL from the registry
            try {
                var key = Registry.CurrentUser.OpenSubKey("Software");
                key = key.OpenSubKey("Mautic");
                key = key.OpenSubKey("Outlook Plugin");
                EndpointUrl = key.GetValue("Endpoint URL").ToString();
                MauticSecret = key.GetValue("Secret").ToString();
            } catch (Exception ex) {
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private static string Compress(string data) {
            using (MemoryStream ms = new MemoryStream()) {
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true)) {
                    zip.Write(Encoding.UTF8.GetBytes(data), 0, data.Length);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public string GetSenderAddress(Outlook.MailItem mail)
        {
            if (mail == null) {
                return "";
            }

            if ((mail.SenderEmailType != "EX")) return mail.SenderEmailAddress;
            Outlook.Account acc = mail.SendUsingAccount;
            if (acc == null) // use first account
            {
                Outlook.Accounts accounts = mail.GetInspector.Session.Accounts;
                acc = accounts[0];
            }
            return acc.SmtpAddress;
        }

        public Stream GenerateStreamFromString(string s) {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public void Application_ItemSend(object item, ref bool cancel) {
            if (!Trackable) return;

            var outlookMailtItem = (Outlook.MailItem)item;

            // Add the tracking gif to the email body if format is HTML
            if (outlookMailtItem.BodyFormat == Outlook.OlBodyFormat.olFormatHTML)
            {
                var a = "";
                foreach (Outlook.Recipient t in outlookMailtItem.Recipients)
                {
                    if (a.Length>0) a += ";";
                    if (t.AddressEntry.GetExchangeUser() == null)
                        a += t.Address;
                    else a += t.AddressEntry.GetExchangeUser().PrimarySmtpAddress;
                }

                var d = Uri.EscapeDataString(Compress($"from={Uri.EscapeDataString(GetSenderAddress(outlookMailtItem))}&email={Uri.EscapeDataString(a)}&subject={Uri.EscapeDataString(outlookMailtItem.Subject)}&body={Uri.EscapeDataString(outlookMailtItem.Body)}"));
                var crc32 = new Crc32();
                var hash = String.Empty;
                var cr = UnixCrypt.crypt(d, MauticSecret);
                using (var s = GenerateStreamFromString(cr))
                {
                    hash = crc32.ComputeHash(s).Aggregate(hash, (current, b) => current + b.ToString("x2").ToLower());
                }
                var trackingGif = $"<img style=\"display: none;\" height=\"1\" width=\"1\" src=\"{EndpointUrl}/plugin/Outlook/tracking.gif?d={d}&sig={hash}\" alt=\"Mautic is open source marketing automation\">";

                outlookMailtItem.HTMLBody = Regex.Replace(outlookMailtItem.HTMLBody, "</body>", trackingGif + "</body>", RegexOptions.IgnoreCase);
            }

            cancel = false;
        }

        void Application_OptionsPagesAdd(Outlook.PropertyPages pages) {
            pages.Add(new PluginOptionsControl(), "");
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e) {
            // Note: Outlook no longer raises this event. If you have code that 
            //    must run when Outlook shuts down, see http://go.microsoft.com/fwlink/?LinkId=506785
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup() {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion

        public string EndpointUrl { get; set; }

        public string MauticSecret { get; set; }

        public bool Trackable { get; set; }
    }
}
