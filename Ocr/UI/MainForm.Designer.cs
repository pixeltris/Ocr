namespace Ocr.UI
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.imageTextSplitContainer = new System.Windows.Forms.SplitContainer();
            this.ocrImagePanel = new Ocr.UI.OcrImagePanel();
            this.textSplitContainer = new System.Windows.Forms.SplitContainer();
            this.originalTextPanel = new System.Windows.Forms.Panel();
            this.originalRichTextBox = new System.Windows.Forms.RichTextBox();
            this.normalTranslateBtn = new System.Windows.Forms.Button();
            this.translatedTextPanel = new System.Windows.Forms.Panel();
            this.translatedRichTextBox = new System.Windows.Forms.RichTextBox();
            this.reverseTranslateBtn = new System.Windows.Forms.Button();
            this.toolStrip1 = new Ocr.UI.ToolStripEx();
            this.ocrEngineToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.ocrProfileToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.translateApiToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.translateFromToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.switchTranslateToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.translateToToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.translateToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.languageQuickslotToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.captureAreaToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.captureAreaFreezeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.captureAreaEditableToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.editImageSelectionToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.refreshImageToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.openFileToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveFileToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripButton = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.imageTextSplitContainer)).BeginInit();
            this.imageTextSplitContainer.Panel1.SuspendLayout();
            this.imageTextSplitContainer.Panel2.SuspendLayout();
            this.imageTextSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ocrImagePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textSplitContainer)).BeginInit();
            this.textSplitContainer.Panel1.SuspendLayout();
            this.textSplitContainer.Panel2.SuspendLayout();
            this.textSplitContainer.SuspendLayout();
            this.originalTextPanel.SuspendLayout();
            this.translatedTextPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageTextSplitContainer
            // 
            this.imageTextSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageTextSplitContainer.Location = new System.Drawing.Point(0, 25);
            this.imageTextSplitContainer.Name = "imageTextSplitContainer";
            this.imageTextSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // imageTextSplitContainer.Panel1
            // 
            this.imageTextSplitContainer.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.imageTextSplitContainer.Panel1.Controls.Add(this.ocrImagePanel);
            this.imageTextSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding(1);
            // 
            // imageTextSplitContainer.Panel2
            // 
            this.imageTextSplitContainer.Panel2.Controls.Add(this.textSplitContainer);
            this.imageTextSplitContainer.Size = new System.Drawing.Size(804, 527);
            this.imageTextSplitContainer.SplitterDistance = 208;
            this.imageTextSplitContainer.TabIndex = 12;
            this.imageTextSplitContainer.TabStop = false;
            // 
            // ocrImagePanel
            // 
            this.ocrImagePanel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ocrImagePanel.CenterDragHandles = true;
            this.ocrImagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ocrImagePanel.Editable = false;
            this.ocrImagePanel.Location = new System.Drawing.Point(1, 1);
            this.ocrImagePanel.Name = "ocrImagePanel";
            this.ocrImagePanel.Selection = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.ocrImagePanel.SelectionImage = null;
            this.ocrImagePanel.SelectionMode = Cyotek.Windows.Forms.ImageBoxSelectionMode.Rectangle;
            this.ocrImagePanel.ShowPixelGrid = true;
            this.ocrImagePanel.Size = new System.Drawing.Size(802, 206);
            this.ocrImagePanel.SourceImage = null;
            this.ocrImagePanel.TabIndex = 0;
            this.ocrImagePanel.TabStop = false;
            // 
            // textSplitContainer
            // 
            this.textSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.textSplitContainer.Name = "textSplitContainer";
            // 
            // textSplitContainer.Panel1
            // 
            this.textSplitContainer.Panel1.Controls.Add(this.originalTextPanel);
            this.textSplitContainer.Panel1.Controls.Add(this.normalTranslateBtn);
            // 
            // textSplitContainer.Panel2
            // 
            this.textSplitContainer.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.textSplitContainer.Panel2.Controls.Add(this.translatedTextPanel);
            this.textSplitContainer.Panel2.Controls.Add(this.reverseTranslateBtn);
            this.textSplitContainer.Size = new System.Drawing.Size(804, 315);
            this.textSplitContainer.SplitterDistance = 392;
            this.textSplitContainer.TabIndex = 13;
            this.textSplitContainer.TabStop = false;
            // 
            // originalTextPanel
            // 
            this.originalTextPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.originalTextPanel.Controls.Add(this.originalRichTextBox);
            this.originalTextPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.originalTextPanel.Location = new System.Drawing.Point(0, 0);
            this.originalTextPanel.Name = "originalTextPanel";
            this.originalTextPanel.Padding = new System.Windows.Forms.Padding(1);
            this.originalTextPanel.Size = new System.Drawing.Size(392, 292);
            this.originalTextPanel.TabIndex = 2;
            // 
            // originalRichTextBox
            // 
            this.originalRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.originalRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.originalRichTextBox.Location = new System.Drawing.Point(1, 1);
            this.originalRichTextBox.Name = "originalRichTextBox";
            this.originalRichTextBox.Size = new System.Drawing.Size(390, 290);
            this.originalRichTextBox.TabIndex = 8;
            this.originalRichTextBox.Text = "";
            this.originalRichTextBox.WordWrap = false;
            this.originalRichTextBox.TextChanged += new System.EventHandler(this.originalRichTextBox_TextChanged);
            // 
            // normalTranslateBtn
            // 
            this.normalTranslateBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.normalTranslateBtn.Location = new System.Drawing.Point(0, 292);
            this.normalTranslateBtn.Name = "normalTranslateBtn";
            this.normalTranslateBtn.Size = new System.Drawing.Size(392, 23);
            this.normalTranslateBtn.TabIndex = 10;
            this.normalTranslateBtn.Text = "English -> Chinese (Traditional)";
            this.normalTranslateBtn.UseVisualStyleBackColor = true;
            this.normalTranslateBtn.Visible = false;
            // 
            // translatedTextPanel
            // 
            this.translatedTextPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.translatedTextPanel.Controls.Add(this.translatedRichTextBox);
            this.translatedTextPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.translatedTextPanel.Location = new System.Drawing.Point(0, 0);
            this.translatedTextPanel.Name = "translatedTextPanel";
            this.translatedTextPanel.Padding = new System.Windows.Forms.Padding(1);
            this.translatedTextPanel.Size = new System.Drawing.Size(408, 292);
            this.translatedTextPanel.TabIndex = 3;
            // 
            // translatedRichTextBox
            // 
            this.translatedRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.translatedRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.translatedRichTextBox.Location = new System.Drawing.Point(1, 1);
            this.translatedRichTextBox.Name = "translatedRichTextBox";
            this.translatedRichTextBox.Size = new System.Drawing.Size(406, 290);
            this.translatedRichTextBox.TabIndex = 9;
            this.translatedRichTextBox.Text = "";
            this.translatedRichTextBox.WordWrap = false;
            // 
            // reverseTranslateBtn
            // 
            this.reverseTranslateBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.reverseTranslateBtn.Location = new System.Drawing.Point(0, 292);
            this.reverseTranslateBtn.Name = "reverseTranslateBtn";
            this.reverseTranslateBtn.Size = new System.Drawing.Size(408, 23);
            this.reverseTranslateBtn.TabIndex = 11;
            this.reverseTranslateBtn.Text = "Chinese (Traditional) -> English";
            this.reverseTranslateBtn.UseVisualStyleBackColor = true;
            this.reverseTranslateBtn.Visible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ocrEngineToolStripComboBox,
            this.ocrProfileToolStripComboBox,
            this.translateApiToolStripComboBox,
            this.translateFromToolStripComboBox,
            this.switchTranslateToolStripButton,
            this.translateToToolStripComboBox,
            this.translateToolStripButton,
            this.languageQuickslotToolStripButton,
            this.toolStripSeparator3,
            this.captureAreaToolStripButton,
            this.captureAreaFreezeToolStripButton,
            this.captureAreaEditableToolStripButton,
            this.editImageSelectionToolStripButton,
            this.refreshImageToolStripButton,
            this.toolStripSeparator2,
            this.openFileToolStripButton,
            this.saveFileToolStripButton,
            this.copyToolStripButton,
            this.pasteToolStripButton,
            this.toolStripSeparator1,
            this.settingsToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(804, 25);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ocrEngineToolStripComboBox
            // 
            this.ocrEngineToolStripComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ocrEngineToolStripComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ocrEngineToolStripComboBox.AutoSize = false;
            this.ocrEngineToolStripComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.ocrEngineToolStripComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ocrEngineToolStripComboBox.Name = "ocrEngineToolStripComboBox";
            this.ocrEngineToolStripComboBox.Size = new System.Drawing.Size(100, 21);
            this.ocrEngineToolStripComboBox.ToolTipText = "OCR Engine";
            this.ocrEngineToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox_SelectedIndexChanged);
            this.ocrEngineToolStripComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripComboBox_KeyDown);
            this.ocrEngineToolStripComboBox.Validated += new System.EventHandler(this.toolStripComboBox_Validated);
            // 
            // ocrProfileToolStripComboBox
            // 
            this.ocrProfileToolStripComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ocrProfileToolStripComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ocrProfileToolStripComboBox.AutoSize = false;
            this.ocrProfileToolStripComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.ocrProfileToolStripComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ocrProfileToolStripComboBox.Name = "ocrProfileToolStripComboBox";
            this.ocrProfileToolStripComboBox.Size = new System.Drawing.Size(70, 21);
            this.ocrProfileToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox_SelectedIndexChanged);
            this.ocrProfileToolStripComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripComboBox_KeyDown);
            this.ocrProfileToolStripComboBox.Validated += new System.EventHandler(this.toolStripComboBox_Validated);
            // 
            // translateApiToolStripComboBox
            // 
            this.translateApiToolStripComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.translateApiToolStripComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.translateApiToolStripComboBox.AutoSize = false;
            this.translateApiToolStripComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.translateApiToolStripComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.translateApiToolStripComboBox.Name = "translateApiToolStripComboBox";
            this.translateApiToolStripComboBox.Size = new System.Drawing.Size(75, 21);
            this.translateApiToolStripComboBox.ToolTipText = "Translate Engine";
            this.translateApiToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox_SelectedIndexChanged);
            this.translateApiToolStripComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripComboBox_KeyDown);
            this.translateApiToolStripComboBox.Validated += new System.EventHandler(this.toolStripComboBox_Validated);
            // 
            // translateFromToolStripComboBox
            // 
            this.translateFromToolStripComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.translateFromToolStripComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.translateFromToolStripComboBox.AutoSize = false;
            this.translateFromToolStripComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.translateFromToolStripComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.translateFromToolStripComboBox.Name = "translateFromToolStripComboBox";
            this.translateFromToolStripComboBox.Size = new System.Drawing.Size(125, 21);
            this.translateFromToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox_SelectedIndexChanged);
            this.translateFromToolStripComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripComboBox_KeyDown);
            this.translateFromToolStripComboBox.Validated += new System.EventHandler(this.toolStripComboBox_Validated);
            // 
            // switchTranslateToolStripButton
            // 
            this.switchTranslateToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.switchTranslateToolStripButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.switchTranslateToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.switchTranslateToolStripButton.Name = "switchTranslateToolStripButton";
            this.switchTranslateToolStripButton.Size = new System.Drawing.Size(26, 22);
            this.switchTranslateToolStripButton.Text = "<->";
            this.switchTranslateToolStripButton.ToolTipText = "Swap Languages";
            this.switchTranslateToolStripButton.Click += new System.EventHandler(this.switchTranslateToolStripButton_Click);
            // 
            // translateToToolStripComboBox
            // 
            this.translateToToolStripComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.translateToToolStripComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.translateToToolStripComboBox.AutoSize = false;
            this.translateToToolStripComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.translateToToolStripComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.translateToToolStripComboBox.Name = "translateToToolStripComboBox";
            this.translateToToolStripComboBox.Size = new System.Drawing.Size(125, 21);
            this.translateToToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox_SelectedIndexChanged);
            this.translateToToolStripComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripComboBox_KeyDown);
            this.translateToToolStripComboBox.Validated += new System.EventHandler(this.toolStripComboBox_Validated);
            // 
            // translateToolStripButton
            // 
            this.translateToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.translateToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("translateToolStripButton.Image")));
            this.translateToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.translateToolStripButton.Name = "translateToolStripButton";
            this.translateToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.translateToolStripButton.Text = "toolStripButton7";
            this.translateToolStripButton.ToolTipText = "Translate";
            this.translateToolStripButton.Click += new System.EventHandler(this.translateToolStripButton_Click);
            // 
            // languageQuickslotToolStripButton
            // 
            this.languageQuickslotToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.languageQuickslotToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("languageQuickslotToolStripButton.Image")));
            this.languageQuickslotToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.languageQuickslotToolStripButton.Name = "languageQuickslotToolStripButton";
            this.languageQuickslotToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.languageQuickslotToolStripButton.ToolTipText = "Quick Slot";
            this.languageQuickslotToolStripButton.Click += new System.EventHandler(this.languageQuickslotToolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // captureAreaToolStripButton
            // 
            this.captureAreaToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.captureAreaToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("captureAreaToolStripButton.Image")));
            this.captureAreaToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.captureAreaToolStripButton.Name = "captureAreaToolStripButton";
            this.captureAreaToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.captureAreaToolStripButton.Text = "toolStripButton3";
            this.captureAreaToolStripButton.ToolTipText = "Capture Area";
            this.captureAreaToolStripButton.Click += new System.EventHandler(this.captureAreaToolStripButton_Click);
            // 
            // captureAreaFreezeToolStripButton
            // 
            this.captureAreaFreezeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.captureAreaFreezeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("captureAreaFreezeToolStripButton.Image")));
            this.captureAreaFreezeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.captureAreaFreezeToolStripButton.Name = "captureAreaFreezeToolStripButton";
            this.captureAreaFreezeToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.captureAreaFreezeToolStripButton.Text = "toolStripButton4";
            this.captureAreaFreezeToolStripButton.ToolTipText = "Capture Area (Freeze Screen)";
            this.captureAreaFreezeToolStripButton.Click += new System.EventHandler(this.captureAreaFreezeToolStripButton_Click);
            // 
            // captureAreaEditableToolStripButton
            // 
            this.captureAreaEditableToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.captureAreaEditableToolStripButton.Enabled = false;
            this.captureAreaEditableToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("captureAreaEditableToolStripButton.Image")));
            this.captureAreaEditableToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.captureAreaEditableToolStripButton.Name = "captureAreaEditableToolStripButton";
            this.captureAreaEditableToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.captureAreaEditableToolStripButton.Text = "toolStripButton6";
            this.captureAreaEditableToolStripButton.ToolTipText = "Capture Area (Editable)";
            this.captureAreaEditableToolStripButton.Visible = false;
            this.captureAreaEditableToolStripButton.Click += new System.EventHandler(this.captureAreaEditableToolStripButton_Click);
            // 
            // editImageSelectionToolStripButton
            // 
            this.editImageSelectionToolStripButton.CheckOnClick = true;
            this.editImageSelectionToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editImageSelectionToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("editImageSelectionToolStripButton.Image")));
            this.editImageSelectionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editImageSelectionToolStripButton.Name = "editImageSelectionToolStripButton";
            this.editImageSelectionToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.editImageSelectionToolStripButton.Text = "toolStripButton1";
            this.editImageSelectionToolStripButton.ToolTipText = "Edit Selection";
            this.editImageSelectionToolStripButton.CheckedChanged += new System.EventHandler(this.editImageSelectionToolStripButton_CheckedChanged);
            // 
            // refreshImageToolStripButton
            // 
            this.refreshImageToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refreshImageToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("refreshImageToolStripButton.Image")));
            this.refreshImageToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshImageToolStripButton.Name = "refreshImageToolStripButton";
            this.refreshImageToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.refreshImageToolStripButton.Text = "toolStripButton1";
            this.refreshImageToolStripButton.ToolTipText = "Refresh Screen";
            this.refreshImageToolStripButton.Click += new System.EventHandler(this.refreshImageToolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // openFileToolStripButton
            // 
            this.openFileToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openFileToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openFileToolStripButton.Image")));
            this.openFileToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openFileToolStripButton.Name = "openFileToolStripButton";
            this.openFileToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openFileToolStripButton.Text = "toolStripButton2";
            this.openFileToolStripButton.ToolTipText = "Open";
            this.openFileToolStripButton.Click += new System.EventHandler(this.openFileToolStripButton_Click);
            // 
            // saveFileToolStripButton
            // 
            this.saveFileToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveFileToolStripButton.Enabled = false;
            this.saveFileToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveFileToolStripButton.Image")));
            this.saveFileToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveFileToolStripButton.Name = "saveFileToolStripButton";
            this.saveFileToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveFileToolStripButton.Text = "toolStripButton3";
            this.saveFileToolStripButton.ToolTipText = "Save";
            this.saveFileToolStripButton.Visible = false;
            this.saveFileToolStripButton.Click += new System.EventHandler(this.saveFileToolStripButton_Click);
            // 
            // copyToolStripButton
            // 
            this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripButton.Image")));
            this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripButton.Name = "copyToolStripButton";
            this.copyToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.copyToolStripButton.Text = "toolStripButton7";
            this.copyToolStripButton.ToolTipText = "Copy";
            this.copyToolStripButton.Click += new System.EventHandler(this.copyToolStripButton_Click);
            // 
            // pasteToolStripButton
            // 
            this.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pasteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripButton.Image")));
            this.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripButton.Name = "pasteToolStripButton";
            this.pasteToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.pasteToolStripButton.Text = "toolStripButton8";
            this.pasteToolStripButton.ToolTipText = "Paste";
            this.pasteToolStripButton.Click += new System.EventHandler(this.pasteToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // settingsToolStripButton
            // 
            this.settingsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.settingsToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("settingsToolStripButton.Image")));
            this.settingsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.settingsToolStripButton.Name = "settingsToolStripButton";
            this.settingsToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.settingsToolStripButton.Text = "toolStripButton5";
            this.settingsToolStripButton.ToolTipText = "Settings";
            this.settingsToolStripButton.Click += new System.EventHandler(this.settingsToolStripButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 552);
            this.Controls.Add(this.imageTextSplitContainer);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "OCR";
            this.imageTextSplitContainer.Panel1.ResumeLayout(false);
            this.imageTextSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageTextSplitContainer)).EndInit();
            this.imageTextSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ocrImagePanel)).EndInit();
            this.textSplitContainer.Panel1.ResumeLayout(false);
            this.textSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textSplitContainer)).EndInit();
            this.textSplitContainer.ResumeLayout(false);
            this.originalTextPanel.ResumeLayout(false);
            this.translatedTextPanel.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer imageTextSplitContainer;
        private System.Windows.Forms.SplitContainer textSplitContainer;
        private System.Windows.Forms.RichTextBox originalRichTextBox;
        private System.Windows.Forms.RichTextBox translatedRichTextBox;
        private OcrImagePanel ocrImagePanel;
        private System.Windows.Forms.Button normalTranslateBtn;
        private System.Windows.Forms.Panel originalTextPanel;
        private System.Windows.Forms.Panel translatedTextPanel;
        private System.Windows.Forms.Button reverseTranslateBtn;
        private ToolStripEx toolStrip1;
        private System.Windows.Forms.ToolStripComboBox ocrEngineToolStripComboBox;
        private System.Windows.Forms.ToolStripComboBox translateApiToolStripComboBox;
        private System.Windows.Forms.ToolStripButton switchTranslateToolStripButton;
        private System.Windows.Forms.ToolStripComboBox translateFromToolStripComboBox;
        private System.Windows.Forms.ToolStripComboBox translateToToolStripComboBox;
        private System.Windows.Forms.ToolStripComboBox ocrProfileToolStripComboBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton captureAreaToolStripButton;
        private System.Windows.Forms.ToolStripButton captureAreaFreezeToolStripButton;
        private System.Windows.Forms.ToolStripButton settingsToolStripButton;
        private System.Windows.Forms.ToolStripButton translateToolStripButton;
        private System.Windows.Forms.ToolStripButton captureAreaEditableToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton openFileToolStripButton;
        private System.Windows.Forms.ToolStripButton saveFileToolStripButton;
        private System.Windows.Forms.ToolStripButton copyToolStripButton;
        private System.Windows.Forms.ToolStripButton pasteToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton languageQuickslotToolStripButton;
        private System.Windows.Forms.ToolStripButton editImageSelectionToolStripButton;
        private System.Windows.Forms.ToolStripButton refreshImageToolStripButton;
    }
}