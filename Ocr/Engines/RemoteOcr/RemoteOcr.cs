using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocr
{
    public class RemoteOcr : OcrEngine
    {
        private static Dictionary<Type, OcrEngineType> remoteTypes = new Dictionary<Type, OcrEngineType>();

        public OcrEngineType TargetType { get; private set; }

        public override string Name
        {
            get { return "Remote"; }
        }

        public override bool IsInstalled
        {
            get { return true; }
        }

        public RemoteOcr(OcrEngineType targetType)
        {
            TargetType = targetType;
        }

        public override OcrResult Load(OcrImage image, string language, string apiLanguage)
        {
            return OcrNetwork.Load(image, language, TargetType);
        }

        protected override void GetSupportedLanguages()
        {
        }

        public static void ClearRemote()
        {
            remoteTypes.Clear();
        }

        public static void SetRemote(OcrEngineType type)
        {
            remoteTypes[OcrEngine.GetType(type)] = type;
        }

        public static bool IsRemote(Type type)
        {
            return remoteTypes.ContainsKey(type);
        }

        public static bool IsRemote<T>() where T : OcrEngine
        {
            return IsRemote(typeof(T));
        }

        public static bool IsRemote(OcrEngineType type)
        {
            return IsRemote(OcrEngine.GetType(type));
        }
    }
}