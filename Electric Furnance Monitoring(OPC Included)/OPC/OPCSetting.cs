using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace Electric_Furnance_Monitoring_OPC_Included_
{
    public partial class OPCSetting : Form
    {
        MainForm main;
        CustomOPC opc;
        Configuration config;

        public OPCSetting(MainForm _main)
        {
            this.main = _main;
            InitializeComponent();
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        private void OPCSetting_Load(object sender, EventArgs e)
        {
            opc = (CustomOPC)main.CustomOPC_forPublicRef();

            string value = "";
            value = opc.Channel;
            textBox_channel.Text = value;
            value = opc.Device;
            textBox_device.Text = value;
            value = opc.nodeName;
            textBox_endpoint.Text = value; 
        }

        private void button_Accept_Click(object sender, EventArgs e)
        {
            if (main.OPCActivated == true)
            {
                main.OPCTimer.Stop();
                main.OPCActivated = false;
            }
            opc.ServerDisconnection();
            opc.connectFailed = true;
            opc.detected = false;
            main.OPCTimerActivated = false;

            string value = "";
            value = textBox_channel.Text;
            opc.Channel = value;
            value = textBox_device.Text;
            opc.Device = value;
            value = textBox_endpoint.Text;
            opc.nodeName = value;

            Close();

            opc.ServerDetection();
            if (opc.detected == true)
            {
                opc.ServerConnection();
            }
            if (opc.connectFailed == false && main.OPCActivated==false)
            {
                main.OPCActivated = true;
                main.InitOPCTimer();
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OPCSetting_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
