using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhiteMagic;
using System.Runtime.InteropServices;
using System.Threading;
using WhiteMagic.Internals;
using System.Windows.Forms;

namespace Huuk
{
    class HookCore
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate bool PeekMessage(IntPtr lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        private Magic magic;
        private Dictionary<string, Detour> originalFuncs;
        private Dictionary<string, Delegate> myFuncs;
        private bool running = true;

        public List<Detour> Detours { get { return magic.Detours.Applications.Values.ToList(); } }

        private int loopSleep = 1;
        public int LoopSleep
        {
            get { return loopSleep; }
            set { loopSleep = value; }
        }

        Version win8version = new Version(6, 2, 9200, 0);

        public bool IsWindows8
        {
            get { return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= win8version; }
        }

        public virtual bool AllocConsole
        {
            get { return true; }
        }

        public HookCore()
        {
            magic = new Magic();
            myFuncs = new Dictionary<string, Delegate>();
            originalFuncs = new Dictionary<string, Detour>();

            PInvoke.FreeConsole();
            if(AllocConsole)
                PInvoke.AllocConsole();
            Initialize();            
        }

        public void Run()
        {
            while (running)
            {
                Update();
                if (loopSleep > 0)
                    Thread.Sleep(loopSleep);
            }
        }

        protected void InitMainThreadUpdate()
        {
            AddHook("PeekMessage", GetAddress("User32.dll", "PeekMessageA"),
                new PeekMessage(delegate(IntPtr lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg)
                    {
                        bool result = (bool)CallOriginal(lpMsg, hWnd, wMsgFilterMin, wMsgFilterMax, wRemoveMsg);
                        if (!result)
                            MainThreadUpdate();
                        if (!OnPeekMessage(hWnd, (Message)Marshal.PtrToStructure(lpMsg, typeof(Message))))
                            return false;
                        return result;
                    }));
        }

        protected virtual bool OnPeekMessage(IntPtr hwnd, Message msg)
        {
            return true;
        }

        protected virtual void Initialize()
        {
        }

        protected virtual void Update()
        {
        }

        protected virtual void MainThreadUpdate()
        {            
        }

        public void Close()
        {
            running = false;
            PInvoke.FreeConsole();
        }

        public int GetTargetAddress(string name)
        {
            Detour detour = magic.Detours[name];
            if(detour != null)
                return Marshal.GetFunctionPointerForDelegate(detour.Target).ToInt32();
            return 0;
        }

        public void AddHook(string name, IntPtr address, Delegate function)
        {
            Delegate target = Marshal.GetDelegateForFunctionPointer(address, function.GetType());            
            myFuncs.Add(name, function);
            originalFuncs.Add(function.Method.Name, magic.Detours.CreateAndApply(target, function, name));
        }

        /*public void AddHookWithCallstack(string name, IntPtr address, Delegate function)
        {
            Delegate target = Marshal.GetDelegateForFunctionPointer(address, function.GetType());
            myFuncs.Add(name, function);
            Detour detour = magic.Detours.Create(target, function, name);
            detour.ApplyWithCallstack(GetAddress("ManagedLoader.dll", "SaveCallStack"));
            originalFuncs.Add(function.Method.Name, detour);
        }*/

        public void RemoveHook(string name)
        {
            if (myFuncs.ContainsKey(name))
            {
                magic.Detours.Delete(name);
                originalFuncs.Remove(myFuncs[name].Method.Name);
                myFuncs.Remove(name);
            }
        }

        public object CallOriginal(params object[] parameters)
        {
            string callingName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
            return originalFuncs[callingName].CallOriginal(parameters);
        }

        public string GetFunctionName()
        {
            return new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
        }

        public void Invoke(string name, params object[] arg)
        {
            if (myFuncs.ContainsKey(name))
                myFuncs[name].DynamicInvoke(arg);
        }

        public IntPtr GetAddress(string moduleName, string procName)
        {
            return PInvoke.GetProcAddress(PInvoke.GetModuleHandle(moduleName), procName);
        }

        public string GetCallstack()
        {
            return DbgHelp.StackWalk64.GetCallstack();
        }

        public string GetCallstack(bool skipManaged, bool skipFirstFrame)
        {
            return DbgHelp.StackWalk64.GetCallstack(skipManaged, skipFirstFrame);
        }

        protected byte[] ReadBytes(IntPtr address, int length)
        {
            byte[] buffer = new byte[length];
            try
            {
                Marshal.Copy(address, buffer, 0, length);
                return buffer;
            }
            catch
            {
                return null;
            }
        }

        /*public bool IsCallstackEnabled()
        {
            string callingName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
            return originalFuncs[callingName].CallstackEnabled;
        }*/        
    }
}