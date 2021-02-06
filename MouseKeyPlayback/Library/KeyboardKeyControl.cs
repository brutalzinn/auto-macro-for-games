using MouseKeyboardLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseKeyPlayback.Library
{
   public class KeyboardKeyControl
    {

        public Keys LastKey { get; set; }
        public KeyboardHook.KeyState LastKeyState { get; set; }


    }
}
