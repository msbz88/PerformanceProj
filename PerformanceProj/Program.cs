using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using TestStack.White.Configuration;
using TestStack.White.InputDevices;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;

namespace PerformanceProj {
    class Program {
        public static void CreateMultOrders() {
            //github.com/TestStack/White/blob/master/LICENSE-MIT.txt

            string portalName = "TilePortal: SimCorp Dimension 6.3 PUBLIC";
            //string portalName = "TilePortal: SimCorp Dimension 6.3 (CONFIG_IMM)";
            string scdPath = @"\\Dk01snt899\public\PUBLIC63\Bin\";
            //string scdPath = @"\\Dk01sv7033\t7020230\READYFORTEST\63\CONFIG_IMM\Bin\";
            string titleLogon = "Logon - SimCorp Dimension Version 6.3 (PUBLIC)";
            //string titleLogon = "Logon - SimCorp Dimension [Release Candidate] 6.3 (CONFIG_IMM)";

            //Console.WriteLine("Portal:");
            //string portalName = Console.ReadLine();
            //Console.WriteLine("SCD path:");
            //string scdPath = Console.ReadLine();
            //Console.WriteLine("Logon window:");
            //string titleLogon = Console.ReadLine();

            //Start App
            Console.WriteLine("Starting application...");
            Installation scd = new Installation(portalName, scdPath);
            scd.StartOrAttach();

            //Logon if needed

            scd.TryLogon(titleLogon, "MSBZ", "MSBZ");

            //Portal search
            Console.WriteLine("Openning the window...");
            WindowSCD.WaitWindow(portalName);
            Window portal = WindowSCD.GetWindow(portalName);       
            TestStack.White.UIItems.TextBox textBoxSearch = portal.Get<TestStack.White.UIItems.TextBox>(SearchCriteria.ByText("SearchTextBox"));
            portal.Focus(DisplayState.Restored);
            string windowName = "Multiple Portfolio Orders";
            textBoxSearch.Text = windowName;
            Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);

            //Open window
            WindowSCD.WaitWindow(windowName);
            Window window = WindowSCD.GetWindow(windowName);

            //set value
            List<string> gridColumns = new List<string> { "Security ID", "Leg No.", "Custody", "Portfolio*", "Payment date" };
            WindowSCD.PrepareMultipleGridFields(window, gridColumns);
            Thread.Sleep(500);
            string filePath = @"U:\Desktop\PerformanceProj\orders.txt";
            try {
                string orders = File.ReadAllText(filePath);
                //Clipboard.SetText("J ACCR INT\t1\tHEG_TEST2\tGEN\t01.10.2018");
                Clipboard.SetText(orders);
                WindowSCD.PasteClipboardToGrid(windowName);
                Thread.Sleep(500);
                WindowSCD.Save();
            } catch (Exception) {
                Console.WriteLine("File not found");
            }
        }

        [STAThread]
        static void Main(string[] args) {
            CoreAppXmlConfiguration.Instance.LoggerFactory = new WhiteDefaultLoggerFactory(LoggerLevel.Off);
            CoreAppXmlConfiguration.Instance.BusyTimeout = 20000;                  
            CreateMultOrders();
            Console.WriteLine("Script ended.");
            Console.ReadKey();
        }
    }
}
