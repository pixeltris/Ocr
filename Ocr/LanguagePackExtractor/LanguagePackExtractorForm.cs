using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ocr
{
    partial class LanguagePackExtractorForm : Form
    {
        private Thread installThread;
        private Dictionary<string, LanguagePack> languagePacks;

        public LanguagePackExtractorForm()
        {
            InitializeComponent();

            languagePacks = new Dictionary<string, LanguagePack>();
            installBtn.Enabled = false;
            installSelectedBtn.Enabled = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            UpdateLanguagePacksAndSizes();
            base.OnLoad(e);
        }

        private void installBtn_Click(object sender, EventArgs e)
        {
            if (installBtn.Text == "Cancel")
            {
                CancelInstall();
            }
            else
            {
                Install(false);
            }
        }

        private void installSelectedBtn_Click(object sender, EventArgs e)
        {
            if (installSelectedBtn.Text == "Cancel")
            {
                CancelInstall();
            }
            else
            {
                Install(true);
            }
        }

        private void clearLogsBtn_Click(object sender, EventArgs e)
        {
            ClearLogs();
        }

        private void Log(string value, params object[] args)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { Log(value, args); });
                return;
            }

            logsRichTextBox.AppendText(string.Format(value, args) + Environment.NewLine);
        }

        private void ClearLogs()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { ClearLogs(); });
                return;
            }

            logsRichTextBox.Clear();
        }

        private void Install(bool selected)
        {
            if (InvokeRequired)
            {
                Dictionary<string, LanguagePack> packs = GetLanguagePacks();
                bool installed = false;

                List<string> selectedNames = new List<string>();
                if (selected)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        foreach (ListViewItem item in languagePacksListView.Items)
                        {
                            if (item.Selected)
                            {
                                selectedNames.Add(item.SubItems[0].Text);
                            }
                        }
                    });
                }
                else
                {
                    foreach (LanguagePack languagePack in packs.Values)
                    {
                        selectedNames.Add(languagePack.DisplayName);
                    }
                }

                foreach (LanguagePack languagePack in packs.Values)
                {
                    if (!selectedNames.Contains(languagePack.DisplayName) || languagePack.Installed)
                        continue;                    

                    if (languagePack.Type == LanguagePackType.OCR)
                    {
                        installed = true;
                        RunCommand("/Online /Add-Capability /CapabilityName:{0}", languagePack.Name);
                    }
                }

                if (installed)
                {
                    UpdateLanguagePacks(true);
                }
                Invoke((MethodInvoker)delegate
                {
                    installBtn.Text = "Install";
                    installSelectedBtn.Text = "Install Selected";
                });
            }
            else
            {
                StopInstallThread();
                installBtn.Text = "Cancel";
                installSelectedBtn.Text = "Cancel";
                installThread = new Thread(delegate ()
                {
                    Install(selected);
                });
                installThread.Start();
            }
        }

        private void CancelInstall()
        {
            StopInstallThread();
            Invoke((MethodInvoker)delegate
            {
                installBtn.Text = "Install";
                installSelectedBtn.Text = "Install Selected";
            });
        }

        private void StopInstallThread()
        {
            if (installThread != null)
            {
                installThread.Abort();
                installThread = null;
            }
        }

        private void UpdateLanguagePacksAndSizes()
        {
            new Thread(delegate ()
            {
                UpdateLanguagePacks();
                Dictionary<string, LanguagePack> packs = GetLanguagePacks();
                foreach (LanguagePack languagePack in packs.Values)
                {
                    if (languagePack.Type != LanguagePackType.OCR)
                    {
                        continue;
                    }

                    List<LanguagePack> pairPacks = GetLanguagePacks(languagePack.Language);
                    LanguagePack basic = pairPacks.FirstOrDefault(x => x.Type == LanguagePackType.Basic);

                    if (!string.IsNullOrEmpty(languagePack.Name))
                    {
                        string downloadHeader = "Download Size : ";
                        string[] capabilityInfo = RunCommand("/Online /Get-CapabilityInfo /CapabilityName:{0}", languagePack.Name);
                        foreach (string info in capabilityInfo)
                        {
                            if (info.StartsWith(downloadHeader))
                            {
                                languagePack.Size = info.Substring(downloadHeader.Length).Trim();
                            }
                        }

                        if (basic != null)
                        {
                            capabilityInfo = RunCommand("/Online /Get-CapabilityInfo /CapabilityName:{0}", basic.Name);
                            foreach (string info in capabilityInfo)
                            {
                                if (info.StartsWith(downloadHeader))
                                {
                                    basic.Size = info.Substring(downloadHeader.Length).Trim();
                                }
                            }
                        }
                    }
                }
                packs = GetLanguagePacks();
                if (packs.Count > 0)
                {
                    UpdateLanguagePacks(true);
                }
                Invoke((MethodInvoker)delegate
                {
                    installBtn.Enabled = true;
                    installSelectedBtn.Enabled = true;
                });
            }).Start();
        }

        private void UpdateLanguagePacks()
        {
            UpdateLanguagePacks(false);
        }

        private void UpdateLanguagePacks(bool silent)
        {
            if (InvokeRequired)
            {
                string languageHeader = "Capability Identity : Language.";

                string[] capabilities = RunCommand(silent, "/Online /Get-Capabilities");
                for (int i = 0; i < capabilities.Length; i++)
                {
                    if (capabilities[i].StartsWith(languageHeader))
                    {
                        string languageInfo = capabilities[i].Substring(languageHeader.Length);
                        LanguagePackType type;
                        string language, name;
                        if (GetInfo(languageInfo, out type, out language, out name))
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                bool installed = i < capabilities.Length - 1 ? capabilities[i + 1].Contains("Installed") : false;
                                if (languagePacks.ContainsKey(name))
                                {
                                    LanguagePack languagePack = languagePacks[name];
                                    languagePack.Installed = installed;
                                }
                                else
                                {
                                    LanguagePack languagePack = new LanguagePack(type, language, name);
                                    languagePack.Installed = installed;
                                    languagePacks[name] = languagePack;
                                }
                            });
                        }
                    }
                }

                UpdateLanguagePacksTable();           
            }
            else
            {
                new Thread(delegate ()
                {
                    UpdateLanguagePacks(silent);
                }).Start();
            }
        }

        private void UpdateLanguagePacksTable()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { UpdateLanguagePacksTable(); });
                return;
            }

            foreach (LanguagePack languagePack in languagePacks.Values)
            {
                if (languagePack.Type != LanguagePackType.Basic)
                    continue;

                List<LanguagePack> packs = GetLanguagePacks(languagePack.Language);
                LanguagePack ocrLanguagePack = packs.FirstOrDefault(x => x.Type == LanguagePackType.OCR);

                ListViewItem listViewItem = null;
                foreach (ListViewItem item in languagePacksListView.Items)
                {
                    if (item.SubItems[0].Text == languagePack.DisplayName)
                    {
                        listViewItem = item;
                        break;
                    }
                }

                if (listViewItem == null)
                {
                    listViewItem = new ListViewItem(new string[] { "", "", "", "" });
                    languagePacksListView.Items.Add(listViewItem);
                }

                string types = string.Empty;
                List<LanguagePackType> packTypes = new List<LanguagePackType>();
                foreach (LanguagePack pack in packs)
                {
                    if (!packTypes.Contains(pack.Type))
                    {
                        types += pack.Type + ", ";
                    }
                }
                types = types.TrimEnd(',', ' ');
                string downloadSize = languagePack.Size;
                if (ocrLanguagePack != null)
                {
                    downloadSize += " + " + packs.FirstOrDefault(x => x.Type == LanguagePackType.OCR).Size;
                }

                listViewItem.SubItems[0].Text = languagePack.DisplayName;
                listViewItem.SubItems[1].Text = types;
                listViewItem.SubItems[2].Text = downloadSize;
                listViewItem.SubItems[3].Text = ocrLanguagePack == null ? languagePack.Installed.ToString() : ocrLanguagePack.Installed.ToString();
            }
        }

        private List<LanguagePack> GetLanguagePacks(string language)
        {
            List<LanguagePack> result = new List<LanguagePack>();

            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { result = GetLanguagePacks(language); });
                return result;
            }

            try
            {
                CultureInfo culture = new CultureInfo(language);
                foreach (LanguagePack languagePack in languagePacks.Values)
                {
                    CultureInfo languagePackCulture = new CultureInfo(languagePack.Language);
                    if (languagePack.Type == LanguagePackType.Font)
                    {
                        if (languagePackCulture.TwoLetterISOLanguageName == culture.TwoLetterISOLanguageName)
                        {
                            result.Add(languagePack);
                        }
                    }
                    else if (languagePackCulture.Name == culture.Name)
                    {
                        result.Add(languagePack);
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        private Dictionary<string, LanguagePack> GetLanguagePacks()
        {
            if (InvokeRequired)
            {
                Dictionary<string, LanguagePack> result = null;
                Invoke((MethodInvoker)delegate { result = GetLanguagePacks(); });
                return result;
            }
            return new Dictionary<string, LanguagePack>(languagePacks);
        }

        private string[] RunCommand(string command, params object[] args)
        {
            return RunCommand(false, command, args);
        }

        private string RunCommandFlat(string command, params object[] args)
        {
            return RunCommandFlat(false, command, args);
        }

        private string[] RunCommand(bool silent, string command, params object[] args)
        {
            string[] lines = RunCommandFlat(silent, command, args).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return lines;
        }

        private string RunCommandFlat(bool silent, string command, params object[] args)
        {
            string formattedCommand = string.Format(command, args);
            string result = string.Empty;

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "Dism";
                process.StartInfo.Arguments = formattedCommand;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        result += e.Data + Environment.NewLine;
                        if (!silent)
                            Log(e.Data);
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        result += e.Data + Environment.NewLine;
                        if(!silent)
                            Log(e.Data);
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }

            return result;
        }

        private bool GetInfo(string capability, out LanguagePackType type, out string language, out string name)
        {
            type = LanguagePackType.Unknown;
            language = null;
            name = "Language." + capability;

            string[] splitted = capability.Split(new char[] { '.', '~' }, StringSplitOptions.RemoveEmptyEntries);

            string packageTypeStr = splitted[0].ToLower();
            switch (packageTypeStr)
            {
                case "basic": type = LanguagePackType.Basic; break;
                case "fonts": return false;//type = LanguagePackType.Font; break;
                case "ocr": type = LanguagePackType.OCR; break;
                case "handwriting": type = LanguagePackType.Handwriting; break;
                case "texttospeech": type = LanguagePackType.TextToSpeech; break;
                case "speech": type = LanguagePackType.Speech; break;
                default: Log("[WARNING] Unknown package type " + packageTypeStr); return false;
            }

            try
            {
                language = LocaleHelper.GetCultureCode(splitted[1]);
            }
            catch (Exception e)
            {
                Log("Invalid language " + language + Environment.NewLine + "Exception: " + e);
            }            

            return true;
        }

        class LanguagePack
        {
            public LanguagePackType Type { get; set; }
            public string Language { get; set; }
            public string Name { get; set; }
            public string Size { get; set; }
            public bool Installed { get; set; }
            
            public string DisplayName
            {
                get
                {
                    CultureInfo cultureInfo = LocaleHelper.GetCulture(Language);
                    return cultureInfo.Name + " (" + cultureInfo.DisplayName + ")";
                }
            }

            public LanguagePack(LanguagePackType type, string language, string name)
            {
                Type = type;
                Language = language;
                Name = name;
            }
        }

        enum LanguagePackType
        {
            Unknown,
            Basic,
            Font,
            OCR,
            Handwriting,
            TextToSpeech,
            Speech
        }
    }
}