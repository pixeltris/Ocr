using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ocr.Translate
{
    public class GoogleTranslator : Translator
    {
        public override bool IsEnabled
        {
            get { return true; }
        }

        protected override string Translate(string value, string fromLanguage, string toLanguage, string fromApiLanguage, string toApiLanguage)
        {
            // Use the JSON API if possible, otherwise create a full web request
            string jsonResponse = null;// TranslateJson(value, fromApiLanguage, toApiLanguage);
            if (string.IsNullOrEmpty(jsonResponse))
            {
                return Translate(value, fromApiLanguage, toApiLanguage);
            }
            return jsonResponse;
        }

        public static new string Translate(string input, string fromLanguage, string toLanguage)
        {
            return Translate(input, fromLanguage + "|" + toLanguage);
        }

        public static string Translate(string input, string languagePair)
        {
            input = HttpUtility.UrlEncode(input);
            string url = string.Format("https://translate.google.com/?hl=en&ie=UTF8&oe=UTF8&text={0}&langpair={1}", input, languagePair);
            using (WebClient webClient = new WebClient())
            {
                webClient.Proxy = null;
                webClient.Encoding = Encoding.UTF8;
                string result = webClient.DownloadStringAwareOfEncoding(url);
                return ReadSpans(result, "<span id=result_box", "</span>");
            }
        }

        private static string ReadSpans(string strSource, string strStart, string strEnd)
        {
            string result = string.Empty;
            int index = strSource.IndexOf(strStart);
            bool first = true;
            int spanCount = 1;

            while (spanCount != 0)
            {
                if (strSource[index] == '<')
                {
                    bool skip = true;
                    if (strSource.Substring(index).StartsWith("<span"))
                        spanCount++;
                    else if (strSource.Substring(index).StartsWith(strEnd))
                        spanCount--;
                    else if (strSource.Substring(index).StartsWith("<br"))
                    {
                        result += Environment.NewLine;
                    }
                    else
                        skip = false;

                    if (skip)
                    {
                        int subCount = 0;
                        while (strSource[index] != '>' || subCount > 0)
                        {
                            if (strSource[index] == '>')
                                subCount--;
                            index++;
                            if (strSource[index] == '<')
                                subCount++;
                        }
                    }
                    else
                    {
                        if ((byte)strSource[index] == 0x0D)
                            result += "\r\n";
                        else
                            result += strSource[index];
                    }

                    if (first)
                    {
                        spanCount--;
                        first = false;
                    }
                }
                else
                {
                    if ((byte)strSource[index] == 0x0D)
                        result += "\r\n";
                    else
                        result += strSource[index];
                }

                index++;
            }

            return result;
        }
        
        public static string TranslateJson(string strToTranslate, string fromLanguage, string toLanguage)
        {
            // Adapted from
            // http://www.crifan.com/teach_you_how_to_find_free_google_translate_api/

            string encodedStr = HttpUtility.UrlEncode(strToTranslate);
            string googleTransBaseUrl = "https://translate.google.com/translate_a/single?";
            string googleTransUrl = googleTransBaseUrl;
            googleTransUrl += "&client=" + "t";//t - translation of source text
            googleTransUrl += "&q=" + encodedStr;//googleTransUrl += "&text=" + encodedStr;
            googleTransUrl += "&hl=" + "en";
            googleTransUrl += "&sl=" + fromLanguage;// source   language
            googleTransUrl += "&tl=" + toLanguage;  // to       language
            googleTransUrl += "&ie=" + "UTF-8";     // input    encode
            googleTransUrl += "&oe=" + "UTF-8";     // output   encode

            googleTransUrl += "&dt=t";//t - default translation

            // This tk implementation almost 100% incorrect. The server seems to accept it anyway more than omitting it.
            // Perhaps multiple layers of spam detection? One checks nothing, one checks if tk exists and one validates it.
            googleTransUrl += "&&tk=" + "985444.601258";//GenerateTk("hello world");//strToTranslate);

            //http://stackoverflow.com/questions/26714426/what-is-the-meaning-of-google-translate-query-params
            //googleTransUrl += "&dt=t";//t - default translation
            //googleTransUrl += "&dt=at";//at - alternate translations
            //googleTransUrl += "&dt=rm";//rm - "read phonetically" option / transcription / transliteration of source and translated texts
            //googleTransUrl += "&dt=bd";//bd - dictionary, in case source text is one word (you get translations with articles, reverse translations, etc.)
            //googleTransUrl += "&dt=md";//md - definitions of source text, if it's one word
            //googleTransUrl += "&dt=ss";//ss - synonyms of source text, if it's one word
            //googleTransUrl += "&dt=ex";//ex - examples
            //googleTransUrl += "&dt=rw";//rw - See also list.
            //&dt=qca - ?
            //&dt=ld - ?
            //&otf=1&rom=1&ssel=0&tsel=0&kc=1 - ???
            //&tk=894929.756001 - some kind of obfuscation of the request url

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    webClient.Encoding = Encoding.UTF8;
                    string response = webClient.DownloadStringAwareOfEncoding(googleTransUrl);

                    int firstOpenQuote = response.IndexOf("\"") + 1;
                    int secondOpenQuote = response.IndexOf("\"", firstOpenQuote);

                    return response.Substring(firstOpenQuote, secondOpenQuote - firstOpenQuote);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                System.Diagnostics.Debugger.Break();
            }

            return null;
        }

        // http://stackoverflow.com/a/34687566
        // https://github.com/Stichoza/google-translate-php/issues/32

        private static string GenerateTk(string request)
        {
            return TL(request);
        }

        private static int RL(int a, string b)
        {
            for (int c = 0; c < b.Length - 2; c += 3)
            {
                int d = b[c + 2];
                d = d >= 'a' ? d - 87 : d;
                d = b[c + 1] == '+' ? a >> d : a << d;
                a = (int)(b[c] == '+' ? a + d & 4294967295 : a ^ d);
            }
            return a;
        }

        private static string TL(string a)
        {
            const int b = 402890;
            int[] d = new int[a.Length];
            for (int e = 0, f = 0; f < a.Length; f++)
            {
                int g = a[f];

                if (128 > g)
                {
                    d[e++] = g;
                }
                else
                {
                    if (2048 > g)
                    {
                        d[e++] = g >> 6 | 192;
                    }
                    else
                    {
                        if (55296 == (g & 64512) && f + 1 < a.Length && 56320 == (a[f + 1] & 64512))
                        {
                            g = 65536 + ((g & 1023) << 10) + (a[++f] & 1023);
                            d[e++] = g >> 18 | 240;
                            d[e++] = g >> 12 & 63 | 128;
                        }
                        else
                        {
                            d[e++] = g >> 12 | 224;
                            d[e++] = g >> 6 & 63 | 128;
                        }
                    }
                    d[e++] = g & 63 | 128;
                }
            }

            int ab = b;
            for (int e = 0; e < d.Length; e++)
            {
                ab += d[e];
                ab = RL(ab, "+-a^+6");
            }
            ab = RL(ab, "+-3^+b+-f");
            if (0 > ab) ab = (int)((ab & 2147483647) + 2147483648);
            ab %= (int)Math.Pow(10, 6);
            return ab + "." + (ab ^ b);
        }

        protected override void GetSupportedLanguages()
        {
            AddSupportedLanguage("af");//Afrikaans
            AddSupportedLanguage("sq");//Albanian
            AddSupportedLanguage("am");//Amharic
            AddSupportedLanguage("ar");//Arabic
            AddSupportedLanguage("hy");//Armenian
            AddSupportedLanguage("az");//Azerbaijani / Azerbaijan
            AddSupportedLanguage("eu");//Basque
            AddSupportedLanguage("be");//Belarusian
            AddSupportedLanguage("bn");//Bengali
            AddSupportedLanguage("bs");//Bosnian
            AddSupportedLanguage("bg");//Bulgarian
            AddSupportedLanguage("my");//Burmese <---- This language is not supported in .net?
            AddSupportedLanguage("ca");//Catalan
            AddSupportedLanguage("ceb");//Cebuano <---- This language is not supported in .net?
            AddSupportedLanguage("zh-CN");//Chinese (Simplified)
            AddSupportedLanguage("zh-TW");//Chinese (Traditional)
            AddSupportedLanguage("co");//Corsican
            AddSupportedLanguage("hr");//Croatian
            AddSupportedLanguage("cs");//Czech
            AddSupportedLanguage("da");//Danish
            AddSupportedLanguage("nl");//Dutch
            AddSupportedLanguage("en");//English
            AddSupportedLanguage("eo");//Esperanto <---- This language is not supported in .net?
            AddSupportedLanguage("et");//Estonian
            AddSupportedLanguage("fil", "tl");//Filipino <---- "fil" in .net?
            AddSupportedLanguage("fi");//Finnish
            AddSupportedLanguage("fr");//French
            AddSupportedLanguage("gl");//Galician
            AddSupportedLanguage("ka");//Georgian
            AddSupportedLanguage("de");//German
            AddSupportedLanguage("el");//Greek
            AddSupportedLanguage("gu");//Gujarati
            AddSupportedLanguage("ht");//Haitian <---- This language is not supported in .net?
            AddSupportedLanguage("ha");//Hausa
            AddSupportedLanguage("haw");//Hawaiian <---- This language is not supported in .net?
            AddSupportedLanguage("he", "iw");//Hebrew <---- "he" in .net?
            AddSupportedLanguage("hi");//Hindi
            AddSupportedLanguage("hmn");//Hmong <---- This language is not supported in .net?
            AddSupportedLanguage("hu");//Hungarian
            AddSupportedLanguage("is");//Icelandic
            AddSupportedLanguage("ig");//Igbo
            AddSupportedLanguage("id");//Indonesian
            AddSupportedLanguage("ga");//Irish
            AddSupportedLanguage("it");//Italian
            AddSupportedLanguage("ja");//Japanese
            AddSupportedLanguage("jv");//Javanese <---- This language is not supported in .net?
            AddSupportedLanguage("kn");//Kannada
            AddSupportedLanguage("kk");//Kazakh
            AddSupportedLanguage("km");//Khmer
            AddSupportedLanguage("ko");//Korean
            AddSupportedLanguage("ku");//Kurdish <---- This language is not supported in .net?
            AddSupportedLanguage("ky");//Kyrgyz
            AddSupportedLanguage("lo");//Lao
            AddSupportedLanguage("la");//Latin <---- There are many different codes for latin in .net
            AddSupportedLanguage("lv");//Latvian
            AddSupportedLanguage("lt");//Lithuanian
            AddSupportedLanguage("lb");//Luxembourgish
            AddSupportedLanguage("mk");//Macedonian
            AddSupportedLanguage("mg");//Malagasy <---- This language is not supported in .net?
            AddSupportedLanguage("ms");//Malay
            AddSupportedLanguage("ml");//Malayalam
            AddSupportedLanguage("mt");//Maltese
            AddSupportedLanguage("mi");//Maori
            AddSupportedLanguage("mr");//Marathi
            AddSupportedLanguage("mn");//Mongolian
            AddSupportedLanguage("ne");//Nepali
            AddSupportedLanguage("no");//Norwegian <---- .net has "nb" or "nn", it doesn't have "no"
            AddSupportedLanguage("ny");//Nyanja <---- This language is not supported in .net?
            AddSupportedLanguage("ps");//Pashto
            AddSupportedLanguage("fa");//Persian
            AddSupportedLanguage("pl");//Polish
            AddSupportedLanguage("pt");//Portuguese
            AddSupportedLanguage("pa");//Punjabi
            AddSupportedLanguage("ro");//Romanian
            AddSupportedLanguage("ru");//Russian
            AddSupportedLanguage("sm");//Samoan <---- This language is not supported in .net?
            AddSupportedLanguage("gd");//Scottish Gaelic
            AddSupportedLanguage("sr");//Serbian
            AddSupportedLanguage("sn");//Shona <---- This language is not supported in .net?
            AddSupportedLanguage("sd");//Sindhi <---- This language is not supported in .net?
            AddSupportedLanguage("si");//Sinhala
            AddSupportedLanguage("sk");//Slovak
            AddSupportedLanguage("sl");//Slovenian
            AddSupportedLanguage("so");//Somali <---- This language is not supported in .net?
            AddSupportedLanguage("es");//Spanish
            AddSupportedLanguage("su");//Sundanese <---- This language is not supported in .net?
            AddSupportedLanguage("sw");//Swahili / Kiswahili
            AddSupportedLanguage("sv");//Swedish
            AddSupportedLanguage("tg");//Tajik
            AddSupportedLanguage("ta");//Tamil
            AddSupportedLanguage("te");//Telugu
            AddSupportedLanguage("th");//Thai
            AddSupportedLanguage("tr");//Turkish
            AddSupportedLanguage("uk");//Ukrainian
            AddSupportedLanguage("ur");//Urdu
            AddSupportedLanguage("uz");//Uzbek
            AddSupportedLanguage("vi");//Vietnamese
            AddSupportedLanguage("cy");//Welsh
            AddSupportedLanguage("fy");//Western Frisian / Frisian
            AddSupportedLanguage("xh");//Xhosa / isiXhosa
            AddSupportedLanguage("yi");//Yiddish <---- This language is not supported in .net?
            AddSupportedLanguage("yo");//Yoruba
            AddSupportedLanguage("zu");//Zulu / isiZulu
        }        
    }
}
