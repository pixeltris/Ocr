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
    public class BingTranslator : Translator
    {
        private TimeSpan cookiesExpireTime = TimeSpan.FromMinutes(5);
        private DateTime lastCookiesTime;
        private string cookies;

        public override bool IsEnabled
        {
            get { return true; }
        }

        protected override string Translate(string value, string fromLanguage, string toLanguage, string fromApiLanguage, string toApiLanguage)
        {
            TimeSpan elapsedTime = DateTime.Now - lastCookiesTime;
            if (cookies == null || elapsedTime > cookiesExpireTime)
            {
                cookies = GetCookies("https://www.bing.com/translator");
            }
            lastCookiesTime = DateTime.Now;

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string url = "https://www.bing.com/ttranslate";//?&category=&IG=7206AF8836B34421880FE9079EAE74A9&IID=translator.5034.6

                    webClient.Proxy = null;
                    webClient.Encoding = Encoding.UTF8;
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded; charset=UTF-8";
                    webClient.Headers[HttpRequestHeader.Cookie] = cookies;
                    string json = webClient.UploadString(url, "text=" + HttpUtility.UrlEncode(value) + "&from=" + fromApiLanguage + "&to=" + toApiLanguage);
                    TranslateResponse response = JSONSerializer<TranslateResponse>.DeSerialize(json);
                    return response.translationResponse;
                }
            }
            catch(Exception e)
            {
                cookies = null;
                throw e;
            }
        }

        private void Test(string fromApiLanguage, string toApiLanguage)
        {
            using (WebClient webClient = new WebClient())
            {
                //string url = string.Format("https://www.bing.com/translator/api/Translate/TranslateArray?from={0}&to={1}", fromApiLanguage, toApiLanguage);
                //webClient.Proxy = null;
                //webClient.Encoding = Encoding.UTF8;
                //webClient.Headers[HttpRequestHeader.Accept] = "application/json, text/javascript, */*; q=0.01";
                //webClient.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";                
                //webClient.Headers[HttpRequestHeader.AcceptLanguage] = "en-US,en;q=0.8";
                //webClient.Headers[HttpRequestHeader.CacheControl] = "no-cache";
                //webClient.Headers[HttpRequestHeader.KeepAlive] = "keep-alive";
                //webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                //webClient.Headers[HttpRequestHeader.Cookie] = "mtstkn=8a6JytGbeF5p4wtdlDLDrcMz4GyGTbuZb%2FXUVB4%2BI3GZmqcJNliW1BOHtjZwg%2B8s; _EDGE_S=F=1&SID=388272162E8D69F23FEA7AF52FB5681E; _EDGE_V=1; MUID=14C661CEC64366FD2C9B692DC77B67D2; MUIDB=14C661CEC64366FD2C9B692DC77B67D2; destLang=ko; dmru_list=en%2Cko; destDia=ko-KR; _SS=SID=81C3F9DA0EC14DAB925B330A06749DE6; SRCHUID=V=2&GUID=76DC9C42207A4B3ABE2B155679E33DEB; SRCHUSR=AUTOREDIR=0&GEOVAR=&DOB=20160325; SRCHD=AF=NOFORM; WLS=C=&N=; srcLang=-; smru_list=";                
                //webClient.Headers[HttpRequestHeader.Host] = "www.bing.com";
                //webClient.Headers["Origin"] = "https://www.bing.com";
                //webClient.Headers[HttpRequestHeader.Pragma] = "no-cache";
                //webClient.Headers[HttpRequestHeader.Referer] = "https://www.bing.com/translator";
                //webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.87 Safari/537.36";
                //webClient.Headers["X-Requested-With"] = "XMLHttpRequest";
                //string response = webClient.UploadString(url, "[{\"id\":3556498,\"text\":\"test\"}]");
            }
        }

        [DataContract]
        public class TranslateResponse
        {
            [DataMember]
            public int statusCode { get; set; }
            [DataMember]
            public string translationResponse { get; set; }
        }

        /*[DataContract]
        public class TranslateResponse
        {
            [DataMember]
            public string from { get; set; }
            [DataMember]
            public string to { get; set; }
            [DataMember]
            public TranslateResponseItem[] items { get; set; }
        }

        [DataContract]
        public class TranslateResponseItem
        {
            [DataMember]
            public string id { get; set; }
            [DataMember]
            public string text { get; set; }
            [DataMember]
            public string wordAlignment { get; set; }

            // wordAlignment allows you to hover over words and show you what the word is in the corresponding original / translated box
        }*/

        protected override void GetSupportedLanguages()
        {
            AddSupportedLanguage("ar");//Arabic
            AddSupportedLanguage("bs", "bs-Latn");//Bosnian (Latin) <---- "bs-Latn-BA" for full description, otherwise "bs"
            AddSupportedLanguage("bg");//Bulgarian
            AddSupportedLanguage("ca");//Catalan
            AddSupportedLanguage("zh-CN", "zh-CHS");//Chinese Simplified
            AddSupportedLanguage("zh-TW", "zh-CHT");//Chinese Traditional
            AddSupportedLanguage("hr");//Croatian
            AddSupportedLanguage("cs");//Czech
            AddSupportedLanguage("da");//Danish
            AddSupportedLanguage("nl");//Dutch
            AddSupportedLanguage("en");//English
            AddSupportedLanguage("et");//Estonian
            AddSupportedLanguage("fi");//Finnish
            AddSupportedLanguage("fr");//French
            AddSupportedLanguage("de");//German
            AddSupportedLanguage("el");//Greek
            AddSupportedLanguage("ht");//Haitian Creole <---- This language is not supported in .net?
            AddSupportedLanguage("he");//Hebrew
            AddSupportedLanguage("hi");//Hindi
            AddSupportedLanguage("mww");//Hmong Daw <---- This language is not supported in .net?
            AddSupportedLanguage("hu");//Hungarian
            AddSupportedLanguage("id");//Indonesian
            AddSupportedLanguage("it");//Italian
            AddSupportedLanguage("ja");//Japanese
            AddSupportedLanguage("sw");//Kiswahili
            AddSupportedLanguage("tlh");//Klingon <---- This language is not supported in .net?
            AddSupportedLanguage("tlh-Qaak");//Klingon (pIqaD) <---- This language is not supported in .net?
            AddSupportedLanguage("ko");//Korean
            AddSupportedLanguage("lv");//Latvian
            AddSupportedLanguage("lt");//Lithuanian
            AddSupportedLanguage("ms");//Malay
            AddSupportedLanguage("mt");//Maltese
            AddSupportedLanguage("nb", "no");//Norwegian Bokmål <---- .net has "nb" or "nn", it doesn't have "no"
            AddSupportedLanguage("fa");//Persian
            AddSupportedLanguage("pt");//Portuguese
            AddSupportedLanguage("otq");//Querétaro Otomi <---- This language is not supported in .net?
            AddSupportedLanguage("ro");//Romanian
            AddSupportedLanguage("ru");//Russian
            AddSupportedLanguage("sr-Cyrl");//Serbian (Cyrillic) <---- Maybe just "sr" or one of the "sr-Cyrl-XX" variants?
            AddSupportedLanguage("sr-Latn");//Serbian (Latin) <---- "sr-Latn-XX" variants?
            AddSupportedLanguage("sk");//Slovak
            AddSupportedLanguage("sl");//Slovenian
            AddSupportedLanguage("es");//Spanish
            AddSupportedLanguage("sv");//Swedish
            AddSupportedLanguage("th");//Thai
            AddSupportedLanguage("tr");//Turkish
            AddSupportedLanguage("uk");//Ukrainian
            AddSupportedLanguage("ur");//Urdu
            AddSupportedLanguage("vi");//Vietnamese
            AddSupportedLanguage("cy");//Welsh
            AddSupportedLanguage("yua");//Yucatec Maya <---- This language is not supported in .net?
        }        
    }
}
