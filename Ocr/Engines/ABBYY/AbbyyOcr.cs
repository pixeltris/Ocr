using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ocr
{
    public class AbbyyOcr : OcrEngine
    {
        private static bool installed = ProcessExists("FineReader");

        public override string Name
        {
            get { return "ABBYY"; }
        }

        public override bool IsInstalled
        {
            get { return installed; }
        }

        public override OcrResult Load(OcrImage image, string language, string apiLanguage)
        {
            try
            {
                Automation automation = new Automation();
                automation.UseImageCaching = true;
                automation.Path = System.IO.Path.Combine("Images", "ABBYY");

                // Task icon
                automation.AddReferenceImage("AbbyyReference.png");

                // Task icon
                automation.AddStartImage("AbbyyStart.png");

                // Click on language text box
                automation.AddRelativeClick(450, 50);
                //automation.AddWait(50);

                // Select language text box text
                automation.AddSelectAll();
                automation.AddWait(50);

                automation.AddText(apiLanguage, true);
                automation.AddWait(50);
                automation.AddKey(Keys.Enter);
                automation.AddWait(50);
                automation.AddKey(Keys.Enter);

                // Click open image button
                automation.AddRelativeClick(140, 30);

                // Wait for the open image dialog
                automation.AddWaitImage("OpenImage.png");

                // Type the file name
                automation.AddOpenImage();

                // Wait for the complete dialog
                automation.AddWaitImage("Complete.png");

                // Press escape to close complete dialog
                automation.AddKey(Keys.Escape);

                // Seems to freeze if we don't do this
                for (int i = 0; i < 15; i++)
                {
                    // Click the ocr text box
                    automation.AddMoveMouseImageOffset("OutputOffset.png", 50 + ((i + 1) * 1), 0 + ((i + 1) * 1));
                    automation.AddWait(100);
                }

                automation.AddClickableImageOffset("OutputOffset.png", 50, 0);
                //automation.AddWait(50);

                automation.AddSelectAll();
                //automation.AddWait(50);

                automation.AddCopy();

                if (!SaveLoadableImage(image))
                {
                    return OcrResult.Create(OcrResultType.InvalidFilePath);
                }

                // Clear the clipboard
                Clipboard.Clear();

                if (automation.Run())
                {
                    string text = WaitForClipboard();
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
            }
            catch(Exception e)
            {
                //MessageBox.Show(e.ToString());
                return OcrResult.Create(OcrResultType.Exception, e.ToString());
            }

            return OcrResult.Create(OcrResultType.AutomationFailed);
        }

        protected override void GetSupportedLanguages()
        {
            // Commented out without anything on left languages are not supported by .net
            // Commented out with text on left are languages not supported by translators

            //AddSupportedLanguage("", "Abkhaz");
            //AddSupportedLanguage("", "Adyghe");
            AddSupportedLanguage("af", "Afrikaans");
            //AddSupportedLanguage("", "Agul");
            AddSupportedLanguage("sq", "Albanian");
            //AddSupportedLanguage("", "Altai");
            AddSupportedLanguage("ar", "Arabic (Saudi Arabia)");

            AddSupportedLanguage("hy", "Armenian");// <---- Let ABBYY pick which to use
            //AddSupportedLanguage("hy", "Armenian (Eastern)");
            //AddSupportedLanguage("hy", "Armenian (Grabar)");
            //AddSupportedLanguage("hy", "Armenian (Western)");
            
            //AddSupportedLanguage("", "Avar");
            //AddSupportedLanguage("", "Aymara");

            AddSupportedLanguage("az", "Azeri");// <---- Let ABBYY pick which to use
            //AddSupportedLanguage("az-Cyrl-AZ", "Azeri (Cyrillic)");
            //AddSupportedLanguage("az-Latn-AZ", "Azeri (Latin)");

            AddSupportedLanguage("ba", "Bashkir");
            AddSupportedLanguage("eu", "Basque");
            AddSupportedLanguage("be", "Belarusian");
            //AddSupportedLanguage("", "Bemba");
            //AddSupportedLanguage("", "Blackfoot");
            //AddSupportedLanguage("br", "Breton");
            //AddSupportedLanguage("", "Bugotu");
            AddSupportedLanguage("bg", "Bulgarian");
            //AddSupportedLanguage("", "Buryat");
            AddSupportedLanguage("ca", "Catalan");
            AddSupportedLanguage("ceb", "Cebuano");// <---- This language is not supported in .net?
            //AddSupportedLanguage("", "Chamorro");
            //AddSupportedLanguage("", "Chechen");

            AddSupportedLanguage("zh-CN", "Chinese Simplified");
            //AddSupportedLanguage("zh-CN", "Chinese Simplified and English");
            
            AddSupportedLanguage("zh-TW", "Chinese Traditional");
            //AddSupportedLanguage("zh-TW", "Chinese Traditional and English");
            
            //AddSupportedLanguage("", "Chukchee");
            //AddSupportedLanguage("", "Chuvash");
            AddSupportedLanguage("co", "Corsican");
            //AddSupportedLanguage("", "Crimean Tatar");
            AddSupportedLanguage("hr", "Croatian");
            //AddSupportedLanguage("", "Crow");
            AddSupportedLanguage("cs", "Czech");
            //AddSupportedLanguage("", "Dakota");
            AddSupportedLanguage("da", "Danish");
           // AddSupportedLanguage("", "Dargwa");
            //AddSupportedLanguage("", "Dungan");

            AddSupportedLanguage("nl", "Dutch");
            //AddSupportedLanguage("nl", "Dutch (Belgian)");

            AddSupportedLanguage("en", "English");
            //AddSupportedLanguage("", "Eskimo (Cyrillic)");
            //AddSupportedLanguage("", "Eskimo (Latin)");
            AddSupportedLanguage("et", "Estonian");
            //AddSupportedLanguage("", "Even");
            //AddSupportedLanguage("", "Evenki");
            //AddSupportedLanguage("fo", "Faroese");
            //AddSupportedLanguage("", "Fijan");
            AddSupportedLanguage("fi", "Finnish");
            AddSupportedLanguage("fr", "French");
            AddSupportedLanguage("fy", "Frisian");
            //AddSupportedLanguage("", "Friulian");
            //AddSupportedLanguage("", "Gagauz");
            AddSupportedLanguage("gl", "Galician");
            //AddSupportedLanguage("", "Ganda");
            
            AddSupportedLanguage("de", "German");
            //AddSupportedLanguage("lb", "German (Luxembourg)");
            //AddSupportedLanguage("", "German (New Spelling)");
            
            AddSupportedLanguage("el", "Greek");
            //AddSupportedLanguage("", "Guarani");
            //AddSupportedLanguage("", "Hani");
            AddSupportedLanguage("ha", "Hausa");
            AddSupportedLanguage("haw", "Hawaiian");// <---- This language is not supported in .net?
            AddSupportedLanguage("he", "Hebrew");// <---- "he" in .net?
            AddSupportedLanguage("hu", "Hungarian");
            AddSupportedLanguage("is", "Icelandic");
            AddSupportedLanguage("id", "Indonesian");
            //AddSupportedLanguage("", "Ingush");
            AddSupportedLanguage("ga", "Irish");
            AddSupportedLanguage("it", "Italian");

            AddSupportedLanguage("ja", "Japanese");
            //AddSupportedLanguage("ja", "Japanese and English");
            
            //AddSupportedLanguage("", "Jingpo");
            //AddSupportedLanguage("", "Kabardian");
            //AddSupportedLanguage("", "Kalmyk");
            //AddSupportedLanguage("", "Karachay-balkar");
            //AddSupportedLanguage("", "Karakalpak");
            //AddSupportedLanguage("", "Kashubian");
            //AddSupportedLanguage("", "Kawa");
            AddSupportedLanguage("kk", "Kazakh");
            //AddSupportedLanguage("", "Khakass");
            //AddSupportedLanguage("", "Khanty");
            //AddSupportedLanguage("", "Kikuyu");
            //AddSupportedLanguage("", "Kirghiz");
            //AddSupportedLanguage("", "Kongo");
            
            AddSupportedLanguage("ko", "Korean");
            //AddSupportedLanguage("ko", "Korean (Hangul)");
            //AddSupportedLanguage("ko", "Korean and English");
            
            //AddSupportedLanguage("", "Koryak");
            //AddSupportedLanguage("", "Kpelle");
            //AddSupportedLanguage("", "Kumyk");
            AddSupportedLanguage("ku", "Kurdish");// <---- This language is not supported in .net?
            //AddSupportedLanguage("", "Lak");
            AddSupportedLanguage("la", "Latin");// <---- There are many different codes for latin in .net
            AddSupportedLanguage("lv", "Latvian");
            //AddSupportedLanguage("", "Lezgi");
            AddSupportedLanguage("lt", "Lithuanian");
            //AddSupportedLanguage("", "Luba");
            AddSupportedLanguage("mk", "Macedonian");
            AddSupportedLanguage("mg", "Malagasy");// <---- This language is not supported in .net?
            AddSupportedLanguage("ms", "Malay");// Malay (Malaysian)
            //AddSupportedLanguage("", "Malinke");
            AddSupportedLanguage("mt", "Maltese");
            //AddSupportedLanguage("", "Mansi");
            AddSupportedLanguage("mi", "Maori");
            //AddSupportedLanguage("", "Mari");
            //AddSupportedLanguage("", "Maya");
            //AddSupportedLanguage("", "Miao");
            //AddSupportedLanguage("", "Minangkabau");
            //AddSupportedLanguage("moh", "Mohawk");
            //AddSupportedLanguage("", "Moldavian");
            AddSupportedLanguage("mn", "Mongol");
            //AddSupportedLanguage("", "Mordvin");
            //AddSupportedLanguage("", "Nahuatl");
            //AddSupportedLanguage("", "Nenets");
            //AddSupportedLanguage("", "Nivkh");
            //AddSupportedLanguage("", "Nogay");

            AddSupportedLanguage("no", "Norwegian");// <---- .net has "nb" or "nn", it doesn't have "no"
            //AddSupportedLanguage("nb", "Norwegian (Bokmal)");
            //AddSupportedLanguage("nn", "Norwegian (Nynorsk)");

            AddSupportedLanguage("ny", "Nyanja");// <---- This language is not supported in .net?
            //AddSupportedLanguage("oc", "Occitan");
            //AddSupportedLanguage("", "Ojibway");
            //AddSupportedLanguage("", "Ossetian");
            //AddSupportedLanguage("", "Papiamento");
            AddSupportedLanguage("pl", "Polish");
            
            AddSupportedLanguage("pt", "Portuguese");
            //AddSupportedLanguage("pt", "Portuguese (Brazilian)");
            
            //AddSupportedLanguage("", "Quechua (Bolivia)");
            //AddSupportedLanguage("", "Rhaeto-Romanic");
            AddSupportedLanguage("ro", "Romanian");
            //AddSupportedLanguage("", "Romany");
            //AddSupportedLanguage("", "Rundi");
            
            AddSupportedLanguage("ru", "Russian");
            //AddSupportedLanguage("ru", "Russian (Old Spelling)");
            //AddSupportedLanguage("ru", "Russian with accent");
            
            //AddSupportedLanguage("", "Rwanda");
            //AddSupportedLanguage("", "Sami (Lappish)");
            AddSupportedLanguage("sm", "Samoan");// <---- This language is not supported in .net?
            AddSupportedLanguage("gd", "Scottish Gaelic");
            //AddSupportedLanguage("", "Selkup");
            AddSupportedLanguage("sr-Cyrl", "Serbian (Cyrillic)");//Serbian (Cyrillic) <---- Maybe just "sr" or one of the "sr-Cyrl-XX" variants?
            AddSupportedLanguage("sr-Latn", "Serbian (Latin)");//Serbian (Latin) <---- "sr-Latn-XX" variants?
            AddSupportedLanguage("sn", "Shona");// <---- This language is not supported in .net?
            AddSupportedLanguage("sk", "Slovak");
            AddSupportedLanguage("sl", "Slovenian");
            AddSupportedLanguage("so", "Somali");// <---- This language is not supported in .net?
            //AddSupportedLanguage("", "Sorbian");
            //AddSupportedLanguage("", "Sotho");
            AddSupportedLanguage("es", "Spanish");
            AddSupportedLanguage("su", "Sunda");// <---- This language is not supported in .net?
            AddSupportedLanguage("sw", "Swahili");
            //AddSupportedLanguage("", "Swazi");
            AddSupportedLanguage("sv", "Swedish");
            //AddSupportedLanguage("", "Tabasaran");
            //AddSupportedLanguage("", "Tagalog");
            //AddSupportedLanguage("", "Tahitian");
            AddSupportedLanguage("tg", "Tajik");
            AddSupportedLanguage("tt", "Tatar");
            AddSupportedLanguage("th", "Thai");
            //AddSupportedLanguage("", "Tok Pisin");
            //AddSupportedLanguage("", "Tongan");
            //AddSupportedLanguage("", "Tswana");
            //AddSupportedLanguage("", "Tun");
            AddSupportedLanguage("tr", "Turkish");
            //AddSupportedLanguage("tk", "Turkmen (Cyrillic)");
            //AddSupportedLanguage("tk", "Turkmen (Latin)");
            //AddSupportedLanguage("", "Tuvinian");
            //AddSupportedLanguage("", "Udmurt");
            //AddSupportedLanguage("", "Uighur (Cyrillic)");
            //AddSupportedLanguage("", "Uighur (Latin)");
            AddSupportedLanguage("uk", "Ukrainian");

            AddSupportedLanguage("uz", "Uzbek");// <---- Let ABBYY pick which to use
            //AddSupportedLanguage("uz", "Uzbek (Cyrillic)");
            //AddSupportedLanguage("uz", "Uzbek (Latin)");
            
            AddSupportedLanguage("vi", "Vietnamese");
            AddSupportedLanguage("cy", "Welsh");
            //AddSupportedLanguage("wo", "Wolof");
            AddSupportedLanguage("xh", "Xhosa");
            //AddSupportedLanguage("sah", "Yakut");
            AddSupportedLanguage("yi", "Yiddish");// <---- This language is not supported in .net?
            //AddSupportedLanguage("", "Zapotec");
            AddSupportedLanguage("zu", "Zulu");
        }        
    }
}
