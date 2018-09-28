using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocr
{
    public class WindowsOcr : OcrEngine
    {
        public override string Name
        {
            get { return "Windows"; }
        }

        protected override string CommandPath
        {
            get { return Path.GetFullPath(Path.Combine("Windows", "WindowsOcr.exe")); }
        }

        public override bool IsInstalled
        {
            get { return File.Exists(CommandPath) && IsWindows8OrNewer(); }
        }

        public override OcrResult Load(OcrImage image, string language, string apiLanguage)
        {
            string inputPath = null;
            string outputPath = null;

            try
            {
                inputPath = GetTempPath();
                outputPath = GetTempPath(false);
                string spacing = AllowsSpacing(language) ? string.Empty : "--nospacing";
                string newLines = AllowsNewLines(language) ? string.Empty : "--nolines";

                File.WriteAllBytes(inputPath, image.Data);
                RunCommand("\"{0}\" \"{1}\" \"{2}\" {3} {4}", apiLanguage, inputPath, outputPath, spacing, newLines);

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
            AddSupportedLanguage("cs", "cs");//Czech
            AddSupportedLanguage("da", "da");//Danish
            AddSupportedLanguage("de", "de-DE");//German (Germany)
            AddSupportedLanguage("el", "el");//Greek
            //AddSupportedLanguage("en", "en-GB");//English (United Kingdom)
            AddSupportedLanguage("en", "en-US");//English (United States)
            AddSupportedLanguage("es", "es-ES");//Spanish (Spain)
            //AddSupportedLanguage("es", "es-MX");//Spanish (Mexico)
            AddSupportedLanguage("fi", "fi");//Finnish
            //AddSupportedLanguage("fr", "fr-CA");//French (Canada)
            AddSupportedLanguage("fr", "fr-FR");//French (France)
            AddSupportedLanguage("hu", "hu");//Hungarian
            AddSupportedLanguage("it", "it-IT");//Italian (Italy)
            AddSupportedLanguage("ja", "ja");//Japanese
            AddSupportedLanguage("ko", "ko");//Korean
            AddSupportedLanguage("no", "nb");//Norwegian (Bokmål)
            AddSupportedLanguage("nl", "nl-NL");//Dutch (Netherlands)
            AddSupportedLanguage("pl", "pl");//Polish
            //AddSupportedLanguage("", "pt-BR");//Portuguese (Brazil)
            AddSupportedLanguage("pt", "pt-PT");//Portuguese (Portugal)
            AddSupportedLanguage("ro", "ro-RO");//Romanian (Romania)
            AddSupportedLanguage("ru", "ru");//Russian
            AddSupportedLanguage("sk", "sk");//Slovak
            AddSupportedLanguage("sr-Cyrl", "sr-Cyrl-RS");//Serbian (Cyrillic, Serbia)
            AddSupportedLanguage("sr-Latn", "sr-Latn-RS");//Serbian (Latin, Serbia)
            AddSupportedLanguage("sv", "sv-SE");//Swedish (Sweden)
            AddSupportedLanguage("tr", "tr");//Turkish
            AddSupportedLanguage("zh-CN", "zh-Hans-CN");//Chinese (Simplified, China)
            //AddSupportedLanguage("zh-TW", "zh-Hant-HK");//Chinese (Traditional, Hong Kong SAR)
            AddSupportedLanguage("zh-TW", "zh-Hant-TW");//Chinese (Traditional, Taiwan)
        }

        private bool AllowsSpacing(string language)
        {
            if (language == "ja")
            {
                return false;
            }
            return true;
        }

        private bool AllowsNewLines(string language)
        {
            return true;
        }

        private bool IsWindows8OrNewer()
        {
            var os = Environment.OSVersion;
            return os.Platform == PlatformID.Win32NT && (os.Version.Major > 6 || (os.Version.Major == 6 && os.Version.Minor >= 2));
        }
    }
}
