using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocr
{
    public partial class OcrResult
    {
        public OcrRect Rect { get; set; }
        public List<OcrArea> Areas { get; private set; }
        public List<OcrLine> Lines { get; private set; }

        public OcrResultType ResultType { get; set; }
        public string Error { get; set; }
        public bool Succes
        {
            get { return ResultType == OcrResultType.Success; }
        }

        public string Text
        {
            get
            {
                StringBuilder text = new StringBuilder();
                foreach (OcrArea area in Areas)
                {
                    foreach (OcrLine line in area.Lines)
                    {
                        text.Append(line.Text);
                        if (line != area.Lines.Last())
                            text.AppendLine();//text.Append("\n");                        
                    }
                    text.AppendLine();//text.Append("\n");
                    //text.AppendLine();
                }
                foreach (OcrLine line in Lines)
                {
                    text.Append(line.Text);
                    if (line != Lines.Last())
                        text.AppendLine();//text.Append("\n");
                }
                return text.ToString();
            }
        }

        public static OcrResult Create(OcrResultType resultType, string error = null)
        {
            return new OcrResult()
            {
                ResultType = resultType,
                Error = error
            };
        }

        public OcrResult(string text)
        {
            Areas = new List<OcrArea>();
            Lines = new List<OcrLine>();
            ResultType = OcrResultType.Success;
            if (!string.IsNullOrEmpty(text))
            {
                string[] splitted = text.Replace("\r", "").Split('\n');
                foreach (string line in splitted)
                {
                    AddLine(line);
                }
            }
        }

        public OcrResult()
            : this(null)
        {
        }

        public OcrArea AddArea(OcrRect rect)
        {
            return AddArea(new OcrArea(rect));
        }

        public OcrArea AddArea(OcrArea area)
        {
            if (!Areas.Contains(area))
            {
                Areas.Add(area);
            }
            return area;
        }

        public OcrLine AddLine(OcrLine line)
        {
            if (!Lines.Contains(line))
            {
                Lines.Add(line);
            }
            return line;
        }

        public OcrLine AddLine(string line)
        {
            if (line == null)
                line = string.Empty;

            string[] words = line.Split();

            OcrLine ocrLine = new OcrLine();
            foreach (string word in words)
            {
                OcrWord ocrWord = new OcrWord();
                ocrWord.Text = word;
                ocrLine.Words.Add(ocrWord);
            }            

            Lines.Add(ocrLine);
            return ocrLine;
        }        
    }

    public enum OcrResultType
    {
        Success,
        InvalidFilePath,
        InvalidFile,
        InvalidImageSize,
        LanguageNotSupported,        
        NotImplemented,
        NotInstalled,
        AutomationFailed,
        Failed,
        Exception,
    }
}