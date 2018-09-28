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
    class CompactHeader : Canvas
    {
        private bool creatingLayout;

        private CompactButton iconButton;
        private CompactButton languageButton;
        private CompactButton layoutButton;
        private CompactButton ocrEngineButton;
        private CompactButton ocrEngineProfileButton;
        private CompactButton translateAPIButton;
        private CompactButton captureButton;
        private CompactButton captureAreaButton;
        private CompactButton captureAreaFrozenButton;
        private CompactButton captureAreaEditableButton;

        private CompactButton closeButton;
        private CompactButton minimizeButton;
        private CompactButton sizeControlsOverlay;
        private CompactButton sizeControlsFadeOverlay;

        private List<CompactButton> leftButtons = new List<CompactButton>();
        private List<CompactButton> rightButtons = new List<CompactButton>();

        public Color BackColor { get; set; }

        public int HeaderSize
        {
            get { return (int)Height; }
            set { Height = value; }
        }

        public string FontFamily { get; set; }
        public double FontSize { get; set; }
        public int TextPadding { get; set; }
        public Color TextColor { get; set; }
        public Color TextBackgroundColor { get; set; }
        public Color TextColorDeactivatedDraw { get; set; }

        public string ControlBoxFontFamily { get; set; }
        public double ControlBoxFontSize { get; set; }
        public Color ControlBoxImageColorOffset { get; set; }

        public int ControlBoxPaddingTop { get; set; }
        public int ControlBoxPaddingBottom { get; set; }
        public int ControlBoxPaddingLeft { get; set; }
        public int ControlBoxPaddingRight { get; set; }
        public int ControlBoxPaddingItem { get; set; }
        public int ControlBoxFadeSize { get; set; }

        public int ItemPadding { get; set; }
        public Brush ItemPaddingBrush { get; set; }
        public bool DrawItemPadding { get; set; }

        public int IconPadding { get; set; }
        public bool IconPaddingBorder { get; set; }
        public bool IconPaddingHalfBorder { get; set; }
        public bool IconPaddingHalf2Border { get; set; }

        public bool DrawDeactivated { get; set; }
        public bool IsActivatedDraw
        {
            get { return Window.IsActivated || !DrawDeactivated; }
        }

        public bool AllowWindowDragOnButtons { get; set; }
        public bool AllowBorderResizeOnButtons { get; set; }
        public bool AllowBorderResizeOnIcon { get; set; }
        public bool AllowBorderResizeOnControlBoxButton { get; set; }

        public WPFCompactForm Window { get; private set; }
        public bool AdditionalWindowDrag { get; set; }

        public CompactHeader(WPFCompactForm window)
        {
            Window = window;

            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            HeaderSize = 31;
            DrawItemPadding = true;
            ItemPadding = 1;
            //ItemPaddingBrush = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));

            FontFamily = "Arial";
            FontSize = 13;
            TextPadding = 6;
            TextColor = Colors.White;
            TextBackgroundColor = Color.FromArgb(25, 255, 255, 255);
            TextColorDeactivatedDraw = Colors.LightGray;

            ControlBoxFontFamily = "Arial";//Estrangelo Edessa
            ControlBoxFontSize = 15;
            //ControlBoxImageColorOffset = Colors.Tomato;

            ControlBoxPaddingTop = 3;
            ControlBoxPaddingBottom = 3;
            ControlBoxPaddingLeft = 3;
            ControlBoxPaddingRight = Window.BorderSize;
            ControlBoxFadeSize = 25;

            IconPaddingBorder = false;
            IconPaddingHalfBorder = false;
            IconPaddingHalf2Border = true;
            if (IconPaddingHalf2Border)
            {
                IconPadding = (int)(Window.BorderSize / 1.2f);
            }
            else if (IconPaddingHalfBorder)
            {
                IconPadding = Window.BorderSize / 2;
            }
            else if (IconPaddingBorder)
            {
                IconPadding = Window.BorderSize;
            }

            AllowBorderResizeOnButtons = true;
            AllowWindowDragOnButtons = false;
            AdditionalWindowDrag = true;

            //DrawDeactivated = true;

            Window.Activated += Window_Activated;
            Window.Deactivated += Window_Deactivated;

            creatingLayout = true;

            ///////////////////////////////////////////////////
            // Left controls
            ///////////////////////////////////////////////////

            iconButton = new CompactButton(this);
            iconButton.IsIconButton = true;
            iconButton.Click += iconButton_Click;
            leftButtons.Add(iconButton);
            Children.Add(iconButton);

            languageButton = new CompactButton(this);
            languageButton.Click += languageButton_Click;
            leftButtons.Add(languageButton);
            Children.Add(languageButton);

            layoutButton = new CompactButton(this);
            layoutButton.Click += layoutButton_Click;
            leftButtons.Add(layoutButton);
            Children.Add(layoutButton);

            ocrEngineButton = new CompactButton(this);
            ocrEngineButton.Click += ocrEngineButton_Click;
            leftButtons.Add(ocrEngineButton);
            Children.Add(ocrEngineButton);

            ocrEngineProfileButton = new CompactButton(this);
            ocrEngineProfileButton.Click += ocrEngineProfileButton_Click;
            leftButtons.Add(ocrEngineProfileButton);
            Children.Add(ocrEngineProfileButton);

            translateAPIButton = new CompactButton(this);
            translateAPIButton.Click += translateAPIButton_Click;
            leftButtons.Add(translateAPIButton);
            Children.Add(translateAPIButton);

            captureButton = new CompactButton(this);
            captureButton.Click += captureButton_Click;
            leftButtons.Add(captureButton);
            Children.Add(captureButton);

            captureAreaButton = new CompactButton(this);
            captureAreaButton.Click += captureAreaButton_Click;
            leftButtons.Add(captureAreaButton);
            Children.Add(captureAreaButton);

            captureAreaFrozenButton = new CompactButton(this);
            captureAreaFrozenButton.Click += captureAreaFrozenButton_Click;
            leftButtons.Add(captureAreaFrozenButton);
            Children.Add(captureAreaFrozenButton);

            captureAreaEditableButton = new CompactButton(this);
            captureAreaEditableButton.Click += captureAreaEditableButton_Click;
            leftButtons.Add(captureAreaEditableButton);
            Children.Add(captureAreaEditableButton);

            //iconButton.IsEnabled = false;
            //languageButton.IsEnabled = false;
            //layoutButton.IsEnabled = false;
            //ocrEngineButton.IsEnabled = false;
            //ocrEngineProfileButton.IsEnabled = false;
            translateAPIButton.IsEnabled = false;

            captureButton.IsEnabled = false;
            captureAreaButton.IsEnabled = false;
            captureAreaFrozenButton.IsEnabled = false;
            captureAreaEditableButton.IsEnabled = false;

            ///////////////////////////////////////////////////
            // Right controls
            ///////////////////////////////////////////////////

            sizeControlsOverlay = new CompactButton(this);
            sizeControlsOverlay.IsControlBoxOverlay = true;
            Children.Add(sizeControlsOverlay);
            Canvas.SetRight(sizeControlsOverlay, 0);

            sizeControlsFadeOverlay = new CompactButton(this);
            sizeControlsFadeOverlay.IsControlBoxFade = true;
            Children.Add(sizeControlsFadeOverlay);

            closeButton = new CompactButton(this);
            closeButton.IsControlBoxButton = true;
            closeButton.Click += closeButton_Click;
            rightButtons.Add(closeButton);
            Children.Add(closeButton);

            minimizeButton = new CompactButton(this);
            minimizeButton.IsControlBoxButton = true;
            minimizeButton.Click += minimizeButton_Click;
            rightButtons.Add(minimizeButton);
            Children.Add(minimizeButton);

            Canvas.SetZIndex(sizeControlsFadeOverlay, 999);
            Canvas.SetZIndex(sizeControlsOverlay, 1000);
            Canvas.SetZIndex(closeButton, 1001);
            Canvas.SetZIndex(minimizeButton, 1002);

            iconButton.Width += IconPadding * 2;
            iconButton.Image = Properties.Resources.Default27.ToBitmapImage();
            iconButton.DeactivatedImage = Properties.Resources.LostFocus27.ToBitmapImage();

            //minimizeButton.Text = "_";
            //minimizeButton.TextOffset = new Point(0, -2);
            //closeButton.Text = "X";
            //closeButton.TextOffset = new Point(0, 0);
            if (ControlBoxImageColorOffset != default(Color))
            {
                using (var closeImage = Properties.Resources.Close.ColorTint(ControlBoxImageColorOffset))
                {
                    closeButton.Image = closeImage.ToBitmapImage();
                }
                using (var minimizeImage = Properties.Resources.Minimize.ColorTint(ControlBoxImageColorOffset))
                {
                    minimizeButton.Image = minimizeImage.ToBitmapImage();
                }
            }
            else
            {
                closeButton.Image = Properties.Resources.Close.ToBitmapImage();
                minimizeButton.Image = Properties.Resources.Minimize.ToBitmapImage();
            }
            closeButton.HoverBackColor = Color.FromArgb(0xFF, 0xFC, 0x3A, 0x3A).DarkenBy(3);
            minimizeButton.HoverBackColor = Color.FromArgb(0xFF, 0x37, 0x9D, 0xE6);
            closeButton.PressBackColor = closeButton.HoverBackColor.DarkenBy(12);
            minimizeButton.PressBackColor = minimizeButton.HoverBackColor.DarkenBy(12);

            languageButton.Text = "Chinese (Traditional) - English";
            layoutButton.Text = "Translated";
            ocrEngineButton.Text = "Windows";
            ocrEngineProfileButton.Text = "1x Linear";

            creatingLayout = false;
            Layout();
        }

        void Window_Activated(object sender, EventArgs e)
        {
            foreach (UIElement child in Children)
                child.InvalidateVisual();
        }

        void Window_Deactivated(object sender, EventArgs e)
        {
            foreach (UIElement child in Children)
                child.InvalidateVisual();
        }

        void iconButton_Click(object sender, RoutedEventArgs e)
        {
            CreateContextMenu<MainContextMenu>(iconButton);
        }

        void languageButton_Click(object sender, RoutedEventArgs e)
        {
            CreateContextMenu<LanguageContextMenu>(languageButton);
        }

        void layoutButton_Click(object sender, RoutedEventArgs e)
        {
            CreateContextMenu<LayoutContextMenu>(layoutButton);
        }

        void ocrEngineButton_Click(object sender, RoutedEventArgs e)
        {
            CreateContextMenu<OcrEngineContextMenu>(ocrEngineButton);
        }

        void ocrEngineProfileButton_Click(object sender, RoutedEventArgs e)
        {
            CreateContextMenu<OcrEngineContextMenu>(ocrEngineProfileButton);
        }

        void translateAPIButton_Click(object sender, RoutedEventArgs e)
        {
            CreateContextMenu<TranslateContextMenu>(translateAPIButton);
        }

        void captureButton_Click(object sender, RoutedEventArgs e)
        {
            CreateContextMenu<CaptureContextMenu>(captureAreaButton);
        }

        void captureAreaButton_Click(object sender, RoutedEventArgs e)
        {
        }

        void captureAreaFrozenButton_Click(object sender, RoutedEventArgs e)
        {
        }

        void captureAreaEditableButton_Click(object sender, RoutedEventArgs e)
        {
        }

        void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Window.Close();
        }

        void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Window.WindowState = WindowState.Minimized;
        }

        private T CreateContextMenu<T>(CompactButton target) where T : CompactContextMenu, new()
        {
            T contextMenu = new T();
            contextMenu.Window = Window;
            contextMenu.PlacementTarget = target;
            contextMenu.Placement = PlacementMode.Bottom;
            //contextMenu.Placement = PlacementMode.Relative;
            //contextMenu.PlacementRectangle = new Rect(iconButton.Width, iconButton.Height, 100, 100);
            contextMenu.Build();
            if (contextMenu.HasItems)
                contextMenu.IsOpen = true;
            return contextMenu;
        }

        public void Layout()
        {
            if (creatingLayout)
            {
                return;
            }

            creatingLayout = true;

            foreach (CompactButton button in leftButtons)
            {
                if (!button.IsIconButton && string.IsNullOrEmpty(button.Text))
                {
                    button.IsEnabled = false;
                }
            }

            int xOffset = 0;
            foreach (CompactButton button in leftButtons)
            {
                if (!button.IsEnabled)
                    continue;

                if (!button.IsIconButton && !button.IsControlBoxButton)
                {
                    button.BackColor = TextBackgroundColor;
                }

                Canvas.SetLeft(button, xOffset);
                xOffset += (int)button.Width;
                if (button != iconButton)
                    xOffset += ItemPadding;
            }

            xOffset = Window.BorderSize;
            foreach (CompactButton button in rightButtons)
            {
                button.Height = HeaderSize - ControlBoxPaddingTop - ControlBoxPaddingBottom;
                button.Width = button.Height;
                Canvas.SetTop(button, ControlBoxPaddingTop);
                Canvas.SetRight(button, xOffset);
                xOffset += (int)((int)button.Width + ControlBoxPaddingItem);
            }

            sizeControlsOverlay.Width = xOffset + ControlBoxPaddingLeft;
            sizeControlsOverlay.BackColor = BackColor;

            sizeControlsFadeOverlay.Width = ControlBoxFadeSize;
            sizeControlsFadeOverlay.BackColor = BackColor;
            Canvas.SetRight(sizeControlsFadeOverlay, xOffset + ControlBoxPaddingLeft);

            foreach (UIElement child in Children)
                child.InvalidateVisual();

            creatingLayout = false;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if (DrawItemPadding)
            {
                int xOffset = 0;
                foreach (CompactButton button in leftButtons)
                {
                    if (!button.IsEnabled)
                        continue;

                    xOffset += (int)button.Width;
                    if (button != iconButton)
                    {
                        dc.DrawRectangle(ItemPaddingBrush, null, new Rect(xOffset, 0, ItemPadding, HeaderSize));
                        xOffset += ItemPadding;
                    }
                }
            }
        }
    }
}
