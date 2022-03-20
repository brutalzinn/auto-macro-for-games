using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseKeyPlayback.Views
{
    public partial class KeyInfoHelper : Form
    {
        public List<Keys> modifierKeys;
        public List<Keys> nonModifierKeys;
        public string keysResult { get; set; }
    
        private void CloseWithResult(DialogResult result)
        {
            DialogResult = result;
            Close();
        }

   
        public bool ListenToKeys { get; set; } = true;
        public KeyInfoHelper()
        {
            InitializeComponent();
            textBox1.Click += (sender, e) => {
                if (!ListenToKeys)
                {
                    ListenToKeys = true;
                    nonModifierKeys = new List<Keys>();
                    modifierKeys = new List<Keys>();
                    UpdateTextBox();
                }
            };
        }
        private void UpdateTextBox()
        {
            if (modifierKeys != null && nonModifierKeys != null)
            {
                string value1 = string.Join(" + ", modifierKeys.Where(c => c != Keys.None).Select(c => c.ToString()).OrderBy(c => c));
                string value2 = string.Join(" + ", nonModifierKeys.Where(c => !(c == Keys.ShiftKey || c == Keys.ControlKey || c == Keys.Menu)));
               
            keysResult = string.IsNullOrEmpty(value1) ? value2 : string.IsNullOrEmpty(value2) ? value1 : (string.Join(" + ", value1, value2));
                textBox1.Text = keysResult;
             }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!ListenToKeys) return;
            List<Keys> modifiers = new List<Keys>();
            List<Keys> nonModifiers = new List<Keys>();
            foreach (Keys r in Enum.GetValues(typeof(Keys)))
            {
                if (e.Modifiers != Keys.None && e.Modifiers.HasFlag(r))
                    modifiers.Add(r);
                else if (e.KeyCode == (r))
                    nonModifiers.Add(r);
            }
            nonModifierKeys = nonModifiers;
            modifierKeys = modifiers;
            UpdateTextBox();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            ListenToKeys = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CloseWithResult(DialogResult.OK);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CloseWithResult(DialogResult.Cancel);

        }

        private void KeyInfoHelper_Load(object sender, EventArgs e)
        {

        }
    }
}
