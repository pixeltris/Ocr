using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocr
{
    public class TesseractOcr : OcrEngine
    {
        public override string Name
        {
            get { return "Tesseract"; }
        }

        protected override string CommandPath
        {
            get { return Path.GetFullPath(Path.Combine("Tesseract", "x86", "tesseract.exe")); }
        }

        public override bool IsInstalled
        {
            get { return File.Exists(CommandPath); }
        }

        public override OcrResult Load(OcrImage image, string language, string apiLanguage)
        {
            string inputPath = null;
            string outputPath = null;
            string outputPath2 = null;
            string outputPath3 = null;

            try
            {
                inputPath = GetTempPath();
                outputPath = GetTempPath(false);
                outputPath2 = outputPath + (IncludeTextRegions ? ".hocr" : ".txt");
                outputPath3 = outputPath + (IncludeTextRegions ? ".txt" : ".hocr");

                File.WriteAllBytes(inputPath, image.Data);
                RunCommand("\"{0}\" \"{1}\" -l {2} {3}", inputPath, outputPath, apiLanguage, IncludeTextRegions ? "-c tessedit_create_hocr=1" : string.Empty);

                OcrResult result = null;

                if (IncludeTextRegions)
                {
                    result = OcrXml.FromFile(outputPath2);
                }
                else
                {
                    string[] lines = File.ReadAllLines(outputPath2);

                    result = new OcrResult();
                    result.ResultType = OcrResultType.Success;
                    foreach (string line in lines)
                    {
                        result.AddLine(line);
                    }
                }                

                return result;
            }
            catch(Exception e)
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
                try
                {
                    if (!string.IsNullOrEmpty(outputPath2))
                        File.Delete(outputPath2);
                }
                catch { }
                try
                {
                    if (!string.IsNullOrEmpty(outputPath3))
                        File.Delete(outputPath3);
                }
                catch { }
            }
        }

        protected override void GetSupportedLanguages()
        {
            string[] result = RunCommand("--list-langs");
            if (result != null && result.Length > 1 && result[0].StartsWith("List of available languages"))
            {
                for (int i = 1; i < result.Length; i++)
                {
                    string language = result[i];
                    switch (language)
                    {
                        case "chi_sim":
                            language = "zh-CN";
                            break;
                        case "chi_tra":
                            language = "zh-TW";
                            break;
                    }
                    AddSupportedLanguage(language);
                }
            }
        }        
    }
}