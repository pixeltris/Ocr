using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ocr.UI
{
    class CompactTextBox : DockPanel
    {
        public TextBox TextBox { get; private set; }
        public CompactScrollbar HorizontalScroll { get; private set; }
        public CompactScrollbar VerticalScroll { get; private set; }

        public CompactTextBox()
        {
            TextBox = new TextBox();
            TextBox.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            TextBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            TextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            TextBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            TextBox.AcceptsReturn = true;
            TextBox.BorderThickness = new Thickness(0);

            /*Style scrollStyle = new Style(typeof(ScrollViewer));
            scrollStyle.Setters.Add(new Setter(ScrollViewer.PanningModeProperty, PanningMode.None));
            TextBox.Resources.Add(typeof(ScrollViewer), scrollStyle);*/

            Children.Add(TextBox);
        }
    }

    class CompactScrollbar : StackPanel
    {
    }
}
