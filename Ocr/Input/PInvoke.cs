using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace Ocr.Input
{
    class PInvoke
    {
        const int INPUT_MOUSE = 0;
        const int INPUT_KEYBOARD = 1;
        const int INPUT_HARDWARE = 2;
        const int KEYEVENTF_KEYUP = 2;
        const int KEYEVENTF_UNICODE = 4;

        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(int key);//public static extern short GetAsyncKeyState(System.Windows.Forms.Keys key);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool ClientToScreen(IntPtr hwnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint cmd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        public static extern int ActivateKeyboardLayout(int HKL, int flags);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);        

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern short VkKeyScan(char ch);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        public static uint MakeLParam(uint value)
        {
            return (MapVirtualKey(value, 0) << 16);
        }

        [StructLayout(LayoutKind.Explicit, Size = 28)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public uint type;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
        }

        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public long time;
            public uint dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        private enum Mouse
        {
            LEFTDOWN = 0x2,
            LEFTUP = 0x4,
        }

        public static bool GetWindowRect(IntPtr handle, out Rectangle rect)
        {
            return (rect = GetWindowRect(handle)) != Rectangle.Empty;
        }

        public static Rectangle GetWindowRect(IntPtr handle)
        {
            RECT nativeRect;
            if (!GetClientRect(handle, out nativeRect))
                return Rectangle.Empty;

            Point pos = new Point(nativeRect.Left, nativeRect.Top);
            if (!ClientToScreen(handle, ref pos))
                return Rectangle.Empty;

            nativeRect.Left = pos.X;
            nativeRect.Top = pos.Y;

            Rectangle rect = Rectangle.Empty;
            rect.X = nativeRect.Left;
            rect.Y = nativeRect.Top;
            rect.Width = nativeRect.Right - nativeRect.Left + 1;
            rect.Height = nativeRect.Bottom - nativeRect.Top + 1;
            return rect;
        }

        public static bool IsKeyDown(Keys key)
        {
            return 0 != (GetAsyncKeyState((int)key) & 0x8000);
        }

        public static void KeysDown(params Keys[] keys)
        {
            INPUT[] structInputs = new INPUT[keys.Length];
            for (int i = 0; i < structInputs.Length; i++)
            {
                INPUT structInput = new INPUT();
                structInput.type = (uint)INPUT_KEYBOARD;
                structInput.ki.wScan = 0;
                structInput.ki.time = 0;
                structInput.ki.dwFlags = 0;//key down
                structInput.ki.wVk = (ushort)keys[i];
                structInputs[i] = structInput;
            }
            SendInput((uint)structInputs.Length, structInputs, Marshal.SizeOf(structInputs[0]));
        }

        public static void KeysUp(params Keys[] keys)
        {
            INPUT[] structInputs = new INPUT[keys.Length];
            for (int i = 0; i < structInputs.Length; i++)
            {
                INPUT structInput = new INPUT();
                structInput.type = (uint)INPUT_KEYBOARD;
                structInput.ki.wScan = 0;
                structInput.ki.time = 0;
                structInput.ki.dwFlags = 2;//key up
                structInput.ki.wVk = (ushort)keys[i];
                structInputs[i] = structInput;
            }
            SendInput((uint)structInputs.Length, structInputs, Marshal.SizeOf(structInputs[0]));
        }

        public static void KeyDown(Keys key, bool extend = false)
        {
            /*INPUT structInput = new INPUT();
            structInput.type = (uint)INPUT_KEYBOARD;
            structInput.ki.wScan = 0;
            structInput.ki.time = 0;
            structInput.ki.dwFlags = 1;//key down
            structInput.ki.wVk = (ushort)key;
            if (extend)
            {
                structInput.ki.dwFlags |= 1;
            }
            SendInput(1, ref structInput, Marshal.SizeOf(structInput));*/
            PInvoke.keybd_event((byte)key, (byte)PInvoke.MapVirtualKey((uint)key, 0), 0, 0);
        }

        public static void KeyUp(Keys key, bool extend = false)
        {
            /*INPUT structInput = new INPUT();
            structInput.type = (uint)INPUT_KEYBOARD;
            structInput.ki.wScan = 0;
            structInput.ki.time = 0;
            structInput.ki.dwFlags = 2;//key up
            if (extend)
            {
                structInput.ki.dwFlags |= 1;
            }
            structInput.ki.wVk = (ushort)key;
            SendInput(1, ref structInput, Marshal.SizeOf(structInput));*/
            PInvoke.keybd_event((byte)key, (byte)PInvoke.MapVirtualKey((uint)key, 0), 2, 0);
        }

        public static void SendCharUnicode(char character)
        {
            string unicodeString = character.ToString();
            INPUT[] input = new INPUT[unicodeString.Length];

            for (int i = 0; i < input.Length; i++)
            {
                input[i] = new INPUT();
                input[i].type = INPUT_KEYBOARD;
                input[i].ki.wVk = 0;
                input[i].ki.wScan = (ushort)unicodeString[i];
                input[i].ki.time = 0;
                input[i].ki.dwFlags = KEYEVENTF_UNICODE;
                input[i].ki.dwExtraInfo = 0;
            }

            SendInput((uint)input.Length, input, Marshal.SizeOf(typeof(INPUT)));            
        }

        public static void SendCharUnicode(int utf32)
        {
            SendCharUnicode(Char.ConvertFromUtf32(utf32)[0]);
        }        

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            return lpPoint;
        }

        public static void Click(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event((uint)Mouse.LEFTDOWN, 0, 0, 0, 0);
            mouse_event((uint)Mouse.LEFTUP, 0, 0, 0, 0);
        }

        public static void Click(Point pos)
        {
            Click(pos.X, pos.Y);
        }
    }
}
