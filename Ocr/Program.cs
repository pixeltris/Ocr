using Ocr.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ocr
{
    static class Program
    {
        private static Dictionary<OcrEngineType, OcrEngine> engines;
        private static Dictionary<TranslatorType, Translator> translators;

        public static Dictionary<OcrEngineType, OcrEngine> Engines
        {
            get { return engines; }
        }

        public static Dictionary<TranslatorType, Translator> Translators
        {
            get { return translators; }
        }

        public static Settings Settings { get; set; }
        public static MainForm MainForm { get; private set; }
        public static SettingsForm SettingsForm { get; private set; }
        public static WPFCompactForm CompactForm { get; private set; }
        public static WPFCompactForm PopupForm { get; private set; }
        public static CompactToolForm PopupToolForm { get; private set; }

        // Number of OCR engine profiles
        public const int ProfilesCount = 5;
        public const char LanguageFilterSplitChar = '|';

        public static OcrEngine ActiveOcrEngine { get; set; }
        public static Translator ActiveTranslator { get; set; }
        public static int ActiveOcrProfileIndex { get; set; }
        public static string ActiveLanguageFrom { get; set; }
        public static string ActiveLanguageTo { get; set; }

        public static string Text { get; set; }
        public static string TranslatedText { get; set; }

        public static string LastText { get; set; }
        public static string LastTranslatedText { get; set; }
        public static DateTime LastTranslate { get; set; }
        public static DateTime LastOCR { get; set; }

        public static ClipSyncServer ClipSync { get; private set; }

        public static bool Running { get; private set; }        

        [STAThread]
        static void Main()
        {
            // Not even worth running this OCR engine (requires Bluestacks / another app)
            //if (OcrEngine.Create(OcrEngineType.Google).IsInstalled)
            //{
            //    ClipSync = new ClipSyncServer();
            //    ClipSync.Start();
            //}

            Running = true;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);       

            Settings = Settings.Load();
            MainForm = new MainForm();
            SettingsForm = new SettingsForm();
            CompactForm = new WPFCompactForm();
            PopupForm = new WPFCompactForm();
            PopupToolForm = new CompactToolForm();

            MainForm.Initialize();

            //System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(CompactForm);
            //CompactForm.Show();
            
            Application.Run(MainForm);

            Settings.Save();
            Running = false;
            Environment.Exit(0);
        }

        public static void SetEngines(Dictionary<OcrEngineType, OcrEngine> engines)
        {
            Program.engines = engines;
        }

        public static void SetTranslators(Dictionary<TranslatorType, Translator> translators)
        {
            Program.translators = translators;
        }

        public static void UpdateSettings()
        {
            try
            {
                if (MainForm != null)
                {
                    MainForm.UpdateSettings();
                }
            }
            catch
            {
            }
        }

        private static void RunLanguageExtractor()
        {
            LanguagePackExtractor.Run();
        }

        private static void RunNicomsoftOcrTests()
        {
            while (true)
            {
                OcrEngine engine = OcrEngine.Create(OcrEngineType.Nicomsoft);
                MessageBox.Show("Installed: " + engine.IsInstalled);
                Stopwatch sw1 = new Stopwatch();
                sw1.Start();
                if (engine.IsInstalled)
                {
                    OcrResult result = engine.LoadFile("image1.png", "ko");
                    if (result != null)
                    {
                        sw1.Stop();
                        if (!string.IsNullOrEmpty(result.Text))
                        {
                            Clipboard.SetText(result.Text);
                        }
                        MessageBox.Show(result.ResultType + Environment.NewLine + result.Text + " " + result.Error + " " + sw1.Elapsed);
                    }
                }
            }
        }

        private static void RunGoogleOcrTests()
        {
            while (true)
            {
                OcrEngine abbyy = OcrEngine.Create(OcrEngineType.Google);//OcrEngineType.ABBYY);
                MessageBox.Show("Installed: " + abbyy.IsInstalled);
                Stopwatch sw1 = new Stopwatch();
                sw1.Start();
                if (abbyy.IsInstalled)
                {
                    OcrResult result = abbyy.LoadFile("image1.png", "ko");
                    if (result != null)
                    {
                        sw1.Stop();
                        if (!string.IsNullOrEmpty(result.Text))
                        {
                            Clipboard.SetText(result.Text);
                        }
                        MessageBox.Show(result.ResultType + Environment.NewLine + result.Text + " " + sw1.Elapsed);
                    }
                }
            }
        }
    }
}
