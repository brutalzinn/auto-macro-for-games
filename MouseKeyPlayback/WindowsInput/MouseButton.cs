using WindowsInput.Native;

namespace WindowsInput
{
    /// <summary>
    /// Mouse Button Enum
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        /// Left Button
        /// </summary>
        LeftButton,
        /// <summary>
        /// Middle Button
        /// </summary>
        MiddleButton,
        /// <summary>
        /// Right Button
        /// </summary>
        RightButton,
    }
    /// <summary>
    /// 
    /// </summary>
    public static class MouseButtonExtensions
    {
        internal static MouseFlag ToMouseButtonDownFlag(this MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LeftButton:
                    return MouseFlag.LeftDown;

                case MouseButton.MiddleButton:
                    return MouseFlag.MiddleDown;

                case MouseButton.RightButton:
                    return MouseFlag.RightDown;

                default:
                    return MouseFlag.LeftDown;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static MouseFlag ToMouseButtonUpFlag(this MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LeftButton:
                    return MouseFlag.LeftUp;

                case MouseButton.MiddleButton:
                    return MouseFlag.MiddleUp;

                case MouseButton.RightButton:
                    return MouseFlag.RightUp;

                default:
                    return MouseFlag.LeftUp;
            }
        }
    }
}