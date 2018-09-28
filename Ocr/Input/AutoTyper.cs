using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ocr.Input
{
    //https://en.wikipedia.org/wiki/OpenVanilla
    //https://openvanilla.org/
    //https://github.com/lukhnos/openvanilla/tree/master/DataTables
    //https://github.com/pkg-ime/openvanilla-modules/tree/master/DataTables
    //http://www.opensource.apple.com/source/emacs/emacs-94/emacs/leim/quail/
    //http://www.opensource.apple.com/source/emacs/emacs-94/emacs/leim/quail/hangul.el?txt
    //http://stackoverflow.com/questions/16371565/convert-language-characters-to-latin-alphabet    
    //"Transliteration API"

    //Also might be useful
    //http://memory.loc.gov/diglib/codetables/9.1.html - character set site
    //http://memory.loc.gov/diglib/codetables/9.2.html
    //http://stackoverflow.com/questions/10089798/hiragana-to-kanji-converter
    //http://colspan.net/japaneseime/

    //https://github.com/JLChnToZ/IMEHelper/blob/master/IMEHelper/IMMNativeMethods.cs

    //http://stackoverflow.com/questions/18329040/google-input-tools-api-can-it-be-used

    //kanji / Kana
    //katakana / hiragana

    //Input
    //http://stackoverflow.com/questions/22291282/using-sendinput-to-send-unicode-characters-beyond-uffff

    class AutoTyper
    {
        public static void SendInput(string text)
        {
            SendInput(text, null);
        }

        public static void SendInput(string text, string language)
        {
            int delay = Program.Settings.AutoTyper.KeyDelay;
            int cancelOnMouseMove = Program.Settings.AutoTyper.CancelOnMouseMove;
            bool requireKeyboard = Program.Settings.AutoTyper.RequireKeyboard;
            IntPtr focusedWindow = PInvoke.GetForegroundWindow();
            Point cursorPos = PInvoke.GetCursorPosition();

            if (requireKeyboard)
            {
                // TODO: Change keyboard
            }

            foreach (char character in text)
            {
                PInvoke.SendCharUnicode(character);

                if (delay > 0)
                {
                    Thread.Sleep(delay);
                }

                if (cancelOnMouseMove > 0)
                {
                    if (Distance(cursorPos, PInvoke.GetCursorPosition()) > cancelOnMouseMove ||
                        focusedWindow != PInvoke.GetForegroundWindow())
                    {
                        break;
                    }
                }
            }

            if (requireKeyboard)
            {
                // TODO: Change keyboard back
            }
        }

        private static int Distance(Point p1, Point p2)
        {
            double a = (double)(p2.X - p1.X);
            double b = (double)(p2.Y - p1.Y);
            return (int)Math.Sqrt(a * a + b * b);
        }

        private static void Test()
        {
            string language = null;
            bool requireKeyboard = Program.Settings.AutoTyper.RequireKeyboard;            

            IntPtr currentKeyboardLayout = IntPtr.Zero;

            if (requireKeyboard)
            {
                currentKeyboardLayout = GetCurrentKeyboardLayout();
                CultureInfo currentCulture = new CultureInfo((int)currentKeyboardLayout & 65535);

                InputLanguage currentInputLanguage = InputLanguage.CurrentInputLanguage;
                if (requireKeyboard)
                {
                    if (Program.Settings.AutoTyper.RequireKeyboard)
                    {
                        foreach (InputLanguage inputLanguage in InputLanguage.InstalledInputLanguages)
                        {
                            if (inputLanguage.Culture.Name == language)
                            {

                                break;
                            }
                        }
                    }
                }
            }
        }

        private static IntPtr GetCurrentKeyboardLayout()
        {
            uint processId;
            IntPtr foregroundWindow = PInvoke.GetForegroundWindow();
            uint threadId = PInvoke.GetWindowThreadProcessId(foregroundWindow, out processId);
            IntPtr layout = PInvoke.GetKeyboardLayout(threadId);            
            return layout;
        }
    }
}
