using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ocr.UI
{
    class SnippingTool : Window
    {
        //http://stackoverflow.com/questions/3123776/net-equivalent-of-snipping-tool
        //http://stackoverflow.com/questions/4005910/make-net-snipping-tool-compatible-with-multiple-monitors
        //http://stackoverflow.com/questions/22538140/highlight-effect-like-snipping-tool

        //cursor helper
        //http://stackoverflow.com/a/9078166
        //http://stackoverflow.com/a/2837158

        private Color backgroundColor;
        private Color borderColor;
        private bool borderDashed;

        public SnippingMode Mode { get; private set; }        
        public System.Drawing.Rectangle Selection { get; set; }

        private System.Drawing.Image image;
        public System.Drawing.Image Image
        {
            get
            {
                return image;
            }
            set
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }
                image = value;

                if (image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = ms;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                        ImageSource.Source = bitmapImage;
                    }
                }                
            }
        }

        public Image ImageSource { get; private set; }
        
        public bool Result { get; set; }
        public bool IsClosed { get; set; }

        public SnippingTool(SnippingMode mode, System.Drawing.Image image)
        {
            ImageSource = new Image();
            Mode = mode;
            Image = image;

            backgroundColor = ParseColor(Program.Settings.CaptureAreaStyle.Back);
            borderColor = ParseColor(Program.Settings.CaptureAreaStyle.Border);
            borderDashed = Program.Settings.CaptureAreaStyle.DashedBorder;

            Initialize();
        }

        private Color ParseColor(string value)
        {
            try
            {
                if (value != null)
                {
                    string[] splitted = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (splitted.Length > 2)
                    {
                        byte b1 = splitted.Length >= 1 ? byte.Parse(splitted[0]) : (byte)0;
                        byte b2 = splitted.Length >= 2 ? byte.Parse(splitted[1]) : (byte)0;
                        byte b3 = splitted.Length >= 3 ? byte.Parse(splitted[2]) : (byte)0;
                        byte b4 = splitted.Length >= 4 ? byte.Parse(splitted[3]) : (byte)0;
                        return Color.FromArgb(b1, b2, b3, b4);
                    }
                    if(value.StartsWith("#"))
                    {
                        value = value.Substring(1);
                    }
                    byte[] bytes = BitConverter.GetBytes(int.Parse(value, NumberStyles.HexNumber));
                    return Color.FromArgb(bytes[0], bytes[1], bytes[2], bytes[3]);
                }
            }
            catch
            {                
            }
            return default(Color);
        }

        public System.Drawing.Image GetSlection()
        {
            return GetSelection(Image, Selection);
        }

        public System.Drawing.Image GetSelection(System.Drawing.Rectangle selection)
        {
            return GetSelection(Image, selection);
        }

        public static System.Drawing.Image GetSelection(System.Drawing.Image image, System.Drawing.Rectangle selection)
        {
            var result = new System.Drawing.Bitmap(selection.Width, selection.Height);
            using (var gr = System.Drawing.Graphics.FromImage(result))
            {
                gr.DrawImage(image, new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
                    selection, System.Drawing.GraphicsUnit.Pixel);
            }
            return result;
        }

        public static System.Drawing.Bitmap CreateScreenshot()
        {
            var rc = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            var bmp = new System.Drawing.Bitmap(rc.Width, rc.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using (var gr = System.Drawing.Graphics.FromImage(bmp))
                gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
            return bmp;
        }

        public static bool Snip(SnippingMode mode, out System.Drawing.Image image, out System.Drawing.Rectangle selection)
        {
            SnippingTool snippingTool = new SnippingTool(mode, CreateScreenshot());
            System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(snippingTool);
            snippingTool.ShowDialog();
            image = snippingTool.Image;
            selection = snippingTool.Selection;
            return snippingTool.Result;
        }

        private void Initialize()
        {
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip;
            this.WindowState = System.Windows.WindowState.Maximized;
            this.ShowInTaskbar = false;
            this.AllowsTransparency = true;
            this.Background = Brushes.Transparent;
            this.SnapsToDevicePixels = true;
            this.Loaded += SnippingTool_Loaded;
            this.Closing += SnippingTool_Closing;
            this.Closed += SnippingTool_Closed;

            Container container = new Container(this);
            container.Background = new SolidColorBrush(Color.FromArgb(1, 255, 255, 255));
            container.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            container.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            //container.Cursor = Cursors.Cross;
            container.Cursor = CreateCursor(Properties.Resources.PaintCursor, 10, 10);
            AddChild(container);
        }        

        void SnippingTool_Loaded(object sender, RoutedEventArgs e)
        {
            this.Deactivated += SnippingTool_Deactivated;
            this.Topmost = true;
        }

        void SnippingTool_Deactivated(object sender, EventArgs e)
        {
            if (IsClosed)
            {
                return;
            }

            try
            {
                Close();
            }
            catch
            {
            }            
        }

        void SnippingTool_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IsClosed = true;
        }

        void SnippingTool_Closed(object sender, EventArgs e)
        {
            if (Mode != SnippingMode.Freeze)
            {
                if (Image != null)
                {
                    Image.Dispose();
                    Image = null;
                }
                Image = CreateScreenshot();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }            
            base.OnKeyDown(e);
        }

        class Container : DockPanel
        {
            private System.Drawing.Point dragStart;
            private bool dragging;

            public SnippingTool snippingTool;

            public Container(SnippingTool snippingTool)
            {
                this.snippingTool = snippingTool;
            }

            protected override void OnMouseDown(MouseButtonEventArgs e)
            {
                if (e.ChangedButton != MouseButton.Left)
                    return;

                var pos = this.PointToScreen(e.GetPosition(this));
                dragStart = new System.Drawing.Point((int)pos.X, (int)pos.Y);
                dragging = true;
                UpdateSelection((int)pos.X, (int)pos.Y);
                this.InvalidateVisual();
                base.OnMouseDown(e);
            }

            protected override void OnMouseUp(MouseButtonEventArgs e)
            {
                if (e.ChangedButton != MouseButton.Left && e.ChangedButton != MouseButton.Right)
                    return;

                var pos = this.PointToScreen(e.GetPosition(this));
                UpdateSelection((int)pos.X, (int)pos.Y);

                snippingTool.Result = e.ChangedButton == MouseButton.Left;
                snippingTool.Close();
                base.OnMouseUp(e);
            }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                if (!dragging)
                    return;

                var pos = this.PointToScreen(e.GetPosition(this));
                UpdateSelection((int)pos.X, (int)pos.Y);
                this.InvalidateVisual();
                base.OnMouseMove(e);
            }

            private void UpdateSelection(int x, int y)
            {
                int x1 = Math.Min(x, dragStart.X);
                int y1 = Math.Min(y, dragStart.Y);
                int x2 = Math.Max(x, dragStart.X);
                int y2 = Math.Max(y, dragStart.Y);
                snippingTool.Selection = new System.Drawing.Rectangle(x1, y1, x2 - x1, y2 - y1);
            }

            protected override void OnRender(DrawingContext dc)
            {
                // Background defaults
                Color color = Colors.White;
                byte transparancy = 50;

                Color borderColor = snippingTool.borderColor == default(Color) ? Colors.Black : snippingTool.borderColor;
                Color backgroundColor = snippingTool.backgroundColor == default(Color) ? 
                    Color.FromArgb(transparancy, color.R, color.G, color.B) : snippingTool.backgroundColor;

                Point offset = this.PointFromScreen(new Point(0, 0));
                Rect rect = new Rect(
                    snippingTool.Selection.X + (int)offset.X, snippingTool.Selection.Y + (int)offset.Y,
                    snippingTool.Selection.Width, snippingTool.Selection.Height);

                if (snippingTool.Mode == SnippingMode.Freeze && snippingTool.Image != null)
                    dc.DrawImage(snippingTool.ImageSource.Source, new Rect(offset.X, offset.Y, snippingTool.Image.Width, snippingTool.Image.Height));
                
                Brush br = new SolidColorBrush(backgroundColor);
                int x1 = (int)rect.X; int x2 = (int)(rect.X + rect.Width);
                int y1 = (int)rect.Y; int y2 = (int)(rect.Y + rect.Height);
                dc.DrawRectangle(br, null, new Rect(0, 0, x1, this.ActualHeight));
                dc.DrawRectangle(br, null, new Rect(x2, 0, this.ActualWidth - x2, this.ActualHeight));
                dc.DrawRectangle(br, null, new Rect(x1, 0, x2 - x1, y1));
                dc.DrawRectangle(br, null, new Rect(x1, y2, x2 - x1, this.ActualHeight - y2));
                if (dragging)
                {
                    DrawRectangle(dc, borderColor, rect);
                }
                base.OnRender(dc);
            }

            private void DrawRectangle(DrawingContext dc, Color color, Rect rect)
            {
                Pen pen = new Pen(new SolidColorBrush(color), 1);
                if (snippingTool.borderDashed)
                {
                    pen.DashStyle = DashStyles.Dash;
                }

                double halfPenWidth = pen.Thickness / 2;

                // Create a guidelines set
                GuidelineSet guidelines = new GuidelineSet();
                guidelines.GuidelinesX.Add(rect.Left + halfPenWidth);
                guidelines.GuidelinesX.Add(rect.Right + halfPenWidth);
                guidelines.GuidelinesY.Add(rect.Top + halfPenWidth);
                guidelines.GuidelinesY.Add(rect.Bottom + halfPenWidth);

                dc.PushGuidelineSet(guidelines);
                dc.DrawRectangle(null, pen, rect);
                dc.Pop();
            }
        }        

        public static Cursor CreateCursor(System.Drawing.Bitmap bmp, int xHotSpot, int yHotSpot)
        {            
            using (var stream = new MemoryStream())
            {
                // Save to .ico format
                using (System.Drawing.Icon icon = IconFromImage(bmp))
                    icon.Save(stream);

                // Convert saved file into .cur format
                stream.Seek(2, SeekOrigin.Begin);
                stream.WriteByte(2);
                stream.Seek(10, SeekOrigin.Begin);
                stream.WriteByte((byte)xHotSpot);
                stream.Seek(12, SeekOrigin.Begin);
                stream.WriteByte((byte)yHotSpot);
                stream.Seek(0, SeekOrigin.Begin);

                //File.WriteAllBytes("C:/PaintCursor.cur", stream.ToArray());

                // Construct Cursor
                return new Cursor(stream);
            }
        }

        //http://stackoverflow.com/a/21389253
        public static System.Drawing.Icon IconFromImage(System.Drawing.Image img)
        {
            var ms = new System.IO.MemoryStream();
            var bw = new System.IO.BinaryWriter(ms);
            // Header
            bw.Write((short)0);   // 0 : reserved
            bw.Write((short)1);   // 2 : 1=ico, 2=cur
            bw.Write((short)1);   // 4 : number of images
            // Image directory
            var w = img.Width;
            if (w >= 256) w = 0;
            bw.Write((byte)w);    // 0 : width of image
            var h = img.Height;
            if (h >= 256) h = 0;
            bw.Write((byte)h);    // 1 : height of image
            bw.Write((byte)0);    // 2 : number of colors in palette
            bw.Write((byte)0);    // 3 : reserved
            bw.Write((short)0);   // 4 : number of color planes
            bw.Write((short)0);   // 6 : bits per pixel
            var sizeHere = ms.Position;
            bw.Write((int)0);     // 8 : image size
            var start = (int)ms.Position + 4;
            bw.Write(start);      // 12: offset of image data
            // Image data
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var imageSize = (int)ms.Position - start;
            ms.Seek(sizeHere, System.IO.SeekOrigin.Begin);
            bw.Write(imageSize);
            ms.Seek(0, System.IO.SeekOrigin.Begin);

            // And load it
            return new System.Drawing.Icon(ms);
        }
    }

    public enum SnippingMode
    {
        Default,
        Freeze,
        Editable
    }
}
