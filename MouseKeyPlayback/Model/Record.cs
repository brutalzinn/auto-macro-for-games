﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseKeyPlayback
{
    public class Record
    {
     
        public int Id { get; set; }
        public string Content { get; set; }
        public MouseEvent EventMouse { get; set; }
        public KeyboardEvent EventKey { get; set; }
        public int Group { get; set; }
        public string ElementName { get; set; }
        public string Type { get; set; }
		public int WaitMs { get; set; }
        public int TimeSinceLastEvent { get; set; }
        public int WaitMaxMs { get; set; }
        public int WaitMinMs { get; set; }
    }
}
