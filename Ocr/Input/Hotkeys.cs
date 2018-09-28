using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ocr.Input
{
    public sealed class Hotkeys : IDisposable
    {
        // Registers a hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        // Unregisters the hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// Represents the window that is used internally to get the messages.
        /// </summary>
        private class Window : NativeWindow, IDisposable
        {
            private static int WM_HOTKEY = 0x0312;

            public Window()
            {
                // create the handle for the window.
                this.CreateHandle(new CreateParams());
            }

            /// <summary>
            /// Overridden to get the notifications.
            /// </summary>
            /// <param name="m"></param>
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                // check if we got a hot key pressed.
                if (m.Msg == WM_HOTKEY)
                {
                    // get the keys.
                    Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                    ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

                    // invoke the event to notify the parent.
                    if (KeyPressed != null)
                        KeyPressed(this, new KeyPressedEventArgs(modifier, key));
                }
            }

            public event EventHandler<KeyPressedEventArgs> KeyPressed;

            #region IDisposable Members

            public void Dispose()
            {
                this.DestroyHandle();
            }

            #endregion
        }

        private Window _window = new Window();
        private int _currentId;

        public Hotkeys()
        {
            // register the event of the inner native window.
            _window.KeyPressed += delegate(object sender, KeyPressedEventArgs args)
            {
                if (KeyPressed != null)
                    KeyPressed(this, args);
            };
        }

        public void RegisterHotKey(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            ModifierKeys modifier;
            Keys key;
            ParseShortcut(text, "+", out modifier, out key);
            if (modifier == ModifierKeys.None)
                return;
            try
            {
                RegisterHotKey(modifier, key);
            }
            catch
            {
                ClearHotKey();
            }
        }

        public void ClearHotKey()
        {
            // unregister all the registered hot keys.
            for (int i = _currentId; i > 0; i--)
            {
                UnregisterHotKey(_window.Handle, i);
            }
        }

        /// <summary>
        /// Registers a hot key in the system.
        /// </summary>
        /// <param name="modifier">The modifiers that are associated with the hot key.</param>
        /// <param name="key">The key itself that is associated with the hot key.</param>
        public void RegisterHotKey(ModifierKeys modifier, Keys key)
        {
            // increment the counter.
            _currentId = _currentId + 1;

            // register the hot key.
            if (!RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key))
                throw new InvalidOperationException("Couldn’t register the hot key.");
        }

        /// <summary>
        /// A hot key has been pressed.
        /// </summary>
        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        #region IDisposable Members

        public void Dispose()
        {
            ClearHotKey();

            // dispose the inner native window.
            _window.Dispose();
        }

        #endregion

        public static void ParseShortcut(string text, string separator, out ModifierKeys modifier, out Keys key)
        {
            bool HasAlt = false; bool HasControl = false; bool HasShift = false; bool HasWin = false;

            modifier = ModifierKeys.None;        //Variable to contain modifier.
            key = 0;           //The key to register.

            string[] result;
            string[] separators = new string[] { separator };
            result = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            //Iterate through the keys and find the modifier.
            foreach (string entry in result)
            {
                string keyName = entry.Trim().ToLower();

                //Find the Control Key.
                if (keyName == Keys.Control.ToString().ToLower() || keyName == "ctrl")
                {
                    HasControl = true;
                }
                //Find the Alt key.
                if (keyName == Keys.Alt.ToString().ToLower())
                {
                    HasAlt = true;
                }
                //Find the Shift key.
                if (keyName == Keys.Shift.ToString().ToLower())
                {
                    HasShift = true;
                }
                //Find the Window key.
                if (keyName == Keys.LWin.ToString().ToLower())
                {
                    HasWin = true;
                }
            }

            if (HasControl) { modifier |= ModifierKeys.Control; }
            if (HasAlt) { modifier |= ModifierKeys.Alt; }
            if (HasShift) { modifier |= ModifierKeys.Shift; }
            if (HasWin) { modifier |= ModifierKeys.Win; }

            KeysConverter keyconverter = new KeysConverter();
            key = (Keys)keyconverter.ConvertFrom(result.GetValue(result.Length - 1));
        }
    }

    /// <summary>
    /// Event Args for the event that is fired after the hot key has been pressed.
    /// </summary>
    public class KeyPressedEventArgs : EventArgs
    {
        private ModifierKeys _modifier;
        private Keys _key;

        internal KeyPressedEventArgs(ModifierKeys modifier, Keys key)
        {
            _modifier = modifier;
            _key = key;
        }

        public ModifierKeys Modifier
        {
            get { return _modifier; }
        }

        public Keys Key
        {
            get { return _key; }
        }
    }

    /// <summary>
    /// The enumeration of possible modifiers.
    /// </summary>
    [Flags]
    public enum ModifierKeys : uint
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }
}
