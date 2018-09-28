using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Ocr.UI
{
    public class Settings
    {
        public MainViewSettings Main { get; set; }
        public TranslateSettings Translate { get; set; }
        public ClipboardSettings Clipboard { get; set; }
        public WindowSettings Window { get; set; }
        public AutoTyperSettings AutoTyper { get; set; }
        public CompactViewSettings CompactView { get; set; }
        public EditableCaptureAreaSettings EditableCaptureArea { get; set; }
        public CaptureAreaSettings CaptureArea { get; set; }
        public CaptureAreaStyleSettings CaptureAreaStyle { get; set; }
        public MiniOutputSettings MiniOutput { get; set; }
        public HotkeysSettings Hotkeys { get; set; }
        public TargetLanguageSettings TargetLanguage { get; set; }
        public LanguageProfilesSettings LanguageProfiles { get; set; }
        public LanguageFiltersSettings LanguageFilters { get; set; }
        public ServerInfoSettings ServerInfo { get; set; }
        public KeyInfoSettings KeyInfo { get; set; }

        private static string settingsFile;
        private static string SettingsFile
        {
            get
            {
                if (string.IsNullOrEmpty(settingsFile))
                {
                    settingsFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Settings.xml");
                }
                return settingsFile;
            }
        }

        public Settings()
        {
            SetDefault();
        }

        public static Settings Load()
        {
            Settings settings = null;
            if (File.Exists(SettingsFile))
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(SettingsFile))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                        settings = (Settings)serializer.Deserialize(reader);
                    }
                }
                catch
                {
                }
            }
            if (settings == null)
                settings = new Settings();
            settings.FixConflicts();
            return settings;
        }

        public void Save()
        {
            try
            {
                FixConflicts();

                XmlWriterSettings xmlSettings = new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineOnAttributes = true,
                    OmitXmlDeclaration = true
                };

                using (XmlWriter writer = XmlWriter.Create(SettingsFile, xmlSettings))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    serializer.Serialize(writer, this);
                }
            }
            catch
            {
            }
        }

        public void FixConflicts()
        {
            if (!Clipboard.DontSaveToClipboard &&
                !Clipboard.SaveImageToClipboard &&
                !Clipboard.SaveOriginalTextToClipboard &&
                !Clipboard.SaveTranslatedTextToClipboard)
            {
                Clipboard.DontSaveToClipboard = true;
            }

            if (!ServerInfo.Client && !ServerInfo.Server && !ServerInfo.Proxy)
            {
                ServerInfo.Client = true;
            }
        }

        private void SetDefault()
        {
            Main = new MainViewSettings();
            Translate = new TranslateSettings();
            Clipboard = new ClipboardSettings();
            Window = new WindowSettings();
            AutoTyper = new AutoTyperSettings();
            CompactView = new CompactViewSettings();
            EditableCaptureArea = new EditableCaptureAreaSettings();
            CaptureArea = new CaptureAreaSettings();
            CaptureAreaStyle = new CaptureAreaStyleSettings();
            MiniOutput = new MiniOutputSettings();
            Hotkeys = new HotkeysSettings();
            TargetLanguage = new TargetLanguageSettings();
            LanguageProfiles = new LanguageProfilesSettings();
            LanguageFilters = new LanguageFiltersSettings();
            ServerInfo = new ServerInfoSettings();
            KeyInfo = new KeyInfoSettings();

            Main.SetDefault();
            Translate.SetDefault();
            Clipboard.SetDefault();
            Window.SetDefault();
            AutoTyper.SetDefault();
            CompactView.SetDefault();
            EditableCaptureArea.SetDefault();
            CaptureArea.SetDefault();
            CaptureAreaStyle.SetDefault();
            MiniOutput.SetDefault();
            Hotkeys.SetDefault();
            TargetLanguage.SetDefault();
            LanguageProfiles.SetDefault();
            LanguageFilters.SetDefault();
            ServerInfo.SetDefault();
            KeyInfo.SetDefault();
        }

        public class MainViewSettings
        {
            public bool Enabled { get; set; }
            public bool Topmost { get; set; }
            public bool Wordwrap { get; set; }
            public bool PreserveNewLineCharacters { get; set; }
            public bool ImagePanel { get; set; }
            public bool FixedImagePanel { get; set; }

            public int Width { get; set; }
            public int Height { get; set; }

            public void SetDefault()
            {
                Enabled = true;
            }
        }

        public class TranslateSettings
        {
            public bool AutoTranslate { get; set; }
            public float AutoTranslateDelay { get; set; }

            public void SetDefault()
            {
                AutoTranslate = false;
                AutoTranslateDelay = 1;
            }
        }

        public class ClipboardSettings
        {
            public bool DontSaveToClipboard { get; set; }
            public bool SaveImageToClipboard { get; set; }
            public bool SaveOriginalTextToClipboard { get; set; }
            public bool SaveTranslatedTextToClipboard { get; set; }
            public bool DontSaveEditableAreaToClipboard { get; set; }

            public void SetDefault()
            {
                DontSaveToClipboard = true;
            }
        }

        public class WindowSettings
        {
            public bool ShowInTaskBar { get; set; }
            public bool ShowInTray { get; set; }
            public bool MinimizeToTray { get; set; }
            public bool CloseToTray { get; set; }

            public void SetDefault()
            {
            }
        }

        public class AutoTyperSettings
        {
            public bool Unicode { get; set; }
            public bool RequireKeyboard { get; set; }
            public int CancelOnMouseMove { get; set; }
            public int KeyDelay { get; set; }
            public string Hotkey { get; set; }

            public void SetDefault()
            {
            }
        }

        public class CompactViewSettings
        {
            public bool Enabled { get; set; }
            public bool Topmost { get; set; }
            public bool Wordwrap { get; set; }
            public bool Autosize { get; set; }
            public bool AutoReposition { get; set; }
            public bool ClickThrough { get; set; }
            public bool Toolbar { get; set; }
            public bool PreserveNewlineCharacters { get; set; }
            public CompactViewLayout Layout { get; set; }
            public CompactViewDock Dock { get; set; }
            public int MaxAutoSizeWidth { get; set; }
            public int MaxAutoSizeHeight { get; set; }
            public int PaddingLeft { get; set;}
            public int PaddingTop { get; set; }
            public int PaddingRight { get; set; }
            public int PaddingBottom { get; set; }
            public string Font { get; set; }
            public string Color { get; set; }
            public string BackColor { get; set; }
            public string ShadowColor { get; set; }
            public int ShadowOffsetX { get; set; }
            public int ShadowOffsetY { get; set; }
            public int ShadowOpacity { get; set; }
            public int BackOpacity { get; set; }
            public int Opacity { get; set; }

            public void SetDefault()
            {
            }
        }

        public class EditableCaptureAreaSettings
        {
            public float UpdateDelay { get; set; }
            public int DeactivatedBackHue { get; set; }
            public int DeactivatedBorderHue { get; set; }
            public int DeactivatedOpacityHue { get; set; }

            public void SetDefault()
            {
            }
        }

        public class CaptureAreaSettings
        {
            public string BackColor { get; set; }
            public string BorderColor { get; set; }
            public int BackOpacity { get; set; }
            public int BorderOpacity { get; set; }
            public int Opacity { get; set; }
            public bool ShowMiniOutput { get; set; }

            public void SetDefault()
            {
            }
        }

        public class CaptureAreaStyleSettings
        {
            public string Back { get; set; }
            public string Border { get; set; }            
            public bool DashedBorder { get; set; }

            public void SetDefault()
            {
                DashedBorder = true;
            }
        }

        public class MiniOutputSettings
        {
            public MiniOutputPosition Position { get; set; }
            public CompactViewLayout Layout { get; set; }
            public bool Styled { get; set; }
            public bool EmulateCaptureAreaTextLayout { get; set; }
            public bool ClickThrough { get; set; }
            public int PaddingLeft { get; set; }
            public int PaddingTop { get; set; }
            public int PaddingRight { get; set; }
            public int PaddingBottom { get; set; }
            public string Font { get; set; }
            public string TextColor { get; set; }
            public string BackColor { get; set; }
            public string ShadowColor { get; set; }
            public int ShadowOffsetX { get; set; }
            public int ShadowOffsetY { get; set; }
            public int ShadowOpacity { get; set; }
            public int BackOpacity { get; set; }
            public int Opacity { get; set; }

            public void SetDefault()
            {
            }
        }

        public class HotkeysSettings
        {
            public string SetLanguageProfile1 { get; set; }
            public string SetLanguageProfile2 { get; set; }
            public string SetLanguageProfile3 { get; set; }
            public string SetLanguageProfile4 { get; set; }
            public string SetLanguageProfile5 { get; set; }
            public string CaptureArea { get; set; }
            public string CaptureAreaFreezeScreen { get; set; }
            public string CaptureAreaEditable { get; set; }
            public string CancelCapture { get; set; }
            public string CloseEditableCaptureArea { get; set; }
            public string SwitchTextDirection { get; set; }
            public string AutoType { get; set; }
            public string CancelAutoType { get; set; }
            public string TranslateClipboard { get; set; }

            public void SetDefault()
            {
                SetLanguageProfile1 = null;
                SetLanguageProfile2 = null;
                SetLanguageProfile3 = null;
                SetLanguageProfile4 = null;
                SetLanguageProfile5 = null;
            }
        }

        public class TargetLanguageSettings
        {
            public string TargetLanguage { get; set; }

            public void SetDefault()
            {
                TargetLanguage = null;
            }
        }

        public class LanguageProfilesSettings
        {
            public string Slot1 { get; set; }
            public string Slot2 { get; set; }
            public string Slot3 { get; set; }
            public string Slot4 { get; set; }
            public string Slot5 { get; set; }

            public void SetDefault()
            {
                Slot1 = null;
                Slot2 = null;
                Slot3 = null;
                Slot4 = null;
                Slot5 = null;
            }
        }

        public class LanguageFiltersSettings
        {
            public string Languages { get; set; }

            public void SetDefault()
            {
            }
        }

        public class ServerInfoSettings
        {
            public bool Client { get; set; }
            public bool Server { get; set; }
            public bool Proxy { get; set; }
            public string IP1 { get; set; }
            public string IP2 { get; set; }

            public void SetDefault()
            {
                Client = true;
            }
        }

        public class KeyInfoSettings
        {
            public string Key { get; set; }

            public void SetDefault()
            {
            }
        }
    }

    public enum CompactViewLayout
    {
        Original,
        Translated,
        BothStackVertical,
        BothStackHorizontal
    }

    public enum CompactViewDock
    {
        Float,
        MousePosition,

        ScreenTopLeft,
        ScreenTopCenter,
        ScreenTopRight,
        ScreenMiddleLeft,
        ScreenMiddleCenter,
        ScreenMiddleRight,
        ScreenBottomLeft,
        ScreenBottomRight,
    }

    public enum MiniOutputPosition
    {
        CaptureAreaTopLeft,
        CaptureAreaTopRignt,
        CaptureAreaBottomLeft,
        CaptureAreaBottomRight,
        CaptureAreaLeft,
        CaptureAreaRight,
        CaptureAreaTop,
        CaptureAreaBottom
    }
}