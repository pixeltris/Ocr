using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ocr.UI
{
    class ToolStripEx : ToolStrip
    {
        private const uint WM_MOUSEACTIVATE = 0x21;
        private const uint MA_ACTIVATE = 1;
        private const uint MA_ACTIVATEANDEAT = 2;
        private const uint MA_NOACTIVATE = 3;
        private const uint MA_NOACTIVATEANDEAT = 4;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_MOUSEACTIVATE &&
                m.Result == (IntPtr)MA_ACTIVATEANDEAT)
            {
                m.Result = (IntPtr)MA_ACTIVATE;
            }
        }
    }
}
