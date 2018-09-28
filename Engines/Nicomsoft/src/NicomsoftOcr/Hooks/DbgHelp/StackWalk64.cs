using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Huuk.DbgHelp
{
    sealed class StackWalk64
    {
        private static bool SymbolsLoaded = false;

        public static bool LoadSymbols()
        {
            IntPtr process = Native.GetCurrentProcess();
            return Native.SymInitialize(process, null, true);
        }

        private static Native.IMAGEHLP_MODULE64_SHORT GetModuleInfo(IntPtr process, ulong address)
        {
            Native.IMAGEHLP_MODULE64_SHORT moduleInfo = new Native.IMAGEHLP_MODULE64_SHORT();
            moduleInfo.SizeOfStruct = (uint)Marshal.SizeOf(typeof(Native.IMAGEHLP_MODULE64_SHORT));
            Native.SymGetModuleInfo64(process, address, ref moduleInfo);
            return moduleInfo;
        }

        public static string GetCallstack()
        {
            return GetCallstack(true, true);
        }

        public static string GetCallstack(bool skipManaged, bool skipFirstFrame)
        {
            Native.STACKFRAME64 stackFrame = new Native.STACKFRAME64();
            Native.CONTEXT context = new Native.CONTEXT();
            context.ContextFlags = (uint)Native.CONTEXT_FLAGS.CONTEXT_FULL;

            IntPtr process = Native.GetCurrentProcess();
            IntPtr thread = Native.GetCurrentThread();

            Native.RtlCaptureContext(ref context);
            context.ContextFlags = (uint)Native.CONTEXT_FLAGS.CONTEXT_FULL;

            if (!SymbolsLoaded)
                LoadSymbols();

            stackFrame.AddrPC.Offset = context.Eip;
            stackFrame.AddrPC.Mode = Native.ADDRESS_MODE.AddrModeFlat;
            stackFrame.AddrStack.Offset = context.Esp;
            stackFrame.AddrStack.Mode = Native.ADDRESS_MODE.AddrModeFlat;

            Native.IMAGEHLP_SYMBOL64 symbolInfo = new Native.IMAGEHLP_SYMBOL64();
            symbolInfo.MaxNameLen = Native.MAX_SYMBOL_NAME;
            symbolInfo.SizeOfStruct = (uint)(Marshal.SizeOf(typeof(Native.SYMBOL_INFO)) - Native.MAX_SYMBOL_NAME);

            string callstack = string.Empty;
            bool foundManagedDll = false;
            bool foundUnmanagedDll = false;

            for (int frame = 0; ; frame++)
            {
                if (!Native.StackWalk64(Native.IMAGE_FILE_MACHINE_I386, process, thread, ref stackFrame, ref context, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero))
                    break;

                string moduleName = GetModuleInfo(process, stackFrame.AddrPC.Offset).ModuleName;

                if (skipManaged)
                {
                    if (!foundManagedDll)
                    {
                        if (moduleName == "mscorwks")
                            foundManagedDll = true;
                        if (stackFrame.AddrPC.Offset == 0)
                            break;
                        else
                            continue;
                    }
                    else if (!foundUnmanagedDll)
                    {
                        if (moduleName != "mscorwks")
                        {
                            foundUnmanagedDll = true;
                            frame = 0;
                        }
                        else if (stackFrame.AddrPC.Offset == 0)
                            break;
                        else
                            continue;
                    }
                }

                if (skipFirstFrame && frame == 0)
                {
                    if (stackFrame.AddrPC.Offset == 0)
                        break;
                    else
                        continue;
                }

                ulong displacement = 0;
                Native.SetLastError(0);
                if (Native.SymGetSymFromAddr64(process, stackFrame.AddrPC.Offset, ref displacement, ref symbolInfo))
                    callstack += string.Format("{0}: {1} ({2}) ({3})\n", frame, stackFrame.AddrPC.Offset.ToString("X8"), moduleName, symbolInfo.Name);
                else
                {
                    int error = Native.GetLastError();
                    if(error != 0)
                        callstack += string.Format("{0}: {1} ({2}) (Error: {3})\n", frame, stackFrame.AddrPC.Offset.ToString("X8"), moduleName, error);
                    else
                        callstack += string.Format("{0}: {1} ({2}) (Unknown)\n", frame, stackFrame.AddrPC.Offset.ToString("X8"), moduleName);
                }

                if (stackFrame.AddrReturn.Offset == 0)
                    break;
            }
            return callstack;
        }        
    }
}