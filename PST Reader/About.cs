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
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void lienGit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Redirige vers le Repo Git
            System.Diagnostics.Process.Start("https://github.com/NastyZ98/PST-Reader");
        }
    }
}
