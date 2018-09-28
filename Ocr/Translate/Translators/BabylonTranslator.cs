using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Resolvers;

namespace Ocr.Translate
{
    class BabylonTranslator : Translator
    {
        private Dictionary<string, int> languageIds = new Dictionary<string, int>();

        public override bool IsEnabled
        {
            get { return true; }
        }

        protected override string Translate(string value, string fromLanguage, string toLanguage, string fromApiLanguage, string toApiLanguage)
        {
            using (WebClient webClient = new WebClient())
            {
                string url = string.Format("https://translation.babylon-software.com/translate/babylon.php?v=1.0&q={0}&langpair={1}|{2}&callback=babylonTranslator.callback&context=babylon.0.3._babylon_api_response",
                    HttpUtility.UrlEncode(value), languageIds[fromApiLanguage], languageIds[toApiLanguage]);

                webClient.Proxy = null;
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded; charset=UTF-8";
                string json = webClient.DownloadStringAwareOfEncoding(url);
                json = json.Substring(json.IndexOf('{'));
                json = json.Substring(0, json.LastIndexOf('}') + 1);
                TranslateResponse response = JSONSerializer<TranslateResponse>.DeSerialize(json);
                string result = HttpUtility.UrlDecode(response.translatedText);
                //string result = HttpUtility.HtmlDecode(response.translatedText);
                return result;
            }
        }

        [DataContract]
        public class TranslateResponse
        {
            [DataMember]
            public string translatedText { get; set; }
        }

        protected override void GetSupportedLanguages()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.XmlResolver = new XmlPreloadedResolver(XmlKnownDtds.Xhtml10);

            string xml = @"<option value=""0"" selected=""selected"" >English</option><option value=""1"">French</option><option value=""2"">Italian</option><option value=""6"">German</option><option value=""5"">Portuguese</option><option value=""3"">Spanish</option><option value=""SEPERATOR"" disabled=""disabled"">--------------</option><option value=""15"">Arabic</option><option value=""99"">Catalan</option><option value=""344"">Castilian</option><option value=""31"">Czech</option><option value=""10"">Chinese (s)</option><option value=""9"">Chinese (t)</option><option value=""43"">Danish</option><option value=""11"">Greek</option><option value=""14"">Hebrew</option><option value=""60"">Hindi</option><option value=""30"">Hungarian</option><option value=""51"">Persian</option><option value=""8"">Japanese</option><option value=""12"">Korean</option><option value=""4"">Dutch</option><option value=""46"">Norwegian</option><option value=""29"">Polish</option><option value=""47"">Romanian</option><option value=""7"">Russian</option><option value=""48"">Swedish</option><option value=""13"">Turkish</option><option value=""16"">Thai</option><option value=""49"">Ukrainian</option><option value=""39"">Urdu</option>";

            using (StringReader stringReader = new StringReader(xml))
            using (XmlReader reader = XmlReader.Create(stringReader, settings))
            {
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
                            string valueStr = reader.GetAttribute("value");
                            int value;
                            if (int.TryParse(valueStr, out value))
                            {
                                string languageName = reader.ReadInnerXml();
                                read = true;

                                switch (languageName)
                                {
                                    case "Castilian":
                                        // Some type of spanish
                                        break;
                                    case "Chinese (s)":
                                        languageName = "zh-CN";//"Chinese (Simplified)";
                                        break;
                                    case "Chinese (t)":
                                        languageName = "zh-TW";//"Chinese (Traditional)";
                                        break;
                                }

                                CultureInfo culture;
                                if (!LocaleHelper.TryParseCulture(languageName, out culture))
                                {
                                    break;
                                }
                                if (culture != null)
                                {
                                    languageIds[culture.Name] = value;
                                    AddSupportedLanguage(culture);
                                }                                
                            }
                            break;
                    }
                }
            }
        }
    }
}
