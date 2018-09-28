using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ocr
{
    public abstract class OcrEngine
    {
        protected virtual string CommandPath
        {
            get { return null; }
        }
        protected int Timeout { get; set; }        

        public abstract string Name { get; }
        public string Language { get; set; }
        public bool IncludeTextRegions { get; set; }

        public abstract bool IsInstalled { get; }

        // <CaultureInfo.Name, ApiSpecificLanguageName>
        public Dictionary<string, string> SupportedLanguages { get; private set; }

        public OcrEngine()
        {
            IncludeTextRegions = true;
            Timeout = 30000;
            SupportedLanguages = new Dictionary<string, string>();
            if (!RemoteOcr.IsRemote(GetType()) && GetType() != typeof(RemoteOcr) && IsInstalled)
            {
                GetSupportedLanguages();
            }
        }

        public abstract OcrResult Load(OcrImage image, string language, string apiLanguage);

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

        protected void AddSupportedLanguage(CultureInfo culture)
        {
            AddSupportedLanguage(culture, culture.Name);
        }

        protected void AddSupportedLanguage(string language)
        {
            AddSupportedLanguage(language, language);
        }

        protected void AddSupportedLanguage(CultureInfo culture, string ocrLanguageName)
        {
            AddSupportedLanguage(culture.Name, ocrLanguageName);
        }

        protected void AddSupportedLanguage(string language, string ocrLanguageName)
        {
            CultureInfo culture;
            if (LocaleHelper.TryParseCulture(language, out culture))
            {
                SupportedLanguages[culture.Name] = ocrLanguageName;
                //SupportedLanguages[language] = ocrLanguageName;
            }
        }

        public OcrResult Load(OcrImage image, string language)
        {
            try
            {
                OcrResult result = null;

                if (GetType() != typeof(RemoteOcr) && RemoteOcr.IsRemote(GetType()))
                {
                    OcrEngineType targetType = GetType(GetType());
                    RemoteOcr remoteOcr = new RemoteOcr(targetType);
                    result = remoteOcr.Load(image, language, language);
                }
                else if (!IsInstalled)
                {
                    return OcrResult.Create(OcrResultType.NotInstalled);
                }
                else
                {
                    string apiLanguage = language == null ? null : GetSupportedLanguageName(language);
                    if (string.IsNullOrEmpty(apiLanguage))
                        return OcrResult.Create(OcrResultType.LanguageNotSupported);
                    result = Load(image, language, apiLanguage);
                }

                if (result != null && result.Rect == OcrRect.Empty)
                {
                    result.Rect = new OcrRect(0, 0, image.Width, image.Height);
                }

                return result;
            }
            catch (Exception e)
            {
                return OcrResult.Create(OcrResultType.Exception, e.ToString());
            }
        }

        public OcrResult LoadFile(string file, string language)
        {
            try
            {
                if (!File.Exists(file))
                {
                    return OcrResult.Create(OcrResultType.InvalidFilePath);
                }

                OcrImage image = new OcrImage();
                try
                {
                    using (Image bitmap = Bitmap.FromFile(file))
                    {
                        image.Width = bitmap.Width;
                        image.Height = bitmap.Height;
                    }
                    image.Data = File.ReadAllBytes(file);
                    // Path is mostly ignored, remove it for now to stop it being sent over the network
                    //image.Path = file;
                }
                catch(Exception e)
                {
                    return OcrResult.Create(OcrResultType.InvalidFile, e.ToString());
                }

                return Load(image, language);
            }
            catch(Exception e)
            {
                return OcrResult.Create(OcrResultType.Exception, e.ToString());
            }
        }        

        public OcrResult LoadFile(string file)
        {
            return LoadFile(file, Language);
        }

        public void LoadFileAsync(string file, Action<OcrResult> callback)
        {
            LoadFileAsync(file, Language, callback);
        }

        public void LoadFileAsync(string file, string language, Action<OcrResult> callback)
        {
            Task.Factory.StartNew(() =>
            {
                OcrResult ocrResult = LoadFile(file, language);
                if (callback != null)
                {
                    callback(ocrResult);
                }
            });
        }        

        public static OcrResult LoadFile<T>(string file, string language) where T : OcrEngine, new()
        {
            T ocrEngine = new T();
            return ocrEngine.LoadFile(file, language);
        }

        public static OcrResult LoadFile(OcrEngineType type, string file, string language)
        {
            OcrEngine ocrEngine = Create(type);
            return ocrEngine.LoadFile(file, language);
        }

        public static void LoadFileAsync(OcrEngineType type, string file, string language, Action<OcrResult> callback)
        {
            OcrEngine ocrEngine = Create(type);
            ocrEngine.LoadFile(file, language);
        }

        public static void LoadFileAsync<T>(string file, string language, Action<OcrResult> callback) where T : OcrEngine, new()
        {
            Task.Factory.StartNew(() =>
            {
                OcrResult ocrResult = LoadFile<T>(file, language);
                if (callback != null)
                {
                    callback(ocrResult);
                }
            });
        }

        public static OcrEngine Create(OcrEngineType type)
        {
            return Create(type, null);
        }

        public static OcrEngine Create(OcrEngineType type, string language)
        {
            OcrEngine ocrEngine = null;
            Type ocrEngineType = GetType(type);
            if (ocrEngineType != null)
            {
                ocrEngine = (OcrEngine)Activator.CreateInstance(ocrEngineType);
                ocrEngine.Language = language;
            }
            return ocrEngine;
        }

        public static T Create<T>() where T : OcrEngine, new()
        {
            return Create<T>(null);
        }

        public static T Create<T>(string language) where T : OcrEngine, new()
        {
            T ocrEngine = new T();
            ocrEngine.Language = language;
            return ocrEngine;
        }

        public static Type GetType(OcrEngineType type)
        {
            switch (type)
            {
                case OcrEngineType.Tesseract: return typeof(TesseractOcr);
                case OcrEngineType.Windows: return typeof(WindowsOcr);
                case OcrEngineType.Google: return typeof(GoogleOcr);
                case OcrEngineType.GoogleVision: return typeof(GoogleVisionOcr);
                case OcrEngineType.ABBYY: return typeof(AbbyyOcr);
                case OcrEngineType.Nicomsoft: return typeof(NicomsoftOcr);
                default: return null;
            }
        }

        public static OcrEngineType GetType(Type type)
        {
            if (type == typeof(TesseractOcr))
                return OcrEngineType.Tesseract;

            if (type == typeof(WindowsOcr))
                return OcrEngineType.Windows;

            if (type == typeof(GoogleOcr))
                return OcrEngineType.Google;

            if (type == typeof(GoogleVisionOcr))
                return OcrEngineType.GoogleVision;            

            if (type == typeof(AbbyyOcr))
                return OcrEngineType.ABBYY;

            if (type == typeof(NicomsoftOcr))
                return OcrEngineType.Nicomsoft;

            return (OcrEngineType)0;
        }

        protected string[] RunCommand(string command, params object[] args)
        {
            return RunCommand(false, command, args);
        }

        protected string RunCommandFlat(string command, params object[] args)
        {
            return RunCommandFlat(false, command, args);
        }

        protected string[] RunCommand(bool silent, string command, params object[] args)
        {
            string[] lines = RunCommandFlat(silent, command, args).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return lines;
        }

        protected string RunCommandFlat(bool silent, string command, params object[] args)
        {
            string formattedCommand = string.Format(command, args);
            string result = string.Empty;

            using (Process process = new Process())
            {
                process.StartInfo.FileName = CommandPath;
                process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                process.StartInfo.Arguments = formattedCommand;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        result += e.Data + Environment.NewLine;
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        result += e.Data + Environment.NewLine;
                    }
                };
                
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                if (Timeout > 0)
                {
                    if (!process.WaitForExit(Timeout))
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch
                        {

                        }
                    }
                }
            }

            return result;
        }

        protected string GetTempPath()
        {
            return Path.GetTempFileName();
        }

        protected string GetTempPath(bool create)
        {
            return create ? GetTempPath() : Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        }

        protected string GetTempPath(string extension)
        {
            return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + extension);
        }

        protected string WaitForClipboard()
        {
            return WaitForClipboard(TimeSpan.FromSeconds(3));
        }

        protected string WaitForClipSync()
        {
            return WaitForClipSync(TimeSpan.FromSeconds(5));
        }

        protected string WaitForClipboard(TimeSpan timeout)
        {
            string text = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (true)
            {
                text = System.Windows.Forms.Clipboard.GetText();
                if (!string.IsNullOrEmpty(text) || stopwatch.Elapsed > timeout)
                {
                    break;
                }
                Thread.Sleep(1);
            }
            stopwatch.Stop();
            return text;
        }

        protected string WaitForClipSync(TimeSpan timeout)
        {
            string text = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (true)
            {
                text = Program.ClipSync.Text;
                if (!string.IsNullOrEmpty(text) || stopwatch.Elapsed > timeout)
                {
                    break;
                }
                Thread.Sleep(1);
            }
            stopwatch.Stop();
            return text;
        }

        protected static bool ProcessExists(string name)
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(name);
                try
                {
                    return processes.Length > 0;
                }
                finally
                {
                    try
                    {
                        foreach (Process proc in processes)
                            proc.Dispose();
                    }
                    catch { }
                    processes = null;
                }
            }
            catch
            {                
            }
            return false;
        }

        protected bool SaveLoadableImage(OcrImage image)
        {
            try
            {
                File.WriteAllBytes(GetLoadableImagePath(), image.Data);
                return true;
            }
            catch { }
            return false;
        }

        protected string GetLoadableImagePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "ocr_image.png");
        }
    }
}