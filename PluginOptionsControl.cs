using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MauticOutlookPlugin {

    [ComVisible(true)]
    public partial class PluginOptionsControl : UserControl, Outlook.PropertyPage {
        private const int CaptionDispId = -518;
        private Outlook.PropertyPageSite propertyPageSite = null;
        bool isDirty = false;

        public PluginOptionsControl() {
            InitializeComponent();
        }

        void Outlook.PropertyPage.Apply()
        {
            if (!Regex.IsMatch(textBox1.Text, @"^http(s)?://([\w-]+.)+[\w-]+/index.php$", RegexOptions.IgnoreCase)) {
                MessageBox.Show("The URL does not seem like a valid URL. Please type in a valid URL (ending with index.php)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox1.Focus();
            }

            try {
                var key = Registry.CurrentUser.OpenSubKey("Software", true);
                if (key.OpenSubKey("Mautic") == null)
                    key.CreateSubKey("Mautic");
                key = key.OpenSubKey("Mautic", true);
                if (key.OpenSubKey("Outlook Plugin") == null)
                    key.CreateSubKey("Outlook Plugin");
                key = key.OpenSubKey("Outlook Plugin", true);
                key.SetValue("Endpoint URL", textBox1.Text);
                Globals.ThisAddIn.EndpointUrl = textBox1.Text;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        bool Outlook.PropertyPage.Dirty => isDirty;

        void Outlook.PropertyPage.GetPageInfo(ref string helpFile, ref int helpContext) {

        }

        [DispId(CaptionDispId)]
        public string PageCaption => "Mautic Options";

        private Outlook.PropertyPageSite GetPropertyPageSite() {
            Type myType = typeof(System.Object);
            string assembly = Regex.Replace(myType.Assembly.CodeBase, "mscorlib.dll", "System.Windows.Forms.dll");
            assembly = System.Text.RegularExpressions.Regex.Replace(assembly, "file:///", "");
            assembly = System.Reflection.AssemblyName.GetAssemblyName(assembly).FullName;
            Type unmanaged = Type.GetType(System.Reflection.Assembly.CreateQualifiedName(assembly, "System.Windows.Forms.UnsafeNativeMethods"));
            Type oleObj = unmanaged.GetNestedType("IOleObject");
            System.Reflection.MethodInfo mi = oleObj.GetMethod("GetClientSite");
            object myppSite = mi.Invoke(this, null);
            return (Outlook.PropertyPageSite)myppSite;
        }

        private void UserControl1_Load(object sender, EventArgs e) {
            propertyPageSite = GetPropertyPageSite();
            textBox1.Text = Globals.ThisAddIn.EndpointUrl;
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            isDirty = true;
            propertyPageSite.OnStatusChange();
        }

    }
}
