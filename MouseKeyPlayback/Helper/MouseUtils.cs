﻿using GregsStack.InputSimulatorStandard;
using MouseKeyboardLibrary;
using System.Runtime.InteropServices;
using System.Threading;

namespace MouseKeyPlayback
{

    public class MouseUtils {

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public static void PerformMouseEvent(InputSimulator instance,MouseHook.MouseEvents mEvent, CursorPoint location)
        {
         

            int x = (int)location.X;
            int y = (int)location.Y;
            SetCursorPos(x, y);

            switch (mEvent)
            {

                case MouseHook.MouseEvents.LeftDown:
                    instance.Mouse.MoveMouseBy(x,y);
                    instance.Mouse.LeftButtonDown();
                 //   mouse_event(Constants.MOUSEEVENT_LEFTDOWN, x, y, 0, 0);
                    break;
                case MouseHook.MouseEvents.LeftUp:
                    instance.Mouse.MoveMouseBy(x, y);
                    instance.Mouse.LeftButtonUp();
                    break;
                case MouseHook.MouseEvents.RightDown:
                    instance.Mouse.MoveMouseBy(x, y);
                    instance.Mouse.RightButtonDown();
                    break;
                case MouseHook.MouseEvents.RightUp:
                    instance.Mouse.MoveMouseBy(x, y);
                    instance.Mouse.RightButtonUp();

                    break;
                case MouseHook.MouseEvents.ScrollDown:
                
                    mouse_event(Constants.MOUSEEVENTF_WHEEL, 0, 0, -120, 0);
                    break;
                case MouseHook.MouseEvents.ScrollUp:
                    mouse_event(Constants.MOUSEEVENTF_WHEEL, 0, 0, 120, 0);
                    break;
            }

            if (mEvent != MouseHook.MouseEvents.MouseMove)
                Thread.Sleep(30);
        }
    }
}
