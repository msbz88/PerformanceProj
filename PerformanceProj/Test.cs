using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;

namespace PerformanceProj {
    class Test {
        public Application RunApp(string path) {
            Application app = Application.AttachOrLaunch(new System.Diagnostics.ProcessStartInfo(path));
            return app;
        }

        public Window GetMainWindow(Application app) {
            Window mainWindow = app.GetWindows().First();
            return mainWindow;
        }

        public bool IsWindowLaunched(string title) {
            return Desktop.Instance.Windows().Any(item => item.Title == title);
        }
    }
}
