﻿namespace GregsStack.InputSimulatorStandard
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Native;

    /// <inheritdoc />
    /// <summary>
    /// A helper class for building a list of <see cref="Input" /> messages ready to be sent to the native Windows API.
    /// </summary>
    internal class InputBuilder : IEnumerable<Input>
    {
        /// <summary>
        /// The public list of <see cref="Input"/> messages being built by this instance.
        /// </summary>
        private readonly List<Input> inputList;

        private static readonly IReadOnlyList<VirtualKeyCode> ExtendedKeys = new List<VirtualKeyCode>
        {
            VirtualKeyCode.NUMPAD_RETURN,
            VirtualKeyCode.MENU,
            VirtualKeyCode.RMENU,
            VirtualKeyCode.CONTROL,
            VirtualKeyCode.RCONTROL,
            VirtualKeyCode.INSERT,
            VirtualKeyCode.DELETE,
            VirtualKeyCode.HOME,
            VirtualKeyCode.END,
            VirtualKeyCode.PRIOR,
            VirtualKeyCode.NEXT,
            VirtualKeyCode.RIGHT,
            VirtualKeyCode.UP,
            VirtualKeyCode.LEFT,
            VirtualKeyCode.DOWN,
            VirtualKeyCode.NUMLOCK,
            VirtualKeyCode.CANCEL,
            VirtualKeyCode.SNAPSHOT,
            VirtualKeyCode.DIVIDE
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBuilder"/> class.
        /// </summary>
        public InputBuilder()
        {
            this.inputList = new List<Input>();
        }

        /// <summary>
        /// Returns the list of <see cref="Input"/> messages as a <see cref="Array"/> of <see cref="Input"/> messages.
        /// </summary>
        /// <returns>The <see cref="System.Array"/> of <see cref="Input"/> messages.</returns>
        public Input[] ToArray() => this.inputList.ToArray();

        /// <inheritdoc />
        /// <summary>
        /// Returns an enumerator that iterates through the list of <see cref="Input" /> messages.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator" /> that can be used to iterate through the list of <see cref="Input" /> messages.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Input> GetEnumerator() => this.inputList.GetEnumerator();

        /// <inheritdoc />
        /// <summary>
        /// Returns an enumerator that iterates through the list of <see cref="Input" /> messages.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator" /> object that can be used to iterate through the list of <see cref="Input" /> messages.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        /// Gets the <see cref="Input"/> at the specified position.
        /// </summary>
        /// <value>The <see cref="Input"/> message at the specified position.</value>
        public Input this[int position] => this.inputList[position];

        /// <summary>
        /// Determines if the <see cref="VirtualKeyCode"/> is an ExtendedKey
        /// </summary>
        /// <param name="keyCode">The key code.</param>
        /// <returns>true if the key code is an extended key; otherwise, false.</returns>
        /// <remarks>
        /// The extended keys consist of the ALT and CTRL keys on the right-hand side of the keyboard; the INS, DEL, HOME, END, PAGE UP, PAGE DOWN, and arrow keys in the clusters to the left of the numeric keypad; the NUM LOCK key; the BREAK (CTRL+PAUSE) key; the PRINT SCRN key; and the divide (/) and ENTER keys in the numeric keypad.
        ///
        /// See http://msdn.microsoft.com/en-us/library/ms646267(v=vs.85).aspx Section "Extended-Key Flag"
        /// </remarks>
        public static bool IsExtendedKey(VirtualKeyCode keyCode) => ExtendedKeys.Contains(keyCode);

        /// <summary>
        /// Adds a key down to the list of <see cref="Input"/> messages.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/>.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddKeyDown(VirtualKeyCode keyCode)
        {
            var down =
                new Input
                {
                    Type = (uint)InputType.Keyboard,
                    Data =
                            {
                                Keyboard =
                                    new KeyboardInput
                                        {
                                            KeyCode = (ushort) keyCode,
                                            Scan = (ushort)(NativeMethods.MapVirtualKey((uint)keyCode, 0) & 0xFFU),
                                            Flags = IsExtendedKey(keyCode) ? (uint) KeyboardFlag.ExtendedKey : 0,
                                            Time = 0,
                                            ExtraInfo = IntPtr.Zero
                                        }
                            }
                };

            this.inputList.Add(down);
            return this;
        }

        /// <summary>
        /// Adds a key up to the list of <see cref="Input"/> messages.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/>.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddKeyUp(VirtualKeyCode keyCode)
        {
            var up =
                new Input
                {
                    Type = (uint)InputType.Keyboard,
                    Data =
                            {
                                Keyboard =
                                    new KeyboardInput
                                        {
                                            KeyCode = (ushort) keyCode,
                                            Scan = (ushort)(NativeMethods.MapVirtualKey((uint)keyCode, 0) & 0xFFU),
                                            Flags = (uint) (IsExtendedKey(keyCode)
                                                                  ? KeyboardFlag.KeyUp | KeyboardFlag.ExtendedKey
                                                                  : KeyboardFlag.KeyUp),
                                            Time = 0,
                                            ExtraInfo = IntPtr.Zero
                                        }
                            }
                };

            this.inputList.Add(up);
            return this;
        }

        /// <summary>
        /// Adds a key press to the list of <see cref="Input"/> messages which is equivalent to a key down followed by a key up.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/>.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddKeyPress(VirtualKeyCode keyCode)
        {
            this.AddKeyDown(keyCode);
            this.AddKeyUp(keyCode);
            return this;
        }

        /// <summary>
        /// Adds the character to the list of <see cref="Input"/> messages.
        /// </summary>
        /// <param name="character">The <see cref="System.Char"/> to be added to the list of <see cref="Input"/> messages.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddCharacter(char character)
        {
            ushort scanCode = character;

            var down = new Input
            {
                Type = (uint)InputType.Keyboard,
                Data =
                                   {
                                       Keyboard =
                                           new KeyboardInput
                                               {
                                                   KeyCode = 0,
                                                   Scan = scanCode,
                                                   Flags = (uint)KeyboardFlag.Unicode,
                                                   Time = 0,
                                                   ExtraInfo = IntPtr.Zero
                                               }
                                   }
            };

            var up = new Input
            {
                Type = (uint)InputType.Keyboard,
                Data =
                                 {
                                     Keyboard =
                                         new KeyboardInput
                                             {
                                                 KeyCode = 0,
                                                 Scan = scanCode,
                                                 Flags =
                                                     (uint)(KeyboardFlag.KeyUp | KeyboardFlag.Unicode),
                                                 Time = 0,
                                                 ExtraInfo = IntPtr.Zero
                                             }
                                 }
            };

            // Handle extended keys:
            // If the scan code is preceded by a prefix byte that has the value 0xE0 (224),
            // we need to include the KEYEVENTF_EXTENDEDKEY flag in the Flags property.
            if ((scanCode & 0xFF00) == 0xE000)
            {
                down.Data.Keyboard.Flags |= (uint)KeyboardFlag.ExtendedKey;
                up.Data.Keyboard.Flags |= (uint)KeyboardFlag.ExtendedKey;
            }

            this.inputList.Add(down);
            this.inputList.Add(up);
            return this;
        }

        /// <summary>
        /// Adds all of the characters in the specified <see cref="IEnumerable{T}"/> of <see cref="char"/>.
        /// </summary>
        /// <param name="characters">The characters to add.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddCharacters(IEnumerable<char> characters)
        {
            foreach (var character in characters)
            {
                this.AddCharacter(character);
            }

            return this;
        }

        /// <summary>
        /// Adds the characters in the specified <see cref="string"/>.
        /// </summary>
        /// <param name="characters">The string of <see cref="char"/> to add.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddCharacters(string characters) => this.AddCharacters(characters.ToCharArray());

        /// <summary>
        /// Moves the mouse relative to its current position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddRelativeMouseMovement(int x, int y)
        {
            var movement = new Input { Type = (uint)InputType.Mouse };
            movement.Data.Mouse.Flags = (uint)MouseFlag.Move;
            movement.Data.Mouse.X = x;
            movement.Data.Mouse.Y = y;

            this.inputList.Add(movement);

            return this;
        }

        /// <summary>
        /// Move the mouse to an absolute position.
        /// </summary>
        /// <param name="absoluteX"></param>
        /// <param name="absoluteY"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddAbsoluteMouseMovement(int absoluteX, int absoluteY)
        {
            var movement = new Input { Type = (uint)InputType.Mouse };
            movement.Data.Mouse.Flags = (uint)(MouseFlag.Move | MouseFlag.Absolute);
            movement.Data.Mouse.X = absoluteX;
            movement.Data.Mouse.Y = absoluteY;

            this.inputList.Add(movement);

            return this;
        }

        /// <summary>
        /// Move the mouse to the absolute position on the virtual desktop.
        /// </summary>
        /// <param name="absoluteX"></param>
        /// <param name="absoluteY"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddAbsoluteMouseMovementOnVirtualDesktop(int absoluteX, int absoluteY)
        {
            var movement = new Input { Type = (uint)InputType.Mouse };
            movement.Data.Mouse.Flags = (uint)(MouseFlag.Move | MouseFlag.Absolute | MouseFlag.VirtualDesk);
            movement.Data.Mouse.X = absoluteX;
            movement.Data.Mouse.Y = absoluteY;

            this.inputList.Add(movement);

            return this;
        }

        /// <summary>
        /// Adds a mouse button down for the specified button.
        /// </summary>
        /// <param name="button"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseButtonDown(MouseButton button)
        {
            var buttonDown = new Input { Type = (uint)InputType.Mouse };
            buttonDown.Data.Mouse.Flags = (uint)ToMouseButtonDownFlag(button);

            this.inputList.Add(buttonDown);

            return this;
        }

        /// <summary>
        /// Adds a mouse button down for the specified button.
        /// </summary>
        /// <param name="xButtonId"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseXButtonDown(int xButtonId)
        {
            var buttonDown = new Input { Type = (uint)InputType.Mouse };
            buttonDown.Data.Mouse.Flags = (uint)MouseFlag.XDown;
            buttonDown.Data.Mouse.MouseData = (uint)xButtonId;

            this.inputList.Add(buttonDown);

            return this;
        }

        /// <summary>
        /// Adds a mouse button up for the specified button.
        /// </summary>
        /// <param name="button"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseButtonUp(MouseButton button)
        {
            var buttonUp = new Input { Type = (uint)InputType.Mouse };
            buttonUp.Data.Mouse.Flags = (uint)ToMouseButtonUpFlag(button);

            this.inputList.Add(buttonUp);

            return this;
        }

        /// <summary>
        /// Adds a mouse button up for the specified button.
        /// </summary>
        /// <param name="xButtonId"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseXButtonUp(int xButtonId)
        {
            var buttonUp = new Input { Type = (uint)InputType.Mouse };
            buttonUp.Data.Mouse.Flags = (uint)MouseFlag.XUp;
            buttonUp.Data.Mouse.MouseData = (uint)xButtonId;

            this.inputList.Add(buttonUp);

            return this;
        }

        /// <summary>
        /// Adds a single click of the specified button.
        /// </summary>
        /// <param name="button"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseButtonClick(MouseButton button) => this.AddMouseButtonDown(button).AddMouseButtonUp(button);

        /// <summary>
        /// Adds a single click of the specified button.
        /// </summary>
        /// <param name="xButtonId"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseXButtonClick(int xButtonId) => this.AddMouseXButtonDown(xButtonId).AddMouseXButtonUp(xButtonId);

        /// <summary>
        /// Adds a double click of the specified button.
        /// </summary>
        /// <param name="button"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseButtonDoubleClick(MouseButton button) => this.AddMouseButtonClick(button).AddMouseButtonClick(button);

        /// <summary>
        /// Adds a double click of the specified button.
        /// </summary>
        /// <param name="xButtonId"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseXButtonDoubleClick(int xButtonId) => this.AddMouseXButtonClick(xButtonId).AddMouseXButtonClick(xButtonId);

        /// <summary>
        /// Scroll the vertical mouse wheel by the specified amount.
        /// </summary>
        /// <param name="scrollAmount"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseVerticalWheelScroll(int scrollAmount)
        {
            var scroll = new Input { Type = (uint)InputType.Mouse };
            scroll.Data.Mouse.Flags = (uint)MouseFlag.VerticalWheel;
            scroll.Data.Mouse.MouseData = (uint)scrollAmount;

            this.inputList.Add(scroll);

            return this;
        }

        /// <summary>
        /// Scroll the horizontal mouse wheel by the specified amount.
        /// </summary>
        /// <param name="scrollAmount"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseHorizontalWheelScroll(int scrollAmount)
        {
            var scroll = new Input { Type = (uint)InputType.Mouse };
            scroll.Data.Mouse.Flags = (uint)MouseFlag.HorizontalWheel;
            scroll.Data.Mouse.MouseData = (uint)scrollAmount;

            this.inputList.Add(scroll);

            return this;
        }

        private static MouseFlag ToMouseButtonDownFlag(MouseButton button)
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

        private static MouseFlag ToMouseButtonUpFlag(MouseButton button)
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
