using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ocr
{
    internal class LanguagePackExtractor
    {
        // Features On Demand V2 (Capabilities)
        // https://msdn.microsoft.com/en-us/library/windows/hardware/mt171094(v=vs.85).aspx

        public static void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LanguagePackExtractorForm());
        }
    }
}
