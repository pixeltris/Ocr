using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocr.Translate;
using System.Net;

namespace Ocr
{
    public abstract class Translator
    {
        // <CaultureInfo.Name, ApiSpecificLanguageName>
        public Dictionary<string, string> SupportedLanguages { get; private set; }

        public abstract bool IsEnabled { get; }

        public Translator()
        {
            SupportedLanguages = new Dictionary<string, string>();
            GetSupportedLanguages();
        }        

        protected abstract string Translate(string value, string fromLanguage, string toLanguage, string fromApiLanguage, string toApiLanguage);

        public string Translate(string value, string fromLanguage, string toLanguage)
        {
            CultureInfo fromCulture, toCulture;
            if(!LocaleHelper.TryParseCulture(fromLanguage, out fromCulture) || !LocaleHelper.TryParseCulture(toLanguage, out toCulture))
                return null;

            string fromApiLanguage = GetSupportedLanguageName(fromCulture.Name);
            string toApiLanguage = GetSupportedLanguageName(toCulture.Name);

            if (string.IsNullOrEmpty(fromApiLanguage) || string.IsNullOrEmpty(toApiLanguage))
                return null;

            try
            {
                return Translate(value, fromCulture.Name, toCulture.Name, fromApiLanguage, toApiLanguage);
            }
            catch
            {
                return null;
            }
        }

        public static string Translate<T>(string value, string fromLanguage, string toLanguage) where T : Translator, new()
        {
            return Create<T>().Translate(value, fromLanguage, toLanguage);
        }

        public static string Translate(TranslatorType type, string value, string fromLanguage, string toLanguage)
        {
            return Create(type).Translate(value, fromLanguage, toLanguage);
        }

        protected string GetCookies(string url)
        {
            string response;
            return GetCookies(url, null, out response);
        }

        protected string GetCookies(string url, string existingCookies, out string response)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Proxy = null;
                if (!string.IsNullOrEmpty(existingCookies))
                {
                    webClient.Headers[HttpRequestHeader.Cookie] = existingCookies;
                }
                response = webClient.DownloadString(url);

                string responseCookiesStr = webClient.ResponseHeaders[HttpResponseHeader.SetCookie];
                if (string.IsNullOrEmpty(responseCookiesStr))
                {
                    return existingCookies;
                }
                CookieCollection cookies = WebUtils.GetAllCookiesFromHeader(responseCookiesStr);
                string result = string.Empty;
                foreach (Cookie cookie in cookies)
                {
                    result += cookie.Name + "=" + cookie.Value + ";";
                }
                result = result.TrimEnd(';');
                return result;
            }
        }

        protected abstract void GetSupportedLanguages();

        // Returns the culture for the language if supported by the current engine API
        protected CultureInfo GetSupportedLanguageCulture(string language)
        {
            return LocaleHelper.GetSupportedLanguageCulture(SupportedLanguages, language);
        }

        // Returns the API specific language name
        protected string GetSupportedLanguageName(string language)
        {
            return LocaleHelper.GetSupportedLanguageName(SupportedLanguages, language);
        }

        private string GetLanguageKey(string language)
        {
            return LocaleHelper.GetLanguageKey(SupportedLanguages, language);
        }

        protected bool AddSupportedLanguage(CultureInfo culture)
        {
            return AddSupportedLanguage(culture, culture.Name);
        }

        protected bool AddSupportedLanguage(string language)
        {
            return AddSupportedLanguage(language, language);
        }

        protected bool AddSupportedLanguage(CultureInfo culture, string ocrLanguageName)
        {
            return AddSupportedLanguage(culture.Name, ocrLanguageName);
        }

        protected bool AddSupportedLanguage(string language, string ocrLanguageName)
        {
            CultureInfo culture;
            if (LocaleHelper.TryParseCulture(language, out culture))
            {
                SupportedLanguages[culture.Name] = ocrLanguageName;
                //SupportedLanguages[language] = ocrLanguageName;
                return true;
            }
            return false;
        }

        public static Translator Create(TranslatorType type)
        {
            Translator translator = null;
            Type translatorType = GetType(type);
            if (translatorType != null)
            {
                translator = (Translator)Activator.CreateInstance(translatorType);
            }
            return translator;
        }

        public static T Create<T>() where T : Translator, new()
        {
            T ocrEngine = new T();
            return ocrEngine;
        }

        public static Type GetType(TranslatorType type)
        {
            switch (type)
            {
                case TranslatorType.Google: return typeof(GoogleTranslator);
                case TranslatorType.Bing: return typeof(BingTranslator);
                case TranslatorType.Yandex: return typeof(YandexTranslator);
                case TranslatorType.Prompt: return typeof(PromptTranslator);
                case TranslatorType.Babylon: return typeof(BabylonTranslator);
                case TranslatorType.SDL: return typeof(SDLTranslator);
                case TranslatorType.Baidu: return typeof(BaiduTranslator);
                default: return null;
            }
        }

        public static TranslatorType GetType(Type type)
        {
            if (type == typeof(GoogleTranslator))
                return TranslatorType.Google;

            if (type == typeof(BingTranslator))
                return TranslatorType.Bing;

            if (type == typeof(YandexTranslator))
                return TranslatorType.Yandex;

            if (type == typeof(PromptTranslator))
                return TranslatorType.Prompt;

            if (type == typeof(BabylonTranslator))
                return TranslatorType.Babylon;

            if (type == typeof(SDLTranslator))
                return TranslatorType.SDL;

            if (type == typeof(BaiduTranslator))
                return TranslatorType.Baidu;

            return (TranslatorType)0;
        }
    }
}