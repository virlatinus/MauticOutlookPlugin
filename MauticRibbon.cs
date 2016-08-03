using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;

namespace MauticOutlookPlugin {
    public partial class MauticRibbon
    {

        private void Ribbon1_Load(object sender, RibbonUIEventArgs e) {
            

        }

        private void toggleButton1_Click(object sender, RibbonControlEventArgs e) {
            Globals.ThisAddIn.Trackable = toggleButton1.Checked;
        }
    }
}
