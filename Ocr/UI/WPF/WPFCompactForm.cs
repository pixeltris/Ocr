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

namespace Ocr.UI
{    
    class WPFCompactForm : Window
    {        
        private CompactContainer container;
        private CompactContentContainer contentContainer;
        private Grid contentGrid;
        private GridSplitter contentSplitter;
        private CompactTextBox originalTextBox;
        private CompactTextBox translatedTextBox;

        public CompactHeader Header { get; private set; }

        public Color BorderColor { get; set; }
        public int BorderSize { get; set; }
        public int SplitterSize { get; set; }
        public bool StackVertically { get; set; }
        public bool ShowTranslated { get; set; }
        public bool ShowOriginal { get; set; }
        public bool SwitchTranslatedOriginalOrder { get; set; }

        public bool ShowTranslatedTextBoxCaret { get; set; }

        public bool UseExtendedResizeCorners { get; set; }
        public int ExtendedResizeCornerSize { get; set; }
        public int CornerSize
        {
            get { return UseExtendedResizeCorners ? ExtendedResizeCornerSize : BorderSize; }
        }

        public bool IsActivated { get; private set; }

        public WPFCompactForm()
        {
            UseExtendedResizeCorners = true;
            ExtendedResizeCornerSize = 15;

            BorderSize = 5;            
            SplitterSize = 5;
            BorderColor = Color.FromArgb(0xFF, 0x2C, 0x3A, 0x56);
            ShowOriginal = true;
            ShowTranslated = true;
            StackVertically = true;

            ShowTranslatedTextBoxCaret = false;

            Initialize();
        }

        private void Initialize()
        {            
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip;

            //this.Topmost = true;

            // This removes the resize border with CanResizeWithGrip
            this.AllowsTransparency = true;

            // Remove the resize grip
            Style gripStyle = new Style(typeof(ResizeGrip));
            gripStyle.Setters.Add(new Setter(ResizeGrip.TemplateProperty, null));
            Resources.Add(typeof(ResizeGrip), gripStyle);

            this.Loaded += WPFCompactForm_Loaded;

            container = new CompactContainer();
            container.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            container.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;            

            contentGrid = new Grid();            
            contentSplitter = new GridSplitter();
            contentSplitter.ShowsPreview = true;
            contentSplitter.IsTabStop = false;

            contentContainer = new CompactContentContainer(this);
            contentContainer.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            contentContainer.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;            
            contentContainer.Children.Add(contentGrid);

            originalTextBox = new CompactTextBox();
            originalTextBox.Background = Brushes.White;
            originalTextBox.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            originalTextBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            translatedTextBox = new CompactTextBox();
            translatedTextBox.TextBox.IsReadOnly = true;
            translatedTextBox.TextBox.IsReadOnlyCaretVisible = ShowTranslatedTextBoxCaret;
            translatedTextBox.Background = Brushes.White;
            translatedTextBox.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            translatedTextBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            Header = new CompactHeader(this);
            Header.BackColor = BorderColor;
            Header.Layout();

            this.SnapsToDevicePixels = true;
            this.FocusVisualStyle = null;
            contentSplitter.FocusVisualStyle = null;

            // Maybe shouldn't need BorderSize here, should the border be able to collapse into the header?
            //MinHeight = MinWidth = header.HeaderSize + BorderSize;
            this.MinWidth = 150;
            this.MinHeight = 100;

            // Alternative method for borderless with resize functionality (buggy)
            /*this.BorderThickness = new Thickness(0);
            WindowChrome windowChrome = new WindowChrome();
            windowChrome.GlassFrameThickness = new Thickness(5);
            windowChrome.CornerRadius = new CornerRadius(0);
            windowChrome.CaptionHeight = header.HeaderSize - 4;
            WindowChrome.SetWindowChrome(this, windowChrome);*/

            AddChild(container);
            CreateLayout();
        }

        protected override void OnActivated(EventArgs e)
        {
            IsActivated = true;
            base.OnActivated(e);
        }

        protected override void OnDeactivated(EventArgs e)
        {
            IsActivated = false;
            base.OnDeactivated(e);
        }

        void WPFCompactForm_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr mainWindowPtr = new WindowInteropHelper(this).Handle;
            HwndSource mainWindowSrc = HwndSource.FromHwnd(mainWindowPtr);
            mainWindowSrc.AddHook(WndProc);
        }        

        private void CreateLayout()
        {
            contentContainer.Background = new SolidColorBrush(BorderColor);
            contentSplitter.Background = new SolidColorBrush(BorderColor);
            Header.Background = new SolidColorBrush(Header.BackColor);

            container.Children.Clear();
            container.RowDefinitions.Clear();
            container.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(Header.HeaderSize) });
            container.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            Grid.SetRow(Header, 0);
            Grid.SetRow(contentContainer, 1);

            container.Children.Add(Header);
            container.Children.Add(contentContainer);

            contentGrid.Margin = new Thickness(BorderSize, 0, BorderSize, BorderSize);
            contentGrid.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            contentGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            CreateContentGrid();
        }

        private void CreateContentGrid()
        {
            contentGrid.Children.Clear();
            contentGrid.RowDefinitions.Clear();
            contentGrid.ColumnDefinitions.Clear();            

            if (StackVertically)
            {
                if (ShowOriginal)
                {
                    contentGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                }
                if (ShowOriginal && ShowTranslated)
                {
                    contentGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(SplitterSize) });
                }
                if (ShowTranslated)
                {
                    contentGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                }

                contentSplitter.Height = 5;
                contentSplitter.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                contentSplitter.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            }
            else
            {
                if (ShowOriginal)
                {
                    contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                }
                if (ShowOriginal && ShowTranslated)
                {
                    contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(SplitterSize) });
                }
                if (ShowTranslated)
                {
                    contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });                    
                }

                contentSplitter.Width = 5;
                contentSplitter.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                contentSplitter.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            }

            if (ShowOriginal && ShowTranslated)
            {
                if (StackVertically)
                {
                    Grid.SetRow(SwitchTranslatedOriginalOrder ? translatedTextBox : originalTextBox, 0);
                    Grid.SetRow(contentSplitter, 1);
                    Grid.SetRow(SwitchTranslatedOriginalOrder ? originalTextBox : translatedTextBox, 2);
                }
                else
                {
                    Grid.SetColumn(SwitchTranslatedOriginalOrder ? translatedTextBox : originalTextBox, 0);
                    Grid.SetColumn(contentSplitter, 1);
                    Grid.SetColumn(SwitchTranslatedOriginalOrder ? originalTextBox : translatedTextBox, 2);
                }

                if (SwitchTranslatedOriginalOrder)
                {                    
                    contentGrid.Children.Add(translatedTextBox);
                    contentGrid.Children.Add(contentSplitter);
                    contentGrid.Children.Add(originalTextBox);
                }
                else
                {
                    contentGrid.Children.Add(originalTextBox);
                    contentGrid.Children.Add(contentSplitter);
                    contentGrid.Children.Add(translatedTextBox);
                }
            }
            else if (ShowOriginal)
            {
                contentGrid.Children.Add(originalTextBox);
                if (StackVertically)
                {
                    Grid.SetRow(originalTextBox, 0);
                }
                else
                {
                    Grid.SetColumn(originalTextBox, 0);
                }
            }
            else if (ShowTranslated)
            {
                contentGrid.Children.Add(translatedTextBox);
                if (StackVertically)
                {
                    Grid.SetRow(translatedTextBox, 0);
                }
                else
                {
                    Grid.SetColumn(translatedTextBox, 0);
                }
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Native.WM_NCHITTEST:
                    Point pos = new Point((lParam.ToInt32() & 0xFFFF), (lParam.ToInt32() >> 16));
                    pos = PointFromScreen(pos);

                    var result = VisualTreeHelper.HitTest(this, pos);
                    CompactButton button = result.VisualHit as CompactButton;
                    bool ignoreButton = false;
                    bool ignoreButtonDrag = false;
                    if (button != null)
                    {
                        if (button.IsControlBoxFade || button.IsControlBoxOverlay)
                        {
                            ignoreButton = true;
                            ignoreButtonDrag = true;
                        }
                        else if (button.IsIconButton && Header.AllowBorderResizeOnIcon)
                        {
                            ignoreButton = true;
                        }
                        else if (button.IsControlBoxButton && Header.AllowBorderResizeOnControlBoxButton)
                        {
                            ignoreButton = true;
                        }
                        else if (!button.IsControlBoxButton && !button.IsIconButton && Header.AllowBorderResizeOnButtons)
                        {
                            ignoreButton = true;
                        }
                        else
                        {
                            break;
                        }
                    }

                    Rect leftRect = new Rect(0, 0, BorderSize, ActualHeight);
                    Rect rightRect = new Rect(ActualWidth - BorderSize, 0, BorderSize, ActualHeight);
                    Rect topRect = new Rect(0, 0, ActualWidth, BorderSize);
                    Rect bottomRect = new Rect(0, ActualHeight - BorderSize, ActualWidth, BorderSize);
                    Rect topLeftRect = new Rect(0, 0, CornerSize, CornerSize);
                    Rect topRightRect = new Rect(ActualWidth - CornerSize, 0, CornerSize, CornerSize);
                    Rect bottomLeftRect = new Rect(0, ActualHeight - CornerSize, CornerSize, CornerSize);
                    Rect bottomRightRect = new Rect(ActualWidth - CornerSize, ActualHeight - CornerSize, CornerSize, CornerSize);
                    Rect captionRect = new Rect(0, 0, ActualWidth, Header.HeaderSize);
                    if (bottomRightRect.Contains(pos))
                    {
                        handled = true;
                        return (IntPtr)HitTest.HTBOTTOMRIGHT;
                    }
                    else if (bottomLeftRect.Contains(pos))
                    {
                        handled = true;
                        return (IntPtr)HitTest.HTBOTTOMLEFT;
                    }
                    else if (topLeftRect.Contains(pos))
                    {
                        handled = true;
                        return (IntPtr)HitTest.HTTOPLEFT;
                    }
                    else if (topRightRect.Contains(pos))
                    {
                        handled = true;
                        return (IntPtr)HitTest.HTTOPRIGHT;
                    }
                    else if (leftRect.Contains(pos))
                    {
                        handled = true;
                        return (IntPtr)HitTest.HTLEFT;
                    }
                    else if (rightRect.Contains(pos))
                    {
                        handled = true;
                        return (IntPtr)HitTest.HTRIGHT;
                    }
                    else if (topRect.Contains(pos))
                    {
                        handled = true;
                        return (IntPtr)HitTest.HTTOP;
                    }
                    else if (bottomRect.Contains(pos))
                    {
                        handled = true;
                        return (IntPtr)HitTest.HTBOTTOM;
                    }
                    else if (captionRect.Contains(pos))
                    {
                        if ((result.VisualHit as CompactHeader) == null && (!ignoreButton || !Header.AllowWindowDragOnButtons) && !ignoreButtonDrag)
                        {
                            break;
                        }
                        handled = true;
                        return (IntPtr)HitTest.HTCAPTION;
                    }
                    else
                    {
                        Cursor = Cursors.Arrow;
                    }
                    // Consume all NCHITTEST so that resize grip doesn't take control
                    //handled = true;
                    break;

                case Native.WM_GETMINMAXINFO:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }

            return IntPtr.Zero;
        }

        private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            Native.MINMAXINFO mmi = (Native.MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(Native.MINMAXINFO));

            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = Native.MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != IntPtr.Zero)
            {
                Native.MONITORINFO monitorInfo = new Native.MONITORINFO();
                Native.GetMonitorInfo(monitor, monitorInfo);
                Native.RECT rcWorkArea = monitorInfo.rcWork;
                Native.RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);// -3;
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);// -3;
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);// +6;
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);// +6;
                mmi.ptMinTrackSize.x = (int)this.MinWidth;
                mmi.ptMinTrackSize.y = (int)this.MinHeight;
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }
    }
}
