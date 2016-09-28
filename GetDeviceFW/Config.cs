using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GetDeviceFW
{
    public partial class Config : Form
    {
        IniHelper ini = new IniHelper();
        public Config()
        {
            InitializeComponent();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            try
            {
                ini.WriteValue("Setting", "VID", textBox_VID.Text);
                ini.WriteValue("Setting", "PID", textBox_PID.Text);
                ini.WriteValue("Setting", "FWVer", textBox_TargetFW.Text);
            }
            catch 
            {
                ini.WriteValue("Setting", "VID", "");
                ini.WriteValue("Setting", "PID", "");
                ini.WriteValue("Setting", "FWVer", "");
            }
     
            this.Close();
        }

        private void Config_Load(object sender, EventArgs e)
        {
            try
            {
                textBox_VID.Text = ini.ReadValue("Setting", "VID");
                textBox_PID.Text = ini.ReadValue("Setting", "PID");
                textBox_TargetFW.Text = ini.ReadValue("Setting", "FWVer");
            }
            catch
            {
            	textBox_VID.Text = "";
                textBox_PID.Text = "";
                textBox_TargetFW.Text = "";
            }
       
        }
    }
}
