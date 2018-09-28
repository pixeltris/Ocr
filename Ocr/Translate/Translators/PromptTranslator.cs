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
    class PromptTranslator : Translator
    {
        public override bool IsEnabled
        {
            get { return true; }
        }

        protected override string Translate(string value, string fromLanguage, string toLanguage, string fromApiLanguage, string toApiLanguage)
        {
            TranslateRequest request = new TranslateRequest()
            {
                dirCode = fromApiLanguage + "-" + toApiLanguage,
                template = "General",
                text = WebUtils.FormatJson(value),
                lang = fromApiLanguage,
                limit = 3000,
                useAutoDetect = false,
                key = string.Empty,
                ts = "MainSite",
                tid = string.Empty,
                IsMobile = true
            };
            string requestJson = JSONSerializer<TranslateRequest>.Serialize(request);

            string url = "http://www.online-translator.com/services/TranslationService.asmx/GetTranslateNew";

            using (WebClient webClient = new WebClient())
            {
                webClient.Proxy = null;
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";

                string json = webClient.UploadString(url, requestJson);

                // Lazy hack, find the correct way to handle "__type"
                json = json.Replace("__type", "__nype__");

                TranslateResponse response = JSONSerializer<TranslateResponse>.DeSerialize(json);
                string result = response.d.result;
                if (!string.IsNullOrEmpty(result) && result.StartsWith("<style"))
                {
                    result = HttpUtility.HtmlDecode(result);

                    string resultHeader = "ref_result";
                    int resultOffset = result.IndexOf(resultHeader) + resultHeader.Length;
                    result = result.Substring(resultOffset);
                    result = result.Substring(result.IndexOf('>') + 1);
                    result = result.Substring(0, result.IndexOf('<')).Trim();
                    return result;
                }
                else
                {
                    return result;
                }
            }
        }

        [DataContract]
        public class TranslateRequest
        {
            [DataMember]
            public string dirCode { get; set; }
            [DataMember]
            public string template { get; set; }
            [DataMember]
            public string text { get; set; }
            [DataMember]
            public string lang { get; set; }
            [DataMember]
            public int limit { get; set; }
            [DataMember]
            public bool useAutoDetect { get; set; }
            [DataMember]
            public string key { get; set; }
            [DataMember]
            public string ts { get; set; }
            [DataMember]
            public string tid { get; set; }
            [DataMember]
            public bool IsMobile { get; set; }
        }

        [DataContract]
        public class TranslateResponse
        {
            [DataMember]
            public TranslateResponseItem d { get; set; }
        }

        [DataContract]
        public class TranslateResponseItem
        {
            [DataMember]
            public int errCode { get; set; }
            [DataMember]
            public int errCodeInt { get; set; }
            [DataMember]
            public string result { get; set; }
        }

        protected override void GetSupportedLanguages()
        {
            AddSupportedLanguage("ar");//Arabic
            AddSupportedLanguage("ca");//Catalan
            AddSupportedLanguage("en");//English
            AddSupportedLanguage("fi");//Finnish
            AddSupportedLanguage("fr");//French
            AddSupportedLanguage("de");//German
            AddSupportedLanguage("it");//Italian
            AddSupportedLanguage("ja");//Japanese
            AddSupportedLanguage("kk");//Kazakh
            AddSupportedLanguage("pt");//Portuguese
            AddSupportedLanguage("ru");//Russian
            AddSupportedLanguage("es");//Spanish
            AddSupportedLanguage("tr");//Turkish
        }
    }
}
