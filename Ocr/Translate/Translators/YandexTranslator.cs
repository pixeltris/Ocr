using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ocr.Translate
{
    class YandexTranslator : Translator
    {
        private bool usePost = false;

        private TimeSpan sessionIdExpireTime = TimeSpan.FromMinutes(5);
        private DateTime lastSessionIdTime;
        private string sessionId;

        public override bool IsEnabled
        {
            get { return true; }
        }

        protected override string Translate(string value, string fromLanguage, string toLanguage, string fromApiLanguage, string toApiLanguage)
        {
            TimeSpan elapsedTime = DateTime.Now - lastSessionIdTime;
            if (sessionId == null || elapsedTime > sessionIdExpireTime)
                sessionId = GetSessionId();

            lastSessionIdTime = DateTime.Now;

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string formattedValue = HttpUtility.UrlEncode(value);

                    //Alternative: 2-0
                    string url = string.Format("https://translate.yandex.net/api/v1/tr.json/translate?id={0}-0-0&srv=tr-text&lang={1}-{2}", sessionId, fromApiLanguage, toApiLanguage);
                    if (!usePost)
                        url += "&text=" + formattedValue;

                    webClient.Proxy = null;
                    webClient.Encoding = Encoding.UTF8;

                    string json = null;
                    if (usePost)
                    {
                        webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded; charset=UTF-8";
                        json = webClient.UploadString(url, "text=" + formattedValue);
                    }
                    else
                    {
                        webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                        json = webClient.DownloadStringAwareOfEncoding(url);
                    }
                    TranslateResponse response = JSONSerializer<TranslateResponse>.DeSerialize(json);
                    return response.text[0];
                }
            }
            catch(Exception e)
            {
                sessionId = null;
                throw e;
            }
        }

        private string GetSessionId()
        {
            using (WebClient webClient = new WebClient())
            {
                string sessionIdHeader = "SID: '";

                webClient.Proxy = null;
                string response = webClient.DownloadString("https://translate.yandex.com");
                int index = response.IndexOf(sessionIdHeader);
                if (index > 0)
                {
                    response = response.Substring(index + sessionIdHeader.Length);                    
                    response = response.Substring(0, response.IndexOf('\''));
                    string[] splitted = response.Split('.');
                    return Reverse(splitted[0]) + "." +
                           Reverse(splitted[1]) + "." +
                           Reverse(splitted[2]);
                }
                return null;
            }
        }

        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        [DataContract]
        public class TranslateResponse
        {
            [DataMember]
            public string[] align { get; set; }
            [DataMember]
            public int code { get; set; }
            [DataMember]
            public string lang { get; set; }            
            [DataMember]
            public string[] text { get; set; }
        }

        protected override void GetSupportedLanguages()
        {
            AddSupportedLanguage("af");//Afrikaans
            AddSupportedLanguage("sq");//Albanian
            AddSupportedLanguage("ar");//Arabic
            AddSupportedLanguage("hy");//Armenian
            AddSupportedLanguage("az");//Azerbaijani
            AddSupportedLanguage("ba");//Bashkir
            AddSupportedLanguage("eu");//Basque
            AddSupportedLanguage("be");//Belarusian
            AddSupportedLanguage("bs");//Bosnian
            AddSupportedLanguage("bg");//Bulgarian
            AddSupportedLanguage("ca");//Catalan
            //AddSupportedLanguage("zh");//Chinese <---- traditional / simplified not specified, might have to choose which
            AddSupportedLanguage("zh-CN", "zh");//Chinese Simplified
            AddSupportedLanguage("zh-TW", "zh");//Chinese Traditional
            AddSupportedLanguage("hr");//Croatian
            AddSupportedLanguage("cs");//Czech
            AddSupportedLanguage("da");//Danish
            AddSupportedLanguage("nl");//Dutch
            AddSupportedLanguage("sjn");//Elvish (Sindarin) <---- This language is not supported in .net?
            AddSupportedLanguage("en");//English
            AddSupportedLanguage("et");//Estonian
            AddSupportedLanguage("fi");//Finnish
            AddSupportedLanguage("fr");//French
            AddSupportedLanguage("gl");//Galician
            AddSupportedLanguage("ka");//Georgian
            AddSupportedLanguage("de");//German
            AddSupportedLanguage("el");//Greek
            AddSupportedLanguage("ht");//Haitian <---- This language is not supported in .net?
            AddSupportedLanguage("he");//Hebrew
            AddSupportedLanguage("hi");//Hindi
            AddSupportedLanguage("hu");//Hungarian
            AddSupportedLanguage("is");//Icelandic
            AddSupportedLanguage("id");//Indonesian
            AddSupportedLanguage("ga");//Irish
            AddSupportedLanguage("it");//Italian
            AddSupportedLanguage("ja");//Japanese
            AddSupportedLanguage("kk");//Kazakh
            AddSupportedLanguage("ky");//Kirghiz <---- This language is not supported in .net?
            AddSupportedLanguage("ko");//Korean
            AddSupportedLanguage("la");//Latin <---- May need additional specifiers, there are many times of latin
            AddSupportedLanguage("lv");//Latvian
            AddSupportedLanguage("lt");//Lithuanian
            AddSupportedLanguage("mk");//Macedonian
            AddSupportedLanguage("mg");//Malagasy <---- This language is not supported in .net?
            AddSupportedLanguage("ms");//Malay
            AddSupportedLanguage("mt");//Maltese
            AddSupportedLanguage("mn");//Mongolian
            AddSupportedLanguage("no");//Norwegian <---- .net has "nb" or "nn", it doesn't have "no"
            AddSupportedLanguage("fa");//Persian
            AddSupportedLanguage("pl");//Polish
            AddSupportedLanguage("pt");//Portuguese
            AddSupportedLanguage("ro");//Romanian
            AddSupportedLanguage("ru");//Russian
            AddSupportedLanguage("sr");//Serbian <---- May need additional specifiers, there are many times of "sr"
            AddSupportedLanguage("sk");//Slovak
            AddSupportedLanguage("sl");//Slovenian
            AddSupportedLanguage("es");//Spanish
            AddSupportedLanguage("sw");//Swahili / Kiswahili
            AddSupportedLanguage("sv");//Swedish
            AddSupportedLanguage("tl");//Tagalog <---- This language is not supported in .net?
            AddSupportedLanguage("tg");//Tajik
            AddSupportedLanguage("tt");//Tatar
            AddSupportedLanguage("th");//Thai
            AddSupportedLanguage("tr");//Turkish
            AddSupportedLanguage("uk");//Ukrainian
            AddSupportedLanguage("ur");//Urdu
            AddSupportedLanguage("uz");//Uzbek
            AddSupportedLanguage("vi");//Vietnamese
            AddSupportedLanguage("cy");//Welsh
        }        
    }
}
