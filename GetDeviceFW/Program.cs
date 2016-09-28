using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
//need
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace GetDeviceFW
{
//     static class Program
//     {
//         /// <summary>
//         /// The main entry point for the application.
//         /// </summary>
//         [STAThread]
//         static void Main()
//         {
//             Application.EnableVisualStyles();
//             Application.SetCompatibleTextRenderingDefault(false);
//             Application.Run(new MainForm());
//         }
//     }
    //I put all my WinAPI stuff in a separate class called WinApi.
    static public class WinApi
    {
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);

        public static int RegisterWindowMessage(string format, params object[] args)
        {
            string message = String.Format(format, args);
            return RegisterWindowMessage(message);
        }

        public const int HWND_BROADCAST = 0xffff;
        public const int SW_SHOWNORMAL = 1;

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void ShowToFront(IntPtr window)
        {
            //MessageBox.Show("Application already started!", "", MessageBoxButtons.OK);
            ShowWindow(window, SW_SHOWNORMAL);
        }
    }
    //And I put all the Mutex code in a separate class called SingleInstance.
    static public class SingleInstance
    {
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(int uAction, int uParam, IntPtr lpvParam, int fuWinIni);

        public static readonly int WM_SHOWFIRSTINSTANCE =
            WinApi.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", ProgramInfo.AssemblyGuid);
        static Mutex mutex;
        static public bool Start()
        {
            bool onlyInstance = false;
            string mutexName = String.Format("Local\\{0}", ProgramInfo.AssemblyGuid);

            // if you want your app to be limited to a single instance
            // across ALL SESSIONS (multiple users & terminal services), then use the following line instead:
            // string mutexName = String.Format("Global\\{0}", ProgramInfo.AssemblyGuid);

            mutex = new Mutex(true, mutexName, out onlyInstance);
            return onlyInstance;
        }
        static public void ShowFirstInstance()
        {
            //SystemParametersInfo(0x2000, 0, IntPtr.Zero, 1 | 2);

            WinApi.PostMessage(
                (IntPtr)WinApi.HWND_BROADCAST,
                WM_SHOWFIRSTINSTANCE,
                IntPtr.Zero,
                IntPtr.Zero);
        }
        static public void Stop()
        {
            mutex.ReleaseMutex();
        }
    }
    //The Mutex name and the call to RegisterWindowMessage() both require an application-specific GUID. 
    //I use a separate class called ProgramInfo for this.ProgramInfo.AssemblyGuid gets the GUID that is automatically associated with the assembly.

    //接口SteelseriesInterface的Guid.
    [Guid("F9168C5E-CEB2-4faa-B6BF-329BF39FA1F5")]

    interface SteelseriesInterface
    {
        void Main();
    }

    static public class ProgramInfo
    {
        static public string AssemblyGuid
        {
            get
            {
                Attribute IMyInterfaceAttribute = Attribute.GetCustomAttribute(typeof(SteelseriesInterface), typeof(GuidAttribute));
                Guid IMyInterfaceGuid = new Guid(((GuidAttribute)IMyInterfaceAttribute).Value);
                return IMyInterfaceGuid.ToString();
            }
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the applica2tion.
        /// </summary>
        [STAThread]
        static void Main(string[] Args)
        {
            //Judge if the main window is running
            //To make all this code work in your application, you just need to do two things.
            //1.Firstly, in your Main() function, you must call the functions from SingleInstance ... Start(), ShowFirstInstance(), and Stop().
            if (!SingleInstance.Start())
            {
                SingleInstance.ShowFirstInstance();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                MainForm mainForm = new MainForm();
                Application.Run(mainForm);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            SingleInstance.Stop();
        }
    }
}
