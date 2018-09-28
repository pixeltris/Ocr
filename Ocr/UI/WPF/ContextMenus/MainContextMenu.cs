using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ocr.UI.ContextMenus
{
    class MainContextMenu : CompactContextMenu
    {
        public MainContextMenu()
        {            
        }

        public override void Build()
        {
            Items.Add(BuildLanguageItem());
            Items.Add(BuildLayoutItem());
            Items.Add(BuildEngineItem());
            Items.Add(BuildEngineProfileItem());
            Items.Add(BuildTranslateItem());
            Items.Add(BuildCaptureItem());
            Items.Add(BuildHeaderItem());
            Items.Add(new Separator());
            Items.Add("Main View");
            Items.Add("Settings");
        }

        private MenuItem BuildLanguageItem()
        {
            MenuItem languageItem = new MenuItem();
            languageItem.Header = "Language";            
            languageItem.Items.Add("Slot1");
            languageItem.Items.Add("Slot2");
            languageItem.Items.Add("Slot3");
            languageItem.Items.Add("Slot4");
            languageItem.Items.Add("Slot5");
            languageItem.Items.Add(new Separator());
            languageItem.Items.Add("Swapped");
            return languageItem;
        }

        private MenuItem BuildLayoutItem()
        {
            MenuItem layoutItem = new MenuItem();
            layoutItem.Header = "Layout";
            layoutItem.Items.Add("Both");
            layoutItem.Items.Add("Original");
            layoutItem.Items.Add("Translated");            
            layoutItem.Items.Add(new Separator());
            layoutItem.Items.Add("Switch Order");
            layoutItem.Items.Add("Stack Vertically");
            return layoutItem;
        }

        private MenuItem BuildEngineItem()
        {
            MenuItem engineItem = new MenuItem();
            engineItem.Header = "Engine";
            // TODO: Find all available engines
            engineItem.Items.Add("Windows");
            engineItem.Items.Add("Google");
            return engineItem;
        }

        private MenuItem BuildEngineProfileItem()
        {
            MenuItem profileItem = new MenuItem();
            profileItem.Header = "Profile";
            profileItem.Items.Add("1x Linear");
            profileItem.Items.Add("2x Linear");
            return profileItem;
        }

        private MenuItem BuildCaptureItem()
        {
            MenuItem captureItem = new MenuItem();
            captureItem.Header = "Capture";
            captureItem.Items.Add("Area");
            captureItem.Items.Add("Area (Freeze Screen)");
            captureItem.Items.Add("Area (Editable)");
            return captureItem;
        }

        private MenuItem BuildTranslateItem()
        {
            MenuItem translateItem = new MenuItem();
            translateItem.Header = "Translate";
            // TODO: Find all available translate APIs
            translateItem.Items.Add("Google");
            translateItem.Items.Add("Bing");
            return translateItem;
        }

        private MenuItem BuildHeaderItem()
        {
            MenuItem headerItem = new MenuItem();
            headerItem.Header = "Headers";
            headerItem.Items.Add("Language");
            headerItem.Items.Add("Layout");
            headerItem.Items.Add("Engine");
            headerItem.Items.Add("Profile");
            headerItem.Items.Add("Translate");
            headerItem.Items.Add("Capture");
            headerItem.Items.Add(new Separator());
            headerItem.Items.Add("Click");
            headerItem.Items.Add("Drag");
            return headerItem;
        }

        void menuItem_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
