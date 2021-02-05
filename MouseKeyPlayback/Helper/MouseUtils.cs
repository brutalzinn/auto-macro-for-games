using GregsStack.InputSimulatorStandard;
using MouseKeyboardLibrary;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MouseKeyPlayback
{

    public class MouseUtils {

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public static void PerformMouseEvent(InputSimulator instance, MouseEvent mEvent, CursorPoint location)
        {
         

            int x = (int)location.X;
            int y = (int)location.Y;
            SetCursorPos(x, y);

            switch (mEvent.Action)
            {

                case MouseHook.MouseEventType.MouseUp:
                    switch (mEvent.Button)
                    {
                        case MouseButtons.Left:
                    instance.Mouse.MoveMouseBy(x,y);
                    instance.Mouse.LeftButtonUp();
                            break;
                        case MouseButtons.Right:
                            instance.Mouse.MoveMouseBy(x, y);
                            instance.Mouse.RightButtonUp();
                            break;
                        case MouseButtons.Middle:
                            instance.Mouse.MoveMouseBy(x, y);
                            instance.Mouse.MiddleButtonUp();
                            break;
                    }
                    
                    break;
                case MouseHook.MouseEventType.MouseDown:
                    switch (mEvent.Button)
                    {
                        case MouseButtons.Left:
                            instance.Mouse.MoveMouseBy(x, y);
                            instance.Mouse.LeftButtonDown();
                            break;
                        case MouseButtons.Right:
                            instance.Mouse.MoveMouseBy(x, y);
                            instance.Mouse.RightButtonDown();
                            break;
                        case MouseButtons.Middle:
                            instance.Mouse.MoveMouseBy(x, y);
                            instance.Mouse.MiddleButtonDown();
                            break;
                    }
                    break;
                case MouseHook.MouseEventType.MouseWheel:

                    instance.Mouse.MouseWheelClickSize = mEvent.Value;
                    break;

            }

            if (mEvent.Action != MouseHook.MouseEventType.MouseMove)
                Thread.Sleep(30);
        }
    }
}
