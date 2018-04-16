using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Electric_Furnance_Monitoring_OPC_Included_
{
    public partial class AboutForm : Form
    {
        MainForm main;
        public AboutForm(MainForm _main)
        {
            this.main = _main;
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Properties.Resources.wwsLogo;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape || e.KeyCode == Keys.Enter)
            {
                Close();
            }
        }
    }
}
