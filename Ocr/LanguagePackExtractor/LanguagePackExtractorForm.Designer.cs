namespace Ocr
{
    partial class LanguagePackExtractorForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.languagePacksListView = new System.Windows.Forms.ListView();
            this.languageColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typesColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.expectedSizeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.installedColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.clearLogsBtn = new System.Windows.Forms.Button();
            this.installSelectedBtn = new System.Windows.Forms.Button();
            this.installBtn = new System.Windows.Forms.Button();
            this.logsRichTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.languagePacksListView);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.logsRichTextBox);
            this.splitContainer1.Size = new System.Drawing.Size(609, 434);
            this.splitContainer1.SplitterDistance = 238;
            this.splitContainer1.TabIndex = 0;
            // 
            // languagePacksListView
            // 
            this.languagePacksListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.languageColumn,
            this.typesColumn,
            this.expectedSizeColumn,
            this.installedColumn});
            this.languagePacksListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.languagePacksListView.Location = new System.Drawing.Point(0, 29);
            this.languagePacksListView.Name = "languagePacksListView";
            this.languagePacksListView.Size = new System.Drawing.Size(609, 209);
            this.languagePacksListView.TabIndex = 1;
            this.languagePacksListView.UseCompatibleStateImageBehavior = false;
            this.languagePacksListView.View = System.Windows.Forms.View.Details;
            // 
            // languageColumn
            // 
            this.languageColumn.Text = "Language";
            this.languageColumn.Width = 143;
            // 
            // typesColumn
            // 
            this.typesColumn.Text = "Types";
            this.typesColumn.Width = 103;
            // 
            // expectedSizeColumn
            // 
            this.expectedSizeColumn.Text = "Expected Size";
            this.expectedSizeColumn.Width = 81;
            // 
            // installedColumn
            // 
            this.installedColumn.Text = "Installed";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.clearLogsBtn);
            this.panel1.Controls.Add(this.installSelectedBtn);
            this.panel1.Controls.Add(this.installBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(609, 29);
            this.panel1.TabIndex = 0;
            // 
            // clearLogsBtn
            // 
            this.clearLogsBtn.Location = new System.Drawing.Point(170, 3);
            this.clearLogsBtn.Name = "clearLogsBtn";
            this.clearLogsBtn.Size = new System.Drawing.Size(75, 23);
            this.clearLogsBtn.TabIndex = 2;
            this.clearLogsBtn.Text = "Clear Logs";
            this.clearLogsBtn.UseVisualStyleBackColor = true;
            this.clearLogsBtn.Click += new System.EventHandler(this.clearLogsBtn_Click);
            // 
            // installSelectedBtn
            // 
            this.installSelectedBtn.Location = new System.Drawing.Point(79, 3);
            this.installSelectedBtn.Name = "installSelectedBtn";
            this.installSelectedBtn.Size = new System.Drawing.Size(90, 23);
            this.installSelectedBtn.TabIndex = 1;
            this.installSelectedBtn.Text = "Install Selected";
            this.installSelectedBtn.UseVisualStyleBackColor = true;
            this.installSelectedBtn.Click += new System.EventHandler(this.installSelectedBtn_Click);
            // 
            // installBtn
            // 
            this.installBtn.Location = new System.Drawing.Point(3, 3);
            this.installBtn.Name = "installBtn";
            this.installBtn.Size = new System.Drawing.Size(75, 23);
            this.installBtn.TabIndex = 0;
            this.installBtn.Text = "Install";
            this.installBtn.UseVisualStyleBackColor = true;
            this.installBtn.Click += new System.EventHandler(this.installBtn_Click);
            // 
            // logsRichTextBox
            // 
            this.logsRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.logsRichTextBox.Name = "logsRichTextBox";
            this.logsRichTextBox.Size = new System.Drawing.Size(609, 192);
            this.logsRichTextBox.TabIndex = 0;
            this.logsRichTextBox.Text = "";
            // 
            // LanguagePackExtractorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 434);
            this.Controls.Add(this.splitContainer1);
            this.Name = "LanguagePackExtractorForm";
            this.Text = "Language Pack Extractor";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView languagePacksListView;
        private System.Windows.Forms.ColumnHeader languageColumn;
        private System.Windows.Forms.ColumnHeader typesColumn;
        private System.Windows.Forms.ColumnHeader expectedSizeColumn;
        private System.Windows.Forms.ColumnHeader installedColumn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button clearLogsBtn;
        private System.Windows.Forms.Button installSelectedBtn;
        private System.Windows.Forms.Button installBtn;
        private System.Windows.Forms.RichTextBox logsRichTextBox;
    }
}