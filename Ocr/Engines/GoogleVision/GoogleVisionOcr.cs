using Ocr.Translate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ocr
{
    class GoogleVisionOcr : OcrEngine
    {
        public override string Name
        {
            get { return "GoogleVision"; }
        }

        public override bool IsInstalled
        {
            get { return true; }
        }

        public override OcrResult Load(OcrImage image, string language, string apiLanguage)
        {
            OcrRequests request = new OcrRequests()
            {
                requests = new OcrRequest[1]
                {
                    new OcrRequest()
                    {
                        image = new OcrRequestImage()
                        {
                            content = Convert.ToBase64String(image.Data)
                        },
                        features = new OcrRequestFeature[1]
                        {
                            new OcrRequestFeature()
                            {
                                type = "TEXT_DETECTION",
                                maxResults = 1
                            }
                        }
                    }
                }
            };

            string requestJson = JSONSerializer<OcrRequests>.Serialize(request);

            using (GZipWebClient webClient = new GZipWebClient())
            {
                string key = Program.Settings.KeyInfo.Key;

                if (string.IsNullOrEmpty(key))
                {
                    key = GetDefaultKey();
                }

                string url = "https://vision.googleapis.com/v1/images:annotate?key=" + key;

                webClient.Proxy = null;
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                webClient.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                string json = webClient.UploadString(url, requestJson);

                string result = string.Empty;
                OcrResponses responses = JSONSerializer<OcrResponses>.DeSerialize(json);
                foreach (OcrResponse response in responses.responses)
                {
                    foreach (OcrTextAnnotation annotation in response.textAnnotations)
                    {
                        if (!string.IsNullOrEmpty(annotation.locale))
                        {
                            result += annotation.description;
                        }                        
                    }
                }
                result = result.TrimEnd('\r', '\n');
                return new OcrResult(result);
            }
        }

        private string GetDefaultKey()
        {
            return null;
        }

        [DataContract]
        public class OcrRequests
        {
            [DataMember]
            public OcrRequest[] requests { get; set; }
        }

        [DataContract]
        public class OcrRequest
        {
            [DataMember]
            public OcrRequestImage image { get; set; }
            [DataMember]
            public OcrRequestFeature[] features { get; set; }
        }

        [DataContract]
        public class OcrRequestImage
        {
            [DataMember]
            public string content { get; set; }
        }

        [DataContract]
        public class OcrRequestFeature
        {
            [DataMember]
            public string type { get; set; }
            [DataMember]
            public int maxResults { get; set; }
        }

        [DataContract]
        public class OcrResponses
        {
            [DataMember]
            public OcrResponse[] responses { get; set; }
        }

        [DataContract]
        public class OcrResponse
        {
            [DataMember]
            public OcrTextAnnotation[] textAnnotations { get; set; }
        }

        [DataContract]
        public class OcrTextAnnotation
        {
            [DataMember]
            public string locale { get; set; }
            [DataMember]
            public string description { get; set; }
            [DataMember]
            public OcrBoundingPoly boundingPoly { get; set; }
        }

        [DataContract]
        public class OcrBoundingPoly
        {
            public OcrVert[] vertices { get; set; }
        }

        [DataContract]
        public class OcrVert
        {
            [DataMember]
            public int X { get; set; }
            [DataMember]
            public int Y { get; set; }
        }

        protected override void GetSupportedLanguages()
        {
            // Assume google vision can handle all google translate languages
            GoogleTranslator translate = new GoogleTranslator();
            foreach (KeyValuePair<string, string> language in translate.SupportedLanguages)
            {
                AddSupportedLanguage(language.Key, language.Value);
            }
        }        

        class GZipWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                return request;
            }
        }
    }
}
