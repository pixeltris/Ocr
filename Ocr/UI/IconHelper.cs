using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Ocr.UI
{
    class IconHelper
    {
        public static bool ConvertToIcon(Stream input, Stream output)
        {
            return ConvertToIcon(new Stream[] { input }, output, new int[] { 16 }, false);
        }

        public static bool ConvertToIcon(Stream[] input, Stream output, int[] sizes, bool preserveAspectRatio)
        {
            if (input.Length == 0 || input.Length != sizes.Length)
                return false;

            Bitmap[] inputBitmaps = new Bitmap[input.Length];
            Bitmap[] newBitmaps = new Bitmap[input.Length];
            MemoryStream[] savedNewBitmap = new MemoryStream[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                inputBitmaps[i] = ((Bitmap)Bitmap.FromStream(input[i]));

                if (inputBitmaps[i] != null)
                {
                    int width, height;
                    if (preserveAspectRatio)
                    {
                        width = sizes[i];
                        height = inputBitmaps[i].Height / inputBitmaps[i].Width * sizes[i];
                    }
                    else
                    {
                        width = height = sizes[i];
                    }

                    newBitmaps[i] = new Bitmap(inputBitmaps[i], new Size(width, height));
                    if (newBitmaps[i] != null)
                    {
                        savedNewBitmap[i] = new MemoryStream();
                        newBitmaps[i].Save(savedNewBitmap[i], ImageFormat.Png);
                    }
                }
            }
            bool isValid = !inputBitmaps.Contains(null);

            if (isValid)
            {
                BinaryWriter iconWriter = new BinaryWriter(output);
                if (output != null && iconWriter != null)
                {
                    // 0-1 reserved, 0
                    iconWriter.Write((byte)0);
                    iconWriter.Write((byte)0);

                    // 2-3 image type, 1 = icon, 2 = cursor
                    iconWriter.Write((short)1);

                    // 4-5 number of images
                    iconWriter.Write((short)inputBitmaps.Length);

                    for (int i = 0; i < input.Length; i++)
                    {
                        // image entry 1
                        // 0 image width
                        iconWriter.Write((byte)newBitmaps[i].Width);
                        // 1 image height
                        iconWriter.Write((byte)newBitmaps[i].Height);

                        // 2 number of colors
                        iconWriter.Write((byte)0);

                        // 3 reserved
                        iconWriter.Write((byte)0);

                        // 4-5 color planes
                        iconWriter.Write((short)0);

                        // 6-7 bits per pixel
                        iconWriter.Write((short)32);

                        // 8-11 size of image data
                        iconWriter.Write((int)savedNewBitmap[i].Length);

                        int offset = 6 + (16 * input.Length);
                        for (int j = 0; j < i; j++)
                        {
                            offset += (int)savedNewBitmap[j].Length;
                        }

                        // 12-15 offset of image data
                        iconWriter.Write((int)(offset));
                    }

                    for (int i = 0; i < input.Length; i++)
                    {
                        // write image data
                        // png data must contain the whole png data file
                        iconWriter.Write(savedNewBitmap[i].ToArray());
                    }

                    // Flush all of the written data
                    iconWriter.Flush();
                }
            }

            for (int i = 0; i < input.Length; i++)
            {
                if (inputBitmaps[i] != null) inputBitmaps[i].Dispose();
                if (newBitmaps[i] != null) newBitmaps[i].Dispose();
                if (savedNewBitmap[i] != null) savedNewBitmap[i].Close();
            }

            return isValid;
        }

        public static bool ConvertToIcon(string inputPath, string outputPath)
        {
            return ConvertToIcon(inputPath, outputPath, 16, false);
        }

        public static bool ConvertToIcon(string inputPath, string outputPath, int size, bool preserveAspectRatio)
        {
            using (FileStream inputStream = new FileStream(inputPath, FileMode.Open))
            using (FileStream outputStream = new FileStream(outputPath, FileMode.OpenOrCreate))
            {
                return ConvertToIcon(new Stream[] { inputStream }, outputStream, new int[] { size }, preserveAspectRatio);
            }
        }

        public static bool ConvertToIcon(string outputPath, string[] inputPaths, int[] sizes)
        {
            bool valid = false;

            if (sizes.Length != inputPaths.Length)
                return valid;

            List<FileStream> inputStreams = new List<FileStream>();
            foreach (string path in inputPaths)
            {
                inputStreams.Add(File.OpenRead(path));
            }

            using (FileStream outputStream = new FileStream(outputPath, FileMode.OpenOrCreate))
            {
                valid = ConvertToIcon(inputStreams.ToArray(), outputStream, sizes, false);
            }

            foreach (FileStream stream in inputStreams)
            {
                stream.Close();
            }

            return valid;
        }

        public static Icon IconFromFile(string inputPath)
        {
            using (FileStream inputStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read))
            using (MemoryStream ms = new MemoryStream())
            {
                if (ConvertToIcon(inputStream, ms))
                {
                    ms.Position = 0;
                    return new Icon(ms);
                }
                return null;
            }
        }

        public static Icon IconFromFiles(string[] inputPaths, int[] sizes)
        {
            Icon icon = null;

            if (sizes.Length != inputPaths.Length)
                return icon;

            List<FileStream> inputStreams = new List<FileStream>();
            foreach (string path in inputPaths)
            {
                inputStreams.Add(File.OpenRead(path));
            }

            using (MemoryStream ms = new MemoryStream())
            {
                if (ConvertToIcon(inputStreams.ToArray(), ms, sizes, false))
                {
                    ms.Position = 0;
                    icon = new Icon(ms);
                }
            }

            foreach (FileStream stream in inputStreams)
            {
                stream.Close();
            }

            return icon;
        }
    }
}
