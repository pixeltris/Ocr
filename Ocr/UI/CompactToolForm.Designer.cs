namespace Ocr.UI
{
    partial class CompactToolForm
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
            this.originalRichTextBox = new System.Windows.Forms.RichTextBox();
            this.translatedRichTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel1.Controls.Add(this.originalRichTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel2.Controls.Add(this.translatedRichTextBox);
            this.splitContainer1.Size = new System.Drawing.Size(444, 262);
            this.splitContainer1.SplitterDistance = 209;
            this.splitContainer1.TabIndex = 0;
            // 
            // originalRichTextBox
            // 
            this.originalRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.originalRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.originalRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.originalRichTextBox.Name = "originalRichTextBox";
            this.originalRichTextBox.Size = new System.Drawing.Size(209, 262);
            this.originalRichTextBox.TabIndex = 9;
            this.originalRichTextBox.Text = "";
            // 
            // translatedRichTextBox
            // 
            this.translatedRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.translatedRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.translatedRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.translatedRichTextBox.Name = "translatedRichTextBox";
            this.translatedRichTextBox.Size = new System.Drawing.Size(231, 262);
            this.translatedRichTextBox.TabIndex = 10;
            this.translatedRichTextBox.Text = "";
            // 
            // CompactToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 262);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "CompactToolForm";
            this.Text = "Chinese (Traditional) - English | Translated | Windows | 1x Linear";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox originalRichTextBox;
        private System.Windows.Forms.RichTextBox translatedRichTextBox;
    }
}