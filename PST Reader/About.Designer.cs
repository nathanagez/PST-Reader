namespace PST_Reader
{
    partial class About
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.credits = new System.Windows.Forms.Label();
            this.lienGit = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // credits
            // 
            this.credits.AutoSize = true;
            this.credits.Location = new System.Drawing.Point(12, 9);
            this.credits.Name = "credits";
            this.credits.Size = new System.Drawing.Size(125, 39);
            this.credits.TabIndex = 0;
            this.credits.Text = "&Credit : Ruskie Banana\r\n&Using : PST .NET\r\n&Release Date : 07/2017 ";
            // 
            // lienGit
            // 
            this.lienGit.AutoSize = true;
            this.lienGit.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lienGit.Location = new System.Drawing.Point(12, 65);
            this.lienGit.Name = "lienGit";
            this.lienGit.Size = new System.Drawing.Size(40, 13);
            this.lienGit.TabIndex = 1;
            this.lienGit.TabStop = true;
            this.lienGit.Text = "GitHub";
            this.lienGit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lienGit_LinkClicked);
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(146, 87);
            this.Controls.Add(this.lienGit);
            this.Controls.Add(this.credits);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "A Propos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label credits;
        private System.Windows.Forms.LinkLabel lienGit;
    }
}