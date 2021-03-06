﻿using MouseKeyboardLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseKeyPlayback.Views
{
    public partial class KeyInfoHelperAdvaced : Form
    {
      public  List<Keys> modifierKeys { get; set; } = new List<Keys>();
       public List<Keys> nonModifierKeys { get; set; } = new List<Keys>();


        public string keysResult { get; set; }
        private KeyboardHook keyboardHook = new KeyboardHook();

        private void CloseWithResult(DialogResult result)
        {
            DialogResult = result;
            Close();
        }

   
        public bool ListenToKeys { get; set; } = true;
        public KeyInfoHelperAdvaced()
        {
            InitializeComponent();
            keyboardHook.KeyUp += KeyBoardKeyUp;
            keyboardHook.KeyDown += KeyBoardKeyDown;
            keyboardHook.Start();
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

        private void KeyBoardKeyDown(object sender, KeyEventArgs e)
        {
            KeyboardEvent kEvent = new KeyboardEvent
            {
                Key = e.KeyCode,
                Action = GlobalHook.KeyState.Keydown
            };
            LogKeyboardEvents(kEvent);
        }

        private void KeyBoardKeyUp(object sender, KeyEventArgs e)
        {
            KeyboardEvent kEvent = new KeyboardEvent
            {
                Key = e.KeyCode,
                Action = GlobalHook.KeyState.Keyup
            };
            LogKeyboardEvents(kEvent);
        }

    
        private bool KeyboardHook_OnKeyboardEventtt(uint key, GlobalHook.KeyState keyState)
        {
            KeyboardEvent kEvent = new KeyboardEvent
            {
                Key = (Keys)key,
                Action = keyState
            };
            LogKeyboardEvents(kEvent);
            return false;
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
        private void LogKeyboardEvents(KeyboardEvent kEvent)
        {
           if (!ListenToKeys) return;

            Debug.WriteLine(kEvent.Action);
            if (kEvent.Action == GlobalHook.KeyState.Keydown)
            {
                if (kEvent.Key.HasFlag(Control.ModifierKeys))
                {
                    modifierKeys.Add(kEvent.Key);
                }else
                {

                
                    nonModifierKeys.Add(kEvent.Key);
                }
            
                UpdateTextBox();
            }
        
        }

            private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

           
          
          
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
