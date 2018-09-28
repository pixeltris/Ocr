using System;
using System.Collections.Generic;
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
    class BaiduTranslator : Translator
    {
        private TimeSpan cookiesExpireTime = TimeSpan.FromDays(5);
        private DateTime lastCookiesTime;
        private string cookies;
        private string token;
        private string gtk;

        public override bool IsEnabled
        {
            get { return true; }
        }

        protected override string Translate(string value, string fromLanguage, string toLanguage, string fromApiLanguage, string toApiLanguage)
        {
            TimeSpan elapsedTime = DateTime.Now - lastCookiesTime;
            if (cookies == null || elapsedTime > cookiesExpireTime)
            {                
                cookies = GetCookies("https://fanyi.baidu.com");

                // The request requires cookies to get a valid token
                string html;
                cookies = GetCookies("https://fanyi.baidu.com", cookies, out html);

                string tokenPrefix = "token: '";
                string gtkPrefix = "gtk = '";
                int tokenIndex = html.IndexOf(tokenPrefix);
                int gtkIndex = html.IndexOf(gtkPrefix);
                if (tokenIndex >= 0 && gtkIndex >= 0)
                {
                    token = html.Substring(tokenIndex + tokenPrefix.Length);
                    token = token.Substring(0, token.IndexOf('\''));

                    gtk = html.Substring(gtkIndex + gtkPrefix.Length);
                    gtk = gtk.Substring(0, gtk.IndexOf('\''));
                }
                else
                {
                    cookies = null;
                    token = null;
                    gtk = null;
                }
            }
            lastCookiesTime = DateTime.Now;

            using (WebClient webClient = new WebClient())
            {
                string url = "https://fanyi.baidu.com/v2transapi";

                string sign = GetSign(value, gtk);

                webClient.Proxy = null;
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers[HttpRequestHeader.Cookie] = cookies;
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded; charset=UTF-8";
                string json = webClient.UploadString(url, string.Format("from={0}&to={1}&query={2}&transtype=realtime&token={3}&sign={4}",
                    fromApiLanguage, toApiLanguage, HttpUtility.UrlEncode(value), token, sign));
                TranslateResponse response = JSONSerializer<TranslateResponse>.DeSerialize(json);
                return response.trans_result.data[0].dst;
            }
        }

        static ScriptEngine jsEngine = null;
        private string GetSign(string text, string gtk)
        {
            if (jsEngine == null)
            {
                jsEngine = new ScriptEngine("jscript");
            }

            // https://github.com/hujingshuang/MTrans/blob/master/tk/Baidu.js
            ParsedScript parsed = jsEngine.Parse(@"function a(r, o) {
    for (var t = 0; t < o.length - 2; t += 3) {
        var a = o.charAt(t + 2);
        a = a >= ""a"" ? a.charCodeAt(0) - 87 : Number(a),
        a = ""+"" === o.charAt(t + 1) ? r >>> a: r << a,
        r = ""+"" === o.charAt(t) ? r + a & 4294967295 : r ^ a
    }
    return r
}
var C = null;
var token = function(r, _gtk) {
    var o = r.length;
    o > 30 && (r = """" + r.substr(0, 10) + r.substr(Math.floor(o / 2) - 5, 10) + r.substring(r.length, r.length - 10));
    var t = void 0,
    t = null !== C ? C: (C = _gtk || """") || """";
    for (var e = t.split("".""), h = Number(e[0]) || 0, i = Number(e[1]) || 0, d = [], f = 0, g = 0; g < r.length; g++) {
        var m = r.charCodeAt(g);
        128 > m ? d[f++] = m: (2048 > m ? d[f++] = m >> 6 | 192 : (55296 === (64512 & m) && g + 1 < r.length && 56320 === (64512 & r.charCodeAt(g + 1)) ? (m = 65536 + ((1023 & m) << 10) + (1023 & r.charCodeAt(++g)), d[f++] = m >> 18 | 240, d[f++] = m >> 12 & 63 | 128) : d[f++] = m >> 12 | 224, d[f++] = m >> 6 & 63 | 128), d[f++] = 63 & m | 128)
    }
    for (var S = h,
    u = ""+-a^+6"",
    l = ""+-3^+b+-f"",
    s = 0; s < d.length; s++) S += d[s],
    S = a(S, u);

    return S = a(S, l),
    S ^= i,
    0 > S && (S = (2147483647 & S) + 2147483648),
    S %= 1e6,
    S.toString() + ""."" + (S ^ h)
}");
            return parsed.CallMethod("token", text, gtk).ToString();
        }

        [DataContract]
        public class TranslateResponse
        {
            [DataMember]
            public TransResult trans_result { get; set; }
        }

        [DataContract]
        public class TransResult
        {
            [DataMember]
            public string from { get; set; }
            [DataMember]
            public string to { get; set; }
            [DataMember]
            public string domain { get; set; }
            [DataMember]
            public int type { get; set; }
            [DataMember]
            public int status { get; set; }
            [DataMember]
            public TransData[] data { get; set; }
            [DataMember]
            TransPhonetic[] phonetic { get; set; }
        }

        [DataContract]
        public class TransData
        {
            [DataMember]
            public string dst { get; set; }
            [DataMember]
            public string src { get; set; }
        }

        [DataContract]
        public class TransPhonetic
        {
            /*[DataMember]
            public string src_str;
            [DataMember]
            public string trg_str;*/
        }

        protected override void GetSupportedLanguages()
        {
            AddSupportedLanguage("ar", "ara");//Arabic
            AddSupportedLanguage("et", "est");//Estonian
            AddSupportedLanguage("bg", "bul");//Bulgarian
            AddSupportedLanguage("pl");//Polish
            AddSupportedLanguage("da", "dan");//Danish
            AddSupportedLanguage("de");//German
            AddSupportedLanguage("ru");//Russian
            AddSupportedLanguage("fr", "fra");//French
            AddSupportedLanguage("fi", "fin");//Finnish
            AddSupportedLanguage("ko", "kor");//Korean
            AddSupportedLanguage("nl");//Dutch
            AddSupportedLanguage("cs");//Czech
            AddSupportedLanguage("ro", "rom");//Romanian
            AddSupportedLanguage("pt");//Portuguese
            AddSupportedLanguage("ja", "jp");//Japanese
            AddSupportedLanguage("sv", "swe");//Swedish
            AddSupportedLanguage("sl", "slo");//Slovenian
            AddSupportedLanguage("th");//Thai
            AddSupportedLanguage("wyw");//Classical (some type of chinese? simplified?)
            AddSupportedLanguage("es", "spa");//Spanish
            AddSupportedLanguage("el");//Greek
            AddSupportedLanguage("hu");//Hungarian
            AddSupportedLanguage("zh-CN", "zh");//Chinese (simplified chinese)
            AddSupportedLanguage("en");//English
            AddSupportedLanguage("it");//Italian language
            AddSupportedLanguage("yue");//Cantonese <---- This language is not supported in .net?
            AddSupportedLanguage("zh-TW", "cht");//traditional Chinese

            /*XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.XmlResolver = new XmlPreloadedResolver(XmlKnownDtds.Xhtml10);

            string xml = @"<div class=""language-list clearfix""><ul><li class=""language-class""><font><font>ABC</font></font></li><li><a href=""javascript:void(0);"" value=""ara"" class=""data-lang""><font><font>Arabic</font></font></a></li><li><a href=""javascript:void(0);"" value=""est"" class=""data-lang""><font><font>Estonian</font></font></a></li><li><a href=""javascript:void(0);"" value=""bul"" class=""data-lang""><font><font>Bulgarian</font></font></a></li><li><a href=""javascript:void(0);"" value=""pl"" class=""data-lang""><font><font>Polish</font></font></a></li></ul><ul><li class=""language-class""><font><font>DEFG</font></font></li><li><a href=""javascript:void(0);"" value=""dan"" class=""data-lang""><font><font class="""">Danish</font></font></a></li><li><a href=""javascript:void(0);"" value=""de"" class=""data-lang""><font><font>German</font></font></a></li><li><a href=""javascript:void(0);"" value=""ru"" class=""data-lang""><font><font>Russian</font></font></a></li><li><a href=""javascript:void(0);"" value=""fra"" class=""data-lang""><font><font>French</font></font></a></li><li><a href=""javascript:void(0);"" value=""fin"" class=""data-lang""><font><font>Finnish</font></font></a></li></ul><ul><li class=""language-class""><font><font>HIJKLMN</font></font></li><li><a href=""javascript:void(0);"" value=""kor"" class=""data-lang""><font><font>Korean</font></font></a></li><li><a href=""javascript:void(0);"" value=""nl"" class=""data-lang""><font><font>Dutch</font></font></a></li><li><a href=""javascript:void(0);"" value=""cs"" class=""data-lang""><font><font>Czech</font></font></a></li><li><a href=""javascript:void(0);"" value=""rom"" class=""data-lang""><font><font>Romanian</font></font></a></li></ul><ul><li class=""language-class""><font><font>OPQRST</font></font></li><li><a href=""javascript:void(0);"" value=""pt"" class=""data-lang""><font><font>Portuguese</font></font></a></li><li><a href=""javascript:void(0);"" value=""jp"" class=""data-lang""><font><font>Japanese</font></font></a></li><li><a href=""javascript:void(0);"" value=""swe"" class=""data-lang""><font><font>Swedish</font></font></a></li><li><a href=""javascript:void(0);"" value=""slo"" class=""data-lang""><font><font>Slovenian</font></font></a></li><li><a href=""javascript:void(0);"" value=""th"" class=""data-lang""><font><font>Thai</font></font></a></li></ul><ul><li class=""language-class""><font><font>UVWX</font></font></li><li><a href=""javascript:void(0);"" value=""wyw"" class=""data-lang""><font><font>Classical</font></font></a></li><li><a href=""javascript:void(0);"" value=""spa"" class=""data-lang""><font><font>Spanish</font></font></a></li><li><a href=""javascript:void(0);"" value=""el"" class=""data-lang""><font><font>Greek</font></font></a></li><li><a href=""javascript:void(0);"" value=""hu"" class=""data-lang""><font><font>Hungarian</font></font></a></li></ul><ul><li class=""language-class""><font><font>YZ</font></font></li><li><a href=""javascript:void(0);"" value=""zh"" class=""data-lang""><font><font>Chinese</font></font></a></li><li><a href=""javascript:void(0);"" value=""en"" class=""data-lang""><font><font class=""lang-selected"">English</font></font></a></li><li><a href=""javascript:void(0);"" value=""it"" class=""data-lang""><font><font>Italian language</font></font></a></li><li><a href=""javascript:void(0);"" value=""yue"" class=""data-lang""><font><font>Cantonese</font></font></a></li><li><a href=""javascript:void(0);"" value=""cht"" class=""data-lang""><font><font>traditional Chinese</font></font></a></li></ul></div>";

            using (StringReader stringReader = new StringReader(xml))
            using (XmlReader reader = XmlReader.Create(stringReader, settings))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            ReadDiv(reader);
                            break;
                    }
                }
            }*/
        }

        private void ReadDiv(XmlReader parent)
        {
            using (XmlReader reader = parent.ReadSubtree())
            {
                reader.Read();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            ReadUl(reader);
                            break;
                    }
                }
            }
        }

        private void ReadUl(XmlReader parent)
        {
            using (XmlReader reader = parent.ReadSubtree())
            {
                reader.Read();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            ReadLi(reader);
                            break;
                    }
                }
            }
        }

        private void ReadLi(XmlReader parent)
        {
            using (XmlReader reader = parent.ReadSubtree())
            {
                reader.Read();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            string language = reader.GetAttribute("value");
                            string name = ReadFont(reader, 1);
                            if(!string.IsNullOrEmpty(language))
                            {
                                System.Diagnostics.Debug.WriteLine("AddSupportedLanguage(\"" + language + "\");//" + name);
                            }
                            break;
                    }
                }
            }
        }

        private string ReadFont(XmlReader parent, int level)
        {
            using (XmlReader reader = parent.ReadSubtree())
            {
                reader.Read();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (level == 2)
                            {
                                return reader.ReadInnerXml();
                            }
                            else
                            {
                                return ReadFont(reader, level + 1);
                            }
                    }
                }
            }
            return null;
        }
    }
}
