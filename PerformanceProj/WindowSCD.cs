using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.TableItems;
using TestStack.White.UIItems.WindowItems;
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
            long timeToStrat = RunTime.StopWatch();
            Console.WriteLine("[" + title + "] window started after: " + timeToStrat.ToString() + " ms");
        }

        public static Window GetWindow(string title) {
            return Desktop.Instance.Windows().First(item => item.Name == title);
        }

        public static void ClickButton(Window window, SearchCriteria searchCriteria) {
            Button button = window.Get<Button>(searchCriteria);
            button.Click();
        }

        public static void SelectFromList(string title, string item) {
            Window windowFields = GetWindow(title);
            Table listFields = windowFields.Get<Table>(SearchCriteria.ByControlType(ControlType.Table));
            listFields.Rows[0].Cells[5].Click();        
        }

        private static Window LoadSelectFields(Window window) {
            window.Focus(DisplayState.Restored);
            PressAltPlus("V");
            Thread.Sleep(300);
            var popupMenu = window.Popup;
            Menu firstLevelMenu = popupMenu.ItemBy(SearchCriteria.ByText("Select Fields..."));
            firstLevelMenu.Click();
            string titleSelectedFields = "Select Fields - " + window.Name;
            WaitWindow(titleSelectedFields);
            return GetWindow(titleSelectedFields);
        }

        private static void AddFromAvailableFields(Window windowSelectedFields, string columnName) {
            ListView listSelectedFields = windowSelectedFields.Get<ListView>(SearchCriteria.ByControlType(ControlType.DataGrid).AndIndex(0));
            ListViewRow row = listSelectedFields.Rows.First(r => r.Name == columnName);
            row.DoubleClick();
        }

        private static ListView PrepareSelectedFieldsList(Window windowSelectedFields, string columnName) {
            windowSelectedFields.Focus(DisplayState.Restored);
            windowSelectedFields.RightClick();
            ListView listSelectedFields = windowSelectedFields.Get<ListView>(SearchCriteria.ByControlType(ControlType.DataGrid).AndIndex(1));
            bool isSelected = listSelectedFields.Rows.Any(r => r.Name == columnName);
            if (!isSelected) {
                listSelectedFields.Select(0);
                AddFromAvailableFields(windowSelectedFields, columnName);
                windowSelectedFields.RightClick();
            }
            return listSelectedFields;
        }

        private static void PutFieldFirstInList(ListView listSelectedFields, string columnName) {              
            ListViewRow row = listSelectedFields.Rows.First(r => r.Name == columnName);
            int itemIndex = listSelectedFields.Rows.IndexOf(row);
            if (itemIndex != 0) {
                Thread.Sleep(200);
                listSelectedFields.Select(itemIndex - 1);
                Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.SHIFT);
                listSelectedFields.Select(0);
                Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.SHIFT);
                Thread.Sleep(200);
                Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.ALT);
                Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN);
                Thread.Sleep(200);
                Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.ALT);
            }
        }

        public static void PressControlPlus(string key) {
            Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
            Keyboard.Instance.Enter(key);
            Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
        }

        public static void PressShiftPlus(string key) {
            Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.SHIFT);
            Keyboard.Instance.Enter(key);
            Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.SHIFT);
        }

        public static void PressAltPlus(string key) {
            Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.ALT);
            Keyboard.Instance.Enter(key);
            Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.ALT);
        }

        public static void PrepareMultipleGridFields(Window window, List<string> columnNames) {
            Window windowSelectedFields = LoadSelectFields(window);
            columnNames.Reverse();
            foreach (var item in columnNames) {
                ListView selectedFields = PrepareSelectedFieldsList(windowSelectedFields, item);              
                PutFieldFirstInList(selectedFields, item);               
                Thread.Sleep(500);
            }
            ClickButton(windowSelectedFields, SearchCriteria.ByText("OK"));
            Thread.Sleep(500);
            PressControlPlus("R");
            Thread.Sleep(500);
        }

        public static void PasteClipboardToGrid(string windowName) {
            Window window = GetWindow(windowName);
            window.Focus(DisplayState.Restored);
            PressAltPlus("E");
            Thread.Sleep(300);
            var popupMenu = window.Popup;
            Menu firstLevelMenu = popupMenu.ItemBy(SearchCriteria.ByText("Paste"));
            firstLevelMenu.Click();
        }

        public static void Save() {
            PressControlPlus("S");
        }

    }
}
