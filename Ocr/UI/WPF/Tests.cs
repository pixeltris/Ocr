using Ocr.UI.ContextMenus;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shell;

namespace Ocr.UI.WPF
{
    class WPFCompactForm1 : Window
    {
        public WPFCompactForm1()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;

            Setter effectSetter = new Setter();
            effectSetter.Property = ScrollViewer.EffectProperty;
            effectSetter.Value = new DropShadowEffect
            {
                ShadowDepth = 4,
                Direction = 330,
                Color = Colors.Black,
                Opacity = 0.5,
                BlurRadius = 4
            };

            Style dropShadowScrollViewerStyle = new Style(typeof(ScrollViewer));
            dropShadowScrollViewerStyle.Setters.Add(effectSetter);

            TextBox dropShadowTextBox = new TextBox();
            dropShadowTextBox.Text = "Shadow Text" + Environment.NewLine + "HELLO";
            dropShadowTextBox.Foreground = Brushes.Black;//Brushes.Teal;
            //dropShadowTextBox.FontSize = 40;
            dropShadowTextBox.FontSize = 14;
            dropShadowTextBox.FontFamily = new System.Windows.Media.FontFamily("Arial");
            dropShadowTextBox.Margin = new Thickness(10);
            dropShadowTextBox.TextWrapping = TextWrapping.NoWrap;
            dropShadowTextBox.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            dropShadowTextBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            dropShadowTextBox.AcceptsReturn = true;
            dropShadowTextBox.BorderBrush = null;
            dropShadowTextBox.BorderThickness = new Thickness(0);
            dropShadowTextBox.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            //dropShadowTextBox.Background = Brushes.Black;
            //dropShadowTextBox.Padding = new Thickness(5);
            dropShadowTextBox.Margin = new Thickness(0);
            dropShadowTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            dropShadowTextBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            dropShadowTextBox.Resources.Add(typeof(ScrollViewer), dropShadowScrollViewerStyle);
            //dropShadowTextBox.Resources.Add(typeof(ScrollBar), CreateScrollbarStyle());

            //var v12 = dropShadowTextBox.GetTemplateChild("ContentElement") as ScrollViewer;
            var v123 = new ScrollViewer();

            //dropShadowTextBox.Width = 100;
            //dropShadowTextBox.Height = 100;
            dropShadowTextBox.Background = Brushes.Red;
            //Canvas.SetTop(dropShadowTextBox, 100);
            //Canvas.SetLeft(dropShadowTextBox, 100);
            //dropShadowTextBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Stretch;

            CompactPanel compactPanel = new CompactPanel();
            //compactPanel.Opacity = 0.3;
            compactPanel.Children.Add(dropShadowTextBox);
            compactPanel.Background = new SolidColorBrush(Color.FromArgb(128, 0x15, 0x25, 0x44));//Brushes.Red;
            AddChild(compactPanel);

            /*Grid g1 = new Grid();
            //g1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            //g1.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(5) });
            //g1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            g1.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            g1.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5), MaxWidth = 1000 - 100 });
            g1.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            g1.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            var v1233 = new Button() { Width = 50 };
            var v1234 = new GridSplitter() { Width = 5, VerticalAlignment = System.Windows.VerticalAlignment.Stretch, ShowsPreview = true };
            var v12345 = new Button() { Width = 50 };
            Grid.SetColumn(v1233, 0);
            Grid.SetColumn(v1234, 1);
            Grid.SetColumn(v12345, 2);
            g1.Children.Add(v1233);
            g1.Children.Add(v1234);
            g1.Children.Add(v12345);
            AddChild(g1);*/

            /*DevZest.Windows.SplitContainer sc = new DevZest.Windows.SplitContainer();
            sc.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            sc.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            sc.SplitterBackground = Brushes.Black;
            //sc.PreviewBackground = Brushes.Black;
            sc.PreviewOpacity = 0.5;
            sc.Background = Brushes.White;
            sc.SplitterWidth = 15;
            sc.SplitterDistance = new DevZest.Windows.SplitterDistance(10);
            sc.IsPreviewVisible
            //sc.SplitterTemplate.
            AddChild(sc);
            sc.Child1 = new Button();
            sc.Child2 = new Button();*/

            //dropShadowTextBox.Resources[typeof(]

            //this.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));//Brushes.Transparent;//new SolidColorBrush(Color.FromArgb(255, 0x15, 0x25, 0x44));
            //this.Background = Brushes.Transparent;//new SolidColorBrush(Color.FromArgb(128, 0x15, 0x25, 0x44));
            this.Background = Brushes.White;
            this.AllowsTransparency = true;
            //this.Padding = new Thickness(5);
            //this.Margin = new Thickness(5);
            //this.AddChild(dropShadowTextBox);
            //this.AddChild(new WPFPanel());
            //this.Resources.Add(typeof(TextBox), dropShadowTextBox);

            Width = 1000;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        private Style CreateScrollbarStyle()
        {
            /*Setter templateSetter = new Setter();
            templateSetter.Property = ScrollBar.BackgroundProperty;
            templateSetter.Value = Brushes.Black;            

            Style style = new Style(typeof(ScrollBar));
            style.Setters.Add(templateSetter);*/

            Style style = new Style(typeof(ScrollBar));
            SetProperty(style, Stylus.IsFlicksEnabledProperty, false);
            SetProperty(style, ScrollBar.ForegroundProperty, new SolidColorBrush(Color.FromArgb(0xFF, 0xAD, 0xAB, 0xAB)));
            SetProperty(style, ScrollBar.BackgroundProperty, Brushes.Transparent);
            SetProperty(style, ScrollBar.WidthProperty, 7);

            ControlTemplate ct = new ControlTemplate(typeof(ScrollBar));
            var image = new FrameworkElementFactory(typeof(ScrollBar));
            SetProperty(style, ScrollBar.TemplateProperty, ct);

            return style;
        }

        private void SetProperty(Style style, DependencyProperty property, object value)
        {
            style.Setters.Add(new Setter(property, value));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }
    }

    class CompactPanel : DockPanel
    {
    }

    class WPFPanel : DockPanel
    {
        public WPFPanel()
        {
            Setter effectSetter = new Setter();
            effectSetter.Property = ScrollViewer.EffectProperty;
            effectSetter.Value = new DropShadowEffect
            {
                ShadowDepth = 4,
                Direction = 330,
                Color = Colors.Black,
                Opacity = 0.5,
                BlurRadius = 4
            };

            Style dropShadowScrollViewerStyle = new Style(typeof(ScrollViewer));
            dropShadowScrollViewerStyle.Setters.Add(effectSetter);

            TextBox dropShadowTextBox = new TextBox();
            dropShadowTextBox.Text = "Shadow Text" + Environment.NewLine + "HELLO";
            dropShadowTextBox.Foreground = Brushes.Teal;
            //dropShadowTextBox.FontSize = 40;
            dropShadowTextBox.FontSize = 14;
            dropShadowTextBox.FontFamily = new System.Windows.Media.FontFamily("Arial");
            dropShadowTextBox.Margin = new Thickness(10);
            dropShadowTextBox.TextWrapping = TextWrapping.NoWrap;
            dropShadowTextBox.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            dropShadowTextBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            dropShadowTextBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Stretch;
            dropShadowTextBox.AcceptsReturn = true;
            dropShadowTextBox.BorderBrush = null;
            dropShadowTextBox.BorderThickness = new Thickness(0);
            dropShadowTextBox.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            //dropShadowTextBox.Background = Brushes.Black;
            //dropShadowTextBox.Padding = new Thickness(5);
            dropShadowTextBox.Margin = new Thickness(0);
            dropShadowTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            dropShadowTextBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            dropShadowTextBox.Resources.Add(typeof(ScrollViewer), dropShadowScrollViewerStyle);
            //dropShadowTextBox.IsInactiveSelectionHighlightEnabled = true;
            //dropShadowTextBox.SelectionBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0xFF));
            //dropShadowTextBox.SelectionOpacity = 0.9;

            dropShadowTextBox.LostFocus += dropShadowTextBox_LostFocus;

            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            this.Background = Brushes.Red;//new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));//Brushes.Red;

            this.LastChildFill = true;
            this.Margin = new Thickness(0);

            this.Children.Add(dropShadowTextBox);
        }

        void dropShadowTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
