//A C# wrapper class for NSOCR.dll functions (calling NSOCR API via platform invoke)

//******************************************************************************
//                        Nicomsoft OCR DLL interface
//                    Copyright (c) 2010-2016 Nicomsoft
//                 Copyright (c) 2010-2016 Michael Kapustin
//                           www.nsocr.com
//******************************************************************************



using System;
using System.Text;
using System.Runtime.InteropServices;


namespace NSOCR_NameSpace
{
    public class TNSOCR
    {
        //// Error codes
        public const int ERROR_FIRST					= 0x70000000;
        public const int ERROR_FILENOTFOUND 			= 0x70000001;
        public const int ERROR_LOADFILE 				= 0x70000002;
        public const int ERROR_SAVEFILE 				= 0x70000003;
        public const int ERROR_MISSEDIMGLOADER 		    = 0x70000004;
        public const int ERROR_OPTIONNOTFOUND		    = 0x70000005;
        public const int ERROR_NOBLOCKS				    = 0x70000006;
        public const int ERROR_BLOCKNOTFOUND			= 0x70000007;
        public const int ERROR_INVALIDINDEX			    = 0x70000008;
        public const int ERROR_INVALIDPARAMETER		    = 0x70000009;
        public const int ERROR_FAILED				    = 0x7000000A;
        public const int ERROR_INVALIDBLOCKTYPE		    = 0x7000000B;
        public const int ERROR_EMPTYTEXT				= 0x7000000D;
        public const int ERROR_LOADINGDICTIONARY		= 0x7000000E;
        public const int ERROR_LOADCHARBASE			    = 0x7000000F;
        public const int ERROR_NOMEMORY				    = 0x70000010;
        public const int ERROR_CANNOTLOADGS			    = 0x70000011;
        public const int ERROR_CANNOTPROCESSPDF		    = 0x70000012;
        public const int ERROR_NOIMAGE				    = 0x70000013;
        public const int ERROR_MISSEDSTEP		    	= 0x70000014;
        public const int ERROR_OUTOFIMAGE               = 0x70000015;
        public const int ERROR_EXCEPTION                = 0x70000016;
        public const int ERROR_NOTALLOWED               = 0x70000017;
        public const int ERROR_NODEFAULTDEVICE          = 0x70000018;
        public const int ERROR_NOTAPPLICABLE			= 0x70000019;
        public const int ERROR_MISSEDBARCODEDLL         = 0x7000001A;
        public const int ERROR_PENDING                  = 0x7000001B;
        public const int ERROR_OPERATIONCANCELLED       = 0x7000001C;
        public const int ERROR_TOOMANYLANGUAGES         = 0x7000001D;
        public const int ERROR_OPERATIONTIMEOUT         = 0x7000001E;
        public const int ERROR_LOAD_ASIAN_MODULE	= 0x7000001F;	
        public const int ERROR_LOAD_ASIAN_LANG		= 0x70000020;

        public const int ERROR_INVALIDOBJECT			= 0x70010000;
        public const int ERROR_TOOMANYOBJECTS	    	= 0x70010001;
        public const int ERROR_DLLNOTLOADED		    	= 0x70010002;
        public const int ERROR_DEMO                     = 0x70010003;

        ////Block type
        public const int BT_DEFAULT		    = 0;
        public const int BT_OCRTEXT		    = 1; //machine-printed text
        public const int BT_ICRDIGIT        = 2; //handwritten digits 
        public const int BT_CLEAR           = 3;
        public const int BT_PICTURE         = 4;
        public const int BT_ZONING          = 5;
        public const int BT_OCRDIGIT = 6; //machine-printed digits
        public const int BT_BARCODE = 7;
        public const int BT_TABLE = 8;
        public const int BT_MRZ = 9;

        ////Constants for Img_LoadBmpData function
        public const int BMP_24BIT          = 0x00;
        public const int BMP_8BIT         = 0x01;
        public const int BMP_1BIT           = 0x02;
        public const int BMP_32BIT           = 0x03;
        public const int BMP_BOTTOMTOP      = 0x100;

        ////Constants for Img_GetImgText, Blk_GetText and Svr_AddPage functions
        public const int FMT_EDITCOPY = 0x00;
        public const int FMT_EXACTCOPY	= 0x01;

        //for Img_OCR
        public const int OCRSTEP_FIRST			= 0x00; //first step
        public const int OCRSTEP_PREFILTERS		= 0x10; //filters before binarizing: scaling, invert, rotate etc
        public const int OCRSTEP_BINARIZE		= 0x20; //binarize
        public const int OCRSTEP_POSTFILTERS	= 0x50; //bin garbage filter etc
        public const int OCRSTEP_REMOVELINES    = 0x60; //find and remove lines
        public const int OCRSTEP_ZONING         = 0x70; //analyze page layout
        public const int OCRSTEP_OCR			= 0x80; //ocr itself
        public const int OCRSTEP_LAST			= 0xFF; //last step

        //for Img_OCR, "Flags" parameter
        public const int OCRFLAG_NONE			= 0x00; //default mode (blocking)
        public const int OCRFLAG_THREAD			= 0x01; //non-blocking mode
        public const int OCRFLAG_GETRESULT		= 0x02; //get result for non-blocking mode
        public const int OCRFLAG_GETPROGRESS	= 0x03; //get progress
        public const int OCRFLAG_CANCEL         = 0x04; //cancel ocr

        //for Img_DrawToDC and Img_GetBmpData
        public const int DRAW_NORMAL			= 0x00;
        public const int DRAW_BINARY			= 0x01;
        public const int DRAW_GETBPP            = 0x100;

        //values for Blk_Inversion function
        public const int BLK_INVERSE_GET        = -1;
        public const int BLK_INVERSE_SET0       = 0;
        public const int BLK_INVERSE_SET1       = 1;
        public const int BLK_INVERSE_DETECT     = 0x100;

        //for Blk_Rotation function
        public const int BLK_ROTATE_GET         = -1;
        public const int BLK_ROTATE_NONE        = 0x00;
        public const int BLK_ROTATE_90			= 0x01;
        public const int BLK_ROTATE_180			= 0x02;
        public const int BLK_ROTATE_270			= 0x03;
        public const int BLK_ROTATE_ANGLE       = 0x100000;
        public const int BLK_ROTATE_DETECT      = 0x100;

        //for Blk_Mirror function
        public const int BLK_MIRROR_GET         = -1;
        public const int BLK_MIRROR_NONE        = 0x00;
        public const int BLK_MIRROR_H			= 0x01;
        public const int BLK_MIRROR_V			= 0x02;

        //for Svr_Create function   
        public const int SVR_FORMAT_PDF             = 0x01;
        public const int SVR_FORMAT_RTF             = 0x02;
        public const int SVR_FORMAT_TXT_ASCII       = 0x03;
        public const int SVR_FORMAT_TXT_UNICODE     = 0x04;
        public const int SVR_FORMAT_XML             = 0x05;
        public const int SVR_FORMAT_PDFA            = 0x06;

        //for Scan_Enumerate function
        public const int SCAN_GETDEFAULTDEVICE      = 0x01;
        public const int SCAN_SETDEFAULTDEVICE      = 0x100;

        //for Scan_ScanToImg and Scan_ScanToFile functions
        public const int SCAN_NOUI                  = 0x01;
        public const int SCAN_SOURCEADF             = 0x02;
        public const int SCAN_SOURCEAUTO            = 0x04;
        public const int SCAN_DONTCLOSEDS           = 0x08;
        public const int SCAN_FILE_SEPARATE		    = 0x10;

        //for Blk_GetWordFontStyle function
        public const int FONT_STYLE_UNDERLINED	    = 0x01;
        public const int FONT_STYLE_STRIKED		    = 0x02;
        public const int FONT_STYLE_BOLD			= 0x04;
        public const int FONT_STYLE_ITALIC          = 0x08;

        //for Img_GetProperty function, PropertyID parameter
        public const int IMG_PROP_DPIX			= 0x01;	//original DPI
        public const int IMG_PROP_DPIY = 0x02;
        public const int IMG_PROP_BPP = 0x03; //color depth
        public const int IMG_PROP_WIDTH = 0x04;	//original width
        public const int IMG_PROP_HEIGHT = 0x05;	//original height
        public const int IMG_PROP_INVERTED = 0x06;   //image inversion flag after OCR_STEP_PREFILTERS step
        public const int IMG_PROP_SKEW	= 0x07;	//image skew angle after OCR_STEP_PREFILTERS step, multiplied by 1000
        public const int IMG_PROP_SCALE = 0x08;	//image scale factor after OCR_STEP_PREFILTERS step, multiplied by 1000
        public const int IMG_PROP_PAGEINDEX = 0x09; //image page index

        //for "Blk_SetWordRegEx" function
        public const int REGEX_SET				= 0x00;
        public const int REGEX_CLEAR			= 0x01;
        public const int REGEX_CLEAR_ALL		= 0x02;
        public const int REGEX_DISABLE_DICT     = 0x04;
        public const int REGEX_CHECK            = 0x08;

        //for Svr_SetDocumentInfo function
        public const int INFO_PDF_AUTHOR		= 0x01;
        public const int INFO_PDF_CREATOR		= 0x02;
        public const int INFO_PDF_PRODUCER		= 0x03;
        public const int INFO_PDF_TITLE			= 0x04;
        public const int INFO_PDF_SUBJECT		= 0x05;
        public const int INFO_PDF_KEYWORDS      = 0x06;

        //for Blk_GetBarcodeType function
        public const int BARCODE_TYPE_EAN8      = 0x01;
        public const int BARCODE_TYPE_UPCE		= 0x02;
        public const int BARCODE_TYPE_ISBN10	= 0x03;
        public const int BARCODE_TYPE_UPCA		= 0x04;
        public const int BARCODE_TYPE_EAN13		= 0x05;
        public const int BARCODE_TYPE_ISBN13	= 0x06;
        public const int BARCODE_TYPE_ZBAR_I25	= 0x07;
        public const int BARCODE_TYPE_CODE39	= 0x08;
        public const int BARCODE_TYPE_QRCODE	= 0x09;
        public const int BARCODE_TYPE_CODE128	= 0x0A;

        //for "BarCode/TypesMask" configuration option
        public const int BARCODE_TYPE_MASK_EAN8     = 0x01;
        public const int BARCODE_TYPE_MASK_UPCE		= 0x02;
        public const int BARCODE_TYPE_MASK_ISBN10	= 0x04;
        public const int BARCODE_TYPE_MASK_UPCA		= 0x08;
        public const int BARCODE_TYPE_MASK_EAN13	= 0x10;
        public const int BARCODE_TYPE_MASK_ISBN13	= 0x20;
        public const int BARCODE_TYPE_MASK_ZBAR_I25	= 0x40;
        public const int BARCODE_TYPE_MASK_CODE39	= 0x80;
        public const int BARCODE_TYPE_MASK_QRCODE	= 0x100;
        public const int BARCODE_TYPE_MASK_CODE128	= 0x200;

        //for Img_SaveToFile function
        public const int IMG_FORMAT_BMP			=  0;
        public const int IMG_FORMAT_JPEG		=  2;
        public const int IMG_FORMAT_PNG			= 13;
        public const int IMG_FORMAT_TIFF        	= 18;
        public const int IMG_FORMAT_FLAG_BINARIZED	= 0x100;
////////////////////////////////////////////////////////////////////

        //use appropriate path, otherwise you will get "BadImageFormatException"
        //for x86
        public const string LIBNAME = "Bin\\NSOCR.dll"; //IMPORTANT: CORRECT PATH TO X86 NSOCR.DLL
        //for x64
        //public const string LIBNAME = "Bin_64\\NSOCR.dll"; //IMPORTANT: CORRECT PATH TO X64 NSOCR.DLL



        [DllImport(LIBNAME)]
        public static extern int Engine_Initialize();

        [DllImport(LIBNAME)]
        public static extern int Engine_InitializeAdvanced(out int CfgObj, out int OcrObj, out int ImgObj);

        [DllImport(LIBNAME)]
        public static extern int Engine_Uninitialize();

        [DllImport(LIBNAME)]
        public static extern int Engine_SetDataDirectory([MarshalAs(UnmanagedType.LPWStr)] string DirectoryPath);

        [DllImport(LIBNAME)]
        public static extern int Engine_GetVersion([MarshalAs(UnmanagedType.LPWStr)] StringBuilder OptionValue);

        [DllImport(LIBNAME)]
        public static extern int Engine_SetLicenseKey([MarshalAs(UnmanagedType.LPWStr)] string LicenseKey);
////////

        [DllImport(LIBNAME)]
        public static extern int Cfg_Create(out int CfgObj);

        [DllImport(LIBNAME)]
        public static extern int Cfg_Destroy(int CfgObj);

        [DllImport(LIBNAME)]
        public static extern int Cfg_LoadOptions(int CfgObj, [MarshalAs(UnmanagedType.LPWStr)] string FileName);

        [DllImport(LIBNAME)]
        public static extern int Cfg_SaveOptions(int CfgObj, [MarshalAs(UnmanagedType.LPWStr)] string FileName);

        [DllImport(LIBNAME)]
        public static extern int Cfg_LoadOptionsFromString(int CfgObj, [MarshalAs(UnmanagedType.LPWStr)] string XMLString);

        [DllImport(LIBNAME)]
        public static extern int Cfg_SaveOptionsToString(int CfgObj, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder XMLString, int MaxLen);

        [DllImport(LIBNAME)]
        public static extern int Cfg_GetOption(int CfgObj, int BlockType, [MarshalAs(UnmanagedType.LPWStr)] string OptionPath, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder OptionValue, int MaxLen);

        [DllImport(LIBNAME)]
        public static extern int Cfg_SetOption(int CfgObj, int BlockType, [MarshalAs(UnmanagedType.LPWStr)] string OptionPath, [MarshalAs(UnmanagedType.LPWStr)] string OptionValue);

        [DllImport(LIBNAME)]
        public static extern int Cfg_DeleteOption(int CfgObj, int BlockType, [MarshalAs(UnmanagedType.LPWStr)] string OptionPath);

////////

        [DllImport(LIBNAME)]
        public static extern int Ocr_Create(int CfgObj, out int OcrObj);

        [DllImport(LIBNAME)]
        public static extern int Ocr_Destroy(int OcrObj);

        [DllImport(LIBNAME)]
        public static extern int Ocr_Destroy(int ImgObj, int SvrObj, int PageIndexStart, int PageIndexEnd, int OcrObjCnt, int Flags);

        [DllImport(LIBNAME)]
        public static extern int Ocr_ProcessPages(int ImgObj, int SvrObj, int PageIndexStart, int PageIndexEnd, int OcrObjCnt, int Flags);
////////

        [DllImport(LIBNAME)]
        public static extern int Img_Create(int OcrObj, out int ImgObj);

        [DllImport(LIBNAME)]
        public static extern int Img_Destroy(int ImgObj);

        [DllImport(LIBNAME)]
        public static extern int Img_LoadFile(int ImgObj, [MarshalAs(UnmanagedType.LPWStr)] string FileName);

        [DllImport(LIBNAME)]
        public static extern int Img_LoadFromMemory(int ImgObj, IntPtr Bytes, int DataSize);

        [DllImport(LIBNAME)]
        public static extern int Img_LoadBmpData(int ImgObj, IntPtr Bytes, int Width, int Height, int Flags, int Stride);

        [DllImport(LIBNAME)]
        public static extern int Img_Unload(int ImgObj);

        [DllImport(LIBNAME)]
        public static extern int Img_GetPageCount(int ImgObj);

        [DllImport(LIBNAME)]
        public static extern int Img_SetPage(int ImgObj, int PageIndex);

        [DllImport(LIBNAME)]
        public static extern int Img_GetSize(int ImgObj, out int Width, out int Height);

        [DllImport(LIBNAME)]
        public static extern int Img_DrawToDC(int ImgObj, int HandleDC, int X, int Y, ref int Width, ref int Height, int Flags);

        [DllImport(LIBNAME)]
        public static extern int Img_DeleteAllBlocks(int ImgObj);

        [DllImport(LIBNAME)]
        public static extern int Img_AddBlock(int ImgObj, int Xpos, int Ypos, int Width, int Height, out int BlkObj);

        [DllImport(LIBNAME)]
        public static extern int Img_DeleteBlock(int ImgObj, int BlkObj);

        [DllImport(LIBNAME)]
        public static extern int Img_GetBlockCnt(int ImgObj);

        [DllImport(LIBNAME)]
        public static extern int Img_GetBlock(int ImgObj, int BlockIndex, out int BlkObj);

        [DllImport(LIBNAME)]
        public static extern int Img_GetImgText(int ImgObj, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder TextStr, int MaxLen, int Flags);

        [DllImport(LIBNAME)]
        public static extern int Img_GetBmpData(int ImgObj, IntPtr Bits, ref int Width, ref int Height, int Flags);

        [DllImport(LIBNAME)]
        public static extern int Img_OCR(int ImgObj, int FirstStep, int LastStep, int Flags);

        [DllImport(LIBNAME)]
        public static extern int Img_SaveBlocks(int ImgObj, [MarshalAs(UnmanagedType.LPWStr)] string FileName);
	
        [DllImport(LIBNAME)]
        public static extern int Img_LoadBlocks(int ImgObj, [MarshalAs(UnmanagedType.LPWStr)] string FileName);
	
        [DllImport(LIBNAME)]
        public static extern int Img_GetSkewAngle(int ImgObj);

        [DllImport(LIBNAME)]
        public static extern int Img_GetPixLineCnt(int ImgObj);

        [DllImport(LIBNAME)]
        public static extern int Img_GetPixLine(int ImgObj, int LineInd, out int X1pos, out int Y1pos, out int X2pos, out int Y2pos, out int Width);

        [DllImport(LIBNAME)]
        public static extern int Img_GetScaleFactor(int ImgObj);

        [DllImport(LIBNAME)]
        public static extern int Img_CalcPointPosition(int ImgObj, ref int Xpos, ref int Ypos, int Mode);

        [DllImport(LIBNAME)]
        public static extern int Img_CopyCurrentPage(int ImgObjSrc, int ImgObjDst, int Flags);

        [DllImport(LIBNAME)]
        public static extern int Img_GetProperty(int ImgObj, int PropertyID);

        [DllImport(LIBNAME)]
        public static extern int Img_SaveToFile(int ImgObj, [MarshalAs(UnmanagedType.LPWStr)] string FileName, int Format, int Flags);

        [DllImport(LIBNAME)]
        public static extern int Img_SaveToMemory(int ImgObj, IntPtr Bytes, int BufferSize, int Format, int Flags);
////////

        [DllImport(LIBNAME)]
        public static extern int Blk_GetType(int BlkObj);

        [DllImport(LIBNAME)]
        public static extern int Blk_SetType(int BlkObj, int BlockType);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetRect(int BlkObj, out int Xpos, out int Ypos, out int Width, out int Height);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetText(int BlkObj, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder TextStr, int MaxLen, int Flags);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetLineCnt(int BlkObj);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetLineText(int BlkObj, int LineIndex, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder TextStr, int MaxLen);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetWordCnt(int BlkObj, int LineIndex);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetWordText(int BlkObj, int LineIndex, int WordIndex, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder TextStr, int MaxLen);

        [DllImport(LIBNAME)]
        public static extern int Blk_SetWordText(int BlkObj, int LineIndex, int WordIndex, [MarshalAs(UnmanagedType.LPWStr)] string TextStr);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetCharCnt(int BlkObj, int LineIndex, int WordIndex);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetCharRect(int BlkObj, int LineIndex, int WordIndex, int CharIndex, out int Xpos, out int Ypos, out int Width, out int Height);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetCharText(int BlkObj, int LineIndex, int WordIndex, int CharIndex, int ResultIndex, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder TextStr, int MaxLen);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetCharQual(int BlkObj, int LineIndex, int WordIndex, int CharIndex, int ResultIndex);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetTextRect(int BlkObj, int LineIndex, int WordIndex, out int Xpos, out int Ypos, out int Width, out int Height);

        [DllImport(LIBNAME)]
        public static extern int Blk_Inversion(int BlkObj, int Inversion);

        [DllImport(LIBNAME)]
        public static extern int Blk_Rotation(int BlkObj, int Rotation);

        [DllImport(LIBNAME)]
        public static extern int Blk_Mirror(int BlkObj, int Mirror);

        [DllImport(LIBNAME)]
        public static extern int Blk_IsWordInDictionary(int BlkObj, int LineIndex, int WordIndex);

        [DllImport(LIBNAME)]
        public static extern int Blk_SetRect(int BlkObj, int Xpos, int Ypos, int Width, int Height);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetWordQual(int BlkObj, int LineIndex, int WordIndex);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetWordFontColor(int BlkObj, int LineIndex, int WordIndex);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetWordFontSize(int BlkObj, int LineIndex, int WordIndex);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetWordFontStyle(int BlkObj, int LineIndex, int WordIndex);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetBackgroundColor(int BlkObj);

        [DllImport(LIBNAME)]
        public static extern int Blk_SetWordRegEx(int BlkObj, int LineIndex, int WordIndex, [MarshalAs(UnmanagedType.LPWStr)] string RegEx, int Flags);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetBarcodeCnt(int BlkObj);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetBarcodeText(int BlkObj, int BarcodeInd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder TextStr, int MaxLen);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetBarcodeRect(int BlkObj, int BarcodeInd, out int Xpos, out int Ypos, out int Width, out int Height);

        [DllImport(LIBNAME)]
        public static extern int Blk_GetBarcodeType(int BlkObj, int BarcodeInd);

        ////////

        [DllImport(LIBNAME)]
        public static extern int Svr_Create(int CfgObj, int Format, out int SvrObj);

        [DllImport(LIBNAME)]
        public static extern int Svr_Destroy(int SvrObj);

        [DllImport(LIBNAME)]
        public static extern int Svr_NewDocument(int SvrObj);

        [DllImport(LIBNAME)]
        public static extern int Svr_AddPage(int SvrObj, int ImgObj, int Flags);

        [DllImport(LIBNAME)]
        public static extern int Svr_SaveToFile(int SvrObj, [MarshalAs(UnmanagedType.LPWStr)] string FileName);

        [DllImport(LIBNAME)]
        public static extern int Svr_SaveToMemory(int SvrObj, IntPtr Bytes, int BufferSize);

        [DllImport(LIBNAME)]
        public static extern int Svr_GetText(int SvrObj, int PageIndex, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder TextStr, int MaxLen);

        [DllImport(LIBNAME)]
        public static extern int Svr_SetDocumentInfo(int SvrObj, int InfoID, [MarshalAs(UnmanagedType.LPWStr)] string InfoStr);

        ////////

        [DllImport(LIBNAME)]
        public static extern int Scan_Create(int CfgObj, out int ScanObj);

        [DllImport(LIBNAME)]
        public static extern int Scan_Destroy(int ScanObj);

        [DllImport(LIBNAME)]
        public static extern int Scan_Enumerate(int ScanObj, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder ScannerNames, int MaxLen, int Flags);

        [DllImport(LIBNAME)]
        public static extern int Scan_ScanToImg(int ScanObj, int ImgObj, int ScannerIndex, int Flags);

        [DllImport(LIBNAME)]
        public static extern int Scan_ScanToFile(int ScanObj, [MarshalAs(UnmanagedType.LPWStr)] string FileName, int ScannerIndex, int Flags);
    }
}