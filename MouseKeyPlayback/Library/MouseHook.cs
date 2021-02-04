﻿using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MouseKeyboardLibrary
{

    /// <summary>
    /// Captures global mouse events
    /// </summary>
    public class MouseHook : GlobalHook
    {

        #region MouseEventType Enum
        public enum MouseKeys
        {
            Left = MouseEvents.LeftDown,
            Middle = MouseEvents.MiddleDown,
            Right = MouseEvents.RightDown

        }
        public enum MouseEventType
        {
            None,
            MouseDown,
            MouseUp,
            DoubleClick,
            MouseWheel,
            MouseMove
        }
        public enum MouseEvents
        {
            LeftDown = 0x201,
            LeftUp = 0x202,
            LeftDoubleClick = 0x203,
            RightDown = 0x204,
            RightUp = 0x205,
            RightDoubleClick = 0x206,
            MiddleDown = 0x207,
            MiddleUp = 0x208,
            MiddleDoubleClick = 0x209,
            MouseScroll = 0x20a,
            ScrollUp = 7864320,
            ScrollDown = -7864320,
            MouseMove = -1
        }

        #endregion

        #region Events

        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseMove;
        public event MouseEventHandler MouseWheel;

        public event EventHandler Click;
        public event EventHandler DoubleClick;

        #endregion

        #region Constructor

        public MouseHook()
        {

            _hookType = WH_MOUSE_LL;

        }

        #endregion

        #region Methods

        protected override int HookCallbackProcedure(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode <= -1 || (MouseDown == null && MouseUp == null && MouseMove == null)) return CallNextHookEx(_handleToHook, nCode, wParam, lParam);
            MouseLLHookStruct mouseHookStruct = (MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct));

            MouseButtons button = GetButton(wParam);
            MouseEventType eventType = GetEventType(wParam);

            MouseEventArgs e = new MouseEventArgs(
                button,
                (eventType == MouseEventType.DoubleClick ? 2 : 1),
                mouseHookStruct.pt.x,
                mouseHookStruct.pt.y,
                (eventType == MouseEventType.MouseWheel ? (short)((mouseHookStruct.mouseData >> 16) & 0xffff) : 0));

            // Prevent multiple Right Click events (this probably happens for popup menus)
            if (button == MouseButtons.Right && mouseHookStruct.flags != 0)
            {
                eventType = MouseEventType.None;
            }

            switch (eventType)
            {
                case MouseEventType.MouseDown:
                    if (MouseDown != null)
                    {
                        MouseDown(this, e);
                    }
                    break;
                case MouseEventType.MouseUp:
                    if (Click != null)
                    {
                        Click(this, new EventArgs());
                    }
                    if (MouseUp != null)
                    {
                        MouseUp(this, e);
                    }
                    break;
                case MouseEventType.DoubleClick:
                    if (DoubleClick != null)
                    {
                        DoubleClick(this, new EventArgs());
                    }
                    break;
                case MouseEventType.MouseWheel:
                    if (MouseWheel != null)
                    {
                        MouseWheel(this, e);
                    }
                    break;
                case MouseEventType.MouseMove:
                    if (MouseMove != null)
                    {
                        MouseMove(this, e);
                    }
                    break;
            }

            return CallNextHookEx(_handleToHook, nCode, wParam, lParam);

        }

        private static MouseButtons GetButton(Int32 wParam)
        {

            switch (wParam)
            {

                case WM_LBUTTONDOWN:
                case WM_LBUTTONUP:
                case WM_LBUTTONDBLCLK:
                    return MouseButtons.Left;
                case WM_RBUTTONDOWN:
                case WM_RBUTTONUP:
                case WM_RBUTTONDBLCLK:
                    return MouseButtons.Right;
                case WM_MBUTTONDOWN:
                case WM_MBUTTONUP:
                case WM_MBUTTONDBLCLK:
                    return MouseButtons.Middle;
                default:
                    return MouseButtons.None;

            }

        }

        private static MouseEventType GetEventType(Int32 wParam)
        {

            switch (wParam)
            {

                case WM_LBUTTONDOWN:
                case WM_RBUTTONDOWN:
                case WM_MBUTTONDOWN:
                    return MouseEventType.MouseDown;
                case WM_LBUTTONUP:
                case WM_RBUTTONUP:
                case WM_MBUTTONUP:
                    return MouseEventType.MouseUp;
                case WM_LBUTTONDBLCLK:
                case WM_RBUTTONDBLCLK:
                case WM_MBUTTONDBLCLK:
                    return MouseEventType.DoubleClick;
                case WM_MOUSEWHEEL:
                    return MouseEventType.MouseWheel;
                case WM_MOUSEMOVE:
                    return MouseEventType.MouseMove;
                default:
                    return MouseEventType.None;

            }
        }

        #endregion
        
    }

}
