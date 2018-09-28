using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocr
{
    public struct OcrRect : IEquatable<OcrRect>
    {
        private static OcrRect emptyRect = new OcrRect();

        public int X;
        public int Y;
        public int Width;
        public int Height;

        public int Left
        {
            get { return this.X; }
        }

        public int Right
        {
            get { return (this.X + this.Width); }
        }

        public int Top
        {
            get { return this.Y; }
        }

        public int Bottom
        {
            get { return (this.Y + this.Height); }
        }

        public static OcrRect Empty
        {
            get { return emptyRect; }
        }

        public OcrRect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public static bool operator ==(OcrRect a, OcrRect b)
        {
            return ((a.X == b.X) && (a.Y == b.Y) && (a.Width == b.Width) && (a.Height == b.Height));
        }

        public static bool operator !=(OcrRect a, OcrRect b)
        {
            return !(a == b);
        }

        public bool Equals(OcrRect other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return (obj is OcrRect) ? this == ((OcrRect)obj) : false;
        }

        public override string ToString()
        {
            return string.Format("{{X:{0} Y:{1} Width:{2} Height:{3}}}", X, Y, Width, Height);
        }

        public override int GetHashCode()
        {
            return (this.X ^ this.Y ^ this.Width ^ this.Height);
        }
    }
}
