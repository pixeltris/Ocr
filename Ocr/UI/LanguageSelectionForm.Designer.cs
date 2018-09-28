namespace Ocr.UI
{
    partial class LanguageSelectionForm
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
            this.availableLanguagesListBox = new System.Windows.Forms.ListBox();
            this.moveRightButton = new System.Windows.Forms.Button();
            this.moveAllRightButton = new System.Windows.Forms.Button();
            this.moveLeftButton = new System.Windows.Forms.Button();
            this.moveAllLeftButton = new System.Windows.Forms.Button();
            this.selectedLanguagesListBox = new System.Windows.Forms.ListBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // availableLanguagesListBox
            // 
            this.availableLanguagesListBox.FormattingEnabled = true;
            this.availableLanguagesListBox.IntegralHeight = false;
            this.availableLanguagesListBox.Location = new System.Drawing.Point(8, 7);
            this.availableLanguagesListBox.Name = "availableLanguagesListBox";
            this.availableLanguagesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.availableLanguagesListBox.Size = new System.Drawing.Size(232, 396);
            this.availableLanguagesListBox.TabIndex = 0;
            // 
            // moveRightButton
            // 
            this.moveRightButton.Location = new System.Drawing.Point(246, 97);
            this.moveRightButton.Name = "moveRightButton";
            this.moveRightButton.Size = new System.Drawing.Size(30, 25);
            this.moveRightButton.TabIndex = 1;
            this.moveRightButton.Text = ">";
            this.moveRightButton.UseVisualStyleBackColor = true;
            this.moveRightButton.Click += new System.EventHandler(this.moveRightButton_Click);
            // 
            // moveAllRightButton
            // 
            this.moveAllRightButton.Location = new System.Drawing.Point(246, 183);
            this.moveAllRightButton.Name = "moveAllRightButton";
            this.moveAllRightButton.Size = new System.Drawing.Size(30, 25);
            this.moveAllRightButton.TabIndex = 2;
            this.moveAllRightButton.Text = ">>";
            this.moveAllRightButton.UseVisualStyleBackColor = true;
            this.moveAllRightButton.Click += new System.EventHandler(this.moveAllRightButton_Click);
            // 
            // moveLeftButton
            // 
            this.moveLeftButton.Location = new System.Drawing.Point(246, 123);
            this.moveLeftButton.Name = "moveLeftButton";
            this.moveLeftButton.Size = new System.Drawing.Size(30, 25);
            this.moveLeftButton.TabIndex = 3;
            this.moveLeftButton.Text = "<";
            this.moveLeftButton.UseVisualStyleBackColor = true;
            this.moveLeftButton.Click += new System.EventHandler(this.moveLeftButton_Click);
            // 
            // moveAllLeftButton
            // 
            this.moveAllLeftButton.Location = new System.Drawing.Point(246, 209);
            this.moveAllLeftButton.Name = "moveAllLeftButton";
            this.moveAllLeftButton.Size = new System.Drawing.Size(30, 25);
            this.moveAllLeftButton.TabIndex = 4;
            this.moveAllLeftButton.Text = "<<";
            this.moveAllLeftButton.UseVisualStyleBackColor = true;
            this.moveAllLeftButton.Click += new System.EventHandler(this.moveAllLeftButton_Click);
            // 
            // selectedLanguagesListBox
            // 
            this.selectedLanguagesListBox.FormattingEnabled = true;
            this.selectedLanguagesListBox.IntegralHeight = false;
            this.selectedLanguagesListBox.Location = new System.Drawing.Point(282, 7);
            this.selectedLanguagesListBox.Name = "selectedLanguagesListBox";
            this.selectedLanguagesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.selectedLanguagesListBox.Size = new System.Drawing.Size(232, 396);
            this.selectedLanguagesListBox.TabIndex = 5;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(39, 5);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(118, 5);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 413);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(521, 33);
            this.panel1.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.okButton);
            this.panel2.Controls.Add(this.cancelButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(321, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 33);
            this.panel2.TabIndex = 8;
            // 
            // LanguageSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 446);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.selectedLanguagesListBox);
            this.Controls.Add(this.moveAllLeftButton);
            this.Controls.Add(this.moveLeftButton);
            this.Controls.Add(this.moveAllRightButton);
            this.Controls.Add(this.moveRightButton);
            this.Controls.Add(this.availableLanguagesListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "LanguageSelectionForm";
            this.Text = "Languages";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox availableLanguagesListBox;
        private System.Windows.Forms.Button moveRightButton;
        private System.Windows.Forms.Button moveAllRightButton;
        private System.Windows.Forms.Button moveLeftButton;
        private System.Windows.Forms.Button moveAllLeftButton;
        private System.Windows.Forms.ListBox selectedLanguagesListBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;

    }
}