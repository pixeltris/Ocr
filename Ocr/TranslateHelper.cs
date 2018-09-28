using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ocr
{
    class TranslateHelper
    {
        public static event TranslateStartHandler Start;
        public static event TranslateCompleteHandler Complete;

        private static object locker = new object();
        private static Thread thread = null;

        private static int idleWait = 50;

        private static string currentText;
        private static string nextText;
        private static DateTime lastTranslate;

        static TranslateHelper()
        {
            thread = new Thread(delegate()
                {
                    string text = null;
                    string result = null;
                    TimeSpan delay;

                    while (Program.Running)
                    {
                        text = null;
                        result = null;
                        delay = TimeSpan.FromSeconds(Program.Settings.Translate.AutoTranslateDelay);

                        if (lastTranslate < DateTime.Now - delay)
                        {
                            lock (locker)
                            {
                                text = currentText;
                            }

                            if (text != null)
                            {
                                try
                                {
                                    if (Program.ActiveLanguageFrom != Program.ActiveLanguageTo)
                                    {
                                        lastTranslate = DateTime.Now;

                                        if (Start != null)
                                        {
                                            Start();
                                        }
                                        result = Program.ActiveTranslator.Translate(text, Program.ActiveLanguageFrom, Program.ActiveLanguageTo);
                                    }
                                }
                                catch
                                {
                                }
                            }

                            if (text != null && Complete != null)
                            {
                                Complete(result);
                            }

                            lock (locker)
                            {
                                if (text != null)
                                {
                                    currentText = nextText;
                                    nextText = null;
                                }
                            }
                        }

                        Thread.Sleep(idleWait);
                    }
                });
            thread.Start();
        }

        public static void Translate(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            lock (locker)
            {
                if (currentText == null)
                {
                    currentText = text;
                }
                else
                {
                    nextText = text;
                }
            }
        }
    }

    public delegate void TranslateCompleteHandler(string value);
    public delegate void TranslateStartHandler();
}
