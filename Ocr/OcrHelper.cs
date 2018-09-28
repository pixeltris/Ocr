using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ocr
{
    class OcrHelper
    {
        public static event OcrCompleteHandler Complete;
        public static event OcrStartHandler Start;

        private static object locker = new object();
        private static Thread thread = null;

        private static int idleWait = 50;

        private static byte[] currentImage;
        private static byte[] nextImage;

        static OcrHelper()
        {
            thread = new Thread(delegate()
            {
                byte[] image = null;
                OcrResult result = null;

                while (Program.Running)
                {
                    image = null;
                    result = null;
             
                    lock (locker)
                    {
                        image = currentImage;
                    }

                    if (image != null)
                    {
                        string path = GetTempPath(".png");

                        try
                        {
                            if (Start != null)
                            {
                                Start();
                            }
                        }
                        catch
                        {
                        }

                        try
                        {                            
                            File.WriteAllBytes(path, currentImage);
                            result = Program.ActiveOcrEngine.LoadFile(path, Program.ActiveLanguageFrom);
                        }
                        finally
                        {
                            try
                            {
                                File.Delete(path);
                            }
                            catch
                            {
                            }
                        }
                    }

                    if (image != null && result != null && Complete != null)
                    {
                        Complete(result);
                    }

                    lock (locker)
                    {
                        if (image != null)
                        {
                            currentImage = nextImage;
                            nextImage = null;
                        }
                    }

                    Thread.Sleep(idleWait);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        public static void Ocr(Image image)
        {
            byte[] buffer = null;
            try
            {
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Png);
                    buffer = ms.ToArray();
                }
            }
            catch
            {
            }

            if (buffer == null)
            {
                return;
            }

            lock (locker)
            {                
                if (currentImage == null)
                {
                    currentImage = buffer;
                }
                else
                {
                    nextImage = buffer;
                }
            }
        }

        public static string GetTempPath(string extension)
        {
            return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + extension);
        }
    }

    public delegate void OcrCompleteHandler(OcrResult result);
    public delegate void OcrStartHandler();
}
