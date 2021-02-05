using MouseKeyboardLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MouseKeyboardLibrary.MouseHook;

namespace MouseKeyPlayback
{
    public class MouseEvent
    {
		public CursorPoint Location { get; set; }
        public MouseEventType Action { get; set; }
        public MouseButtons Button { get; set; }
        public int Value { get; set; }
    }
}
