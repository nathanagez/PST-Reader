using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PST_Reader
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
        }

        private void lienWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Lien Wiki
            System.Diagnostics.Process.Start("https://github.com/NastyZ98/PST-Reader/wiki");
        }
    }
}
