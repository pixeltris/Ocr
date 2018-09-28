using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Resolvers;
using System.Xml.Serialization;

namespace Ocr
{
    class OcrXml
    {
        public static OcrResult FromFile(string file)
        {
            return Parse(File.ReadAllText(file));
        }

        public static OcrResult Parse(string xml)
        {
            Dictionary<string, string> meta = new Dictionary<string, string>();
            List<Page> pages = new List<Page>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;            
            settings.XmlResolver = new XmlPreloadedResolver(XmlKnownDtds.Xhtml10);

            using (StringReader stringReader = new StringReader(xml))
            using (XmlReader reader = XmlReader.Create(stringReader, settings))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.Name)
                            {
                                case "meta":
                                    {
                                        string metaName = reader.GetAttribute("name");
                                        string metaContent = reader.GetAttribute("content");
                                        string metaValue = reader.GetAttribute("value");
                                        if (!string.IsNullOrEmpty(metaName))
                                        {
                                            meta[metaName] = metaContent == null ? metaValue : metaContent;
                                        }
                                    }
                                    break;
                                case "div":
                                case "span":
                                case "p":
                                    {
                                        switch (reader.GetAttribute("class"))
                                        {
                                            case "ocr_page":
                                                pages.Add(Parse<Page>(reader, null));
                                                break;
                                        }
                                    }                                    
                                    break;
                            }
                            break;
                        case XmlNodeType.EndElement:
                            break;
                    }
                }
            }

            OcrResult result = new OcrResult();
            foreach (Page page in pages)
            {
                foreach (Area area in page.Areas)
                {
                    OcrArea ocrArea = new OcrArea(area.Rect.ToRect());

                    List<Line> lines = new List<Line>();
                    lines.AddRange(area.Lines);
                    foreach(Paragraph paragraph in area.Paragraphs)
                    {
                        lines.AddRange(paragraph.Lines);
                    }

                    foreach(Line line in lines)
                    {
                        OcrLine ocrLine = new OcrLine();
                        ocrLine.Rect = line.Rect.ToRect();
                        foreach(Word word in line.Words)
                        {
                            OcrWord ocrWord = new OcrWord();
                            ocrWord.Rect = word.Rect.ToRect();
                            ocrWord.Confidence = word.Confidence;
                            ocrWord.Text = word.Text;
                            ocrLine.Words.Add(ocrWord);
                        }
                        ocrArea.Lines.Add(ocrLine);
                    }

                    if (!string.IsNullOrEmpty(ocrArea.Text.Trim()))
                    {
                        result.AddArea(ocrArea);
                    }                    
                }
            }

            return result;
        }

        private static T Parse<T>(XmlReader elementReader, Element parent) where T : Element, new()
        {
            T element = new T();
            BuildCommon(elementReader, element);
            BuildPage(elementReader, element as Page);
            BuildParagraph(elementReader, element as Paragraph);
            BuildArea(elementReader, element as Area);
            BuildLine(elementReader, element as Line);
            BuildWord(elementReader, element as Word);

            if (elementReader.NodeType == XmlNodeType.Element)
            {
                using (XmlReader reader = elementReader.ReadSubtree())
                {
                    reader.Read();
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                switch (reader.Name)
                                {
                                    case "div":
                                    case "span":
                                    case "p":
                                        {
                                            switch (reader.GetAttribute("class"))
                                            {
                                                case "ocr_carea":
                                                    {
                                                        AddArea(element, Parse<Area>(reader, parent));
                                                    }
                                                    break;
                                                case "ocr_par":
                                                    {
                                                        AddParagraph(element, Parse<Paragraph>(reader, parent));
                                                    }
                                                    break;
                                                case "ocr_line":
                                                    {
                                                        AddLine(element, Parse<Line>(reader, parent));
                                                    }
                                                    break;
                                                case "ocr_word":
                                                case "ocrx_word":
                                                    {
                                                        AddWord(element, Parse<Word>(reader, parent));
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                }
            }        
            return element;
        }

        private static void AddArea(Element element, Area area)
        {
            Page page = element as Page;
            if (page != null)
                page.Areas.Add(area);
        }

        private static void AddParagraph(Element element, Paragraph paragraph)
        {
            Page page = element as Page;
            Area area = element as Area;
            if(page != null)
                page.Paragraphs.Add(paragraph);
            if(area != null)
                area.Paragraphs.Add(paragraph);
        }

        private static void AddLine(Element element, Line line)
        {
            Page page = element as Page;
            Area area = element as Area;
            Paragraph paragraph = element as Paragraph;
            if (page != null)
                page.Lines.Add(line);
            if (area != null)
                area.Lines.Add(line);
            if (paragraph != null)
                paragraph.Lines.Add(line);
        }

        private static void AddWord(Element element, Word word)
        {
            Line line = element as Line;
            if (line != null)
                line.Words.Add(word);
        }

        private static void BuildCommon(XmlReader reader, Element element)
        {
            element.Id = reader.GetAttribute("id");
            element.Title = reader.GetAttribute("title");
        }

        private static void BuildPage(XmlReader reader, Page page)
        {
            if (page == null) return;            
            
            Rect rect;
            int pageNumber, confidence;
            PointF baseLine;
            ParseTitle(page.Title, out rect, out pageNumber, out confidence, out baseLine);
            page.Rect = rect;
            page.PageNumber = pageNumber;
        }

        private static void BuildArea(XmlReader reader, Area area)
        {
            if (area == null) return;

            Rect rect;
            int pageNumber, confidence;
            PointF baseLine;
            ParseTitle(area.Title, out rect, out pageNumber, out confidence, out baseLine);
            area.Rect = rect;            
        }

        private static void BuildParagraph(XmlReader reader, Paragraph paragraph)
        {
            if (paragraph == null) return;

            Rect rect;
            int pageNumber, confidence;
            PointF baseLine;
            ParseTitle(paragraph.Title, out rect, out pageNumber, out confidence, out baseLine);
            paragraph.Rect = rect;
        }

        private static void BuildLine(XmlReader reader, Line line)
        {
            if (line == null) return;

            Rect rect;
            int pageNumber, confidence;
            PointF baseLine;
            ParseTitle(line.Title, out rect, out pageNumber, out confidence, out baseLine);
            line.Rect = rect;
            line.BaseLine = baseLine;
        }

        private static void BuildWord(XmlReader reader, Word word)
        {
            if (word == null) return;

            Rect rect;
            int pageNumber, confidence;
            PointF baseLine;
            ParseTitle(word.Title, out rect, out pageNumber, out confidence, out baseLine);
            word.Rect = rect;
            word.Confidence = confidence;

            word.Language = reader.GetAttribute("lang");
            word.Direction = reader.GetAttribute("dir");
            StringBuilder text = new StringBuilder();

            if (reader.NodeType == XmlNodeType.Element)
            {
                using (XmlReader textReader = reader.ReadSubtree())
                {
                    while (textReader.Read())
                    {
                        if (textReader.NodeType == XmlNodeType.Text || textReader.NodeType == XmlNodeType.Whitespace)
                        {
                            text.Append(textReader.Value);
                        }
                    }
                }
            }

            word.Text = text.ToString();
        }

        private static void ParseTitle(string title, out Rect rect, out int pageNumber, out int confidence, out PointF baseLine)
        {
            rect = new Rect();
            pageNumber = 0;
            confidence = 0;
            baseLine = new PointF();
            string image;

            const string imageHeader = "image";
            const string bboxHeader = "bbox";
            const string baselineHeader = "baseline";
            const string cofidenceHeader = "x_wconf";
            const string pageNumHeader = "ppageno";

            string[] splitted = title.Split(';');
            for (int i = 0; i < splitted.Length; i++)
                splitted[i] = splitted[i].Trim();

            foreach (string str in splitted)
            {
                if (str.StartsWith(bboxHeader))
                {
                    string[] vals = str.Substring(bboxHeader.Length).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    rect.Left = int.Parse(vals[0]);
                    rect.Top = int.Parse(vals[1]);
                    rect.Right = int.Parse(vals[2]);
                    rect.Bottom = int.Parse(vals[3]);
                }
                else if (str.StartsWith(baselineHeader))
                {
                    string[] vals = str.Substring(baselineHeader.Length).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    baseLine.X = float.Parse(vals[0]);
                    baseLine.Y = float.Parse(vals[1]);
                }
                else if (str.StartsWith(cofidenceHeader))
                {
                    string[] vals = str.Substring(cofidenceHeader.Length).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    confidence = int.Parse(vals[0]);
                }
                else if (str.StartsWith(pageNumHeader))
                {
                    string[] vals = str.Substring(pageNumHeader.Length).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    pageNumber = int.Parse(vals[0]);
                }
                else if (str.StartsWith(imageHeader))
                {
                    image = str.Substring(imageHeader.Length + 1);
                }
            }
        }

        class Page : TextElement
        {
            public int PageNumber { get; set; }
            public List<Area> Areas { get; private set; }
            public List<Paragraph> Paragraphs { get; private set; }
            public List<Line> Lines { get; private set; }

            public Page()
            {
                Areas = new List<Area>();
                Paragraphs = new List<Paragraph>();
                Lines = new List<Line>();
            }
        }

        class Paragraph : TextElement
        {
            public string Direction { get; set; }
            public List<Line> Lines { get; private set; }

            public Paragraph()
            {
                Lines = new List<Line>();
            }
        }

        class Area : TextElement
        {
            public List<Paragraph> Paragraphs { get; private set; }
            public List<Line> Lines { get; private set; }

            public Area()
            {
                Paragraphs = new List<Paragraph>();
                Lines = new List<Line>();
            }
        }

        class Line : TextElement
        {
            public PointF BaseLine { get; set; }
            public string Direction { get; set; }
            public List<Word> Words { get; private set; }

            public Line()
            {
                Words = new List<Word>();
            }
        }

        class Word : TextElement
        {
            public string Direction { get; set; }
            public int Confidence { get; set; }
            public string Language { get; set; }
            public string Text { get; set; }
        }        

        class Rect : Element
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }

            public OcrRect ToRect()
            {
                return new OcrRect(Left, Top, Right - Left, Bottom - Top);
            }
        }

        class TextElement : Element
        {
            public Rect Rect { get; set; }
        }

        class Element
        {
            public string Id { get; set; }
            public string Title { get; set; }
        }
    }
}
