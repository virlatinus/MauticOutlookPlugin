namespace MauticOutlookPlugin {
    partial class PluginOptionsControl {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.mauticUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mauticSecret = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mauticUrl
            // 
            this.mauticUrl.Location = new System.Drawing.Point(114, 11);
            this.mauticUrl.Name = "mauticUrl";
            this.mauticUrl.Size = new System.Drawing.Size(366, 22);
            this.mauticUrl.TabIndex = 5;
            this.mauticUrl.Text = "http://YOURDOMAIN.mautic.net/index.php";
            this.mauticUrl.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Mautic URL";
            // 
            // mauticSecret
            // 
            this.mauticSecret.Location = new System.Drawing.Point(114, 39);
            this.mauticSecret.Name = "mauticSecret";
            this.mauticSecret.Size = new System.Drawing.Size(366, 22);
            this.mauticSecret.TabIndex = 7;
            this.mauticSecret.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Plugin Secret";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label3.Location = new System.Drawing.Point(111, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(350, 44);
            this.label3.TabIndex = 8;
            this.label3.Text = "Use the same secret you configured in the Outlook plugin settings in Mautic";
            // 
            // PluginOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mauticSecret);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mauticUrl);
            this.Controls.Add(this.label1);
            this.Name = "PluginOptionsControl";
            this.Size = new System.Drawing.Size(517, 150);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox mauticUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox mauticSecret;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
