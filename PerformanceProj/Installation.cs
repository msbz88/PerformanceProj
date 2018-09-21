using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;

namespace PerformanceProj {
    public class Installation {
        public string PortalName { get; set; }
        string DirectoryPath { get; set; }

        public Installation(string name, string directoryPath) {
            PortalName = name;
            DirectoryPath = directoryPath;
        }

        public void StartOrAttach() {
            RunTime.StartWatch();
            var applicationPath = Path.Combine(DirectoryPath, "scd.exe");
            Application.AttachOrLaunch(new System.Diagnostics.ProcessStartInfo(applicationPath));
            RunTime.PrintStopwatchResult("SCD start/resume");
        }

        public void TryLogon(string titleLogon, string username, string password) {
            if (!WindowSCD.IsWindowLaunched(PortalName)) {
                RunTime.StartWatch();
                WindowSCD.WaitWindow(titleLogon);
                Window windowLogon = WindowSCD.GetWindow(titleLogon);
                windowLogon.Focus(DisplayState.Restored);
                TextBox textBoxUsername = windowLogon.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(1));
                textBoxUsername.Text = username;
                TextBox textBoxPassword = windowLogon.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(2));
                textBoxPassword.Text = password;
                try {
                    WindowSCD.ClickButton(windowLogon, SearchCriteria.ByText("OK"));
                } catch (Exception) {
                    Console.WriteLine(titleLogon + " window closed");
                }
                RunTime.PrintStopwatchResult("Logon");
                WindowSCD.WaitWindow(PortalName);
            }
        }

        public Window PortalSearch(string text) {
            if (!WindowSCD.IsWindowLaunched(text)) {
                Window windowPortal = WindowSCD.GetWindow(PortalName);
                windowPortal.Focus(DisplayState.Restored);
                TextBox textBoxPortalSearch = windowPortal.Get<TextBox>(SearchCriteria.ByAutomationId("SearchTextBox"));
                textBoxPortalSearch.Text = text;
                Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);              
                RunTime.StartWatch();
                WindowSCD.WaitWindow(text);
                RunTime.PrintStopwatchResult("Window load");
            }
            return WindowSCD.GetWindow(text);
        }
    }
}
