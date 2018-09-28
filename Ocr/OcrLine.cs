using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocr
{
    public class OcrLine
    {
        public OcrRect Rect { get; set; }
        public List<OcrWord> Words { get; private set; }

        public string Text
        {
            get
            {
                StringBuilder text = new StringBuilder();
                foreach (OcrWord word in Words)
                {
                    text.Append(word.Text);
                    if (word != Words.Last())
                        text.Append(" ");
                }
                return text.ToString();
            }
        }

        public OcrLine()
        {
            Words = new List<OcrWord>();
        }
    }
}
