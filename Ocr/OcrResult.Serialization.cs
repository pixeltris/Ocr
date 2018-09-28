using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocr
{
    partial class OcrResult
    {
        public static OcrResult FromBinary(byte[] buffer)
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
                {
                    OcrResult result = new OcrResult();

                    result.Rect = ReadRect(reader);
                    result.ResultType = (OcrResultType)reader.ReadByte();
                    result.Error = reader.ReadString();
                    if (string.IsNullOrEmpty(result.Error))
                        result.Error = null;

                    int areaCount = reader.ReadInt32();
                    for (int i = 0; i < areaCount; i++)
                    {
                        result.AddArea(ReadArea(reader));
                    }

					int lineCount = reader.ReadInt32();
                    for (int i = 0; i < lineCount; i++)
                    {
                        result.AddLine(ReadLine(reader));
                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                return OcrResult.Create(OcrResultType.Exception, e.ToString());
            }
        }

        public byte[] ToBinary()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                WriteRect(writer, Rect);
                writer.Write((byte)ResultType);
                writer.Write(Error == null ? string.Empty : null);

                writer.Write(Areas.Count);
                foreach (OcrArea area in Areas)
                {
                    WriteArea(writer, area);
                }

                writer.Write(Lines.Count);
                foreach (OcrLine line in Lines)
                {
                    WriteLine(writer, line);
                }

                return ms.ToArray();
            }
        }

        private static void WriteArea(BinaryWriter writer, OcrArea area)
        {
            WriteRect(writer, area.Rect);
            writer.Write(area.Lines.Count);
            foreach (OcrLine line in area.Lines)
            {
                WriteLine(writer, line);
            }
        }

        private static void WriteLine(BinaryWriter writer, OcrLine line)
        {
            WriteRect(writer, line.Rect);
            writer.Write(line.Words.Count);
            foreach (OcrWord word in line.Words)
            {
                WriteWord(writer, word);
            }
        }

        private static void WriteWord(BinaryWriter writer, OcrWord word)
        {
            WriteRect(writer, word.Rect);
			writer.Write(word.Confidence);
            writer.Write(word.Text == null ? string.Empty : word.Text);
        }

        private static void WriteRect(BinaryWriter writer, OcrRect rect)
        {
            writer.Write(rect.X);
            writer.Write(rect.Y);
            writer.Write(rect.Width);
            writer.Write(rect.Height);
        }

        private static OcrRect ReadRect(BinaryReader reader)
        {
            OcrRect rect = new OcrRect();
            rect.X = reader.ReadInt32();
            rect.Y = reader.ReadInt32();
            rect.Width = reader.ReadInt32();
            rect.Height = reader.ReadInt32();
            return rect;
        }

        private static OcrArea ReadArea(BinaryReader reader)
        {
            OcrArea area = new OcrArea();
            area.Rect = ReadRect(reader);
            int lineCount = reader.ReadInt32();
            for (int i = 0; i < lineCount; i++)
            {
                area.Lines.Add(ReadLine(reader));
            }
            return area;
        }

        private static OcrLine ReadLine(BinaryReader reader)
        {
            OcrLine line = new OcrLine();
            line.Rect = ReadRect(reader);
            int wordCount = reader.ReadInt32();
            for (int i = 0; i < wordCount; i++)
            {
                line.Words.Add(ReadWord(reader));
            }
            return line;
        }

        private static OcrWord ReadWord(BinaryReader reader)
        {
            OcrWord word = new OcrWord();
            word.Rect = ReadRect(reader);
            word.Confidence = reader.ReadInt32();
            word.Text = reader.ReadString();
            return word;
        }		
    }
}
