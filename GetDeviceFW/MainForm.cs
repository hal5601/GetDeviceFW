using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GetDeviceFW
{
    public partial class MainForm : Form
    {
        IniHelper ini = new IniHelper();

        [DllImport("DeviceDll.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static bool OpenDevice(ushort vid, ushort pid);
        [DllImport("DeviceDll.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static int GetFWVersion(ushort vid, ushort pid, byte[] fwver);

        #region Native Win32 API
        /// <summary>
        /// WinAPI functions
        /// </summary>        

        // Structure with information for RegisterDeviceNotification.
        //             [StructLayout(LayoutKind.Sequential)]
        //点项目属性(Properties)->生成(Build)->常规(General)中:钩上允许不安全代码(Allow unsafe code)
        unsafe public struct DEV_BROADCAST_HANDLE
        {
            public int dbcc_size;
            public int dbcc_devicetype;
            public int dbcc_reserved;
            public Guid dbcc_classguid;
            public char dbch_name0;
            public char dbch_name1;
            public char dbch_name2;
            public char dbch_name3;
            public char dbch_name4;
            public char dbch_name5;
            public char dbch_name6;
            public char dbch_name7;
            public char dbch_name8;
            public char dbch_name9;
            public char dbch_name10;
            public char dbch_name11;
            public char dbch_name12;
            public char dbch_name13;
            public char dbch_name14;
            public char dbch_name15;
            public char dbch_name16;
            public char dbch_name17;
            public char dbch_name18;
            public char dbch_name19;
            public char dbch_name20;
            public char dbch_name21;
            public char dbch_name22;
            public char dbch_name23;
            public char dbch_name24;
            public char dbch_name25;
            public char dbch_name26;
            public char dbch_name27;
            public char dbch_name28;
            public char dbch_name29;
            public char dbch_name30;
            public char dbch_name31;
            public char dbch_name32;
            public char dbch_name33;
            public char dbch_name34;
            public char dbch_name35;
            public char dbch_name36;
            public char dbch_name37;
            public char dbch_name38;
            public char dbch_name39;
            public char dbch_name40;
            public char dbch_name41;
            public char dbch_name42;
            public char dbch_name43;
            public char dbch_name44;
            public char dbch_name45;
            public char dbch_name46;
            public char dbch_name47;
            public char dbch_name48;
            public char dbch_name49;
            public char dbch_name50;

        }
        public struct DEV_BROADCAST_HDR
        {     /* */
            public int dbch_size;
            public int dbch_devicetype;
            public int dbch_reserved;
        };

        private class Native
        {
            //   HDEVNOTIFY RegisterDeviceNotification(HANDLE hRecipient,LPVOID NotificationFilter,DWORD Flags);
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            unsafe public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, uint Flags);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern uint UnregisterDeviceNotification(IntPtr hHandle);

        }

        public const int WM_DEVICECHANGE = 0x219;
        public const int WM_INPUT = 0x00FF;
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_CONFIGCHANGECANCELED = 0x0019;
        public const int DBT_CONFIGCHANGED = 0x0018;
        public const int DBT_CUSTOMEVENT = 0x8006;
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;
        public const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public const int DBT_DEVICEREMOVEPENDING = 0x8003;
        public const int DBT_DEVICETYPESPECIFIC = 0x8005;
        public const int DBT_DEVNODES_CHANGED = 0x0007;
        public const int DBT_QUERYCHANGECONFIG = 0x0017;
        public const int DBT_USERDEFINED = 0xFFFF;

        private const int DBT_DEVTYP_DEVICEINTERFACE = 5;
        private const int DBT_DEVTYP_HANDLE = 6;
        private const int BROADCAST_QUERY_DENY = 0x424D5144;
        #endregion

        ushort VID = 0;
        //ushort PID = (ushort)0x1010;  //Rantopad
        ushort PID = 0;  //Steelseries

        string sVID = "";
        string sPID = "";
        string sTargetFW = "";

        public MainForm()
        {
            InitializeComponent();

            RegisterForDeviceChange();  //写注册表
        }

        #region 枚举USB设备
        private void RegisterForDeviceChange()
        {
            Guid rawUsbGUID = new Guid("4d1e55b2-f16f-11cf-88cb-001111000030");
            //Guid rawUsbGUID = new Guid("F9168C5E-CEB2-4faa-B6BF-329BF39FA1F5");
            DEV_BROADCAST_HANDLE hDBH = new DEV_BROADCAST_HANDLE();
            hDBH.dbcc_devicetype = DBT_DEVTYP_DEVICEINTERFACE;
            hDBH.dbcc_classguid = rawUsbGUID;
            int size = Marshal.SizeOf(hDBH);
            hDBH.dbcc_size = size;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(hDBH, buffer, true);
            IntPtr hCtrl = Native.RegisterDeviceNotification(this.Handle, buffer, 0);
        }

        public void ShowWindow()
        {
            // Insert code here to make your form show itself.
            this.ShowInTaskbar = true;
            this.Activate();
            WinApi.ShowToFront(this.Handle);

        }
        //Enum the device
        unsafe protected override void WndProc(ref Message m)
        {
            //show the main window
            if (m.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE || m.Msg == 0x00000320)
            {
                ShowWindow();
            }
            //枚举设备
            if (m.Msg == WM_DEVICECHANGE)
            {
                DEV_BROADCAST_HDR* pHdr = (DEV_BROADCAST_HDR*)m.LParam.ToPointer();

                DEV_BROADCAST_HANDLE* pDevInf = (DEV_BROADCAST_HANDLE*)pHdr;
                String strDevLink = "";
                if (pDevInf != null)
                {
                    strDevLink = pDevInf->dbch_name0.ToString()
                      + pDevInf->dbch_name1.ToString()
                      + pDevInf->dbch_name2.ToString()
                      + pDevInf->dbch_name3.ToString()
                      + pDevInf->dbch_name4.ToString()
                      + pDevInf->dbch_name5.ToString()
                      + pDevInf->dbch_name6.ToString()
                      + pDevInf->dbch_name7.ToString()
                      + pDevInf->dbch_name8.ToString()
                      + pDevInf->dbch_name9.ToString()
                      + pDevInf->dbch_name10.ToString()
                      + pDevInf->dbch_name11.ToString()
                      + pDevInf->dbch_name12.ToString()
                      + pDevInf->dbch_name13.ToString()
                      + pDevInf->dbch_name14.ToString()
                      + pDevInf->dbch_name15.ToString()
                      + pDevInf->dbch_name16.ToString()
                      + pDevInf->dbch_name17.ToString()
                      + pDevInf->dbch_name18.ToString()
                      + pDevInf->dbch_name19.ToString()
                      + pDevInf->dbch_name20.ToString()
                      + pDevInf->dbch_name21.ToString()
                      + pDevInf->dbch_name22.ToString()
                      + pDevInf->dbch_name23.ToString()
                      + pDevInf->dbch_name24.ToString()
                      + pDevInf->dbch_name25.ToString()
                      + pDevInf->dbch_name26.ToString()
                      + pDevInf->dbch_name27.ToString()
                      + pDevInf->dbch_name28.ToString()
                      + pDevInf->dbch_name29.ToString()
                      + pDevInf->dbch_name30.ToString()
                      + pDevInf->dbch_name31.ToString()
                      + pDevInf->dbch_name32.ToString()
                      + pDevInf->dbch_name33.ToString()
                      + pDevInf->dbch_name34.ToString()
                      + pDevInf->dbch_name35.ToString()
                      + pDevInf->dbch_name36.ToString()
                      + pDevInf->dbch_name37.ToString()
                      + pDevInf->dbch_name38.ToString()
                      + pDevInf->dbch_name39.ToString()
                      + pDevInf->dbch_name40.ToString()
                      + pDevInf->dbch_name41.ToString()
                      + pDevInf->dbch_name42.ToString()
                      + pDevInf->dbch_name42.ToString()
                      + pDevInf->dbch_name43.ToString()
                      + pDevInf->dbch_name44.ToString()
                      + pDevInf->dbch_name45.ToString()
                      + pDevInf->dbch_name46.ToString()
                      + pDevInf->dbch_name47.ToString()
                      + pDevInf->dbch_name48.ToString()
                      + pDevInf->dbch_name49.ToString()
                      + pDevInf->dbch_name50.ToString();
                    strDevLink = strDevLink.ToUpper();

                    GetConfigFromIni();        //get config info

                    switch (m.WParam.ToInt32())
                    {
                            
                        case DBT_DEVICEARRIVAL://USB插入

                            if ((strDevLink.Substring(12, 4) == sVID.ToUpper())
                             && (strDevLink.Substring(21, 4) == sPID.ToUpper()))
                            {
                                Start();
                            }
                            Refresh();
                            break;

                        case DBT_DEVICEREMOVECOMPLETE: //USB卸载
                            if ((strDevLink.Substring(12, 4) == sVID.ToUpper())
                             && (strDevLink.Substring(21, 4) == sPID.ToUpper()))
                            {
                                label_TargetFW.Text = "NULL";
                                label_CurrentFW.Text = "NULL";
                                label_Result.Text = "No Device";
                                label_Result.ForeColor = Color.Blue;
                                label_Result.Font = new Font("Arial", 30F, FontStyle.Bold, GraphicsUnit.Point, ((Byte)(0)));
                            }
                            Refresh();
                            break;
                        default:
                            break;
                    }
                }
            }
            //无边框拖动代码
            //  public const int WM_NCLBUTTONDBLCLK = 0x00A3; //当用户双击鼠标左键同时光标某个窗口在非客户区时发送此消息 
            if (m.Msg == 163 && this.ClientRectangle.Contains(
                this.PointToClient(new Point(m.LParam.ToInt32()))
                ) && m.WParam.ToInt32() == 2)
                m.WParam = (IntPtr)1;

            base.WndProc(ref m);

            //  public const int WM_NCHITTEST = 0x0084; //移动鼠标，按住或释放鼠标时发生  
            if (m.Msg == 132 && m.Result.ToInt32() == 1)
                m.Result = (IntPtr)2;
        }
        #endregion

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Config config = new Config();
            config.ShowDialog();
        }

        private void button_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void GetConfigFromIni()
        {
            try
            {
                sVID = ini.ReadValue("Setting", "VID");
                sPID = ini.ReadValue("Setting", "PID");
                int iVID = Convert.ToInt32(sVID, 16);
                int iPID = Convert.ToInt32(sPID, 16);
                VID = (ushort)iVID;
                PID = (ushort)iPID;
                sTargetFW = ini.ReadValue("Setting", "FWVer");
            }
            catch
            {

            }
        }

        private void Start()
        {
            label_TargetFW.Text = "NULL";
            label_CurrentFW.Text = "NULL";
            
            if (OpenDevice(VID, PID))
            {
                if (CompareFW())
                {
                    label_Result.Text = "PASS";
                    label_Result.ForeColor = Color.Green;
                }
                else
                {
                    label_Result.Text = "FAIL";
                    label_Result.ForeColor = Color.Red;
                }

                label_Result.Font = new Font("Arial", 40F, FontStyle.Bold, GraphicsUnit.Point, ((Byte)(0)));
            }
            else
            {
                label_Result.Text = "No Device";
                label_Result.ForeColor = Color.Blue;
                label_Result.Font = new Font("Arial", 30F, FontStyle.Bold, GraphicsUnit.Point, ((Byte)(0)));
            }
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            GetConfigFromIni();
            Start();
        }

        private bool CompareFW()
        {
            label_TargetFW.Text = sTargetFW;

            byte[] Version = new byte[3];
            GetFWVersion(VID, PID, Version);
            label_CurrentFW.Text = Version[0].ToString() + "." + Version[1].ToString() + "." + Version[2].ToString();
            if (label_TargetFW.Text.Trim() == label_CurrentFW.Text.Trim())
            {
                return true;
            }
            else
                return false;
        }
    }
}
