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

namespace PerformanceProj {
    class Program {
        static void Main(string[] args) {           
            string scdName = "TilePortal: SimCorp Dimension 6.3 PUBLIC";
            string scdPath = @"\\Dk01snt899\public\PUBLIC63\Bin\";       

            //Start App
            Installation scd = new Installation(scdName, scdPath);
            scd.StartOrAttach();

            //Logon if needed
            string titleLogon = "Logon - SimCorp Dimension Version 6.3 (PUBLIC)";
            scd.TryLogon(titleLogon, "MSBZ", "MSBZ");

            //Portal search
            Window portal = WindowSCD.GetWindow(scdName);
            GroupBox assManItem = portal.Get<GroupBox>(SearchCriteria.ByControlType(ControlType.Group).AndByText("Tiles"));
            
            var solutions = assManItem.Get(SearchCriteria.ByText("Asset Manager"));
            solutions.Click();
            //solutions.Click();
            WindowSCD.WaitWindow("Asset Manager");
            Window assMan = WindowSCD.GetWindow("Asset Manager");
            WindowSCD.ClickButton(assMan, SearchCriteria.ByText("Open"));
            ListView listFields = assMan.MdiChild(SearchCriteria.ByText("Search List Asset Manager")).Get<ListView>(SearchCriteria.ByControlType(ControlType.Table));
            listFields.Select("ADL");

            //Load window and set up value
            /*
            TextBox textBoxSecID = WindowSCD.ActivateCell(mltplPortOrders, "Security ID");
            textBoxSecID.Text = "H BLAIR";
            Thread.Sleep(1000);
            Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
            */
        }
    }
}
