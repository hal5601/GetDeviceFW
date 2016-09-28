using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GetDeviceFW
{
    class IniHelper
    {
        //Statement INI file write operation function - WritePrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        //Statement INI file read operation function - GetPrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);

        private string sPath = Directory.GetCurrentDirectory() + "\\config.ini";

        public void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, sPath);
        }

        public string ReadValue(string section, string key)
        {
            System.Text.StringBuilder temp = new System.Text.StringBuilder(255);
            GetPrivateProfileString(section, key, "", temp, 255, sPath);
            return temp.ToString();
        }

        public string ReadBoolValue(string section, string key)
        {
            System.Text.StringBuilder temp = new System.Text.StringBuilder(255);
            GetPrivateProfileString(section, key, "false", temp, 255, sPath);
            return temp.ToString();
        }
    }
}
