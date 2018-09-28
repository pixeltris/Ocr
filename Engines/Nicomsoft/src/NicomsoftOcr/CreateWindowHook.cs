using Huuk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NicomsoftOcr
{
    class CreateWindowHook : HookCore
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        static extern int PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        public override bool AllocConsole
        {
            get { return false; }
        }

        protected override void Initialize()
        {
            AddHook("ShowWindow", GetAddress("user32.dll", "ShowWindow"),
                            new ShowWindow(delegate(IntPtr hWnd, int nCmdShow)
                            {
                                PostMessage(hWnd, 0x10, 0, 0);
                                return true;
                            }));
        }
    }
}
