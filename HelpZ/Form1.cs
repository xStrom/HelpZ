// Copyright 2012-2013 Kaur Kuut
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace HelpZ {
    public partial class Form1 : Form {
        private static int clickX = 0;
        private static int clickY = 0;

        private static void SetClickCoordinates(int width, int height) {
            if (width == 1920 && height == 1200) {
                clickX = (int)Math.Round(650.0 / 1920.0 * 65535.0);
                clickY = (int)Math.Round(610.0 / 1200.0 * 65535.0);
            }
            else if (width == 1280 && height == 768) {
                clickX = (int)Math.Round(392.0 / 1280.0 * 65535.0);
                clickY = (int)Math.Round(392.0 / 768.0 * 65535.0);
            }
            else if (width == 1024 && height == 600) {
                clickX = (int)Math.Round(295.0 / 1024.0 * 65535.0);
                clickY = (int)Math.Round(305.0 / 600.0 * 65535.0);
            }
        }

        KeyboardHook hook = new KeyboardHook();

        #region Imports

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        public static extern short VkKeyScan(char ch);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        #endregion

        public static uint ClickMouse() {
            INPUT structure = new INPUT();
            structure.mi.dx = clickX;
            structure.mi.dy = clickY;
            structure.mi.mouseData = 0;
            structure.mi.dwFlags = (int)MOUSEEVENTF.LEFTDOWN | (int)MOUSEEVENTF.ABSOLUTE | (int)MOUSEEVENTF.MOVE;
            INPUT input2 = structure;
            input2.mi.dwFlags = (int)MOUSEEVENTF.LEFTUP;
            INPUT[] pInputs = new INPUT[] { structure, input2 };
            return SendInput(2, pInputs, Marshal.SizeOf(structure));
        }

        public static uint Z() {
            INPUT structure = new INPUT();
            structure.type = (int)InputType.INPUT_KEYBOARD;
            structure.ki.wVk = VkKeyScan((char)System.Windows.Forms.Keys.Z);
            structure.ki.dwFlags = (int)KEYEVENTF.KEYDOWN;
            structure.ki.dwExtraInfo = GetMessageExtraInfo();

            INPUT input2 = new INPUT();
            structure.type = (int)InputType.INPUT_KEYBOARD;
            structure.ki.wVk = VkKeyScan((char)System.Windows.Forms.Keys.Z);
            input2.mi.dwFlags = (int)KEYEVENTF.KEYUP;
            input2.ki.dwExtraInfo = GetMessageExtraInfo();

            INPUT[] pInputs = new INPUT[] { structure, input2 };

            return SendInput(2, pInputs, Marshal.SizeOf(structure));
        }

        public static uint Esc() {
            INPUT structure = new INPUT();
            structure.type = (int)InputType.INPUT_KEYBOARD;
            structure.ki.wVk = VkKeyScan((char)System.Windows.Forms.Keys.Escape);
            structure.ki.dwFlags = (int)KEYEVENTF.KEYDOWN;
            structure.ki.dwExtraInfo = GetMessageExtraInfo();

            INPUT input2 = new INPUT();
            structure.type = (int)InputType.INPUT_KEYBOARD;
            structure.ki.wVk = VkKeyScan((char)System.Windows.Forms.Keys.Escape);
            input2.mi.dwFlags = (int)KEYEVENTF.KEYUP;
            input2.ki.dwExtraInfo = GetMessageExtraInfo();

            INPUT[] pInputs = new INPUT[] { structure, input2 };

            return SendInput(2, pInputs, Marshal.SizeOf(structure));
        }

        public static void SendKeyAsInput(System.Windows.Forms.Keys key) {
            INPUT structure = new INPUT();
            structure.type = (int)InputType.INPUT_KEYBOARD;
            structure.ki.wVk = (short)key;
            structure.ki.dwFlags = (int)KEYEVENTF.KEYDOWN;
            structure.ki.dwExtraInfo = GetMessageExtraInfo();

            INPUT input2 = new INPUT();
            input2.type = (int)InputType.INPUT_KEYBOARD;
            input2.ki.wVk = (short)key;
            input2.mi.dwFlags = (int)KEYEVENTF.KEYUP;
            input2.ki.dwExtraInfo = GetMessageExtraInfo();

            INPUT[] pInputs = new INPUT[] { structure, input2 };

            SendInput(2, pInputs, Marshal.SizeOf(structure));
        }

        public static void SendKeyAsInput(System.Windows.Forms.Keys key, int HoldTime) {
            INPUT INPUT1 = new INPUT();
            INPUT1.type = (int)InputType.INPUT_KEYBOARD;
            INPUT1.ki.wVk = (short)key;
            INPUT1.ki.dwFlags = (int)KEYEVENTF.KEYDOWN;
            INPUT1.ki.dwExtraInfo = GetMessageExtraInfo();
            SendInput(1, new INPUT[] { INPUT1 }, Marshal.SizeOf(INPUT1));

            WaitForSingleObject((IntPtr)0xACEFDB, (uint)HoldTime);

            INPUT INPUT2 = new INPUT();
            INPUT2.type = (int)InputType.INPUT_KEYBOARD;
            INPUT2.ki.wVk = (short)key;
            INPUT2.mi.dwFlags = (int)KEYEVENTF.KEYUP;
            INPUT2.ki.dwExtraInfo = GetMessageExtraInfo();
            SendInput(1, new INPUT[] { INPUT2 }, Marshal.SizeOf(INPUT2));

        }

        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT {
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public int type;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [Flags]
        public enum InputType {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2
        }

        [Flags]
        public enum MOUSEEVENTF {
            MOVE = 0x0001, /* mouse move */
            LEFTDOWN = 0x0002, /* left button down */
            LEFTUP = 0x0004, /* left button up */
            RIGHTDOWN = 0x0008, /* right button down */
            RIGHTUP = 0x0010, /* right button up */
            MIDDLEDOWN = 0x0020, /* middle button down */
            MIDDLEUP = 0x0040, /* middle button up */
            XDOWN = 0x0080, /* x button down */
            XUP = 0x0100, /* x button down */
            WHEEL = 0x0800, /* wheel button rolled */
            MOVE_NOCOALESCE = 0x2000, /* do not coalesce mouse moves */
            VIRTUALDESK = 0x4000, /* map to entire virtual desktop */
            ABSOLUTE = 0x8000 /* absolute move */
        }

        [Flags]
        public enum KEYEVENTF {
            KEYDOWN = 0,
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            UNICODE = 0x0004,
            SCANCODE = 0x0008,
        }

        #region SUBCLASS
        #region API

        [DllImport("kernel32.dll", EntryPoint = "GetLastError", SetLastError = false, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int GetLastError();

        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        private class User32 {
            public enum WindowState {
                SW_SHOWNORMAL = 1,
                SW_SHOWMINIMIZED = 2,
                SW_SHOWMAXIMIZED = 3
            }
            [StructLayout(LayoutKind.Sequential)]
            public struct WINDOWPLACEMENT {
                public int length;
                public int flags;
                public int showCmd;
                public System.Drawing.Point ptMinPosition;
                public System.Drawing.Point ptMaxPosition;
                public System.Drawing.Rectangle rcNormalPosition;
            }
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }
            [DllImport("user32.dll")]
            public static extern int GetForegroundWindow();
            [DllImport("user32.dll")]
            public static extern int FindWindow(string lpClassName, string lpWindowName);
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
            [DllImport("user32.dll")]
            public static extern IntPtr GetClientRect(IntPtr hWnd, ref RECT rect);
            [DllImport("user32.dll")]
            public static extern bool SetForegroundWindow(IntPtr hWnd);


            [DllImport("user32.dll")]
            public static extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, UInt32 dwData, IntPtr dwExtraInfo);

            public const int MOUSEEVENTF_LEFTDOWN = 0x02;
            public const int MOUSEEVENTF_LEFTUP = 0x04;
            public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
            public const int MOUSEEVENTF_RIGHTUP = 0x10;
        }
        #endregion
        #endregion

        public Form1() {
            InitializeComponent();

            SetClickCoordinates(1920, 1200);

            /*
            // register the event that is fired after the key press.
            hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            // register the hot key.
            hook.RegisterHotKey(0, Keys.F2);
             * */
        }

        private void SendLeftClick() {
            User32.mouse_event(User32.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
            User32.mouse_event(User32.MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
        }

        private void SendRightClick() {
            User32.mouse_event(User32.MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, new System.IntPtr());
            User32.mouse_event(User32.MOUSEEVENTF_RIGHTUP, 0, 0, 0, new System.IntPtr());
        }

        /*
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            Keys k = Keys.F2;
            HotKey.RegisterHotKey(this, k);
        }
        
        // CF Note: The WndProc is not present in the Compact Framework (as of vers. 3.5)! please derive from the MessageWindow class in order to handle WM_HOTKEY
        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);

            if (m.Msg == HotKey.WM_HOTKEY) {
                DoRespawn();
            }
        }
         * */

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            //HotKey.UnregisterHotKey(this);
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            DoRespawn();
        }

        private void DoRespawn() {
            //SendLeftClick();
            //Esc();
            //Z();
            //Application.Exit();
        }

        private void SpamRespawn() {
            int spamCount = Int32.Parse(cmbClick.Text) * 100;
            for (int i = 0; i < spamCount; i++) {
                ClickMouse();
                Thread.Sleep(10);
            }
        }

        private void timer1_Tick(object sender, EventArgs e) {
            timer1.Enabled = false;
            button1.Text = "Spamming ..";
            Application.DoEvents();

            SpamRespawn();

            button1.Text = "Start!";
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e) {
            button1.Enabled = false;
            button1.Text = "Waiting ..";
            timer1.Interval = Int32.Parse(cmbWait.Text) * 1000;
            timer1.Enabled = true;
        }

        private void cmbResolution_SelectedIndexChanged(object sender, EventArgs e) {
            if (cmbResolution.Text == "1920x1200") {
                SetClickCoordinates(1920, 1200);
            }
            else if (cmbResolution.Text == "1280x768") {
                SetClickCoordinates(1280, 768);
            }
            else if (cmbResolution.Text == "1024x600") {
                SetClickCoordinates(1024, 600);
            }
            else {
                SetClickCoordinates(1920, 1200);
            }
        }
    }
}
