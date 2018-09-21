using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.TableItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WindowStripControls;
using TestStack.White.WindowsAPI;

namespace PerformanceProj {

    public static class WindowSCD {
        public static bool IsWindowLaunched(string title) {
            return Desktop.Instance.Windows().Any(item => item.Name == title);
        }

        public static void WaitWindow(string title) {
            RunTime.StartWatch();
            while (!IsWindowLaunched(title)) {
                Thread.Sleep(1000);
            }
            RunTime.PrintStopwatchResult("Wait window");
        }

        public static Window GetWindow(string title) {
            return Desktop.Instance.Windows().First(item => item.Name == title);
        }

        public static void ClickButton(Window window, SearchCriteria searchCriteria) {
            RunTime.StartWatch();
            Button button = window.Get<Button>(searchCriteria);
            button.Click();
            RunTime.PrintStopwatchResult("Button click");
        }

        public static void SelectFromList(string title, string item) {
            Window windowFields = GetWindow(title);
            Table listFields = windowFields.Get<Table>(SearchCriteria.ByControlType(ControlType.Table));
            listFields.Rows[0].Cells[5].Click();        
        }

        private static Window LoadSelectFields(Window window) {
            window.Focus(DisplayState.Restored);
            Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.ALT);
            Keyboard.Instance.Enter("V");
            Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.ALT);
            Thread.Sleep(300);
            var popupMenu = window.Popup;
            Menu firstLevelMenu = popupMenu.ItemBy(SearchCriteria.ByText("Select Fields..."));
            firstLevelMenu.Click();
            string titleSelectedFields = "Select Fields - " + window.Name;
            WaitWindow(titleSelectedFields);
            return GetWindow(titleSelectedFields);
        }

        private static void AddFromAvailableFields(Window windowSelectedFields, string cellName) {
            ListView listSelectedFields = windowSelectedFields.Get<ListView>(SearchCriteria.ByControlType(ControlType.DataGrid).AndIndex(0));
            ListViewRow row = listSelectedFields.Rows.First(r => r.Name == cellName);
            row.DoubleClick();
        }

        public static void PutColumnFirstInGrid(Window window, string cellName) {
            Window windowSelectedFields = LoadSelectFields(window);
            windowSelectedFields.Focus(DisplayState.Restored);
            ListView listSelectedFields = windowSelectedFields.Get<ListView>(SearchCriteria.ByControlType(ControlType.DataGrid).AndIndex(1));
            bool isSelected = listSelectedFields.Rows.Any(r => r.Name == cellName);
            if (!isSelected) {
                listSelectedFields.Rows[0].Select();
                AddFromAvailableFields(windowSelectedFields, cellName);
                window.Click();
            }
            ListViewRow row = listSelectedFields.Rows.First(r => r.Name == cellName);
            row.Select();
            int itemIndex = listSelectedFields.Rows.IndexOf(listSelectedFields.SelectedRows[0]);
            if (itemIndex != 0) {
                listSelectedFields.Select(itemIndex - 1);
                Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.SHIFT);
                listSelectedFields.Select(0);
                Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.SHIFT);
                Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.ALT);
                Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN);
                Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.ALT);
                Thread.Sleep(300);
                ClickButton(windowSelectedFields, SearchCriteria.ByText("OK"));
                Thread.Sleep(300);
                Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.LEFT);
            } else {
                ClickButton(windowSelectedFields, SearchCriteria.ByText("OK"));
            }
        }

        public static void PrepareGridFields(Window window, string[] cellNames) {
            Window windowSelectedFields = LoadSelectFields(window);
            ListView listSelectedFields = windowSelectedFields.Get<ListView>(SearchCriteria.ByControlType(ControlType.DataGrid).AndIndex(1));
            List<string> mandatoryFields = listSelectedFields.Rows.Where(i=>i.Name.Contains("*")).Select(i=>i.Name).ToList();
            listSelectedFields.Focus();
            listSelectedFields.Rows[0].Select();
            Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.SHIFT);
            int countFields = listSelectedFields.Rows.Count - 1;
            listSelectedFields.Rows[countFields].Select();
            Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.SHIFT);
            Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
            foreach (var item in mandatoryFields) {
                listSelectedFields.Rows.First(r => r.Name == item).Click();
            }
            Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
            window.Click();
            Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.ALT);
            Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.LEFT);
            Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.ALT);
        }

    }
}
