using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ocr.UI
{
    public partial class SimpleSettingsForm : Form
    {
        private Dictionary<string, string> selectedLanguages;
        private Dictionary<string, string> alllanguages;
        public Settings Settings { get; private set; }

        public SimpleSettingsForm(Dictionary<string, string> alllanguages, Dictionary<string, string> selectedLanguages)
        {
            InitializeComponent();

            this.SuspendLayout();
            languageProfiles_Slot1ComboBox.BeginUpdate();
            languageProfiles_Slot2ComboBox.BeginUpdate();
            languageProfiles_Slot3ComboBox.BeginUpdate();
            languageProfiles_Slot4ComboBox.BeginUpdate();
            languageProfiles_Slot5ComboBox.BeginUpdate();
            targetLanguageComboBox.BeginUpdate();

            this.alllanguages = alllanguages;
            this.selectedLanguages = selectedLanguages;

            UpdateLanguages();

            Settings = Program.Settings.Copy();

            translate_AutoTranslateCheckBox.Checked = Settings.Translate.AutoTranslate;
            translate_AutoTranslateDelayNumericUpDown.Value = (decimal)Settings.Translate.AutoTranslateDelay;

            clipboard_DontSaveToClipboardRadioButton.Checked = Settings.Clipboard.DontSaveToClipboard;
            clipboard_SaveImageToClipboardRadioButton.Checked = Settings.Clipboard.SaveImageToClipboard;
            clipboard_SaveOriginalTextToClipboardRadioButton.Checked = Settings.Clipboard.SaveOriginalTextToClipboard;
            clipboard_SaveTranslatedTextToClipboardRadioButton.Checked = Settings.Clipboard.SaveTranslatedTextToClipboard;

            autoTyper_UnicodeCheckBox.Checked = Settings.AutoTyper.Unicode;
            autoTyper_RequireKeyboardCheckBox.Checked = Settings.AutoTyper.RequireKeyboard;
            autoType_CancelOnMouseMoveNumericUpDown.Value = Settings.AutoTyper.CancelOnMouseMove;
            autoTyper_KeyDelayNumericUpDown.Value = Settings.AutoTyper.KeyDelay;
            autoTyper_HotkeyTextBox.Text = Settings.AutoTyper.Hotkey;

            SelectLanguageIndex(languageProfiles_Slot1ComboBox, Settings.LanguageProfiles.Slot1);
            SelectLanguageIndex(languageProfiles_Slot2ComboBox, Settings.LanguageProfiles.Slot2);
            SelectLanguageIndex(languageProfiles_Slot3ComboBox, Settings.LanguageProfiles.Slot3);
            SelectLanguageIndex(languageProfiles_Slot4ComboBox, Settings.LanguageProfiles.Slot4);
            SelectLanguageIndex(languageProfiles_Slot5ComboBox, Settings.LanguageProfiles.Slot5);

            languages_LanguageFilterTextBox.Text = Settings.LanguageFilters.Languages;

            SelectLanguageIndex(targetLanguageComboBox, Settings.TargetLanguage.TargetLanguage);

            captureAreaStyle_BackTextBox.Text = Settings.CaptureAreaStyle.Back;
            captureAreaStyle_BorderTextBox.Text = Settings.CaptureAreaStyle.Border;
            captureAreaStyle_DashedBorderCheckBox.Checked = Settings.CaptureAreaStyle.DashedBorder;

            serverInfo_ClientRadioButton.Checked = Settings.ServerInfo.Client;
            serverInfo_ServerRadioButton.Checked = Settings.ServerInfo.Server;
            serverInfo_ProxyRadioButton.Checked = Settings.ServerInfo.Proxy;
            serverInfo_IP1TextBox.Text = Settings.ServerInfo.IP1;
            serverInfo_IP2TextBox.Text = Settings.ServerInfo.IP2;

            vision_KeyTextBox.Text = Settings.KeyInfo.Key;

            this.ResumeLayout();
            languageProfiles_Slot1ComboBox.EndUpdate();
            languageProfiles_Slot2ComboBox.EndUpdate();
            languageProfiles_Slot3ComboBox.EndUpdate();
            languageProfiles_Slot4ComboBox.EndUpdate();
            languageProfiles_Slot5ComboBox.EndUpdate();
            targetLanguageComboBox.EndUpdate();
        }

        private void UpdateLanguages()
        {
            languageProfiles_Slot1ComboBox.Items.Clear();
            languageProfiles_Slot2ComboBox.Items.Clear();
            languageProfiles_Slot3ComboBox.Items.Clear();
            languageProfiles_Slot4ComboBox.Items.Clear();
            languageProfiles_Slot5ComboBox.Items.Clear();

            targetLanguageComboBox.Items.Clear();

            foreach (KeyValuePair<string, string> language in selectedLanguages)
            {
                languageProfiles_Slot1ComboBox.Items.Add(language.Key);
                languageProfiles_Slot2ComboBox.Items.Add(language.Key);
                languageProfiles_Slot3ComboBox.Items.Add(language.Key);
                languageProfiles_Slot4ComboBox.Items.Add(language.Key);
                languageProfiles_Slot5ComboBox.Items.Add(language.Key);

                targetLanguageComboBox.Items.Add(language.Key);
            }
        }

        private void SelectLanguageIndex(ComboBox combobox, string language)
        {
            if (!string.IsNullOrWhiteSpace(language))
            {
                for (int i = 0; i < combobox.Items.Count; i++)
                {
                    if (combobox.Items[i].ToString() == language)
                    {
                        combobox.SelectedIndex = i;
                        return;
                    }
                }
            }
            combobox.SelectedIndex = -1;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Settings.Translate.AutoTranslate = translate_AutoTranslateCheckBox.Checked;
            Settings.Translate.AutoTranslateDelay = (float)translate_AutoTranslateDelayNumericUpDown.Value;

            Settings.Clipboard.DontSaveToClipboard = clipboard_DontSaveToClipboardRadioButton.Checked;
            Settings.Clipboard.SaveImageToClipboard = clipboard_SaveImageToClipboardRadioButton.Checked;
            Settings.Clipboard.SaveOriginalTextToClipboard = clipboard_SaveOriginalTextToClipboardRadioButton.Checked;
            Settings.Clipboard.SaveTranslatedTextToClipboard = clipboard_SaveTranslatedTextToClipboardRadioButton.Checked;

            Settings.AutoTyper.Unicode = autoTyper_UnicodeCheckBox.Checked;
            Settings.AutoTyper.RequireKeyboard = autoTyper_RequireKeyboardCheckBox.Checked;
            Settings.AutoTyper.CancelOnMouseMove = (int)autoType_CancelOnMouseMoveNumericUpDown.Value;
            Settings.AutoTyper.KeyDelay = (int)autoTyper_KeyDelayNumericUpDown.Value;
            Settings.AutoTyper.Hotkey = autoTyper_HotkeyTextBox.Text;

            Settings.LanguageProfiles.Slot1 = languageProfiles_Slot1ComboBox.Text;
            Settings.LanguageProfiles.Slot2 = languageProfiles_Slot2ComboBox.Text;
            Settings.LanguageProfiles.Slot3 = languageProfiles_Slot3ComboBox.Text;
            Settings.LanguageProfiles.Slot4 = languageProfiles_Slot4ComboBox.Text;
            Settings.LanguageProfiles.Slot5 = languageProfiles_Slot5ComboBox.Text;

            Settings.LanguageFilters.Languages = languages_LanguageFilterTextBox.Text;

            Settings.TargetLanguage.TargetLanguage = targetLanguageComboBox.Text;

            Settings.CaptureAreaStyle.Back = captureAreaStyle_BackTextBox.Text;
            Settings.CaptureAreaStyle.Border = captureAreaStyle_BorderTextBox.Text;
            Settings.CaptureAreaStyle.DashedBorder = captureAreaStyle_DashedBorderCheckBox.Checked;

            Settings.ServerInfo.Client = serverInfo_ClientRadioButton.Checked;
            Settings.ServerInfo.Server = serverInfo_ServerRadioButton.Checked;
            Settings.ServerInfo.Proxy = serverInfo_ProxyRadioButton.Checked;
            Settings.ServerInfo.IP1 = serverInfo_IP1TextBox.Text;
            Settings.ServerInfo.IP2 = serverInfo_IP2TextBox.Text;

            Settings.KeyInfo.Key = vision_KeyTextBox.Text;

            Program.Settings = Settings;
            Program.Settings.Save();

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void languages_LanguageFilterButton_Click(object sender, EventArgs e)
        {
            LanguageSelectionForm languageSelection = new LanguageSelectionForm(alllanguages, languages_LanguageFilterTextBox.Text);
            if (languageSelection.ShowDialog() == DialogResult.OK)
            {
                Settings.LanguageFilters.Languages = languageSelection.Languages;
                languages_LanguageFilterTextBox.Text = Settings.LanguageFilters.Languages;
            }
        }

        private void vision_KeyTestButton_Click(object sender, EventArgs e)
        {
            string tempKey = Program.Settings.KeyInfo.Key;
            try
            {
                Program.Settings.KeyInfo.Key = vision_KeyTextBox.Text;

                Rectangle rect = new Rectangle(0, 0, 300, 100);

                using (Bitmap image = new Bitmap(rect.Width, rect.Height))
                using (Graphics graphics = Graphics.FromImage(image))
                using (Font font = new Font("Arial", 50))
                {
                    graphics.Clear(Color.White);

                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;
                    graphics.DrawString("Hello", font, Brushes.Black, rect, sf);

                    string tempPath = OcrHelper.GetTempPath(".png");

                    try
                    {
                        image.Save(tempPath);
                        OcrEngine engine = OcrEngine.Create(OcrEngineType.GoogleVision);
                        OcrResult result = engine.LoadFile(tempPath, "en");
                        MessageBox.Show(this,
                            "Result: " + result.ResultType + Environment.NewLine +
                            (string.IsNullOrEmpty(result.Text) ? string.Empty : "Text: " + result.Text + Environment.NewLine) +
                            (string.IsNullOrEmpty(result.Error) ? string.Empty : "Error:" + result.Error));
                    }
                    finally
                    {
                        try
                        {
                            File.Delete(tempPath);
                        }
                        catch { }
                    }
                }
            }
            finally
            {
                Program.Settings.KeyInfo.Key = tempKey;
            }
        }
    }
}