using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocr
{
    internal class LocaleHelper
    {
        private static CultureInfo[] cultures;

        static LocaleHelper()
        {
            cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        }

        public static bool TryParseCulture(string language, out CultureInfo result)
        {
            result = null;

            if(string.IsNullOrEmpty(language))
                return false;

            result = cultures.FirstOrDefault(x =>
                language.Equals(x.Name, StringComparison.OrdinalIgnoreCase) ||
                language.Equals(x.TwoLetterISOLanguageName, StringComparison.OrdinalIgnoreCase) ||
                language.Equals(x.ThreeLetterISOLanguageName, StringComparison.OrdinalIgnoreCase) ||
                language.Equals(x.DisplayName, StringComparison.OrdinalIgnoreCase));

            return result != null;
        }

        public static CultureInfo GetCulture(string language)
        {
            return new CultureInfo(GetCultureCode(language));
        }

        // Based on:
        // https://msdn.microsoft.com/en-us/library/windows/hardware/mt171094(v=vs.85).aspx
        // https://gist.github.com/padgo/0885be223d1b94a0293a
        // SW_DVD9_NTRL_Win_10_1511_64Bit_MultiLang_Feat_on_Demand_X20-75989.ISO (Released Nov '15) for 1511
        // Windows 10 build 10586.104
        public static string GetCultureCode(string language)
        {
            switch (language.ToLower())
            {
                case "ethi": return "am-ET";//ti-ET
                case "arab": return "ar-SA";//fa-IR, ku-Arab-IQ, pa-Arab-PK, prs-AF, sd-Arab-PK, ug-CN, ur-PK
                case "syrc": return "ar-SY";//syr-SY
                case "beng": return "as-IN";//bn-BD, bn-IN
                case "cher": return "chr-Cher-US";
                case "gujr": return "gu-IN";
                case "hebr": return "he-IL";
                case "deva": return "hi-IN";//kok-IN, mr-IN, ne-NP
                case "jpan": return "ja-JP";
                case "khmr": return "km-KH";
                case "knda": return "kn-IN";
                case "kore": return "ko-KR";
                case "laoo": return "lo-LA";
                case "mlym": return "ml-IN";
                case "orya": return "or-IN";
                case "guru": return "pa-IN";
                case "sinh": return "si-LK";
                case "taml": return "ta-IN";
                case "telu": return "te-IN";
                case "thai": return "th-TH";
                case "hans": return "zh-CN";
                case "hant": return "zh-HK";//zh-TW

                //Display Name : Canadian Aboriginal Syllabics Supplemental Fonts
                //Description : Additional font: Euphemia
                case "cans": System.Diagnostics.Debugger.Break(); goto default; // Find this language
                case "paneuropeansupplementalfonts": return "en";
                default: return new CultureInfo(language).Name;
            }
        }

        // Returns the culture for the language if supported by the current engine API
        public static CultureInfo GetSupportedLanguageCulture(Dictionary<string, string> languages, string language)
        {
            string languageKey = GetLanguageKey(languages, language);
            if (languageKey != null)
                return new CultureInfo(languageKey);
            return null;
        }

        // Returns the API specific language name
        public static string GetSupportedLanguageName(Dictionary<string, string> languages, string language)
        {
            string languageKey = GetLanguageKey(languages, language);
            if (languageKey != null)
                return languages[languageKey];
            return null;
        }

        public static string GetLanguageKey(Dictionary<string, string> languages, string language)
        {
            CultureInfo cultureInfo;
            if (!LocaleHelper.TryParseCulture(language, out cultureInfo))
                return null;

            List<string> systemLanguages = languages.Keys.ToList();

            string name = systemLanguages.FirstOrDefault(x => x.Equals(cultureInfo.Name, StringComparison.OrdinalIgnoreCase));
            if (name != null)
                return name;

            string threeLetterISO = systemLanguages.FirstOrDefault(x => x.Equals(cultureInfo.ThreeLetterISOLanguageName, StringComparison.OrdinalIgnoreCase));
            if (threeLetterISO != null)
                return threeLetterISO;

            string twoLetterISO = systemLanguages.FirstOrDefault(x => x.Equals(cultureInfo.TwoLetterISOLanguageName, StringComparison.OrdinalIgnoreCase));
            if (twoLetterISO != null)
                return twoLetterISO;            

            string displayName = systemLanguages.FirstOrDefault(x => x.Equals(cultureInfo.DisplayName, StringComparison.OrdinalIgnoreCase));
            if (displayName != null)
                return displayName;

            return null;
        }
    }

    // Additional info

    //https://msdn.microsoft.com/library/dd997383(v=vs.100).aspx
    //https://support.microsoft.com/en-us/kb/939949
    //https://msdn.microsoft.com/en-gb/library/ee825488(v=cs.20).aspx
    //https://msdn.microsoft.com/en-us/windows/uwp/publish/supported-languages
    //https://gist.github.com/padgo/0885be223d1b94a0293a
    //http://timtrott.co.uk/culture-codes/

    //https://msdn.microsoft.com/en-us/library/windows/hardware/mt171094(v=vs.85).aspx
    //https://msdn.microsoft.com/en-us/library/windows/hardware/dn898478(v=vs.85).aspx
    //https://msdn.microsoft.com/en-us/library/windows/hardware/dn898434(v=vs.85).aspx
}