using Ocr.Translate;
using Ocr.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ocr
{
    class GoogleOcr : OcrEngine
    {
        private static bool installed = ProcessExists("BlueStacks");
        private Automation automation = new Automation();

        public override string Name
        {
            get { return "Google"; }
        }

        public override bool IsInstalled
        {
            get { return installed; }
        }

        public GoogleOcr()
        {
            //automation.UpdateReferenceImage = true;
            automation.Path = System.IO.Path.Combine("Images", "Google");
            //BuildRelativeAutomation();
            BuildImageAutomation();
        }

        private void BuildRelativeAutomation()
        {            
            // Top left of BlueStacks
            automation.AddReferenceImage("BlueStacksReference.png");

            // Translate icon
            automation.AddStartImage("BlueStacksStart.png");

            // Camera icon
            automation.AddRelativeClick()
                .AddElement(267, 190, "#222222")
                .AddElement(282, 183, "#DADADA")
                .AddElement(288, 199, "#FFFFFF");

            // Load file icon
            automation.AddRelativeClick()
                .AddElement(290, 514, "#828282")
                .AddElement(299, 521, "#676767")
                .AddElement(288, 526, "#D4D4D4");

            // Pick From Windows icon
            automation.AddRelativeClick()
                .AddElement(154, 220, "#F1511B")
                .AddElement(169, 219, "#80CC28")
                .AddElement(155, 231, "#00ADEF");

            automation.AddWait(500);
            automation.AddOpenImage();

            // Select all icon
            automation.AddRelativeClick()
                .AddElement(512, 515, "#4E4E4E")
                .AddElement(513, 524, "#070707")
                .AddElement(519, 521, "#2B2B2B");

            // Search icon
            automation.AddWaitRelativeImage()
                .AddElement(994, 112, "#4688F1")
                .AddElement(1005, 120, "#4789F1")
                .AddElement(997, 115, "#4688F1");

            // Click on text
            automation.AddRelativeClick(93, 69);

            automation.AddSelectAll();
            automation.AddWait(100);
            automation.AddSelectAll();
            automation.AddWait(100);
            automation.AddSelectAll();
            automation.AddWait(100);
            automation.AddCopy();
        }

        private void BuildImageAutomation()
        {            
            // Top left of BlueStacks
            automation.AddReferenceImage("BlueStacksReference.png");

            // Translate icon
            automation.AddStartImage("BlueStacksStart.png");

            // Translate icon
            automation.AddClickableImage("BlueStacksStart.png", 3);

            // Camera icon
            automation.AddClickableImage("Stage1_small.png");

            // Load file icon
            automation.AddClickableImage("Stage2_small.png");

            // Pick From Windows icon
            automation.AddClickableImage("Stage3_small.png");

            automation.AddWaitImage("OpenFile.png");
            automation.AddOpenImage();

            // Select all icon
            automation.AddClickableImage("Stage4_small.png")
                .SetVariance(50);

            // Search icon
            automation.AddWaitImage("Stage5_small.png");

            // Click on text
            automation.AddRelativeClick(93, 69);

            automation.AddSelectAll();
            automation.AddWait(100);
            automation.AddSelectAll();
            automation.AddWait(100);
            automation.AddCopy();
            automation.AddWait(100);
            automation.AddCopy();
            automation.AddCopy();
            automation.AddCopy();
        }

        public override OcrResult Load(OcrImage image, string language, string apiLanguage)
        {
            try
            {
                if (!SaveLoadableImage(image))
                {
                    return OcrResult.Create(OcrResultType.InvalidFilePath);
                }

                Program.ClipSync.Text = null;

                if (automation.Run())
                {
                    string text = WaitForClipSync();
                    if (!string.IsNullOrEmpty(text))
                    {
                        return new OcrResult(text);
                    }
                    return OcrResult.Create(OcrResultType.AutomationFailed);
                }
                else
                {
                    //System.Windows.Forms.MessageBox.Show("Failed at index: " + automation.Index);
                }

                return OcrResult.Create(OcrResultType.AutomationFailed);
            }
            finally
            {
                for (int i = 0; i < 10; i++)
                {
                    automation.SendKey(Keys.Escape);
                    System.Threading.Thread.Sleep(10);
                }
            }
        }        

        protected override void GetSupportedLanguages()
        {
            // Assume google vision can handle all google translate languages
            GoogleTranslator translate = new GoogleTranslator();
            foreach (KeyValuePair<string, string> language in translate.SupportedLanguages)
            {
                AddSupportedLanguage(language.Key, language.Value);
            }
        }        
    }
}