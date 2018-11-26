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
            var applicationPath = Path.Combine(DirectoryPath, "scd.exe");
            Application.AttachOrLaunch(new System.Diagnostics.ProcessStartInfo(applicationPath));
        }

        public void TryLogon(string titleLogon, string username, string password) {
            if (!WindowSCD.IsWindowLaunched(PortalName)) {
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
                    //Console.WriteLine(titleLogon + " window closed");
                }
                WindowSCD.WaitWindow(PortalName);
            }
        }

        public Window PortalSearch(string text) {
            WindowSCD.WaitWindow(text);
            if (WindowSCD.IsWindowLaunched(text)) {
                Window windowPortal = WindowSCD.GetWindow(PortalName);
                windowPortal.Focus(DisplayState.Restored);
                TextBox textBoxPortalSearch = windowPortal.Get<TextBox>(SearchCriteria.ByAutomationId("SearchTextBox"));
                textBoxPortalSearch.Text = text;
                Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);              
                WindowSCD.WaitWindow(text);
            }
            return WindowSCD.GetWindow(text);
        }
    }
}
