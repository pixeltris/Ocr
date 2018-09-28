using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocr
{
    public class NicomsoftOcr : OcrEngine
    {
        public override string Name
        {
            get { return "Nicomsoft"; }
        }

        protected override string CommandPath
        {
            get { return Path.GetFullPath(Path.Combine("Nicomsoft", "NicomsoftOcr.exe")); }
        }

        public override bool IsInstalled
        {
            get { return File.Exists("Nicomsoft/Bin/NSOCR.dll") && File.Exists("Nicomsoft/installed.txt"); }
        }

        public override OcrResult Load(OcrImage image, string language, string apiLanguage)
        {
            string inputPath = null;
            string outputPath = null;

            try
            {
                inputPath = GetTempPath();
                outputPath = GetTempPath(false);

                File.WriteAllBytes(inputPath, image.Data);
                RunCommand("\"{0}\" \"{1}\" \"{2}\"", apiLanguage, inputPath, outputPath);

                if (File.Exists(outputPath))
                {
                    string[] lines = File.ReadAllLines(outputPath);

                    OcrResult result = new OcrResult();
                    result.ResultType = OcrResultType.Success;
                    foreach (string line in lines)
                    {
                        result.AddLine(line);
                    }
                    return result;
                }
                else
                {
                    return OcrResult.Create(OcrResultType.Failed);
                }
            }
            catch (Exception e)
            {
                return OcrResult.Create(OcrResultType.Exception, e.ToString());
            }
            finally
            {
                try
                {
                    if (!string.IsNullOrEmpty(inputPath))
                        File.Delete(inputPath);
                }
                catch { }
                try
                {
                    if (!string.IsNullOrEmpty(outputPath))
                        File.Delete(outputPath);
                }
                catch { }
            }
        }

        protected override void GetSupportedLanguages()
        {
            AddSupportedLanguage("ar", "Arabic");
            AddSupportedLanguage("zh-CN", "Chinese_Simplified");
            AddSupportedLanguage("zh-TW", "Chinese_Traditional");
            AddSupportedLanguage("ja", "Japanese");
            AddSupportedLanguage("ko", "Korean");

            AddSupportedLanguage("bg", "Bulgarian");
            AddSupportedLanguage("ca", "Catalan");
            AddSupportedLanguage("hr", "Croatian");
            AddSupportedLanguage("cs", "Czech");
            AddSupportedLanguage("da", "Danish");
            AddSupportedLanguage("nl", "Dutch");
            AddSupportedLanguage("en", "English");
            AddSupportedLanguage("et", "Estonian");
            AddSupportedLanguage("fi", "Finnish");
            AddSupportedLanguage("fr", "French");
            AddSupportedLanguage("de", "German");
            AddSupportedLanguage("hu", "Hungarian");
            AddSupportedLanguage("id", "Indonesian");
            AddSupportedLanguage("it", "Italian");
            AddSupportedLanguage("lv", "Latvian");
            AddSupportedLanguage("lt", "Lithuanian");
            AddSupportedLanguage("no", "Norwegian");// <---- .net has "nb" or "nn", it doesn't have "no"
            AddSupportedLanguage("pl", "Polish");
            AddSupportedLanguage("pt", "Portuguese");
            AddSupportedLanguage("ro", "Romanian");
            AddSupportedLanguage("ru", "Russian");
            AddSupportedLanguage("sk", "Slovak");
            AddSupportedLanguage("sl", "Slovenian");
            AddSupportedLanguage("es", "Spanish");
            AddSupportedLanguage("sv", "Swedish");
            AddSupportedLanguage("tr", "Turkish");
        }
    }
}
