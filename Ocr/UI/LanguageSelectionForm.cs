using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ocr.UI
{
    public partial class LanguageSelectionForm : Form
    {
        private Dictionary<string, string> languages;
        public string Languages { get; set; }

        public LanguageSelectionForm(Dictionary<string, string> languages, string activeLanguages)
        {
            InitializeComponent();

            this.languages = languages;

            availableLanguagesListBox.Sorted = true;
            selectedLanguagesListBox.Sorted = true;            

            if (string.IsNullOrWhiteSpace(activeLanguages))
            {
                foreach (KeyValuePair<string, string> language in languages)
                {
                    selectedLanguagesListBox.Items.Add(language.Key);
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> language in languages)
                {
                    availableLanguagesListBox.Items.Add(language.Key);
                }

                string[] splitted = activeLanguages.Split(new char[] { Program.LanguageFilterSplitChar }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string value in splitted)
                {
                    string trimmed = value.Trim();
                    if (!string.IsNullOrWhiteSpace(trimmed) && availableLanguagesListBox.Items.Contains(trimmed))
                    {
                        availableLanguagesListBox.Items.Remove(trimmed);
                        selectedLanguagesListBox.Items.Add(trimmed);
                    }
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            string result = string.Empty;
            foreach (string value in selectedLanguagesListBox.Items)
            {
                result += value + " " + Program.LanguageFilterSplitChar + " ";
            }
            result = result.TrimEnd(Program.LanguageFilterSplitChar, ' ');
            Languages = result;
            base.OnClosing(e);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void moveRightButton_Click(object sender, EventArgs e)
        {
            if (availableLanguagesListBox.SelectedItems.Count > 0)
            {
                List<string> items = new List<string>();
                foreach (string value in availableLanguagesListBox.SelectedItems)
                    items.Add(value);

                foreach (string value in items)
                {
                    availableLanguagesListBox.Items.Remove(value);
                    selectedLanguagesListBox.Items.Add(value);
                }
                SortLanguages();
            }
        }

        private void moveLeftButton_Click(object sender, EventArgs e)
        {
            if (selectedLanguagesListBox.SelectedItems.Count > 0)
            {
                List<string> items = new List<string>();
                foreach (string value in selectedLanguagesListBox.SelectedItems)
                    items.Add(value);

                foreach (string value in items)
                {
                    selectedLanguagesListBox.Items.Remove(value);
                    availableLanguagesListBox.Items.Add(value);
                }
                SortLanguages();
            }
        }

        private void moveAllRightButton_Click(object sender, EventArgs e)
        {
            if (availableLanguagesListBox.Items.Count > 0)
            {
                List<string> items = new List<string>();
                foreach (string value in availableLanguagesListBox.Items)
                    items.Add(value);

                foreach (string value in items)
                {
                    availableLanguagesListBox.Items.Remove(value);
                    selectedLanguagesListBox.Items.Add(value);
                }
                SortLanguages();
            }
        }

        private void moveAllLeftButton_Click(object sender, EventArgs e)
        {
            if (selectedLanguagesListBox.Items.Count > 0)
            {
                List<string> items = new List<string>();
                foreach (string value in selectedLanguagesListBox.Items)
                    items.Add(value);

                foreach (string value in items)
                {
                    selectedLanguagesListBox.Items.Remove(value);
                    availableLanguagesListBox.Items.Add(value);
                }
                SortLanguages();
            }
        }

        private void SortLanguages()
        {
            SortLanguages(availableLanguagesListBox);
            SortLanguages(selectedLanguagesListBox);            
        }

        private void SortLanguages(ListBox listbox)
        {
        }
    }
}
