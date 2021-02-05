using System.Windows.Forms;
using static MouseKeyboardLibrary.GlobalHook;

namespace MouseKeyPlayback
{
    public class KeyboardEvent
    {
        public Keys Key { get; set; }
        public KeyState Action { get; set; }
    }
}
