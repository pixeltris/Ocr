namespace Ocr.UI
{
    partial class SimpleSettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.translateGroupBox = new System.Windows.Forms.GroupBox();
            this.translate_AutoTranslateDelayNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.translate_AutoTranslateDelayLabel = new System.Windows.Forms.Label();
            this.translate_AutoTranslateCheckBox = new System.Windows.Forms.CheckBox();
            this.clipboardGroupBox = new System.Windows.Forms.GroupBox();
            this.clipboard_DontSaveToClipboardRadioButton = new System.Windows.Forms.RadioButton();
            this.clipboard_SaveTranslatedTextToClipboardRadioButton = new System.Windows.Forms.RadioButton();
            this.clipboard_SaveOriginalTextToClipboardRadioButton = new System.Windows.Forms.RadioButton();
            this.clipboard_SaveImageToClipboardRadioButton = new System.Windows.Forms.RadioButton();
            this.autoTyperGroupBox = new System.Windows.Forms.GroupBox();
            this.autoTyper_HotkeyLabel = new System.Windows.Forms.Label();
            this.autoTyper_HotkeyTextBox = new System.Windows.Forms.TextBox();
            this.autoType_CancelOnMouseMoveNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.autoTyper_CancelOnMouseMoveLabel = new System.Windows.Forms.Label();
            this.autoTyper_KeyDelayNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.autoTyper_KeyDelayLabel = new System.Windows.Forms.Label();
            this.autoTyper_RequireKeyboardCheckBox = new System.Windows.Forms.CheckBox();
            this.autoTyper_UnicodeCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.languages_LanguageFilterButton = new System.Windows.Forms.Button();
            this.languages_LanguageFilterTextBox = new System.Windows.Forms.TextBox();
            this.targetLanguageGroupBox = new System.Windows.Forms.GroupBox();
            this.targetLanguageComboBox = new System.Windows.Forms.ComboBox();
            this.languageProfilesGroupBox = new System.Windows.Forms.GroupBox();
            this.languageProfiles_Slot5ComboBox = new System.Windows.Forms.ComboBox();
            this.languageProfiles_Slot4ComboBox = new System.Windows.Forms.ComboBox();
            this.languageProfiles_Slot3ComboBox = new System.Windows.Forms.ComboBox();
            this.languageProfiles_Slot2ComboBox = new System.Windows.Forms.ComboBox();
            this.languageProfiles_Slot5Label = new System.Windows.Forms.Label();
            this.languageProfiles_Slot4Label = new System.Windows.Forms.Label();
            this.languageProfiles_Slot3Label = new System.Windows.Forms.Label();
            this.languageProfiles_Slot1ComboBox = new System.Windows.Forms.ComboBox();
            this.languageProfiles_Slot2Label = new System.Windows.Forms.Label();
            this.languageProfiles_Slot1Label = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.serverInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.serverInfo_IP2TextBox = new System.Windows.Forms.TextBox();
            this.serverInfo_IPLabel = new System.Windows.Forms.Label();
            this.serverInfo_IP1TextBox = new System.Windows.Forms.TextBox();
            this.serverInfo_ClientRadioButton = new System.Windows.Forms.RadioButton();
            this.serverInfo_ProxyRadioButton = new System.Windows.Forms.RadioButton();
            this.serverInfo_ServerRadioButton = new System.Windows.Forms.RadioButton();
            this.captureAreaStyle_GroupBox = new System.Windows.Forms.GroupBox();
            this.captureAreaStyle_DashedBorderCheckBox = new System.Windows.Forms.CheckBox();
            this.captureAreaStyle_BorderLabel = new System.Windows.Forms.Label();
            this.captureAreaStyle_BorderTextBox = new System.Windows.Forms.TextBox();
            this.captureAreaStyle_BackLabel = new System.Windows.Forms.Label();
            this.captureAreaStyle_BackTextBox = new System.Windows.Forms.TextBox();
            this.visionGroupBox = new System.Windows.Forms.GroupBox();
            this.vision_KeyLabel = new System.Windows.Forms.Label();
            this.vision_KeyTextBox = new System.Windows.Forms.TextBox();
            this.vision_KeyTestButton = new System.Windows.Forms.Button();
            this.translateGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.translate_AutoTranslateDelayNumericUpDown)).BeginInit();
            this.clipboardGroupBox.SuspendLayout();
            this.autoTyperGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.autoType_CancelOnMouseMoveNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.autoTyper_KeyDelayNumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.targetLanguageGroupBox.SuspendLayout();
            this.languageProfilesGroupBox.SuspendLayout();
            this.serverInfoGroupBox.SuspendLayout();
            this.captureAreaStyle_GroupBox.SuspendLayout();
            this.visionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // translateGroupBox
            // 
            this.translateGroupBox.Controls.Add(this.translate_AutoTranslateDelayNumericUpDown);
            this.translateGroupBox.Controls.Add(this.translate_AutoTranslateDelayLabel);
            this.translateGroupBox.Controls.Add(this.translate_AutoTranslateCheckBox);
            this.translateGroupBox.Location = new System.Drawing.Point(12, 9);
            this.translateGroupBox.Name = "translateGroupBox";
            this.translateGroupBox.Size = new System.Drawing.Size(231, 72);
            this.translateGroupBox.TabIndex = 9;
            this.translateGroupBox.TabStop = false;
            this.translateGroupBox.Text = "Translate";
            // 
            // translate_AutoTranslateDelayNumericUpDown
            // 
            this.translate_AutoTranslateDelayNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.translate_AutoTranslateDelayNumericUpDown.Location = new System.Drawing.Point(163, 41);
            this.translate_AutoTranslateDelayNumericUpDown.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.translate_AutoTranslateDelayNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.translate_AutoTranslateDelayNumericUpDown.Name = "translate_AutoTranslateDelayNumericUpDown";
            this.translate_AutoTranslateDelayNumericUpDown.Size = new System.Drawing.Size(48, 20);
            this.translate_AutoTranslateDelayNumericUpDown.TabIndex = 11;
            this.translate_AutoTranslateDelayNumericUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // translate_AutoTranslateDelayLabel
            // 
            this.translate_AutoTranslateDelayLabel.AutoSize = true;
            this.translate_AutoTranslateDelayLabel.Location = new System.Drawing.Point(8, 44);
            this.translate_AutoTranslateDelayLabel.Name = "translate_AutoTranslateDelayLabel";
            this.translate_AutoTranslateDelayLabel.Size = new System.Drawing.Size(152, 13);
            this.translate_AutoTranslateDelayLabel.TabIndex = 10;
            this.translate_AutoTranslateDelayLabel.Text = "Auto translate delay (seconds):";
            // 
            // translate_AutoTranslateCheckBox
            // 
            this.translate_AutoTranslateCheckBox.AutoSize = true;
            this.translate_AutoTranslateCheckBox.Location = new System.Drawing.Point(11, 22);
            this.translate_AutoTranslateCheckBox.Name = "translate_AutoTranslateCheckBox";
            this.translate_AutoTranslateCheckBox.Size = new System.Drawing.Size(91, 17);
            this.translate_AutoTranslateCheckBox.TabIndex = 9;
            this.translate_AutoTranslateCheckBox.Text = "Auto translate";
            this.translate_AutoTranslateCheckBox.UseVisualStyleBackColor = true;
            // 
            // clipboardGroupBox
            // 
            this.clipboardGroupBox.Controls.Add(this.clipboard_DontSaveToClipboardRadioButton);
            this.clipboardGroupBox.Controls.Add(this.clipboard_SaveTranslatedTextToClipboardRadioButton);
            this.clipboardGroupBox.Controls.Add(this.clipboard_SaveOriginalTextToClipboardRadioButton);
            this.clipboardGroupBox.Controls.Add(this.clipboard_SaveImageToClipboardRadioButton);
            this.clipboardGroupBox.Location = new System.Drawing.Point(12, 87);
            this.clipboardGroupBox.Name = "clipboardGroupBox";
            this.clipboardGroupBox.Size = new System.Drawing.Size(231, 117);
            this.clipboardGroupBox.TabIndex = 10;
            this.clipboardGroupBox.TabStop = false;
            this.clipboardGroupBox.Text = "Clipboard";
            // 
            // clipboard_DontSaveToClipboardRadioButton
            // 
            this.clipboard_DontSaveToClipboardRadioButton.AutoSize = true;
            this.clipboard_DontSaveToClipboardRadioButton.Location = new System.Drawing.Point(11, 21);
            this.clipboard_DontSaveToClipboardRadioButton.Name = "clipboard_DontSaveToClipboardRadioButton";
            this.clipboard_DontSaveToClipboardRadioButton.Size = new System.Drawing.Size(134, 17);
            this.clipboard_DontSaveToClipboardRadioButton.TabIndex = 16;
            this.clipboard_DontSaveToClipboardRadioButton.TabStop = true;
            this.clipboard_DontSaveToClipboardRadioButton.Text = "Don\'t save to clipboard";
            this.clipboard_DontSaveToClipboardRadioButton.UseVisualStyleBackColor = true;
            // 
            // clipboard_SaveTranslatedTextToClipboardRadioButton
            // 
            this.clipboard_SaveTranslatedTextToClipboardRadioButton.AutoSize = true;
            this.clipboard_SaveTranslatedTextToClipboardRadioButton.Location = new System.Drawing.Point(11, 90);
            this.clipboard_SaveTranslatedTextToClipboardRadioButton.Name = "clipboard_SaveTranslatedTextToClipboardRadioButton";
            this.clipboard_SaveTranslatedTextToClipboardRadioButton.Size = new System.Drawing.Size(177, 17);
            this.clipboard_SaveTranslatedTextToClipboardRadioButton.TabIndex = 15;
            this.clipboard_SaveTranslatedTextToClipboardRadioButton.TabStop = true;
            this.clipboard_SaveTranslatedTextToClipboardRadioButton.Text = "Save translated text to clipboard";
            this.clipboard_SaveTranslatedTextToClipboardRadioButton.UseVisualStyleBackColor = true;
            // 
            // clipboard_SaveOriginalTextToClipboardRadioButton
            // 
            this.clipboard_SaveOriginalTextToClipboardRadioButton.AutoSize = true;
            this.clipboard_SaveOriginalTextToClipboardRadioButton.Location = new System.Drawing.Point(11, 67);
            this.clipboard_SaveOriginalTextToClipboardRadioButton.Name = "clipboard_SaveOriginalTextToClipboardRadioButton";
            this.clipboard_SaveOriginalTextToClipboardRadioButton.Size = new System.Drawing.Size(164, 17);
            this.clipboard_SaveOriginalTextToClipboardRadioButton.TabIndex = 14;
            this.clipboard_SaveOriginalTextToClipboardRadioButton.TabStop = true;
            this.clipboard_SaveOriginalTextToClipboardRadioButton.Text = "Save original text to clipboard";
            this.clipboard_SaveOriginalTextToClipboardRadioButton.UseVisualStyleBackColor = true;
            // 
            // clipboard_SaveImageToClipboardRadioButton
            // 
            this.clipboard_SaveImageToClipboardRadioButton.AutoSize = true;
            this.clipboard_SaveImageToClipboardRadioButton.Location = new System.Drawing.Point(11, 44);
            this.clipboard_SaveImageToClipboardRadioButton.Name = "clipboard_SaveImageToClipboardRadioButton";
            this.clipboard_SaveImageToClipboardRadioButton.Size = new System.Drawing.Size(139, 17);
            this.clipboard_SaveImageToClipboardRadioButton.TabIndex = 13;
            this.clipboard_SaveImageToClipboardRadioButton.TabStop = true;
            this.clipboard_SaveImageToClipboardRadioButton.Text = "Save image to clipboard";
            this.clipboard_SaveImageToClipboardRadioButton.UseVisualStyleBackColor = true;
            // 
            // autoTyperGroupBox
            // 
            this.autoTyperGroupBox.Controls.Add(this.autoTyper_HotkeyLabel);
            this.autoTyperGroupBox.Controls.Add(this.autoTyper_HotkeyTextBox);
            this.autoTyperGroupBox.Controls.Add(this.autoType_CancelOnMouseMoveNumericUpDown);
            this.autoTyperGroupBox.Controls.Add(this.autoTyper_CancelOnMouseMoveLabel);
            this.autoTyperGroupBox.Controls.Add(this.autoTyper_KeyDelayNumericUpDown);
            this.autoTyperGroupBox.Controls.Add(this.autoTyper_KeyDelayLabel);
            this.autoTyperGroupBox.Controls.Add(this.autoTyper_RequireKeyboardCheckBox);
            this.autoTyperGroupBox.Controls.Add(this.autoTyper_UnicodeCheckBox);
            this.autoTyperGroupBox.Location = new System.Drawing.Point(12, 210);
            this.autoTyperGroupBox.Name = "autoTyperGroupBox";
            this.autoTyperGroupBox.Size = new System.Drawing.Size(231, 139);
            this.autoTyperGroupBox.TabIndex = 24;
            this.autoTyperGroupBox.TabStop = false;
            this.autoTyperGroupBox.Text = "Auto typer";
            // 
            // autoTyper_HotkeyLabel
            // 
            this.autoTyper_HotkeyLabel.AutoSize = true;
            this.autoTyper_HotkeyLabel.Location = new System.Drawing.Point(10, 114);
            this.autoTyper_HotkeyLabel.Name = "autoTyper_HotkeyLabel";
            this.autoTyper_HotkeyLabel.Size = new System.Drawing.Size(44, 13);
            this.autoTyper_HotkeyLabel.TabIndex = 55;
            this.autoTyper_HotkeyLabel.Text = "Hotkey:";
            // 
            // autoTyper_HotkeyTextBox
            // 
            this.autoTyper_HotkeyTextBox.Location = new System.Drawing.Point(60, 111);
            this.autoTyper_HotkeyTextBox.Name = "autoTyper_HotkeyTextBox";
            this.autoTyper_HotkeyTextBox.Size = new System.Drawing.Size(158, 20);
            this.autoTyper_HotkeyTextBox.TabIndex = 54;
            // 
            // autoType_CancelOnMouseMoveNumericUpDown
            // 
            this.autoType_CancelOnMouseMoveNumericUpDown.Location = new System.Drawing.Point(170, 60);
            this.autoType_CancelOnMouseMoveNumericUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.autoType_CancelOnMouseMoveNumericUpDown.Name = "autoType_CancelOnMouseMoveNumericUpDown";
            this.autoType_CancelOnMouseMoveNumericUpDown.Size = new System.Drawing.Size(48, 20);
            this.autoType_CancelOnMouseMoveNumericUpDown.TabIndex = 53;
            // 
            // autoTyper_CancelOnMouseMoveLabel
            // 
            this.autoTyper_CancelOnMouseMoveLabel.AutoSize = true;
            this.autoTyper_CancelOnMouseMoveLabel.Location = new System.Drawing.Point(10, 63);
            this.autoTyper_CancelOnMouseMoveLabel.Name = "autoTyper_CancelOnMouseMoveLabel";
            this.autoTyper_CancelOnMouseMoveLabel.Size = new System.Drawing.Size(156, 13);
            this.autoTyper_CancelOnMouseMoveLabel.TabIndex = 52;
            this.autoTyper_CancelOnMouseMoveLabel.Text = "Cancel on mouse move (pixels):";
            // 
            // autoTyper_KeyDelayNumericUpDown
            // 
            this.autoTyper_KeyDelayNumericUpDown.Location = new System.Drawing.Point(135, 85);
            this.autoTyper_KeyDelayNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.autoTyper_KeyDelayNumericUpDown.Name = "autoTyper_KeyDelayNumericUpDown";
            this.autoTyper_KeyDelayNumericUpDown.Size = new System.Drawing.Size(61, 20);
            this.autoTyper_KeyDelayNumericUpDown.TabIndex = 51;
            // 
            // autoTyper_KeyDelayLabel
            // 
            this.autoTyper_KeyDelayLabel.AutoSize = true;
            this.autoTyper_KeyDelayLabel.Location = new System.Drawing.Point(10, 88);
            this.autoTyper_KeyDelayLabel.Name = "autoTyper_KeyDelayLabel";
            this.autoTyper_KeyDelayLabel.Size = new System.Drawing.Size(121, 13);
            this.autoTyper_KeyDelayLabel.TabIndex = 50;
            this.autoTyper_KeyDelayLabel.Text = "Key delay (milliseconds):";
            // 
            // autoTyper_RequireKeyboardCheckBox
            // 
            this.autoTyper_RequireKeyboardCheckBox.AutoSize = true;
            this.autoTyper_RequireKeyboardCheckBox.Location = new System.Drawing.Point(12, 41);
            this.autoTyper_RequireKeyboardCheckBox.Name = "autoTyper_RequireKeyboardCheckBox";
            this.autoTyper_RequireKeyboardCheckBox.Size = new System.Drawing.Size(110, 17);
            this.autoTyper_RequireKeyboardCheckBox.TabIndex = 10;
            this.autoTyper_RequireKeyboardCheckBox.Text = "Require keyboard";
            this.autoTyper_RequireKeyboardCheckBox.UseVisualStyleBackColor = true;
            // 
            // autoTyper_UnicodeCheckBox
            // 
            this.autoTyper_UnicodeCheckBox.AutoSize = true;
            this.autoTyper_UnicodeCheckBox.Location = new System.Drawing.Point(12, 19);
            this.autoTyper_UnicodeCheckBox.Name = "autoTyper_UnicodeCheckBox";
            this.autoTyper_UnicodeCheckBox.Size = new System.Drawing.Size(66, 17);
            this.autoTyper_UnicodeCheckBox.TabIndex = 9;
            this.autoTyper_UnicodeCheckBox.Text = "Unicode";
            this.autoTyper_UnicodeCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.languages_LanguageFilterButton);
            this.groupBox1.Controls.Add(this.languages_LanguageFilterTextBox);
            this.groupBox1.Location = new System.Drawing.Point(250, 171);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 53);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Languages";
            // 
            // languages_LanguageFilterButton
            // 
            this.languages_LanguageFilterButton.Location = new System.Drawing.Point(215, 19);
            this.languages_LanguageFilterButton.Name = "languages_LanguageFilterButton";
            this.languages_LanguageFilterButton.Size = new System.Drawing.Size(25, 23);
            this.languages_LanguageFilterButton.TabIndex = 54;
            this.languages_LanguageFilterButton.Text = "...";
            this.languages_LanguageFilterButton.UseVisualStyleBackColor = true;
            this.languages_LanguageFilterButton.Click += new System.EventHandler(this.languages_LanguageFilterButton_Click);
            // 
            // languages_LanguageFilterTextBox
            // 
            this.languages_LanguageFilterTextBox.Location = new System.Drawing.Point(11, 21);
            this.languages_LanguageFilterTextBox.Name = "languages_LanguageFilterTextBox";
            this.languages_LanguageFilterTextBox.Size = new System.Drawing.Size(201, 20);
            this.languages_LanguageFilterTextBox.TabIndex = 53;
            // 
            // targetLanguageGroupBox
            // 
            this.targetLanguageGroupBox.Controls.Add(this.targetLanguageComboBox);
            this.targetLanguageGroupBox.Location = new System.Drawing.Point(250, 225);
            this.targetLanguageGroupBox.Name = "targetLanguageGroupBox";
            this.targetLanguageGroupBox.Size = new System.Drawing.Size(251, 59);
            this.targetLanguageGroupBox.TabIndex = 26;
            this.targetLanguageGroupBox.TabStop = false;
            this.targetLanguageGroupBox.Text = "Target Language";
            // 
            // targetLanguageComboBox
            // 
            this.targetLanguageComboBox.FormattingEnabled = true;
            this.targetLanguageComboBox.Location = new System.Drawing.Point(11, 26);
            this.targetLanguageComboBox.Name = "targetLanguageComboBox";
            this.targetLanguageComboBox.Size = new System.Drawing.Size(227, 21);
            this.targetLanguageComboBox.TabIndex = 21;
            // 
            // languageProfilesGroupBox
            // 
            this.languageProfilesGroupBox.Controls.Add(this.languageProfiles_Slot5ComboBox);
            this.languageProfilesGroupBox.Controls.Add(this.languageProfiles_Slot4ComboBox);
            this.languageProfilesGroupBox.Controls.Add(this.languageProfiles_Slot3ComboBox);
            this.languageProfilesGroupBox.Controls.Add(this.languageProfiles_Slot2ComboBox);
            this.languageProfilesGroupBox.Controls.Add(this.languageProfiles_Slot5Label);
            this.languageProfilesGroupBox.Controls.Add(this.languageProfiles_Slot4Label);
            this.languageProfilesGroupBox.Controls.Add(this.languageProfiles_Slot3Label);
            this.languageProfilesGroupBox.Controls.Add(this.languageProfiles_Slot1ComboBox);
            this.languageProfilesGroupBox.Controls.Add(this.languageProfiles_Slot2Label);
            this.languageProfilesGroupBox.Controls.Add(this.languageProfiles_Slot1Label);
            this.languageProfilesGroupBox.Location = new System.Drawing.Point(250, 9);
            this.languageProfilesGroupBox.Name = "languageProfilesGroupBox";
            this.languageProfilesGroupBox.Size = new System.Drawing.Size(251, 156);
            this.languageProfilesGroupBox.TabIndex = 27;
            this.languageProfilesGroupBox.TabStop = false;
            this.languageProfilesGroupBox.Text = "Language Profiles";
            // 
            // languageProfiles_Slot5ComboBox
            // 
            this.languageProfiles_Slot5ComboBox.FormattingEnabled = true;
            this.languageProfiles_Slot5ComboBox.Location = new System.Drawing.Point(51, 124);
            this.languageProfiles_Slot5ComboBox.Name = "languageProfiles_Slot5ComboBox";
            this.languageProfiles_Slot5ComboBox.Size = new System.Drawing.Size(187, 21);
            this.languageProfiles_Slot5ComboBox.TabIndex = 27;
            // 
            // languageProfiles_Slot4ComboBox
            // 
            this.languageProfiles_Slot4ComboBox.FormattingEnabled = true;
            this.languageProfiles_Slot4ComboBox.Location = new System.Drawing.Point(51, 99);
            this.languageProfiles_Slot4ComboBox.Name = "languageProfiles_Slot4ComboBox";
            this.languageProfiles_Slot4ComboBox.Size = new System.Drawing.Size(187, 21);
            this.languageProfiles_Slot4ComboBox.TabIndex = 26;
            // 
            // languageProfiles_Slot3ComboBox
            // 
            this.languageProfiles_Slot3ComboBox.FormattingEnabled = true;
            this.languageProfiles_Slot3ComboBox.Location = new System.Drawing.Point(51, 73);
            this.languageProfiles_Slot3ComboBox.Name = "languageProfiles_Slot3ComboBox";
            this.languageProfiles_Slot3ComboBox.Size = new System.Drawing.Size(187, 21);
            this.languageProfiles_Slot3ComboBox.TabIndex = 25;
            // 
            // languageProfiles_Slot2ComboBox
            // 
            this.languageProfiles_Slot2ComboBox.FormattingEnabled = true;
            this.languageProfiles_Slot2ComboBox.Location = new System.Drawing.Point(51, 47);
            this.languageProfiles_Slot2ComboBox.Name = "languageProfiles_Slot2ComboBox";
            this.languageProfiles_Slot2ComboBox.Size = new System.Drawing.Size(187, 21);
            this.languageProfiles_Slot2ComboBox.TabIndex = 24;
            // 
            // languageProfiles_Slot5Label
            // 
            this.languageProfiles_Slot5Label.AutoSize = true;
            this.languageProfiles_Slot5Label.Location = new System.Drawing.Point(11, 127);
            this.languageProfiles_Slot5Label.Name = "languageProfiles_Slot5Label";
            this.languageProfiles_Slot5Label.Size = new System.Drawing.Size(37, 13);
            this.languageProfiles_Slot5Label.TabIndex = 23;
            this.languageProfiles_Slot5Label.Text = "Slot 5:";
            // 
            // languageProfiles_Slot4Label
            // 
            this.languageProfiles_Slot4Label.AutoSize = true;
            this.languageProfiles_Slot4Label.Location = new System.Drawing.Point(11, 102);
            this.languageProfiles_Slot4Label.Name = "languageProfiles_Slot4Label";
            this.languageProfiles_Slot4Label.Size = new System.Drawing.Size(37, 13);
            this.languageProfiles_Slot4Label.TabIndex = 22;
            this.languageProfiles_Slot4Label.Text = "Slot 4:";
            // 
            // languageProfiles_Slot3Label
            // 
            this.languageProfiles_Slot3Label.AutoSize = true;
            this.languageProfiles_Slot3Label.Location = new System.Drawing.Point(11, 77);
            this.languageProfiles_Slot3Label.Name = "languageProfiles_Slot3Label";
            this.languageProfiles_Slot3Label.Size = new System.Drawing.Size(37, 13);
            this.languageProfiles_Slot3Label.TabIndex = 21;
            this.languageProfiles_Slot3Label.Text = "Slot 3:";
            // 
            // languageProfiles_Slot1ComboBox
            // 
            this.languageProfiles_Slot1ComboBox.FormattingEnabled = true;
            this.languageProfiles_Slot1ComboBox.Location = new System.Drawing.Point(51, 22);
            this.languageProfiles_Slot1ComboBox.Name = "languageProfiles_Slot1ComboBox";
            this.languageProfiles_Slot1ComboBox.Size = new System.Drawing.Size(187, 21);
            this.languageProfiles_Slot1ComboBox.TabIndex = 20;
            // 
            // languageProfiles_Slot2Label
            // 
            this.languageProfiles_Slot2Label.AutoSize = true;
            this.languageProfiles_Slot2Label.Location = new System.Drawing.Point(11, 50);
            this.languageProfiles_Slot2Label.Name = "languageProfiles_Slot2Label";
            this.languageProfiles_Slot2Label.Size = new System.Drawing.Size(37, 13);
            this.languageProfiles_Slot2Label.TabIndex = 1;
            this.languageProfiles_Slot2Label.Text = "Slot 2:";
            // 
            // languageProfiles_Slot1Label
            // 
            this.languageProfiles_Slot1Label.AutoSize = true;
            this.languageProfiles_Slot1Label.Location = new System.Drawing.Point(11, 26);
            this.languageProfiles_Slot1Label.Name = "languageProfiles_Slot1Label";
            this.languageProfiles_Slot1Label.Size = new System.Drawing.Size(37, 13);
            this.languageProfiles_Slot1Label.TabIndex = 0;
            this.languageProfiles_Slot1Label.Text = "Slot 1:";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(344, 448);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 28;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(425, 448);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 29;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // serverInfoGroupBox
            // 
            this.serverInfoGroupBox.Controls.Add(this.serverInfo_IP2TextBox);
            this.serverInfoGroupBox.Controls.Add(this.serverInfo_IPLabel);
            this.serverInfoGroupBox.Controls.Add(this.serverInfo_IP1TextBox);
            this.serverInfoGroupBox.Controls.Add(this.serverInfo_ClientRadioButton);
            this.serverInfoGroupBox.Controls.Add(this.serverInfo_ProxyRadioButton);
            this.serverInfoGroupBox.Controls.Add(this.serverInfo_ServerRadioButton);
            this.serverInfoGroupBox.Location = new System.Drawing.Point(12, 355);
            this.serverInfoGroupBox.Name = "serverInfoGroupBox";
            this.serverInfoGroupBox.Size = new System.Drawing.Size(231, 71);
            this.serverInfoGroupBox.TabIndex = 30;
            this.serverInfoGroupBox.TabStop = false;
            this.serverInfoGroupBox.Text = "Server Info";
            // 
            // serverInfo_IP2TextBox
            // 
            this.serverInfo_IP2TextBox.Location = new System.Drawing.Point(128, 42);
            this.serverInfo_IP2TextBox.Name = "serverInfo_IP2TextBox";
            this.serverInfo_IP2TextBox.Size = new System.Drawing.Size(93, 20);
            this.serverInfo_IP2TextBox.TabIndex = 58;
            // 
            // serverInfo_IPLabel
            // 
            this.serverInfo_IPLabel.AutoSize = true;
            this.serverInfo_IPLabel.Location = new System.Drawing.Point(8, 45);
            this.serverInfo_IPLabel.Name = "serverInfo_IPLabel";
            this.serverInfo_IPLabel.Size = new System.Drawing.Size(20, 13);
            this.serverInfo_IPLabel.TabIndex = 57;
            this.serverInfo_IPLabel.Text = "IP:";
            // 
            // serverInfo_IP1TextBox
            // 
            this.serverInfo_IP1TextBox.Location = new System.Drawing.Point(32, 42);
            this.serverInfo_IP1TextBox.Name = "serverInfo_IP1TextBox";
            this.serverInfo_IP1TextBox.Size = new System.Drawing.Size(93, 20);
            this.serverInfo_IP1TextBox.TabIndex = 56;
            // 
            // serverInfo_ClientRadioButton
            // 
            this.serverInfo_ClientRadioButton.AutoSize = true;
            this.serverInfo_ClientRadioButton.Location = new System.Drawing.Point(10, 20);
            this.serverInfo_ClientRadioButton.Name = "serverInfo_ClientRadioButton";
            this.serverInfo_ClientRadioButton.Size = new System.Drawing.Size(51, 17);
            this.serverInfo_ClientRadioButton.TabIndex = 19;
            this.serverInfo_ClientRadioButton.TabStop = true;
            this.serverInfo_ClientRadioButton.Text = "Client";
            this.serverInfo_ClientRadioButton.UseVisualStyleBackColor = true;
            // 
            // serverInfo_ProxyRadioButton
            // 
            this.serverInfo_ProxyRadioButton.AutoSize = true;
            this.serverInfo_ProxyRadioButton.Location = new System.Drawing.Point(136, 20);
            this.serverInfo_ProxyRadioButton.Name = "serverInfo_ProxyRadioButton";
            this.serverInfo_ProxyRadioButton.Size = new System.Drawing.Size(51, 17);
            this.serverInfo_ProxyRadioButton.TabIndex = 18;
            this.serverInfo_ProxyRadioButton.TabStop = true;
            this.serverInfo_ProxyRadioButton.Text = "Proxy";
            this.serverInfo_ProxyRadioButton.UseVisualStyleBackColor = true;
            // 
            // serverInfo_ServerRadioButton
            // 
            this.serverInfo_ServerRadioButton.AutoSize = true;
            this.serverInfo_ServerRadioButton.Location = new System.Drawing.Point(71, 20);
            this.serverInfo_ServerRadioButton.Name = "serverInfo_ServerRadioButton";
            this.serverInfo_ServerRadioButton.Size = new System.Drawing.Size(56, 17);
            this.serverInfo_ServerRadioButton.TabIndex = 17;
            this.serverInfo_ServerRadioButton.TabStop = true;
            this.serverInfo_ServerRadioButton.Text = "Server";
            this.serverInfo_ServerRadioButton.UseVisualStyleBackColor = true;
            // 
            // captureAreaStyle_GroupBox
            // 
            this.captureAreaStyle_GroupBox.Controls.Add(this.captureAreaStyle_DashedBorderCheckBox);
            this.captureAreaStyle_GroupBox.Controls.Add(this.captureAreaStyle_BorderLabel);
            this.captureAreaStyle_GroupBox.Controls.Add(this.captureAreaStyle_BorderTextBox);
            this.captureAreaStyle_GroupBox.Controls.Add(this.captureAreaStyle_BackLabel);
            this.captureAreaStyle_GroupBox.Controls.Add(this.captureAreaStyle_BackTextBox);
            this.captureAreaStyle_GroupBox.Location = new System.Drawing.Point(250, 289);
            this.captureAreaStyle_GroupBox.Name = "captureAreaStyle_GroupBox";
            this.captureAreaStyle_GroupBox.Size = new System.Drawing.Size(251, 101);
            this.captureAreaStyle_GroupBox.TabIndex = 31;
            this.captureAreaStyle_GroupBox.TabStop = false;
            this.captureAreaStyle_GroupBox.Text = "Capture Area Style";
            // 
            // captureAreaStyle_DashedBorderCheckBox
            // 
            this.captureAreaStyle_DashedBorderCheckBox.AutoSize = true;
            this.captureAreaStyle_DashedBorderCheckBox.Location = new System.Drawing.Point(11, 76);
            this.captureAreaStyle_DashedBorderCheckBox.Name = "captureAreaStyle_DashedBorderCheckBox";
            this.captureAreaStyle_DashedBorderCheckBox.Size = new System.Drawing.Size(96, 17);
            this.captureAreaStyle_DashedBorderCheckBox.TabIndex = 62;
            this.captureAreaStyle_DashedBorderCheckBox.Text = "Dashed border";
            this.captureAreaStyle_DashedBorderCheckBox.UseVisualStyleBackColor = true;
            // 
            // captureAreaStyle_BorderLabel
            // 
            this.captureAreaStyle_BorderLabel.AutoSize = true;
            this.captureAreaStyle_BorderLabel.Location = new System.Drawing.Point(8, 52);
            this.captureAreaStyle_BorderLabel.Name = "captureAreaStyle_BorderLabel";
            this.captureAreaStyle_BorderLabel.Size = new System.Drawing.Size(41, 13);
            this.captureAreaStyle_BorderLabel.TabIndex = 61;
            this.captureAreaStyle_BorderLabel.Text = "Border:";
            // 
            // captureAreaStyle_BorderTextBox
            // 
            this.captureAreaStyle_BorderTextBox.Location = new System.Drawing.Point(57, 48);
            this.captureAreaStyle_BorderTextBox.Name = "captureAreaStyle_BorderTextBox";
            this.captureAreaStyle_BorderTextBox.Size = new System.Drawing.Size(181, 20);
            this.captureAreaStyle_BorderTextBox.TabIndex = 60;
            // 
            // captureAreaStyle_BackLabel
            // 
            this.captureAreaStyle_BackLabel.AutoSize = true;
            this.captureAreaStyle_BackLabel.Location = new System.Drawing.Point(8, 26);
            this.captureAreaStyle_BackLabel.Name = "captureAreaStyle_BackLabel";
            this.captureAreaStyle_BackLabel.Size = new System.Drawing.Size(35, 13);
            this.captureAreaStyle_BackLabel.TabIndex = 59;
            this.captureAreaStyle_BackLabel.Text = "Back:";
            // 
            // captureAreaStyle_BackTextBox
            // 
            this.captureAreaStyle_BackTextBox.Location = new System.Drawing.Point(57, 22);
            this.captureAreaStyle_BackTextBox.Name = "captureAreaStyle_BackTextBox";
            this.captureAreaStyle_BackTextBox.Size = new System.Drawing.Size(181, 20);
            this.captureAreaStyle_BackTextBox.TabIndex = 58;
            // 
            // visionGroupBox
            // 
            this.visionGroupBox.Controls.Add(this.vision_KeyTestButton);
            this.visionGroupBox.Controls.Add(this.vision_KeyTextBox);
            this.visionGroupBox.Controls.Add(this.vision_KeyLabel);
            this.visionGroupBox.Location = new System.Drawing.Point(250, 396);
            this.visionGroupBox.Name = "visionGroupBox";
            this.visionGroupBox.Size = new System.Drawing.Size(251, 47);
            this.visionGroupBox.TabIndex = 32;
            this.visionGroupBox.TabStop = false;
            this.visionGroupBox.Text = "Vision Key";
            // 
            // vision_KeyLabel
            // 
            this.vision_KeyLabel.AutoSize = true;
            this.vision_KeyLabel.Location = new System.Drawing.Point(10, 22);
            this.vision_KeyLabel.Name = "vision_KeyLabel";
            this.vision_KeyLabel.Size = new System.Drawing.Size(28, 13);
            this.vision_KeyLabel.TabIndex = 63;
            this.vision_KeyLabel.Text = "Key:";
            // 
            // vision_KeyTextBox
            // 
            this.vision_KeyTextBox.Location = new System.Drawing.Point(40, 19);
            this.vision_KeyTextBox.Name = "vision_KeyTextBox";
            this.vision_KeyTextBox.Size = new System.Drawing.Size(161, 20);
            this.vision_KeyTextBox.TabIndex = 64;
            // 
            // vision_KeyTestButton
            // 
            this.vision_KeyTestButton.Location = new System.Drawing.Point(205, 17);
            this.vision_KeyTestButton.Name = "vision_KeyTestButton";
            this.vision_KeyTestButton.Size = new System.Drawing.Size(38, 23);
            this.vision_KeyTestButton.TabIndex = 65;
            this.vision_KeyTestButton.Text = "Test";
            this.vision_KeyTestButton.UseVisualStyleBackColor = true;
            this.vision_KeyTestButton.Click += new System.EventHandler(this.vision_KeyTestButton_Click);
            // 
            // SimpleSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 478);
            this.Controls.Add(this.visionGroupBox);
            this.Controls.Add(this.captureAreaStyle_GroupBox);
            this.Controls.Add(this.serverInfoGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.languageProfilesGroupBox);
            this.Controls.Add(this.targetLanguageGroupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.autoTyperGroupBox);
            this.Controls.Add(this.clipboardGroupBox);
            this.Controls.Add(this.translateGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SimpleSettingsForm";
            this.ShowInTaskbar = false;
            this.Text = "Settings";
            this.translateGroupBox.ResumeLayout(false);
            this.translateGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.translate_AutoTranslateDelayNumericUpDown)).EndInit();
            this.clipboardGroupBox.ResumeLayout(false);
            this.clipboardGroupBox.PerformLayout();
            this.autoTyperGroupBox.ResumeLayout(false);
            this.autoTyperGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.autoType_CancelOnMouseMoveNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.autoTyper_KeyDelayNumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.targetLanguageGroupBox.ResumeLayout(false);
            this.languageProfilesGroupBox.ResumeLayout(false);
            this.languageProfilesGroupBox.PerformLayout();
            this.serverInfoGroupBox.ResumeLayout(false);
            this.serverInfoGroupBox.PerformLayout();
            this.captureAreaStyle_GroupBox.ResumeLayout(false);
            this.captureAreaStyle_GroupBox.PerformLayout();
            this.visionGroupBox.ResumeLayout(false);
            this.visionGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox translateGroupBox;
        private System.Windows.Forms.NumericUpDown translate_AutoTranslateDelayNumericUpDown;
        private System.Windows.Forms.Label translate_AutoTranslateDelayLabel;
        private System.Windows.Forms.CheckBox translate_AutoTranslateCheckBox;
        private System.Windows.Forms.GroupBox clipboardGroupBox;
        private System.Windows.Forms.RadioButton clipboard_DontSaveToClipboardRadioButton;
        private System.Windows.Forms.RadioButton clipboard_SaveTranslatedTextToClipboardRadioButton;
        private System.Windows.Forms.RadioButton clipboard_SaveOriginalTextToClipboardRadioButton;
        private System.Windows.Forms.RadioButton clipboard_SaveImageToClipboardRadioButton;
        private System.Windows.Forms.GroupBox autoTyperGroupBox;
        private System.Windows.Forms.NumericUpDown autoType_CancelOnMouseMoveNumericUpDown;
        private System.Windows.Forms.Label autoTyper_CancelOnMouseMoveLabel;
        private System.Windows.Forms.NumericUpDown autoTyper_KeyDelayNumericUpDown;
        private System.Windows.Forms.Label autoTyper_KeyDelayLabel;
        private System.Windows.Forms.CheckBox autoTyper_RequireKeyboardCheckBox;
        private System.Windows.Forms.CheckBox autoTyper_UnicodeCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button languages_LanguageFilterButton;
        private System.Windows.Forms.TextBox languages_LanguageFilterTextBox;
        private System.Windows.Forms.GroupBox targetLanguageGroupBox;
        private System.Windows.Forms.ComboBox targetLanguageComboBox;
        private System.Windows.Forms.Label autoTyper_HotkeyLabel;
        private System.Windows.Forms.TextBox autoTyper_HotkeyTextBox;
        private System.Windows.Forms.GroupBox languageProfilesGroupBox;
        private System.Windows.Forms.ComboBox languageProfiles_Slot5ComboBox;
        private System.Windows.Forms.ComboBox languageProfiles_Slot4ComboBox;
        private System.Windows.Forms.ComboBox languageProfiles_Slot3ComboBox;
        private System.Windows.Forms.ComboBox languageProfiles_Slot2ComboBox;
        private System.Windows.Forms.Label languageProfiles_Slot5Label;
        private System.Windows.Forms.Label languageProfiles_Slot4Label;
        private System.Windows.Forms.Label languageProfiles_Slot3Label;
        private System.Windows.Forms.ComboBox languageProfiles_Slot1ComboBox;
        private System.Windows.Forms.Label languageProfiles_Slot2Label;
        private System.Windows.Forms.Label languageProfiles_Slot1Label;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox serverInfoGroupBox;
        private System.Windows.Forms.Label serverInfo_IPLabel;
        private System.Windows.Forms.TextBox serverInfo_IP1TextBox;
        private System.Windows.Forms.RadioButton serverInfo_ClientRadioButton;
        private System.Windows.Forms.RadioButton serverInfo_ProxyRadioButton;
        private System.Windows.Forms.RadioButton serverInfo_ServerRadioButton;
        private System.Windows.Forms.GroupBox captureAreaStyle_GroupBox;
        private System.Windows.Forms.CheckBox captureAreaStyle_DashedBorderCheckBox;
        private System.Windows.Forms.Label captureAreaStyle_BorderLabel;
        private System.Windows.Forms.TextBox captureAreaStyle_BorderTextBox;
        private System.Windows.Forms.Label captureAreaStyle_BackLabel;
        private System.Windows.Forms.TextBox captureAreaStyle_BackTextBox;
        private System.Windows.Forms.TextBox serverInfo_IP2TextBox;
        private System.Windows.Forms.GroupBox visionGroupBox;
        private System.Windows.Forms.Button vision_KeyTestButton;
        private System.Windows.Forms.TextBox vision_KeyTextBox;
        private System.Windows.Forms.Label vision_KeyLabel;
    }
}