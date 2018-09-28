using System;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Windows.Media.Ocr;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.Globalization;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;

namespace WindowsOcr
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                    return;

                string language = args[0];
                string file = args[1];
                string outputFile = args.Length < 3 ? "out.txt" : args[2];
				
				bool spacing = true;
				bool newLines = true;
				foreach(string arg in args)
				{
					if(string.IsNullOrEmpty(arg))
					{
						continue;
					}
					switch(arg.Trim())
					{
						case "--nospacing":
							spacing = false;
							break;
						case "--nolines":
							spacing = false;
							break;
					}
				}

                string result = RunOcrTests(language, file, spacing, newLines);
                if (!string.IsNullOrEmpty(result))
                {
                    File.WriteAllText(outputFile, result);
                }
            }
            catch
            {
            }
        }

        static void PrintLanguages()
        {
            foreach (Language language in OcrEngine.AvailableRecognizerLanguages.OrderBy(x => x.DisplayName))
            {
                System.Diagnostics.Debug.WriteLine("AddSupportedLanguage(\"\", \"" + language.LanguageTag + "\");//" + language.DisplayName + "");
            }
        }

        static string RunOcrTests(string language, string file, bool spacing, bool newLines)
        {
            Language lang = null;
            foreach (Language ocrLanguage in OcrEngine.AvailableRecognizerLanguages)
            {
                if (ocrLanguage.LanguageTag.Equals(language, StringComparison.OrdinalIgnoreCase))
                {
                    lang = ocrLanguage;
                }
            }

            if (lang == null)
            {
                return null;
            }

            using (var image = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(file))
            {
                IBuffer buffer = WindowsRuntimeBufferExtensions.AsBuffer(BitmapToByteArray(image));
                using (SoftwareBitmap bitmap = SoftwareBitmap.CreateCopyFromBuffer(buffer, BitmapPixelFormat.Rgba8, image.Width, image.Height))
                {
                    OcrEngine engine = OcrEngine.TryCreateFromLanguage(lang);
                    var result = engine.RecognizeAsync(bitmap);
                    Task.WaitAll(result.AsTask<OcrResult>());

                    string extractedText = string.Empty;
                    OcrResult ocrResult = result.GetResults();
                    if (ocrResult != null && ocrResult.Lines != null)
                    {
                        foreach (OcrLine line in ocrResult.Lines)
                        {
                            foreach (OcrWord word in line.Words)
                            {
                                extractedText += word.Text + (spacing ? " " : string.Empty);
                            }
							extractedText = extractedText.TrimEnd();
							if(newLines)
							{
								extractedText += Environment.NewLine;
							}
                        }
                    }
                    return extractedText.TrimEnd();
                }
            }
        }

        static byte[] BitmapToByteArray(System.Drawing.Bitmap bitmap)
        {
            System.Drawing.Imaging.BitmapData bmpdata = null;
            try
            {
                bmpdata = bitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                int numbytes = bmpdata.Stride * bitmap.Height;
                byte[] bytedata = new byte[numbytes];
                IntPtr ptr = bmpdata.Scan0;
                unsafe
                {
                    byte* buff = (byte*)ptr;
                    for (int i = 0; i < bytedata.Length; i += 4)
                    {
                        byte b1 = buff[0];
                        buff[0] = buff[2];
                        buff[2] = b1;
                        buff += 4;
                    }
                }
                Marshal.Copy(ptr, bytedata, 0, numbytes);
                return bytedata;
            }
            finally
            {
                if (bmpdata != null)
                    bitmap.UnlockBits(bmpdata);
            }
        }
    }
}
