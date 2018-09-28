using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocr
{
    // Some additional translation tools which might be useful (also might help with tokens in Google / Baidu)
    // https://github.com/hujingshuang/MTrans
    // https://github.com/xwjdsh/fy

    public enum TranslatorType
    {
        Google,
        Bing,
        Yandex,
        Prompt,
        Babylon,
        SDL,
        Baidu
    }
}
