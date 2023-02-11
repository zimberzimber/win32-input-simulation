using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace InputSimulator
{
    class Program
    {
        const int ACTIVATOR_KEY = 109; // Numbap dash (-)

        enum KeyStates
        {
            Triggered = 32769,
            Holding = 32768,
            HoldingMouseButton = -32768
        }

        [Flags]
        public enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out MousePoint lpMousePoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct MousePoint
        {
            public int X;
            public int Y;

            public MousePoint(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);



        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);


        public static bool KeyActive { get; set; } = false;

        static void Main(string[] args)
        {
            while (true)
            {
                Thread.Sleep(10);
                var state = GetAsyncKeyState(ACTIVATOR_KEY);
                if (state == (int)KeyStates.Triggered)
                {
                    KeyActive = !KeyActive;

                    if (KeyActive)
                    {
                        mouse_event((int)MouseEventFlags.RightDown, 0, 0, 0, 0);
                        Console.WriteLine($"Enabled!");
                    }
                    else
                    {
                        mouse_event((int)MouseEventFlags.RightUp, 0, 0, 0, 0);
                        Console.WriteLine($"Disabled!");
                    }
                }

                //for (int code = 0; code < 255; code++)
                //{
                //    var state = GetAsyncKeyState(code);
                //    if (state != 0) // 32769 = triggered | 32768 = holding | -32768 = holding mouse buttonfjdfkdk
                //        Console.WriteLine($"{code} : {state}");
                //}
            }
        }
    }
}
