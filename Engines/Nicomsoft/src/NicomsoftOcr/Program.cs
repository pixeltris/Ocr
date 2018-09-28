using NSOCR_NameSpace;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicomsoftOcr
{
    class Program
    {
        private static int CfgObj;
        private static int OcrObj = 0;
        private static int ImgObj = 0;
        private static Rectangle Frame;

        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                    return;

                string language = args[0];
                string file = args[1];
                string outputFile = args.Length < 3 ? "out.txt" : args[2];

                CreateWindowHook windowHook = null;
                try
                {
                    windowHook = new CreateWindowHook();
                }
                catch//(Exception e)
                {
                    //File.AppendAllText("error.txt", e.ToString());
                }

                Initialize();
                SetLanguage(language);
                string result = RunOcr(file);
                File.WriteAllText(outputFile, result);
            }
            catch
            {
            }
            finally
            {
                try
                {
                    Close();
                }
                catch { }
            }
        }

        private static void Initialize()
        {
            //get NSOCR version
            StringBuilder val;
            val = new StringBuilder(256);
            TNSOCR.Engine_GetVersion(val); //if you get "BadImageFormatException" error here: find and check "LIBNAME" constant in "NSOCR.cs"
            //Text = Text + " [ NSOCR version: " + val + " ] ";

            TNSOCR.Engine_SetLicenseKey(""); //required for licensed version only

            //init engine and create ocr-related objects
            TNSOCR.Engine_Initialize();
            TNSOCR.Cfg_Create(out CfgObj);
            TNSOCR.Ocr_Create(CfgObj, out OcrObj);
            TNSOCR.Img_Create(OcrObj, out ImgObj);

            TNSOCR.Cfg_LoadOptions(CfgObj, "Config.dat"); //load options, if path not specified, current folder and folder with NSOCR.dll will be checked

            //Console.WriteLine("Version:{0} CfgObj:{1} OcrObj:{2} ImgObj:{3}", val, CfgObj, OcrObj, ImgObj);

            // These have no use
            //SetAdditionalSettings();
        }

        private static void Close()
        {
            if (ImgObj != 0) TNSOCR.Img_Destroy(ImgObj);
            if (OcrObj != 0) TNSOCR.Ocr_Destroy(OcrObj);
            if (CfgObj != 0) TNSOCR.Cfg_Destroy(CfgObj);
            TNSOCR.Engine_Uninitialize();
        }

        private static void SetAdditionalSettings()
        {
            //copy some settings (this is image scale, we handle this externally)
            /*StringBuilder val = new StringBuilder(256);
            if (TNSOCR.Cfg_GetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoScale", val, 256) < TNSOCR.ERROR_FIRST)
                cbScale.Enabled = (val.ToString() == "1");*/

            //default this option is disabled because it takes about 10% of total recognition time
            //enable it to demonstrate this feature
            TNSOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "Zoning/FindBarcodes", "1");
            //also enable auto-detection of image inversion
            TNSOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/Inversion", "2");
        }        

        private static bool LoadImage(string fileName)
        {
            int res = TNSOCR.Img_LoadFile(ImgObj, fileName);
            
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_LoadFile", res);
                return false;
            }

            int w0, h0, w, h;
            //native size
            TNSOCR.Img_GetSize(ImgObj, out w0, out h0);

            
            //now apply image scaling, binarize image, deskew etc,
            //everything except zoning and OCR
            res = TNSOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_FIRST, TNSOCR.OCRSTEP_ZONING - 1, 0);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_OCR", res);
                return false;
            }

            //final size after pre-processing
            TNSOCR.Img_GetSize(ImgObj, out w, out h);

            /*res = TNSOCR.Img_GetSkewAngle(ImgObj);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_GetSkewAngle", res);
                //lbSkew.Text = "";
            }
            else
            {
                //lbSkew.Text = "Skew angle (degrees): " + (res / 1000.0);
            }

            //pixel lines info
            res = TNSOCR.Img_GetPixLineCnt(ImgObj);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_GetPixLineCnt", res);
                return false;
            }
            //lbLines.Text = "Lines: " + res.ToString();

            //edPage.Text = "1";
            //lbPages.Text = "of " + TNSOCR.Img_GetPageCount(ImgObj).ToString();
            */

            Frame = new Rectangle(0, 0, w, h);

            return true;
        }

        private static void ShowError(string error, int val)
        {
            //Console.WriteLine("Error " + error + " " + val);
        }

        private static string RunOcr(string fileName)
        {
            LoadImage(fileName);
            return RunOcr();
        }

        private static string RunOcr()
        {
            int res;

            TNSOCR.Img_DeleteAllBlocks(ImgObj);
            
            if (Frame.Width > 0)
            {
                Rectangle r = Frame;
                int w, h;
                TNSOCR.Img_GetSize(ImgObj, out w, out h);

                if (r.X < 0) r.X = 0;
                if (r.Y < 0) r.Y = 0;
                if (r.Width > w) r.Width = w;
                if (r.Height > h) r.Height = h;

                int BlkObj;
                res = TNSOCR.Img_AddBlock(ImgObj, r.Left, r.Top, r.Width, r.Height, out BlkObj);
                if (res > TNSOCR.ERROR_FIRST)
                {
                    ShowError("Img_AddBlock", res);
                    return null;
                }

                //detect text block inversion
                TNSOCR.Blk_Inversion(BlkObj, TNSOCR.BLK_INVERSE_DETECT);

                Frame.Width = 0;
            }

            //zoning will not be executed since we have created text block
            res = TNSOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_ZONING, TNSOCR.OCRSTEP_LAST, 0);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_OCR", res);
                return null;
            }

            int flags = TNSOCR.FMT_EDITCOPY; //or TNSOCR.FMT_EXACTCOPY
            StringBuilder txt = new StringBuilder(0);
            int n = TNSOCR.Img_GetImgText(ImgObj, txt, 0, flags); //get buffer text length plus null-terminated zero
            txt = new StringBuilder(n + 1);
            TNSOCR.Img_GetImgText(ImgObj, txt, n + 1, flags);
            return txt.ToString();
        }

        private static void SetLanguage(string language)
        {
            for (int i = 0; i < languages.Length; i++)
            {
                TNSOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "Languages/" + languages[i], language == languages[i] ? "1" : "0");
                //Console.WriteLine("Set " + languages[i] + " " + (language == languages[i] ? "1" : "0") + " " + res);
            }

            /*StringBuilder val;
            for (int i = 0; i < languages.Length; i++)
            {
                val = new StringBuilder(256);
                if (TNSOCR.Cfg_GetOption(CfgObj, TNSOCR.BT_DEFAULT, "Languages/" + languages[i], val, 256) < TNSOCR.ERROR_FIRST)
                {
                    Console.WriteLine("Lang " + languages[i] + " " + val);
                }
            }*/
        }

        static string[] languages =
        {
            // Asian languages
            "Arabic",
            "Chinese_Simplified",
            "Chinese_Traditional",
            "Japanese",
            "Korean",

            // All other languages
            "Bulgarian",
            "Catalan",
            "Croatian",
            "Czech",
            "Danish",
            "Dutch",
            "English",
            "Estonian",
            "Finnish",
            "French",
            "German",
            "Hungarian",
            "Indonesian",
            "Italian",
            "Latvian",
            "Lithuanian",
            "Norwegian",
            "Polish",
            "Portuguese",
            "Romanian",
            "Russian",
            "Slovak",
            "Slovenian",
            "Spanish",
            "Swedish",
            "Turkish"
        };        
    }
}
