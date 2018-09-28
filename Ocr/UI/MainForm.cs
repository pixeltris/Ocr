using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ocr.UI
{
    public partial class MainForm : Form
    {
        private bool isUpdating = false;
        private Input.Hotkeys hotkeys = new Input.Hotkeys();

        private Dictionary<ToolStripComboBox, int> comboBoxPrevIndex = new Dictionary<ToolStripComboBox, int>();

        private Dictionary<OcrEngineType, OcrEngine> engines = new Dictionary<OcrEngineType, OcrEngine>();
        private Dictionary<TranslatorType, Translator> translators = new Dictionary<TranslatorType, Translator>();
        private Dictionary<string, string> languages = new Dictionary<string, string>();//filtered languages
        private Dictionary<string, string> allLanguages = new Dictionary<string, string>();//all languages

        private List<string> translatableLanguages = new List<string>();
        private List<string> ocrableLanguages = new List<string>();
        private List<OcrEngineType> availableEngines = new List<OcrEngineType>();

        private ContextMenu quickSlot;
        private MenuItem quickSlot1;
        private MenuItem quickSlot2;
        private MenuItem quickSlot3;
        private MenuItem quickSlot4;
        private MenuItem quickSlot5;

        public MainForm()
        {
            InitializeComponent();            
        }        

        public void Initialize()
        {
            isUpdating = true;

            Program.SetEngines(engines);
            Program.SetTranslators(translators);

            foreach (var value in Enum.GetValues(typeof(OcrEngineType)))
            {
                OcrEngineType type = (OcrEngineType)value;
                OcrEngine engine = OcrEngine.Create(type);
                if (engine != null)// && engine.IsInstalled)
                {
                    engines[type] = engine;
                    ocrEngineToolStripComboBox.Items.Add(type);
                }
            }
            if (ocrEngineToolStripComboBox.Items.Count > 0)
                ocrEngineToolStripComboBox.SelectedIndex = 0;            

            foreach (var value in Enum.GetValues(typeof(TranslatorType)))
            {
                TranslatorType type = (TranslatorType)value;
                Translator translator = Translator.Create(type);
                if (translator != null && translator.IsEnabled)
                {
                    translators[type] = translator;
                    translateApiToolStripComboBox.Items.Add(type);
                }
            }
            if (translateApiToolStripComboBox.Items.Count > 0)
                translateApiToolStripComboBox.SelectedIndex = 0;

            foreach (var value in Enum.GetValues(typeof(OcrEngineProfile)))
            {
                OcrEngineProfile profile = (OcrEngineProfile)value;
                if (profile < OcrEngineProfile.Linear)
                {
                    ocrProfileToolStripComboBox.Items.Add(profile);
                }
                else
                {
                    for (int i = 2; i <= Program.ProfilesCount; i++)
                    {
                        ocrProfileToolStripComboBox.Items.Add(profile + " " + i + "x");
                    }
                }
            }
            if (ocrProfileToolStripComboBox.Items.Count > 0)
                ocrProfileToolStripComboBox.SelectedIndex = 0;

            UpdateLanguages();

            ocrEngineToolStripComboBox.ComboBox.PreviewKeyDown += ComboBox_PreviewKeyDown;
            ocrProfileToolStripComboBox.ComboBox.PreviewKeyDown += ComboBox_PreviewKeyDown;
            translateApiToolStripComboBox.ComboBox.PreviewKeyDown += ComboBox_PreviewKeyDown;
            translateFromToolStripComboBox.ComboBox.PreviewKeyDown += ComboBox_PreviewKeyDown;
            translateToToolStripComboBox.ComboBox.PreviewKeyDown += ComboBox_PreviewKeyDown;

            ocrEngineToolStripComboBox.ComboBox.MouseWheel += toolStripComboBox_MouseWheel;
            ocrProfileToolStripComboBox.ComboBox.MouseWheel += toolStripComboBox_MouseWheel;
            translateApiToolStripComboBox.ComboBox.MouseWheel += toolStripComboBox_MouseWheel;
            translateFromToolStripComboBox.ComboBox.MouseWheel += toolStripComboBox_MouseWheel;
            translateToToolStripComboBox.ComboBox.MouseWheel += toolStripComboBox_MouseWheel;

            comboBoxPrevIndex[ocrEngineToolStripComboBox] = 0;
            comboBoxPrevIndex[ocrProfileToolStripComboBox] = 0;
            comboBoxPrevIndex[translateApiToolStripComboBox] = 0;
            comboBoxPrevIndex[translateFromToolStripComboBox] = 0;
            comboBoxPrevIndex[translateToToolStripComboBox] = 0;

            translateFromToolStripComboBox.ComboBox.DrawItem += LanguageComboBox_DrawItem;
            translateFromToolStripComboBox.ComboBox.DrawMode = DrawMode.OwnerDrawFixed;

            translateToToolStripComboBox.ComboBox.DrawItem += LanguageComboBox_DrawItem;
            translateToToolStripComboBox.ComboBox.DrawMode = DrawMode.OwnerDrawFixed;

            ocrEngineToolStripComboBox.ComboBox.DrawItem += EngineComboBox_DrawItem;
            ocrEngineToolStripComboBox.ComboBox.DrawMode = DrawMode.OwnerDrawFixed;

            ocrImagePanel.ImageChanged += ocrImagePanel_ImageChanged;
            OcrHelper.Complete += OcrHelper_Complete;
            OcrHelper.Start += OcrHelper_Start;
            TranslateHelper.Start += TranslateHelper_Start;
            TranslateHelper.Complete += TranslateHelper_Complete;

            string from = Program.Settings.LanguageProfiles.Slot1;
            string to = Program.Settings.TargetLanguage.TargetLanguage;
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {                
                int fromIndex = translateFromToolStripComboBox.Items.IndexOf(from);
                int toIndex = translateToToolStripComboBox.Items.IndexOf(to);
                if(fromIndex >= 0 && toIndex >= 0)
                {
                    translateFromToolStripComboBox.SelectedIndex = fromIndex;
                    translateToToolStripComboBox.SelectedIndex = toIndex;
                }
            }

            hotkeys.KeyPressed += hotkeys_KeyPressed;
            hotkeys.ClearHotKey();
            hotkeys.RegisterHotKey(Program.Settings.AutoTyper.Hotkey);

            isUpdating = false;
            SettingsChanged();

            OcrNetwork.Update();
        }        

        void OcrHelper_Start()
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate { OcrHelper_Start(); });
                    return;
                }
            }
            catch
            {
                return;
            }
            originalRichTextBox.Parent.BackColor = Color.DodgerBlue;
        }

        void OcrHelper_Complete(OcrResult result)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate { OcrHelper_Complete(result); });
                    return;
                }
            }
            catch
            {
                return;
            }

            originalRichTextBox.Parent.BackColor = System.Drawing.SystemColors.ControlDark;
            originalRichTextBox.Text = result.Text;
            if (result.Succes && !string.IsNullOrWhiteSpace(result.Text))
            {
                if (Program.Settings.Clipboard.SaveOriginalTextToClipboard)
                {
                    Clipboard.SetText(result.Text);
                }
                if (Program.Settings.Translate.AutoTranslate)
                {
                    TranslateHelper.Translate(result.Text);
                }
            }
        }

        void TranslateHelper_Start()
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate { TranslateHelper_Start(); });
                    return;
                }
            }
            catch
            {
                return;
            }
            translatedRichTextBox.Parent.BackColor = Color.DodgerBlue;
        }

        void TranslateHelper_Complete(string value)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate { TranslateHelper_Complete(value); });
                    return;
                }
            }
            catch
            {
                return;
            }
            
            translatedRichTextBox.Parent.BackColor = System.Drawing.SystemColors.ControlDark;
            translatedRichTextBox.Text = value;
            if (!string.IsNullOrWhiteSpace(value) && Program.Settings.Clipboard.SaveTranslatedTextToClipboard)
            {
                Clipboard.SetText(value);
            }
        }        

        void ocrImagePanel_ImageChanged(object sender, EventArgs e)
        {            
            if (ocrImagePanel.Image != null && ocrImagePanel.SelectionImage != null)
            {
                RunOcr();                
            }
        }

        void hotkeys_KeyPressed(object sender, Input.KeyPressedEventArgs e)
        {            
            string text = Clipboard.GetText();
            if(!string.IsNullOrWhiteSpace(text))
            {
                Input.AutoTyper.SendInput(text, Program.ActiveLanguageFrom);
            }
        }

        void LanguageComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();            

            if(e.Index >= 0)
            {
                ToolStripComboBox owner = GetComboBoxOwner(sender as ComboBox);
                string language = owner.Items[e.Index].ToString();
                Color color = e.ForeColor;
                Color backColor = Color.White;
                if (owner == translateFromToolStripComboBox && !ocrableLanguages.Contains(language))
                {
                    backColor = Color.LightGray;
                }
                else
                {
                    backColor = Color.White;
                }
                if (!translatableLanguages.Contains(language))
                {
                    color = Color.Red; 
                }
                else
                {
                    color = Color.Black; 
                }

                if (e.BackColor == owner.BackColor)
                {
                    using (Brush brush = new SolidBrush(backColor))
                    {
                        e.Graphics.FillRectangle(brush, e.Bounds);
                    }
                }                

                using (Brush brush = new SolidBrush(color))
                {
                    e.Graphics.DrawString(language, e.Font, brush, e.Bounds.X, e.Bounds.Y);
                }
            }
        }

        private void UpdateLanguageComboBoxStyle()
        {
            UpdateLanguageComboBoxStyle(translateFromToolStripComboBox);
            UpdateLanguageComboBoxStyle(translateToToolStripComboBox);
        }

        private void UpdateLanguageComboBoxStyle(ToolStripComboBox comboBox)
        {
            if (comboBox == translateFromToolStripComboBox && comboBox.SelectedItem != null && !ocrableLanguages.Contains(comboBox.SelectedItem.ToString()))
            {
                comboBox.BackColor = Color.LightGray;
            }
            else
            {
                comboBox.BackColor = Color.White;
            }

            if (comboBox.SelectedItem == null || translatableLanguages.Contains(comboBox.SelectedItem.ToString()))
            {
                comboBox.ForeColor = Color.Black;
            }
            else
            {
                comboBox.ForeColor = Color.Red;
            }
        }

        private ToolStripComboBox GetComboBoxOwner(ComboBox comboBox)
        {
            if (comboBox == translateFromToolStripComboBox.ComboBox)
            {
                return translateFromToolStripComboBox;
            }
            else if (comboBox == translateToToolStripComboBox.ComboBox)
            {
                return translateToToolStripComboBox;
            }
            return null;
        }

        public void UpdateLanguages()
        {
            string currentFrom = translateFromToolStripComboBox.SelectedItem == null ? null : translateFromToolStripComboBox.SelectedItem.ToString();
            string currentTo = translateToToolStripComboBox.SelectedItem == null ? null : translateToToolStripComboBox.SelectedItem.ToString();

            translateFromToolStripComboBox.Items.Clear();
            translateToToolStripComboBox.Items.Clear();

            List<string> languageFilters = new List<string>();
            if (!string.IsNullOrEmpty(Program.Settings.LanguageFilters.Languages))
            {
                string[] splitted = Program.Settings.LanguageFilters.Languages.Split(Program.LanguageFilterSplitChar);
                foreach (string language in splitted)
                {
                    if (!string.IsNullOrWhiteSpace(language))
                    {
                        languageFilters.Add(language.Trim());
                    }
                }
            }

            languages.Clear();
            allLanguages.Clear();

            foreach (Translator translator in translators.Values)
            {
                foreach (string language in translator.SupportedLanguages.Keys)
                {
                    CultureInfo culture = new CultureInfo(language);
                    if (!languages.ContainsKey(culture.DisplayName) &&
                        (languageFilters.Count == 0 || languageFilters.Contains(culture.DisplayName)))
                    {
                        languages[culture.DisplayName] = culture.Name;
                    }
                    if (!allLanguages.ContainsKey(culture.DisplayName))
                    {
                        allLanguages[culture.DisplayName] = culture.Name;
                    }
                }
            }

            foreach (string language in languages.Keys.OrderBy(x => x))
            {
                translateFromToolStripComboBox.Items.Add(language);
                translateToToolStripComboBox.Items.Add(language);
            }
            if (translateFromToolStripComboBox.Items.Count > 0)
                translateFromToolStripComboBox.SelectedIndex = 0;
            if (translateToToolStripComboBox.Items.Count > 0)
                translateToToolStripComboBox.SelectedIndex = 0;

            translateFromToolStripComboBox.DropDownWidth = translateFromToolStripComboBox.ComboBox.AutoDropDownWidth();
            translateToToolStripComboBox.DropDownWidth = translateToToolStripComboBox.ComboBox.AutoDropDownWidth();

            if (!string.IsNullOrWhiteSpace(currentFrom) && translateFromToolStripComboBox.Items.Contains(currentFrom))
            {
                translateFromToolStripComboBox.SelectedIndex = translateFromToolStripComboBox.Items.IndexOf(currentFrom);
            }

            if (!string.IsNullOrWhiteSpace(currentTo) && translateToToolStripComboBox.Items.Contains(currentFrom))
            {
                translateToToolStripComboBox.SelectedIndex = translateToToolStripComboBox.Items.IndexOf(currentTo);
            }
        }

        void EngineComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index >= 0)
            {
                ToolStripComboBox owner = ocrEngineToolStripComboBox;
                string ocrEngine = owner.Items[e.Index].ToString();
                bool available = IsEngineAvailable(ocrEngine);
                Color color = e.ForeColor;
                Color backColor = available ? Color.White : Color.LightGray;

                if (e.BackColor == owner.BackColor)
                {
                    using (Brush brush = new SolidBrush(backColor))
                    {
                        e.Graphics.FillRectangle(brush, e.Bounds);
                    }
                }

                using (Brush brush = new SolidBrush(color))
                {
                    e.Graphics.DrawString(ocrEngine, e.Font, brush, e.Bounds.X, e.Bounds.Y);
                }
            }
        }

        private void UpdateEngineComboBoxStyle()
        {
            ToolStripComboBox comboBox = ocrEngineToolStripComboBox;
            if (comboBox.SelectedItem != null && !IsEngineAvailable(comboBox.SelectedItem.ToString()))
            {
                comboBox.BackColor = Color.LightGray;
            }
            else
            {
                comboBox.BackColor = Color.White;
            }
        }

        private bool IsEngineAvailable(string engineName)
        {
            OcrEngineType engineType;
            if (Enum.TryParse<OcrEngineType>(engineName, out engineType))
            {
                return availableEngines.Contains(engineType);
            }
            return false;
        }

        public void UpdateSettings()
        {
            if (IsDisposed)
                return;

            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { UpdateSettings(); });
                return;
            }

            isUpdating = true;

            translatableLanguages.Clear();
            ocrableLanguages.Clear();

            if (Program.ActiveOcrEngine != null)
            {
                if (Program.ActiveOcrEngine.IsInstalled)
                {
                    foreach (string language in Program.ActiveOcrEngine.SupportedLanguages.Keys)
                    {
                        CultureInfo culture = CultureInfo.GetCultureInfo(language);
                        if (!ocrableLanguages.Contains(culture.DisplayName))
                        {
                            ocrableLanguages.Add(culture.DisplayName);
                        }
                    }
                }
                else
                {
                    OcrEngineType engineType = OcrEngine.GetType(Program.ActiveOcrEngine.GetType());
                    foreach (Dictionary<OcrEngineType, List<string>> remoteTypes in OcrNetwork.RemoteTypes.Values)
                    {
                        if (remoteTypes.ContainsKey(engineType))
                        {
                            List<string> supportedLanguages = remoteTypes[engineType];
                            foreach (string language in supportedLanguages)
                            {
                                // Remote PC may support different cultures
                                //CultureInfo culture = CultureInfo.GetCultureInfo(language);
                                CultureInfo culture = null;
                                if (LocaleHelper.TryParseCulture(language, out culture) && !ocrableLanguages.Contains(culture.DisplayName))
                                {
                                    ocrableLanguages.Add(culture.DisplayName);
                                }
                            }
                        }
                    }
                }
            }

            if (Program.ActiveTranslator != null)
            {
                foreach (string language in Program.ActiveTranslator.SupportedLanguages.Keys)
                {
                    CultureInfo culture = CultureInfo.GetCultureInfo(language);
                    if (!translatableLanguages.Contains(culture.DisplayName))
                    {
                        translatableLanguages.Add(culture.DisplayName);
                    }
                }
            }

            availableEngines.Clear();
            foreach (var value in Enum.GetValues(typeof(OcrEngineType)))
            {
                OcrEngineType type = (OcrEngineType)value;
                if (RemoteOcr.IsRemote(type) || OcrEngine.Create(type).IsInstalled)
                {
                    availableEngines.Add(type);
                }
            }

            UpdateLanguageComboBoxStyle();
            UpdateEngineComboBoxStyle();
            RunOcr();

            isUpdating = false;
        }        

        private void SettingsChanged()
        {
            if (isUpdating)
            {
                return;
            }

            if (ocrEngineToolStripComboBox.SelectedIndex >= 0)
            {
                Program.ActiveOcrEngine = engines[(OcrEngineType)ocrEngineToolStripComboBox.SelectedItem];
            }
            if (translateApiToolStripComboBox.SelectedIndex >= 0)
            {
                Program.ActiveTranslator = translators[(TranslatorType)translateApiToolStripComboBox.SelectedItem];
            }
            if (ocrProfileToolStripComboBox.SelectedIndex >= 0)
            {
                Program.ActiveOcrProfileIndex = ocrProfileToolStripComboBox.SelectedIndex;
            }
            if (translateFromToolStripComboBox.SelectedItem != null)
            {
                Program.ActiveLanguageFrom = languages[translateFromToolStripComboBox.SelectedItem.ToString()];
            }
            if (translateToToolStripComboBox.SelectedItem != null)
            {
                Program.ActiveLanguageTo = languages[translateToToolStripComboBox.SelectedItem.ToString()];
            }

            Program.UpdateSettings();
        }

        public void TextTranslated()
        {

        }

        void ComboBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                toolStripComboBox_Validated(sender, e);
            }
        }

        private void toolStripComboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void toolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {            
            ToolStripComboBox comboBox = sender as ToolStripComboBox;
            comboBoxPrevIndex[comboBox] = comboBox.SelectedIndex;

            if (comboBox != ocrProfileToolStripComboBox)
            {
                ocrImagePanel.Focus();
            }

            if (comboBox == translateToToolStripComboBox || comboBox == translateFromToolStripComboBox)
            {
                UpdateLanguageComboBoxStyle(comboBox);
            }            

            SettingsChanged();
        }

        private void toolStripComboBox_Validated(object sender, EventArgs e)
        {
            ToolStripComboBox comboBox = null;
            if (sender is ComboBox)
                comboBox = sender.GetMemberValue("owner") as ToolStripComboBox;
            else
                comboBox = sender as ToolStripComboBox;

            string text = comboBox.Text;
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                string itemText = comboBox.Items[i].ToString();
                if (text.Equals(itemText, StringComparison.OrdinalIgnoreCase))
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }
            if (comboBox.Items.Count > 0)
                comboBox.SelectedIndex = comboBoxPrevIndex[comboBox];
        }

        private void toolStripComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            ToolStripComboBox comboBox = sender as ToolStripComboBox;
            if (e.KeyCode == Keys.Enter)
            {
                toolStripComboBox_Validated(sender, e);
                //comboBox.SelectAll();
                comboBox.SelectionStart = 0;
                comboBox.SelectionLength = 0;
                ocrImagePanel.Focus();
                e.Handled = true;
            }
        }

        private void switchTranslateToolStripButton_Click(object sender, EventArgs e)
        {
            isUpdating = true;

            int fromIndex = translateFromToolStripComboBox.SelectedIndex;
            int toIndex = translateToToolStripComboBox.SelectedIndex;

            if (fromIndex != toIndex)
            {
                translateFromToolStripComboBox.SelectedIndex = toIndex;
                translateToToolStripComboBox.SelectedIndex = fromIndex;
            }

            isUpdating = false;
            SettingsChanged();
        }

        private void translateToolStripButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(originalRichTextBox.Text))
            {
                TranslateHelper.Translate(originalRichTextBox.Text);
            }
        }

        private void languageQuickslotToolStripButton_Click(object sender, EventArgs e)
        {
            Point pos = new Point(
                languageQuickslotToolStripButton.Bounds.X,
                languageQuickslotToolStripButton.Bounds.Y);

            pos.Y += languageQuickslotToolStripButton.Bounds.Height;

            if (quickSlot == null)
            {
                quickSlot = new ContextMenu();
                quickSlot1 = new MenuItem("Slot 1", quickSlot_Click) { Enabled = false };
                quickSlot2 = new MenuItem("Slot 2", quickSlot_Click) { Enabled = false };
                quickSlot3 = new MenuItem("Slot 3", quickSlot_Click) { Enabled = false };
                quickSlot4 = new MenuItem("Slot 4", quickSlot_Click) { Enabled = false };
                quickSlot5 = new MenuItem("Slot 5", quickSlot_Click) { Enabled = false };

                quickSlot.MenuItems.Add(quickSlot1);
                quickSlot.MenuItems.Add(quickSlot2);
                quickSlot.MenuItems.Add(quickSlot3);
                quickSlot.MenuItems.Add(quickSlot4);
                quickSlot.MenuItems.Add(quickSlot5);                
            }

            quickSlot1.Text = "Slot 1";
            quickSlot2.Text = "Slot 2";
            quickSlot3.Text = "Slot 3";
            quickSlot4.Text = "Slot 4";
            quickSlot5.Text = "Slot 5";

            quickSlot1.Enabled = false;
            quickSlot2.Enabled = false;
            quickSlot3.Enabled = false;
            quickSlot4.Enabled = false;
            quickSlot5.Enabled = false;

            if (!string.IsNullOrEmpty(Program.Settings.TargetLanguage.TargetLanguage) &&
                languages.ContainsKey(Program.Settings.TargetLanguage.TargetLanguage))
            {
                if (!string.IsNullOrEmpty(Program.Settings.LanguageProfiles.Slot1) &&
                    languages.ContainsKey(Program.Settings.LanguageProfiles.Slot1))
                {
                    quickSlot1.Text = Program.Settings.LanguageProfiles.Slot1 + " - " + Program.Settings.TargetLanguage.TargetLanguage;
                    quickSlot1.Enabled = true;
                }

                if (!string.IsNullOrEmpty(Program.Settings.LanguageProfiles.Slot2) &&
                    languages.ContainsKey(Program.Settings.LanguageProfiles.Slot2))
                {
                    quickSlot2.Text = Program.Settings.LanguageProfiles.Slot2 + " - " + Program.Settings.TargetLanguage.TargetLanguage;
                    quickSlot2.Enabled = true;
                }

                if (!string.IsNullOrEmpty(Program.Settings.LanguageProfiles.Slot3) &&
                    languages.ContainsKey(Program.Settings.LanguageProfiles.Slot3))
                {
                    quickSlot3.Text = Program.Settings.LanguageProfiles.Slot3 + " - " + Program.Settings.TargetLanguage.TargetLanguage;
                    quickSlot3.Enabled = true;
                }

                if (!string.IsNullOrEmpty(Program.Settings.LanguageProfiles.Slot4) &&
                    languages.ContainsKey(Program.Settings.LanguageProfiles.Slot4))
                {
                    quickSlot4.Text = Program.Settings.LanguageProfiles.Slot4 + " - " + Program.Settings.TargetLanguage.TargetLanguage;
                    quickSlot4.Enabled = true;
                }

                if (!string.IsNullOrEmpty(Program.Settings.LanguageProfiles.Slot5) &&
                    languages.ContainsKey(Program.Settings.LanguageProfiles.Slot5))
                {
                    quickSlot5.Text = Program.Settings.LanguageProfiles.Slot5 + " - " + Program.Settings.TargetLanguage.TargetLanguage;
                    quickSlot5.Enabled = true;
                }
            }

            quickSlot.Show(this, pos);
        }

        void quickSlot_Click(object sender, EventArgs e)
        {
            string currentToLanguage = translateToToolStripComboBox.SelectedItem == null ? null : translateToToolStripComboBox.SelectedItem.ToString();
            string newToLanguage = Program.Settings.TargetLanguage.TargetLanguage;

            string currentFromLanguage = translateFromToolStripComboBox.SelectedItem == null ? null : translateFromToolStripComboBox.SelectedItem.ToString();
            string newFromLanguage = (sender as MenuItem).Text;

            if(!string.IsNullOrWhiteSpace(newFromLanguage))
            {
                newFromLanguage = newFromLanguage.Replace(" - " + Program.Settings.TargetLanguage.TargetLanguage, string.Empty);
            }

            if (newFromLanguage != null && currentFromLanguage != newFromLanguage)
            {
                translateFromToolStripComboBox.SelectedIndex = translateFromToolStripComboBox.Items.IndexOf(newFromLanguage);
            }

            if (newToLanguage != null && currentToLanguage != newToLanguage)
            {
                translateToToolStripComboBox.SelectedIndex = translateToToolStripComboBox.Items.IndexOf(newToLanguage);
            }            
        }

        private void captureAreaToolStripButton_Click(object sender, EventArgs e)
        {
            CaptureArea(SnippingMode.Default);
        }

        private void captureAreaFreezeToolStripButton_Click(object sender, EventArgs e)
        {
            CaptureArea(SnippingMode.Freeze);
        }

        private void captureAreaEditableToolStripButton_Click(object sender, EventArgs e)
        {
            CaptureArea(SnippingMode.Editable);
        }

        private void CaptureArea(SnippingMode mode)
        {
            this.Visible = false;

            Image image;
            Rectangle selection;
            if (SnippingTool.Snip(mode, out image, out selection) && selection.Width > 0 && selection.Height > 0)
            {                
                ocrImagePanel.SetImage(image, selection);
                if (ocrImagePanel.SelectionImage != null && Program.Settings.Clipboard.SaveImageToClipboard)
                {
                    Clipboard.SetImage(ocrImagePanel.SelectionImage);
                }                
            }

            this.Visible = true;
        }

        private void editImageSelectionToolStripButton_CheckedChanged(object sender, EventArgs e)
        {
            ocrImagePanel.Editable = editImageSelectionToolStripButton.Checked;
        }

        private void refreshImageToolStripButton_Click(object sender, EventArgs e)
        {
            if (ocrImagePanel.SelectionImage != null)
            {
                ocrImagePanel.SetImage(SnippingTool.CreateScreenshot(), ocrImagePanel.Selection);
            }
        }

        private void openFileToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                Image image = null;
                try
                {
                    image = Bitmap.FromFile(fileDialog.FileName);
                }
                catch
                {
                }
                if (image != null)
                {
                    ocrImagePanel.SetImage(image);
                }
            }
        }

        private void saveFileToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            if (ocrImagePanel.SelectionImage != null)
            {
                Clipboard.SetImage(ocrImagePanel.SelectionImage);
            }
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            Image image = null;
            try
            {
                image = Clipboard.GetImage();
            }
            catch
            {
            }
            if (image != null)
            {
                ocrImagePanel.SetImage(image);
            }
        }

        private void settingsToolStripButton_Click(object sender, EventArgs e)
        {
            //Program.SettingsForm.Show();
            SimpleSettingsForm settingsForm = new SimpleSettingsForm(allLanguages, languages);
            if (settingsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                UpdateLanguages();
                hotkeys.ClearHotKey();
                hotkeys.RegisterHotKey(Program.Settings.AutoTyper.Hotkey);
                OcrNetwork.Update();
            }
        }

        private void originalRichTextBox_TextChanged(object sender, EventArgs e)
        {
            if (Program.Settings.Translate.AutoTranslate && Program.ActiveTranslator != null)
            {
                TranslateHelper.Translate(originalRichTextBox.Text);
                //Program.ActiveTranslator.Translate(originalRichTextBox.Text, Program.ActiveLanguageFrom, Program.ActiveLanguageTo);
            }
        }

        private void RunOcr()
        {
            if (ocrImagePanel.Image != null && ocrImagePanel.SelectionImage != null && ocrImagePanel.SelectionImage.Width > 0 && ocrImagePanel.SelectionImage.Height > 0)
            {
                using (Image processedImage = ProcessImage(ocrImagePanel.SelectionImage, Program.ActiveOcrProfileIndex))
                {                    
                    OcrHelper.Ocr(processedImage);
                }
                /*using (Image processedImage = ProcessImage(ocrImagePanel.SelectionImage, Program.ActiveOcrProfileIndex))
                {
                    originalRichTextBox.Parent.BackColor = Color.DodgerBlue;
                    processedImage.Save("C:/ocr_tmp.png");
                    OcrResult result = Program.ActiveOcrEngine.LoadFile("C:/ocr_tmp.png", Program.ActiveLanguageFrom);
                    if (result.Succes)
                    {
                        originalRichTextBox.Text = result.Text;
                    }
                    originalRichTextBox.Parent.BackColor = Color.Gray;
                }*/
            }            
        }

        private Image ProcessImage(Image image, int imageProcess)
        {
            int index = 0;
            int scale = 1;
            OcrEngineProfile profile = OcrEngineProfile.Default;
            foreach (var value in Enum.GetValues(typeof(OcrEngineProfile)))
            {                
                scale = 1;
                if ((OcrEngineProfile)value >= OcrEngineProfile.Linear)
                {
                    for (int i = 2; i <= Program.ProfilesCount; i++)
                    {
                        scale = i;                        
                        if (index == imageProcess)
                        {
                            break;
                        }
                        index++;
                    }
                    if (index == imageProcess)
                    {
                        profile = (OcrEngineProfile)value;
                        break;
                    }
                }
                else
                {
                    if (index == imageProcess)
                    {
                        profile = (OcrEngineProfile)value;
                        break;
                    }
                    index++;
                }                
            }

            int width = image.Width * scale;
            int height = image.Height * scale;

            if (scale == 1 || (OcrEngineProfile)imageProcess < OcrEngineProfile.Linear)
                return new Bitmap(image);

            using (Image grey = ImageUtilities.MakeGrayscale(image))
            {
                if (profile == OcrEngineProfile.Soft)
                {
                    int width2 = image.Width * 2;
                    int height2 = image.Width * 2;

                    // Scale * 2
                    // Scale back to * 1
                    // Scale * profile size

                    using (Image doubleImage = ImageUtilities.ResizeImage(profile, grey, width2, height2))
                    {
                        using (Image normalImage = ImageUtilities.ResizeImage(profile, doubleImage, image.Width, image.Height))
                        {
                            return ImageUtilities.ResizeImage(profile, normalImage, width, height);
                        }
                    }
                }

                return ImageUtilities.ResizeImage(profile, image, width, height);
            }
        }        
    }
}