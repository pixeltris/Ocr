using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Resolvers;

namespace Ocr.Translate
{    
    class SDLTranslator : Translator
    {
        private string tracking;// = "applicationKey=dlWbNAC2iLJWujbcIHiNMQ%3D%3D applicationInstance=freetranslation";

        // SDL is now dead?
        public override bool IsEnabled
        {
            get { return false; }
        }

        protected override string Translate(string value, string fromLanguage, string toLanguage, string fromApiLanguage, string toApiLanguage)
        {
            if (string.IsNullOrEmpty(tracking))
            {
                tracking = GetTracking();
            }

            TranslateRequest request = new TranslateRequest()
            {
                text = WebUtils.FormatJson(value),
                from = fromApiLanguage,
                to = toApiLanguage
            };
            string requestJson = JSONSerializer<TranslateRequest>.Serialize(request);

            using (WebClient webClient = new WebClient())
            {
                string url = "https://www.freetranslation.com/gw-mt-proxy-service-web/mt-translation";

                webClient.Proxy = null;
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                webClient.Headers["Tracking"] = tracking;
                string json = webClient.UploadString(url, requestJson);
                TranslateResponse response = JSONSerializer<TranslateResponse>.DeSerialize(json);
                return response.translation;
            }
        }

        private string GetTracking()
        {            
            string result = string.Empty;

            using (WebClient webClient = new WebClient())
            {
                webClient.Proxy = null;
                string response = webClient.DownloadString("https://www.freetranslation.com");

                response = response.Substring(response.IndexOf("home.min"));
                response = response.Substring(0, response.IndexOf(".js"));
                string jsUrl = "https://www.freetranslation.com/js/" + response + ".js";

                response = webClient.DownloadString(jsUrl);

                response = response.Substring(response.IndexOf("applicationKey"));
                string[] splitted = response.Split(',');
                for (int i = 0; i < splitted.Length; i++)
                {
                    if (splitted[i].StartsWith("applicationKey") || splitted[i].StartsWith("applicationInstance"))
                    {
                        result += splitted[i].Replace("\"", string.Empty) + " ";
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return result;
        }

        [DataContract]
        public class TranslateRequest
        {
            [DataMember]
            public string text { get; set; }
            [DataMember]
            public string from { get; set; }
            [DataMember]
            public string to { get; set; }
        }

        [DataContract]
        public class TranslateResponse
        {
            [DataMember]
            public int charCount { get; set; }
            [DataMember]
            public string detectedLanguage { get; set; }
            [DataMember]
            public string from { get; set; }
            [DataMember]
            public bool partialTranslation { get; set; }
            [DataMember]
            public string to { get; set; }
            [DataMember]
            public string translation { get; set; }
            [DataMember]
            public string translationToken { get; set; }
            [DataMember]
            public int wordCount { get; set; }
        }

        protected override void GetSupportedLanguages()
        {
            AddSupportedLanguage("sq", "alb");//Albanian
            AddSupportedLanguage("ar", "ara");//Arabic
            AddSupportedLanguage("bn", "ben");//Bengali
            AddSupportedLanguage("bul");//Bulgarian
            AddSupportedLanguage("zh-CN", "chi");//Chinese (Simplified)
            AddSupportedLanguage("zh-TW", "cht");//Chinese (Traditional)
            AddSupportedLanguage("cs", "cze");//Czech
            AddSupportedLanguage("da", "dan");//Danish
            AddSupportedLanguage("prs", "fad");//Dari
            AddSupportedLanguage("nl", "dut");//Dutch
            AddSupportedLanguage("eng");//English
            AddSupportedLanguage("est");//Estonian
            AddSupportedLanguage("fin");//Finnish
            AddSupportedLanguage("fra");//French
            AddSupportedLanguage("de", "ger");//German
            AddSupportedLanguage("el", "gre");//Greek
            AddSupportedLanguage("hau");//Hausa
            AddSupportedLanguage("heb");//Hebrew
            AddSupportedLanguage("hin");//Hindi
            AddSupportedLanguage("hun");//Hungarian
            AddSupportedLanguage("ind");//Indonesian
            AddSupportedLanguage("ita");//Italian
            AddSupportedLanguage("jpn");//Japanese
            AddSupportedLanguage("kor");//Korean
            AddSupportedLanguage("lit");//Lithuanian
            AddSupportedLanguage("ms", "may");//Malay
            AddSupportedLanguage("nb", "nor");//Norwegian
            AddSupportedLanguage("pus");//Pashto
            AddSupportedLanguage("fa", "per");//Persian
            AddSupportedLanguage("pol");//Polish
            AddSupportedLanguage("por");//Portuguese
            AddSupportedLanguage("ro", "rum");//Romanian
            AddSupportedLanguage("rus");//Russian
            AddSupportedLanguage("sr", "srp");//Serbian
            AddSupportedLanguage("sk", "slo");//Slovak
            AddSupportedLanguage("sl", "slv");//Slovenian
            AddSupportedLanguage("so", "som");//Somali <---- This language is not supported in .net?
            AddSupportedLanguage("spa");//Spanish
            AddSupportedLanguage("swe");//Swedish
            AddSupportedLanguage("tha");//Thai
            AddSupportedLanguage("tur");//Turkish
            AddSupportedLanguage("ukr");//Ukrainian
            AddSupportedLanguage("urd");//Urdu
            AddSupportedLanguage("vie");//Vietnamese

            /*XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.XmlResolver = new XmlPreloadedResolver(XmlKnownDtds.Xhtml10);

            string xml = @"<tr><td><a href=""#"" data-language=""alb"" data-id=""505037985fe01ac20407b832"" class=""lang-elem"">Albanian</a><a href=""#"" data-language=""ara"" data-id=""505037985fe01ac20407b7f2"" class=""lang-elem"">Arabic</a><a href=""#"" data-language=""ben"" data-id=""505037985fe01ac20407b7f3"" class=""lang-elem"">Bengali</a><a href=""#"" data-language=""bul"" data-id=""505037985fe01ac20407b7f4"" class=""lang-elem"">Bulgarian</a><a href=""#"" data-language=""chi"" data-id=""505037985fe01ac20407b7f5"" class=""lang-elem"">Chinese (Simplified)</a><a href=""#"" data-language=""cht"" data-id=""505037985fe01ac20407b7f6"" class=""lang-elem"">Chinese (Traditional)</a><a href=""#"" data-language=""cze"" data-id=""505037985fe01ac20407b7f7"" class=""lang-elem"">Czech</a></td><td><a href=""#"" data-language=""dan"" data-id=""505037985fe01ac20407b7f8"" class=""lang-elem"">Danish</a><a href=""#"" data-language=""fad"" data-id=""505037985fe01ac20407b7f9"" class=""lang-elem"">Dari</a><a href=""#"" data-language=""dut"" data-id=""505037985fe01ac20407b7fa"" class=""lang-elem"">Dutch</a><a href=""#"" data-language=""eng"" data-id=""505037985fe01ac20407b7fb"" class=""lang-elem"">English</a><a href=""#"" data-language=""est"" data-id=""505037985fe01ac20407b7fe"" class=""lang-elem"">Estonian</a><a href=""#"" data-language=""fin"" data-id=""505037985fe01ac20407b7ff"" class=""lang-elem"">Finnish</a><a href=""#"" data-language=""fra"" data-id=""505037985fe01ac20407b800"" class=""lang-elem"">French</a></td><td><a href=""#"" data-language=""ger"" data-id=""505037985fe01ac20407b803"" class=""lang-elem"">German</a><a href=""#"" data-language=""gre"" data-id=""505037985fe01ac20407b804"" class=""lang-elem"">Greek</a><a href=""#"" data-language=""hau"" data-id=""505037985fe01ac20407b805"" class=""lang-elem"">Hausa</a><a href=""#"" data-language=""heb"" data-id=""505037985fe01ac20407b806"" class=""lang-elem"">Hebrew</a><a href=""#"" data-language=""hin"" data-id=""505037985fe01ac20407b807"" class=""lang-elem"">Hindi</a><a href=""#"" data-language=""hun"" data-id=""505037985fe01ac20407b808"" class=""lang-elem"">Hungarian</a><a href=""#"" data-language=""ind"" data-id=""505037985fe01ac20407b809"" class=""lang-elem"">Indonesian</a></td><td><a href=""#"" data-language=""ita"" data-id=""505037985fe01ac20407b80a"" class=""lang-elem"">Italian</a><a href=""#"" data-language=""jpn"" data-id=""505037985fe01ac20407b80b"" class=""lang-elem"">Japanese</a><a href=""#"" data-language=""kor"" data-id=""505037985fe01ac20407b80c"" class=""lang-elem"">Korean</a><a href=""#"" data-language=""lit"" data-id=""505037985fe01ac20407b80e"" class=""lang-elem"">Lithuanian</a><a href=""#"" data-language=""may"" data-id=""505037985fe01ac20407b920"" class=""lang-elem"">Malay</a><a href=""#"" data-language=""nor"" data-id=""505037985fe01ac20407b810"" class=""lang-elem"">Norwegian</a><a href=""#"" data-language=""pus"" data-id=""505037985fe01ac20407b811"" class=""lang-elem"">Pashto</a></td><td><a href=""#"" data-language=""per"" data-id=""505037985fe01ac20407b812"" class=""lang-elem"">Persian</a><a href=""#"" data-language=""pol"" data-id=""505037985fe01ac20407b813"" class=""lang-elem"">Polish</a><a href=""#"" data-language=""por"" data-id=""505037985fe01ac20407b814"" class=""lang-elem"">Portuguese</a><a href=""#"" data-language=""rum"" data-id=""505037985fe01ac20407b817"" class=""lang-elem"">Romanian</a><a href=""#"" data-language=""rus"" data-id=""505037985fe01ac20407b818"" class=""lang-elem"">Russian</a><a href=""#"" data-language=""srp"" data-id=""505037985fe01ac20407b819"" class=""lang-elem"">Serbian</a><a href=""#"" data-language=""slo"" data-id=""505037985fe01ac20407b81c"" class=""lang-elem"">Slovak</a></td><td><a href=""#"" data-language=""slv"" data-id=""505037985fe01ac20407b81d"" class=""lang-elem"">Slovenian</a><a href=""#"" data-language=""som"" data-id=""505037985fe01ac20407b81e"" class=""lang-elem"">Somali</a><a href=""#"" data-language=""spa"" data-id=""505037985fe01ac20407b81f"" class=""lang-elem"">Spanish</a><a href=""#"" data-language=""swe"" data-id=""505037985fe01ac20407b822"" class=""lang-elem"">Swedish</a><a href=""#"" data-language=""tha"" data-id=""505037985fe01ac20407b823"" class=""lang-elem"">Thai</a><a href=""#"" data-language=""tur"" data-id=""505037985fe01ac20407b824"" class=""lang-elem"">Turkish</a><a href=""#"" data-language=""ukr"" data-id=""505037985fe01ac20407b9c1"" class=""lang-elem"">Ukrainian</a></td><td><a href=""#"" data-language=""urd"" data-id=""505037985fe01ac20407b825"" class=""lang-elem"">Urdu</a><a href=""#"" data-language=""vie"" data-id=""505037985fe01ac20407b9c7"" class=""lang-elem"">Vietnamese</a></td></tr>";

            using (StringReader stringReader = new StringReader(xml))
            using (XmlReader reader = XmlReader.Create(stringReader, settings))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            ReadTr(reader);
                            break;
                    }
                }
            }*/
        }

        private void ReadTr(XmlReader parent)
        {
            using (XmlReader reader = parent.ReadSubtree())
            {
                reader.Read();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            ReadTd(reader);
                            break;
                    }
                }
            }
        }

        private void ReadTd(XmlReader parent)
        {
            using (XmlReader reader = parent.ReadSubtree())
            {
                reader.Read();
                bool read = false;
                while (true)
                {
                    if(!read)
                    {
                        if (!reader.Read())
                            break;
                    }
                    read = false;

                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            string language = reader.GetAttribute("data-language");
                            string name = reader.ReadInnerXml();
                            System.Diagnostics.Debug.WriteLine("AddSupportedLanguage(\"" + language + "\");//" + name);
                            read = true;
                            break;
                    }
                }
            }
        }
    }
}
