using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Ocr.UI
{
    static class Extensions
    {
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        public static Rect CenterTo(this Rect rect, Rect other, bool decimalUnits = true)
        {
            double differenceX = other.Width - rect.Width;
            double differenceY = other.Height - rect.Height;
            double halfDifferenceX = differenceX / 2.0;
            double halfDifferenceY = differenceY / 2.0;
            if (decimalUnits)
            {
                return new Rect(other.X + halfDifferenceX, other.Y + halfDifferenceY, rect.Width, rect.Height);
            }
            else
            {
                return new Rect((int)(other.X + halfDifferenceX), (int)(other.Y + halfDifferenceY), (int)rect.Width, (int)rect.Height);
            }
        }

        public static System.Windows.Media.Color LightenBy(this System.Windows.Media.Color color, double percent)
        {
            return LightenBy(color, (int)(percent * 100));
        }

        public static System.Windows.Media.Color LightenBy(this System.Windows.Media.Color color, int percent)
        {
            return color.Lerp(System.Windows.Media.Colors.White, percent / 100.0f);
            //return ChangeColorBrightness(color, percent / 100.0f);
        }

        public static System.Windows.Media.Color DarkenBy(this System.Windows.Media.Color color, double percent)
        {
            return DarkenBy(color, (int)(percent * 100));
        }

        public static System.Windows.Media.Color DarkenBy(this System.Windows.Media.Color color, int percent)
        {
            return color.Lerp(System.Windows.Media.Colors.Black, percent / 100.0f);
            //return ChangeColorBrightness(color, -1 * percent / 100.0f);
        }

        private static System.Windows.Media.Color ChangeColorBrightness(System.Windows.Media.Color color, float amount)
        {
            float red = Math.Min(255, Math.Max(0, (255 - color.R) * amount + color.R));
            float green = Math.Min(255, Math.Max(0, (255 - color.G) * amount + color.G));
            float blue = Math.Min(255, Math.Max(0, (255 - color.B) * amount + color.B));
            return System.Windows.Media.Color.FromArgb((byte)color.A, (byte)red, (byte)green, (byte)blue);
        }

        public static System.Windows.Media.Color Lerp(this System.Windows.Media.Color colour, System.Windows.Media.Color to, float amount)
        {
            // start colours as lerp-able floats
            float sa = colour.A, sr = colour.R, sg = colour.G, sb = colour.B;

            // end colours as lerp-able floats
            float ea = to.A, er = to.R, eg = to.G, eb = to.B;

            // lerp the colours to get the difference
            byte a = (byte)sa.Lerp(ea, amount),
                 r = (byte)sr.Lerp(er, amount),
                 g = (byte)sg.Lerp(eg, amount),
                 b = (byte)sb.Lerp(eb, amount);

            // return the new colour
            return System.Windows.Media.Color.FromArgb(colour.A, r, g, b);
        }

        public static float Lerp(this float start, float end, float amount)
        {
            float difference = end - start;
            float adjusted = difference * amount;
            return start + adjusted;
        }

        public static Bitmap ColorTint(this Bitmap sourceBitmap, System.Windows.Media.Color color)
        {
            return ColorTint(sourceBitmap, color.R, color.G, color.B);
        }

        public static Bitmap ColorTint(this Bitmap sourceBitmap, Color color)
        {
            return ColorTint(sourceBitmap, color.R, color.G, color.B);
        }

        public static Bitmap ColorTint(this Bitmap sourceBitmap, float redTint, float greenTint, float blueTint)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                    sourceBitmap.Width, sourceBitmap.Height),
                                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            float redTintPercent = redTint <= 0 ? 0 : redTint / 255.0f;
            float greenTintPercent = greenTint <= 0 ? 0 : greenTint / 255.0f;
            float blueTintPercent = blueTint <= 0 ? 0 : blueTint / 255.0f;

            float blue = 0;
            float green = 0;
            float red = 0;

            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                //blue = pixelBuffer[k] + (255 - pixelBuffer[k]) * blueTint;
                //green = pixelBuffer[k + 1] + (255 - pixelBuffer[k + 1]) * greenTint;
                //red = pixelBuffer[k + 2] + (255 - pixelBuffer[k + 2]) * redTint;
                red = pixelBuffer[k + 2] * redTintPercent;
                green = pixelBuffer[k + 1] * greenTintPercent;
                blue = pixelBuffer[k + 0] * blueTintPercent;

                blue = Math.Min(255, Math.Max(0, blue));
                green = Math.Min(255, Math.Max(0, green));
                red = Math.Min(255, Math.Max(0, red));

                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }        

        public static int AutoDropDownWidth(this System.Windows.Forms.ComboBox myCombo)
        {
            return AutoDropDownWidth<object>(myCombo, o => o.ToString());
        }
        public static int AutoDropDownWidth<T>(this System.Windows.Forms.ComboBox myCombo, Func<T, string> description)
        {
            int maxWidth = 1;
            int temp = 1;
            int vertScrollBarWidth = (myCombo.Items.Count > myCombo.MaxDropDownItems)
                    ? System.Windows.Forms.SystemInformation.VerticalScrollBarWidth : 0;

            foreach (T obj in myCombo.Items)
            {
                if (obj is T)
                {
                    T t = (T)obj;
                    temp = System.Windows.Forms.TextRenderer.MeasureText(description(t), myCombo.Font).Width;
                    if (temp > maxWidth)
                    {
                        maxWidth = temp;
                    }
                }

            }
            return maxWidth + vertScrollBarWidth;
        }

        public static System.Drawing.Point Center(this System.Drawing.Rectangle rect)
        {
            return new System.Drawing.Point(rect.Left + rect.Width / 2,
                             rect.Top + rect.Height / 2);
        }

        public static System.Drawing.PointF Center(this System.Drawing.RectangleF rect)
        {
            return new System.Drawing.PointF(rect.Left + rect.Width / 2,
                             rect.Top + rect.Height / 2);
        }
    }
}
