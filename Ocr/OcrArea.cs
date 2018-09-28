using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocr
{
    public class OcrArea
    {
        public OcrRect Rect { get; set; }
        //public List<OcrArea> Areas { get; private set; }
        public List<OcrLine> Lines { get; private set; }

        public string Text
        {
            get
            {
                StringBuilder text = new StringBuilder();
                /*foreach (OcrArea area in Areas)
                {
                    text.Append(area.Text);
                    if (area != Areas.Last())
                        text.Append(" ");
                }*/
                foreach (OcrLine line in Lines)
                {
                    text.Append(line.Text);
                    if (line != Lines.Last())
                        text.Append(" ");
                }
                return text.ToString();
            }
        }

        /*public ReadOnlyCollection<OcrLine> LinesHierarchy
        {
            get
            {
                List<OcrLine> lines = new List<OcrLine>();
                foreach (OcrArea area in Areas)
                {
                    lines.AddRange(area.LinesHierarchy);
                }
                lines.AddRange(Lines);
                return lines.AsReadOnly();
            }
        }*/

        public OcrArea()
        {
            //Areas = new List<OcrArea>();
            Lines = new List<OcrLine>();
        }

        public OcrArea(OcrRect rect) : this()
        {
            Rect = rect;
        }
    }
}