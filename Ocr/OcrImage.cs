using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocr
{
    public class OcrImage
    {        
        public byte[] Data { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        //public string Path { get; set; }

        public static OcrImage FromBinary(byte[] buffer)
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
                {
                    OcrImage image = new OcrImage();

                    uint dataLen = reader.ReadUInt32();
                    image.Data = new byte[dataLen];
                    if (dataLen > 0)
                    {
                        image.Data = reader.ReadBytes((int)dataLen);
                    }
                    image.Width = reader.ReadInt32();
                    image.Height = reader.ReadInt32();
                    //image.Path = reader.ReadString();

                    return image;
                }
            }
            catch
            {
                return null;
            }
        }

        public byte[] ToBinary()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                if(Data != null && Data.Length > 0)
                {
                    writer.Write((uint)Data.Length);
                    writer.Write(Data);
                }
                else
                {
                    writer.Write((uint)0);
                }
                writer.Write(Width);
                writer.Write(Height);
                //writer.Write(Path == null ? string.Empty : Path);

                return ms.ToArray();
            }
        }
    }
}
