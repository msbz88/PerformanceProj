using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems;
using System.Threading;
using TestStack.White.WindowsAPI;
using TestStack.White.InputDevices;
using TestStack.White.UIItems.TableItems;
using System.Windows.Automation;
using System.ComponentModel;
using TestStack.White.Configuration;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WPFUIItems;
using TestStack.White.UIItems.MenuItems;
using System.Diagnostics;
using Castle.Core.Logging;

namespace PerformanceProj {
    class Program {

        static void Main(string[] args) {
            CoreAppXmlConfiguration.Instance.LoggerFactory = new WhiteDefaultLoggerFactory(LoggerLevel.Off);
            CoreAppXmlConfiguration.Instance.BusyTimeout = 20000;
            string portalName = "TilePortal: SimCorp Dimension 6.3 PUBLIC";
            string scdPath = @"\\Dk01snt899\public\PUBLIC63\Bin\";

            //Start App
            Console.WriteLine("Starting application...");
            Installation scd = new Installation(portalName, scdPath);
            scd.StartOrAttach();

            //Logon if needed
            string titleLogon = "Logon - SimCorp Dimension Version 6.3 (PUBLIC)";
            scd.TryLogon(titleLogon, "MSBZ", "MSBZ");

            //Portal search
            Console.WriteLine("Openning the window...");
            WindowSCD.WaitWindow(portalName);
            Window portal = WindowSCD.GetWindow(portalName);
            portal.Focus(DisplayState.Restored);
            TextBox textBoxSearch = portal.Get<TextBox>(SearchCriteria.ByText("SearchTextBox"));
            string windowName = "Multiple Portfolio Orders";
            textBoxSearch.Text = windowName;        
            Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);

            //MPO
            WindowSCD.WaitWindow(windowName);
            Window window = WindowSCD.GetWindow(windowName);
            //WindowSCD.ActiveCellInGrid(window);
            //set value
            //WindowSCD.PutColumnFirstInGrid(window, "Security ID");
            string[] gridColumns = { "Security ID", "Currency" };
            WindowSCD.PrepareGridFields(window, gridColumns);
            // TextBox textBoxSecID = WindowSCD.PrepareGridFields(window, "Security ID");
            //   textBoxSecID.Text = "H BLAIR";
            //Prepare Grid FieldsThread.Sleep(1000);
            //Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);

            /*
             *             GroupBox assManItem = portal.Get<GroupBox>(SearchCriteria.ByControlType(ControlType.Group).AndByText("Tiles"));
            var solutions = assManItem.Get(SearchCriteria.ByText("Asset Manager"));
            solutions.Click();
            //solutions.Click();
            
            WindowSCD.WaitWindow("Asset Manager");
            Window assMan = WindowSCD.GetWindow("Asset Manager");
            WindowSCD.ClickButton(assMan, SearchCriteria.ByText("Open"));
            ListView listFields = assMan.ModalWindow(SearchCriteria.ByText("Search List - Asset Manager")).Get<ListView>(SearchCriteria.ByControlType(ControlType.Table));
            listFields.Select(5);

            //Load window and set up value
        
            TextBox textBoxSecID = WindowSCD.ActivateCell(mltplPortOrders, "Security ID");
            textBoxSecID.Text = "H BLAIR";
            Thread.Sleep(1000);
            Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
            */
        }
    }
}
