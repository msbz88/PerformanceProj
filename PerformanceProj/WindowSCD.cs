using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            while (!IsWindowLaunched(title)) {
                Thread.Sleep(1000);
            }
        }

        public static Window GetWindow(string title) {
            return Desktop.Instance.Windows().First(item => item.Name == title);
        }

        public static void ClickButton(Window window, SearchCriteria searchCriteria) {
            Button button = window.Get<Button>(searchCriteria);
            button.Click();
        }

        public static TextBox ActiveCellInGrid(Window window) {
            SearchCriteria searchPanel = SearchCriteria.ByControlType(ControlType.Pane);
            Panel panel = window.Get<Panel>(searchPanel);
            window.Focus(DisplayState.Restored);
            SearchCriteria searchTextBox = SearchCriteria.ByControlType(ControlType.Document);
            return panel.Get<TextBox>(searchTextBox);
        }

        public static void SelectFromList(string title, string item) {
            Window windowFields = GetWindow(title);
            Table listFields = windowFields.Get<Table>(SearchCriteria.ByControlType(ControlType.Table));
            listFields.Rows[0].Cells[5].Click();        
        }

        public static TextBox ActivateCell(Window window, string cellName) {
            TextBox activeTextBox = ActiveCellInGrid(window);
            window.Focus();
            activeTextBox.RightClick();
            var popupMenu = window.Popup;
            Menu level1Menu = popupMenu.ItemBy(SearchCriteria.ByText("View Properties"));
            level1Menu.Click();
            Menu level2Menu = popupMenu.ItemBy(SearchCriteria.ByText("Select Fields..."));
            level2Menu.Click();
            string titleSelectedFields = "Select Fields - " + window.Name;
            WaitWindow(titleSelectedFields);
            Window windowSelectedFields = GetWindow(titleSelectedFields);
            ListView selectedFields = windowSelectedFields.Get<ListView>(SearchCriteria.ByControlType(ControlType.DataGrid).AndIndex(1));
            windowSelectedFields.Focus();
            try {
                selectedFields.Select(cellName);
            } catch (Exception) {
                Console.WriteLine("Not found");
            }           
            int itemIndex = selectedFields.Rows.IndexOf(selectedFields.SelectedRows[0]);
            if (itemIndex != 0) {
                selectedFields.Select(itemIndex - 1);
                Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.SHIFT);
                selectedFields.Select(0);
                Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.SHIFT);
                Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.ALT);
                Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN);
                Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.ALT);
                Thread.Sleep(500);
                ClickButton(windowSelectedFields, SearchCriteria.ByText("OK"));
                Thread.Sleep(500);
                Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.LEFT);
            } else {
                ClickButton(windowSelectedFields, SearchCriteria.ByText("OK"));
            }
            return activeTextBox;
        }
    }
}
