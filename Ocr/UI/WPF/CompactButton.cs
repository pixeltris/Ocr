using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ocr.UI
{
    class CompactButton : ButtonBase
    {
        private bool isHover = false;
        private bool isHeld = false;

        private WindowDragger windowDrag;

        public Color BackColor { get; set; }
        public Color HoverBackColor { get; set; }
        public Color PressBackColor { get; set; }

        public BitmapImage Image { get; set; }
        public BitmapImage HoverImage { get; set; }
        public BitmapImage PressImage { get; set; }

        public BitmapImage DeactivatedImage { get; set; }
        public bool IsIconButton { get; set; }
        public bool IsControlBoxButton { get; set; }
        public bool IsControlBoxOverlay { get; set; }
        public bool IsControlBoxFade { get; set; }

        private FormattedText formattedText;
        private FormattedText formattedTextDeactivated;

        public Point TextOffset { get; set; }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                FontFamily fontFamily = new FontFamily(IsControlBoxButton ? Header.ControlBoxFontFamily : Header.FontFamily);
                FontStyle fontStyle = IsControlBoxButton ? FontStyles.Normal : FontStyles.Normal;
                FontWeight fontWeight = IsControlBoxButton ? FontWeights.Bold : FontWeights.Normal;
                FontStretch fontStretch = IsControlBoxButton ? FontStretches.Normal : FontStretches.Normal;
                Typeface typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
                double fontSize = IsControlBoxButton ? Header.ControlBoxFontSize : Header.FontSize;

                text = value;
                formattedText = new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontSize, new SolidColorBrush(Header.TextColor));
                if (Header.TextColorDeactivatedDraw != default(Color))
                {
                    formattedTextDeactivated = new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontSize, new SolidColorBrush(Header.TextColorDeactivatedDraw));
                }
                else
                {
                    formattedTextDeactivated = formattedText;
                }
                if (!IsControlBoxButton)
                {
                    Width = (int)(formattedText.Width + (Header.TextPadding * 2));// Padding goes on each side
                }
                Header.Layout();
            }
        }

        public CompactHeader Header { get; private set; }

        public CompactButton(CompactHeader header)
        {
            Header = header;
            Width = header.HeaderSize;
            Height = header.HeaderSize;
            windowDrag = new WindowDragger(this, header.Window);
            /*Random rand = new Random(Environment.TickCount + GetHashCode());
            byte[] buff = new byte[3];
            rand.NextBytes(buff);
            BackColor = Color.FromArgb(255, buff[0], buff[1], buff[2]);*/

            header.Window.Deactivated += Window_Deactivated;
        }

        void Window_Deactivated(object sender, EventArgs e)
        {
            isHeld = false;
            windowDrag.Deactivated();
            this.InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (!IsEnabled)
                return;

            Rect bounds = new Rect(0, 0, (int)Width, (int)Height);

            Color backColor = BackColor;
            if (isHeld && isHover)
            {
                backColor = PressBackColor;
            }
            else if (isHeld || (!isHeld && isHover))
            {
                backColor = HoverBackColor;
            }
            if (backColor == default(Color))
            {
                backColor = BackColor;
            }

            drawingContext.PushClip(new RectangleGeometry(bounds));
            //drawingContext.DrawRectangle(Header.Background, new Pen(), bounds);
            if (!IsIconButton)
            {
                Brush brush = null;
                if (IsControlBoxFade)
                {
                    brush = new LinearGradientBrush(Color.FromArgb(0, BackColor.R, BackColor.G, BackColor.B), BackColor, 0);
                }
                else
                {
                    brush = new SolidColorBrush(backColor);
                }
                drawingContext.DrawRectangle(brush, new Pen(), bounds);
            }

            BitmapImage image = Header.IsActivatedDraw ? Image : DeactivatedImage == null ? Image : DeactivatedImage;

            if (image != null)
            {
                Rect imageRect = new Rect(0, 0, Math.Min(image.PixelWidth, (int)bounds.Width), Math.Min(image.PixelHeight, (int)bounds.Height));
                Rect centered = imageRect.CenterTo(bounds, false);
                drawingContext.DrawImage(image, centered);
            }

            // Possible source for different text rendering
            // http://stackoverflow.com/a/33953602

            if (formattedText != null)
            {
                Point textPos = new Point((int)((bounds.Width - formattedText.Width) / 2.0f),
                    (int)((bounds.Height - formattedText.Height) / 2.0f));
                if (Header.IsActivatedDraw)
                    drawingContext.DrawText(formattedText, new Point(textPos.X + TextOffset.X, textPos.Y + TextOffset.Y));
                else
                    drawingContext.DrawText(formattedTextDeactivated, new Point(textPos.X + TextOffset.X, textPos.Y + TextOffset.Y));
            }

            drawingContext.Pop();
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            isHover = true;
            this.InvalidateVisual();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            isHover = false;
            this.InvalidateVisual();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            isHeld = true;
            this.InvalidateVisual();
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (Header.AdditionalWindowDrag)
                windowDrag.MouseDown(e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            windowDrag.MouseUp(e);            
            isHeld = false;
            this.InvalidateVisual();
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (isHeld)
            {
                Rect bounds = new Rect(0, 0, (int)Width, (int)Height);
                if (!bounds.Contains(e.GetPosition(this)))
                {
                    isHover = false;
                }
                else
                {
                    isHover = true;
                }
                this.InvalidateVisual();
            }
            windowDrag.MouseMove(e);
            /*Random rand = new Random(Environment.TickCount + GetHashCode());
            byte[] buff = new byte[3];
            rand.NextBytes(buff);
            BackColor = Color.FromArgb(255, buff[0], buff[1], buff[2]);
            this.InvalidateVisual();*/

            base.OnMouseMove(e);
        }
    }
}
