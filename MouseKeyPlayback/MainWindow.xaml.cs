
using MouseKeyPlayback.Library;
using MouseKeyPlayback.Utils;
using MouseKeyPlayback.Views;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;

namespace MouseKeyPlayback
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Properties
        private KeyboardHook keyboardHook = new KeyboardHook();
        private MouseHook mouseHook = new MouseHook();
        private int count = 0;
        private bool isHooked = false;
        private List<Record> recordList;
        #endregion

        private volatile bool m_StopThread = false;
        private bool NeedExit { get; set; } = false;
        public MainWindow()
        {
            InitializeComponent();
            Globals.MainWindow = this;
            ApplicationSettingsManager.LoadSettings();
            RegisterHotKeys();
            RegisterSpecialKeys();
            recordList = new List<Record>();
            ((INotifyCollectionChanged)listView.Items).CollectionChanged += ListView_CollectionChanged;

            lastInPutNfo = new LASTINPUTINFO();
            lastInPutNfo.cbSize = (uint)Marshal.SizeOf(lastInPutNfo);

            uint idleTime = 0;
            Thread thread = new Thread(new ThreadStart(delegate () {
                while (!m_StopThread)
                {
                    if (idleTime == 0)
                    {
                        if (g != null)
                        {
                            try
                            {
                                RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, RDW_INVALIDATE | RDW_ALLCHILDREN | RDW_UPDATENOW);
                                g.Dispose();
                                ReleaseDC(desktop);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                    }
                    idleTime = GetLastInputTime();
                    if (idleTime < 1)
                        drawOutlineElement();
                    Console.WriteLine(idleTime);
                    Thread.Sleep(1000);
                }
                //if(GetLastInputTime() > 1000)

            }));
            //thread.Start();

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            m_StopThread = true;
        }
        public void RegisterHotKeys()
        {
            if(ApplicationSettingsManager.Settings.HotKeyStartRecord != null)
            {
            GlobalHotKey.RegisterHotKey(ApplicationSettingsManager.Settings.HotKeyStartRecord, StartRecord);

            }
            if (ApplicationSettingsManager.Settings.HotKeyStopRecord != null)
            {
                GlobalHotKey.RegisterHotKey(ApplicationSettingsManager.Settings.HotKeyStopRecord, StopRecord);

            }
            if (ApplicationSettingsManager.Settings.HotKeyPlay != null)
            {
                GlobalHotKey.RegisterHotKey(ApplicationSettingsManager.Settings.HotKeyPlay, Play);

            }
            if (ApplicationSettingsManager.Settings.HotKeyStopMacro != null)
            {
                GlobalHotKey.RegisterHotKey(ApplicationSettingsManager.Settings.HotKeyStartRecord, StopMacro);

            }
        }
        private void StopMacro()
        {
            keyboardHook.Uninstall();
            mouseHook.Uninstall();
            isHooked = false;
            NeedExit = true;
        }
        private void ListView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Scroll to the last item
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                listView.ScrollIntoView(e.NewItems[0]);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //// Setup keyboard hook
            //keyboardHook.OnKeyboardEvent += KeyboardHook_OnKeyboardEvent;
            //keyboardHook.Install();

            //// Setup mouse hook
            //mouseHook.OnMouseEvent += MouseHook_OnMouseEvent;
            //mouseHook.OnMouseMove += MouseHook_OnMouseMove;
            //mouseHook.OnMouseWheelEvent += MouseHook_OnMouseWheelEvent;
            //mouseHook.Install();
            keyboardHook.OnKeyboardEvent += KeyboardHook_OnKeyboardEvent;

            mouseHook.OnMouseEvent += MouseHook_OnMouseEvent;
            mouseHook.OnMouseMove += MouseHook_OnMouseMove;
            mouseHook.OnMouseWheelEvent += MouseHook_OnMouseWheelEvent;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            keyboardHook.Uninstall();
            mouseHook.Uninstall();
        }

        #region Mouse events
        private void ProcessMouseEvent(MouseHook.MouseEvents mAction, int mValue)
        {
            CursorPoint mPoint = GetCurrentMousePosition();
            MouseEvent mEvent = new MouseEvent
            {
                Location = mPoint,
                Action = mAction,
                Value = mValue
            };

            LogMouseEvents(mEvent);
        }

        private bool MouseHook_OnMouseWheelEvent(int wheelValue)
        {
            ProcessMouseEvent((MouseHook.MouseEvents)wheelValue, 120);
            return false;
        }

        private bool MouseHook_OnMouseEvent(int mouseEvent)
        {
            ProcessMouseEvent((MouseHook.MouseEvents)mouseEvent, 0);
            return false;
        }


        private bool MouseHook_OnMouseMove(int x, int y)
        {
            ProcessMouseEvent(MouseHook.MouseEvents.MouseMove, 0);
            return false;
        }
        #endregion

        #region Keyboard events
        private bool KeyboardHook_OnKeyboardEvent(uint key, BaseHook.KeyState keyState)
        {
            KeyboardEvent kEvent = new KeyboardEvent {
                Key = (Keys)key,
                Action = (keyState == BaseHook.KeyState.Keydown) ? Constants.KEY_DOWN : Constants.KEY_UP
            };
            LogKeyboardEvents(kEvent);
            return false;
        }
        #endregion

        #region Record/Stop

        private void StartRecord()
        {
            if (isHooked)
                return;
            if (listView.Items.Count > 0)
            {
                //MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to record again?",
                //                          "Confirmation",
                //                          MessageBoxButton.YesNo,
                //                          MessageBoxImage.Question);
                //switch (result)
                //{
                //    case MessageBoxResult.Yes:
                //        listView.Items.Clear();
                //        recordList = new List<Record>();
                //        count = 0;
                //        break;
                //    default:
                //        return;
                //}

                listView.Items.Clear();
                recordList = new List<Record>();
                count = 0;
            }

            keyboardHook.Install();
            mouseHook.Install();
            isHooked = true;

            LaunchApp();
        }
        private void BtnRecord_Click(object sender, RoutedEventArgs e)
        {
            StartRecord();
        }
        private void StopRecord()
        {
            keyboardHook.Uninstall();
            mouseHook.Uninstall();
            isHooked = false;
        }
        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            StopRecord();
        }
        #endregion

        #region Helper + Logging methods

        private void LaunchApp()
        {
            // An app is supposed to launch
            if (appPath.IsEnabled == false)
            {
                System.Diagnostics.Process.Start(appPath.Text);
            }
        }

        [DllImport("User32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll", CallingConvention = CallingConvention.FastCall)]
        static extern void ReleaseDC(IntPtr dc);

        IntPtr desktop;

        private void TrackAutomationElement(Record item)
        {
            if (item.Type == Constants.MOUSE
                && item.EventMouse.Action == MouseHook.MouseEvents.LeftUp)
            {
                var windowTitle = Win32Utils.GetActiveWindowTitle();
                var position = Control.MousePosition;

                Point coordinates = new Point(position.X, position.Y);
                inspectBox.Text = string.Format("Title: {0}", windowTitle);

                try
                {
                    AutomationElement targetApp = AutomationElement.FromPoint(coordinates);

                    inspectBox.Text += "\n";
                    inspectBox.Text += string.Format("" +
                        "Name: {0}\n" +
                        "Automation Id: {1}\n" +
                        "Text: {2}\n" +
                        "Control Type: {3}",
                        targetApp.Current.Name,
                        targetApp.Current.AutomationId,
                        GetText(targetApp),
                        targetApp.Current.ControlType);

                    //desktop = GetDC(IntPtr.Zero);
                    //using (System.Drawing.Graphics g = System.Drawing.Graphics.FromHdc(desktop))
                    //{
                    //	System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Red, 5);
                    //	Rect rect = targetApp.Current.BoundingRectangle;
                    //	Point point = rect.TopLeft;
                    //	g.DrawRectangle(pen, (float)point.X, (float)point.Y, (float)rect.Width, (float)rect.Height);
                    //}
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid UI element");
                }
            }
        }

        private void drawOutlineElement()
        {
            try
            {
                var position = Control.MousePosition;

                Point coordinates = new Point(position.X, position.Y);
                AutomationElement targetApp = AutomationElement.FromPoint(coordinates);

                Console.WriteLine(targetApp.Current.Name);
                string appName = targetApp.Current.Name;
                List<string> forbiden = new List<string>
                {
                    "",
                    "0",
                    "App Recorder"
                };
                foreach (string s in forbiden)
                {
                    if (appName.Equals(s))
                        return;
                }

                desktop = GetDC(IntPtr.Zero);
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromHdc(desktop))
                {
                    System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Red, 5);
                    Rect rect = targetApp.Current.BoundingRectangle;
                    Point point = rect.TopLeft;
                    g.DrawRectangle(pen, (float)point.X, (float)point.Y, (float)rect.Width, (float)rect.Height);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid UI element");
            }
        }

        private string GetText(AutomationElement element)
        {
            object patternObj;
            if (element.TryGetCurrentPattern(ValuePattern.Pattern, out patternObj))
            {
                var valuePattern = (ValuePattern)patternObj;
                return valuePattern.Current.Value;
            }
            else if (element.TryGetCurrentPattern(TextPattern.Pattern, out patternObj))
            {
                var textPattern = (TextPattern)patternObj;
                return textPattern.DocumentRange.GetText(-1).TrimEnd('\r'); // often there is an extra '\r' hanging off the end.
            }
            else
            {
                return element.Current.Name;
            }
        }

        private CursorPoint GetCurrentMousePosition()
        {
            var position = Control.MousePosition;
            return new CursorPoint(position.X, position.Y);
        }

        private void LogMouseEvents(MouseEvent mEvent)
        {
            count++;
            Record item = new Record
            {
                Id = count,
                EventMouse = mEvent,
                Type = Constants.MOUSE,
                Content = String.Format("{0} was triggered at ({1}, {2})", mEvent.Action, mEvent.Location.X, mEvent.Location.Y)
            };
        
            AddRecordItem(item);
         
        }
   
            

        

        
            
            
        

    

            
        

        private void LogKeyboardEvents(KeyboardEvent kEvent)
        {
            count++;
            Record item = new Record
            {
                Id = count,
                Type = Constants.KEYBOARD,
                EventKey = kEvent,
                Content = String.Format("{0} was {1}", kEvent.Key.ToString(),
                    (kEvent.Action == Constants.KEY_DOWN) ? "pressed" : "released")
            };
            if (CheckBoxRandom.IsChecked.Value)
            {
                Record itemWait = new Record
                {
                    Id = count,

                    WaitMaxMs = ApplicationSettingsManager.Settings.maxRamdomConfig,
                    WaitMinMs = ApplicationSettingsManager.Settings.minRamdomConfig,
                    Type = Constants.WAITRandom,
                    Content = $"Wait {ApplicationSettingsManager.Settings.maxRamdomConfig} MAX Random ms. \n Wait {ApplicationSettingsManager.Settings.minRamdomConfig} Min Random ms "
                };
                if (ApplicationSettingsManager.Settings.BetweenKeys)
                {
                    AddRecordItem(itemWait);
                    AddRecordItem(item);

                }
                else if (ApplicationSettingsManager.Settings.ForeachKeys)
                {
                    AddRecordItem(item);
                    AddRecordItem(itemWait);
                }


            }
          
        else if(CheckBoxDelay.IsChecked.Value)
            {
                Record itemWait = new Record
                {
                    Id = count,

                    WaitMs = Convert.ToInt32(TextBoxDelayTime.Text),
                    Type = Constants.WAIT,
                    Content = $"Wait {Convert.ToInt32(TextBoxDelayTime.Text)} "
                };
                if (ApplicationSettingsManager.Settings.BetweenKeys)
                {
                    AddRecordItem(itemWait);
                    AddRecordItem(item);

                }
                else if (ApplicationSettingsManager.Settings.ForeachKeys)
                {
                    AddRecordItem(item);
                    AddRecordItem(itemWait);
                }
            }
            else
            {
                AddRecordItem(item);
            }
        }

   

        private void LogWaitEvent(Record record)
		{
			count++;
			record.Id = count;
            if (record.Type == Constants.WAIT)
            {
                record.Content = $"Wait {record.WaitMs} MS";

            }
            else
            {
                record.Content = $"Wait {record.WaitMaxMs} MAX Random ms. \n Wait {record.WaitMinMs} Min Random ms ";

            }
            AddRecordItem(record);
		}

        private void AddRecordItem(Record item)
        {
            
            TrackAutomationElement(item);

            AddToListView(item);
            //this.listView.Items.Add(item);
            this.recordList.Add(item);
            countRecord.Content = String.Format("{0} records", count.ToString());
        }

        private void RemoveRecordItem(Record item)
        {

        
           
                switch (item.Type)
                {
                    case Constants.MOUSE:
                        Debug.WriteLine(item.EventMouse.Value);
                   
                       
                         recordList.RemoveAll(x => x.Group == item.Group);

                        break;
                    case Constants.KEYBOARD:
                    recordList.Remove(item);
                        break;
                    case Constants.WAITRandom:
                    recordList.Remove(item);

                    break;
                }
            

            countRecord.Content = String.Format("{0} records", count.ToString());
        }
     
        private void AddToListView(Record item)
        {
            // Check if two last records are similar
            if (listView.Items.Count > 0)
            {
                var lastItem = (Record)listView.Items[listView.Items.Count - 1];
                if (lastItem.Type == item.Type)
                {
                    switch (item.Type)
                    {
                        case Constants.MOUSE:
                            var lastAction = lastItem.EventMouse.Action;
                            if (lastAction == MouseHook.MouseEvents.MouseMove
                                && item.EventMouse.Action == lastAction)
                                
                                this.listView.Items.RemoveAt(this.listView.Items.Count - 1);
                            item.Group = this.listView.Items.Count - 1;
                            break;
                        case Constants.KEYBOARD:
                            break;
                        case Constants.WAITRandom:
                          
                            break;
                    }
                }
            }

            // satisfy every condition
            this.listView.Items.Add(item);
        }
        #endregion

        #region Playback
        private void Play()
        {
            if (isHooked)
                return;

            int num;
            if (int.TryParse(repeatTime.Text, out num))
            {
                for (int i = 0; i < num; ++i)
                {
                    LaunchApp();
                  
                    foreach (var record in recordList)
                    {
                       
                        switch (record.Type)
                        {
                            case Constants.MOUSE:
                                PlaybackMouse(record);
                                break;
                            case Constants.KEYBOARD:
                                PlaybackKeyboard(record);
                                break;
                            case Constants.WAIT:
                                Thread.Sleep(record.WaitMs);
                                break;

                            case Constants.WAITRandom:
                                Random number = new Random();


                                Thread.Sleep(number.Next(record.WaitMinMs, record.WaitMaxMs));
                                break;
                            default:
                                break;
                        }
                      
                        Thread.Sleep(4);
                    }

                    Thread.Sleep(10);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Repeat time is not valid!");
            }
           

        }
        private void BtnPlayback_Click(object sender, RoutedEventArgs e)
        {
            Play();
        }

        private void PlaybackMouse(Record record)
        {
            CursorPoint newPos = record.EventMouse.Location;
            MouseHook.MouseEvents mEvent = record.EventMouse.Action;
            MouseUtils.PerformMouseEvent(mEvent, newPos);
        }

        private void PlaybackKeyboard(Record record)
        {
            Keys key = record.EventKey.Key;
            string action = record.EventKey.Action;

            KeyboardUtils.PerformKeyEvent(key, action);
        }
        #endregion

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.FileName = "";
            openDialog.Title = "Application";
            openDialog.Filter = "All applications|*.exe";
            openDialog.ShowDialog();

            var appName = openDialog.FileName.ToString();
            if (!String.IsNullOrEmpty(appName))
            {
                appPath.Text = appName;
                appPath.IsEnabled = false;
            } 
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            appPath.Clear();
            appPath.IsEnabled = true;
        }

		private void BtnCreateClick_Click(object sender, RoutedEventArgs e)
		{
			var window = new CreateManualClickWindow();
			window.ShowDialog();

			if(window.mouseEvents != null)
			{
				window.mouseEvents.ForEach(me => LogMouseEvents(me));
			}
		}
        public static Dictionary<char, Keys> specialkeys = new Dictionary<char, Keys>();
        
        private void RegisterSpecialKeys()
        {

            specialkeys.Add('Á', Keys.OemOpenBrackets);
            specialkeys.Add('Ó', Keys.OemOpenBrackets);
            specialkeys.Add('É', Keys.OemOpenBrackets);

            specialkeys.Add(';', Keys.OemSemicolon);
            specialkeys.Add('Ã', Keys.Oem7);
            specialkeys.Add('Ñ', Keys.Oem7);
            specialkeys.Add(' ', Keys.Space);
            specialkeys.Add('+', Keys.Oemplus);
            specialkeys.Add('?', Keys.Oemtilde);
            specialkeys.Add('\'', Keys.OemQuotes);
            specialkeys.Add('"', Keys.OemQuotes);
            specialkeys.Add('|', Keys.OemPipe);
            specialkeys.Add('.', Keys.OemPeriod);
            specialkeys.Add(',', Keys.Oemcomma);
            specialkeys.Add('Õ', Keys.Oem7);






        }
        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        private static Keys getKeySpecial(char value)
        {
            Keys result = Keys.None;
            foreach(var item in specialkeys)
            {
                if(item.Key == value)
                {
                    result = item.Value;
                }
               
            }
            return result;
        }
        private void BtnCreateText_Click(object sender, RoutedEventArgs e)
		{
			var window = new CreateManualTypeKeyWindow();
			window.ShowDialog();

			string text = window.text;
			if(text != null)
			{
				text = text.ToUpper();
				foreach(char c in text)
				{
					var code = c;
                    Keys key = Keys.None;
                    try
                    {
					 key = (Keys)Enum.Parse(typeof(Keys), code.ToString());

                    }
                    catch (Exception )
                    {

                        Debug.WriteLine("code:" + code + "key: " + getKeySpecial(code));
                            Keys key_especial = getKeySpecial(code);
                        var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(code);
                        if(unicodeCategory != UnicodeCategory.OtherPunctuation)
                        {
                        key = (Keys)Enum.Parse(typeof(Keys), RemoveDiacritics(code.ToString()));

                        }
LogKeyboardEvents(new KeyboardEvent { Key = key_especial, Action = Constants.KEY_DOWN });

                            LogKeyboardEvents(new KeyboardEvent { Key = key_especial, Action = Constants.KEY_UP });

                    }
                            
                        
                    
                    
					LogKeyboardEvents(new KeyboardEvent { Key = key, Action = Constants.KEY_DOWN });
				
                    LogKeyboardEvents(new KeyboardEvent { Key = key, Action = Constants.KEY_UP });
               
                }
              
            }
		}

		const int RDW_INVALIDATE = 0x0001;
		const int RDW_ALLCHILDREN = 0x0080;
		const int RDW_UPDATENOW = 0x0100;
		[DllImport("User32.dll", CallingConvention = CallingConvention.StdCall)]
		static extern bool RedrawWindow(IntPtr hwnd, IntPtr rcUpdate, IntPtr regionUpdate, int flags);		

		System.Drawing.Graphics g;
		private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			var item = listView.SelectedItem as Record;
            if(item == null)
                {
                return;
            }

          if (!listView.HasItems  ||  item.EventMouse == null  )
				return;
			try
			{	
              
				if(g != null)
				{
					RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, RDW_INVALIDATE | RDW_ALLCHILDREN | RDW_UPDATENOW);
					g.Dispose();
					ReleaseDC(desktop);
				}				
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
			}
			
			int id = item.Id;
			System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Red, 3);

			if (item.EventMouse.Action == MouseHook.MouseEvents.MouseMove)
			{
				Record last = recordList.FindLast(r => 
				{
					if (r.EventMouse == null)
						return false;
					return r.Id < id && r.EventMouse.Action != MouseHook.MouseEvents.MouseMove;
				});
				if (last == null)
				{
					last = recordList[0];
				}
				List<Record> list = recordList.FindAll(r => r.Id <= id && r.Id > last.Id);

				desktop = GetDC(IntPtr.Zero);
				g = System.Drawing.Graphics.FromHdc(desktop);				
				System.Drawing.Point[] points = list.ConvertAll(new Converter<Record, System.Drawing.Point>(RecordToPoint)).ToArray();
				System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
				path.AddLines(points);
				g.DrawPath(pen, path);
				//g.Clear(System.Drawing.Color.Transparent);
			} else if(item.Type == Constants.MOUSE)
			{
				int lengthLine = 40;
				desktop = GetDC(IntPtr.Zero);
				g = System.Drawing.Graphics.FromHdc(desktop);
				System.Drawing.Point point1 = new System.Drawing.Point(
					(int)item.EventMouse.Location.X, (int)item.EventMouse.Location.Y - lengthLine);
				System.Drawing.Point point2 = new System.Drawing.Point(
					(int)item.EventMouse.Location.X, (int)item.EventMouse.Location.Y + lengthLine);
				g.DrawLine(pen, point1, point2);

				System.Drawing.Point point3 = new System.Drawing.Point(
					(int)item.EventMouse.Location.X - lengthLine, (int)item.EventMouse.Location.Y);
				System.Drawing.Point point4 = new System.Drawing.Point(
					(int)item.EventMouse.Location.X + lengthLine, (int)item.EventMouse.Location.Y);
				
				g.DrawLine(pen, point3, point4);
			}
		}

		private struct LASTINPUTINFO
		{
			public uint cbSize;
			public uint dwTime;
		}

		private static LASTINPUTINFO lastInPutNfo;
		[DllImport("user32.dll")]
		static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

		public static uint GetLastInputTime()
		{
			uint idleTime = 0;
			LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
			lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
			lastInputInfo.dwTime = 0;

			uint envTicks = (uint)Environment.TickCount;

			if (GetLastInputInfo(ref lastInputInfo))
			{
				uint lastInputTick = lastInputInfo.dwTime;

				idleTime = envTicks - lastInputTick;
			}

			return ((idleTime > 0) ? (idleTime / 1000) : 0);
		}

		private System.Drawing.Point RecordToPoint(Record r)
		{
			return new System.Drawing.Point((int)r.EventMouse.Location.X, (int)r.EventMouse.Location.Y);
		}

		private void ListView_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
			try
			{
				if (g != null)
				{
					RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, RDW_INVALIDATE | RDW_ALLCHILDREN | RDW_UPDATENOW);
					g.Dispose();
					ReleaseDC(desktop);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		private void BtnInsertKey_Click(object sender, RoutedEventArgs e)
		{
			CreateInsertKeyWindow window = new CreateInsertKeyWindow();
			window.ShowDialog();

			if (window.keyboardEvents != null)
			{
				window.keyboardEvents.ForEach(me => LogKeyboardEvents(me));
			}

		}
        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            var item = listView.SelectedItem as Record;
          
            try
            {
                this.listView.Items.Remove(item);
                RemoveRecordItem(item);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void BtnWait_Click(object sender, RoutedEventArgs e)
		{
			CreateWaitWindow window = new CreateWaitWindow();
			window.ShowDialog();

			Record record = window.waitEvent;
			if (record != null)
			{
				LogWaitEvent(record);
			}
		}
        void Move(int shift)
        {
            var item = listView.SelectedItem as Record;
            int p = listView.SelectedIndex;

            listView.Items.RemoveAt(p);
            recordList.Remove(item);
            listView.Items.Insert(p + shift, item);
            recordList.Insert(p + shift, item);
            listView.SelectedItem = item;
        }
        private void ToUpItem(object sender, RoutedEventArgs e)
        {
            if(listView.SelectedIndex > 0)
            {
Move(-1);
            }
            

        }

        private void ToDownItem(object sender, RoutedEventArgs e)
        {
            int p = listView.SelectedIndex;
            if(p >= 0 && p < listView.Items.Count - 1)
            {
            Move(1);

            }

        }
        private void LogWaitEventUpdate(Record record)
        {
            
            record.Id = count;
            if(record.Type == Constants.WAIT)
            {
                record.Content = $"Wait {record.WaitMs} MS";

            }
            else
            {
            record.Content = $"Wait {record.WaitMaxMs} MAX Random ms. \n Wait {record.WaitMinMs} Min Random ms ";

            }
            foreach (Record itemRecord in listView.Items)
            {
                if (itemRecord.Id == count)
                {

                    var itemListView = listView.SelectedItem as Record;
                    int p = listView.SelectedIndex;

                    listView.Items.RemoveAt(p);
                    listView.Items.Insert(p, record);
                    listView.SelectedItem = record;
                    listView.Items.Refresh();
                    break;
                }

            }
        }
        private void LogKeyboardEventsUpdate(KeyboardEvent kEvent)
        {

            Record item = new Record
            {
                Id = count,
                Type = Constants.KEYBOARD,
                EventKey = kEvent,
                Content = String.Format("{0} was {1}", kEvent.Key.ToString(),
                  (kEvent.Action == Constants.KEY_DOWN) ? "pressed" : "released")
            };
            foreach (Record itemRecord in listView.Items)
            {
                if (itemRecord.Id == count)
                {

                    var itemListView = listView.SelectedItem as Record;
                    int p = listView.SelectedIndex;

                    listView.Items.RemoveAt(p);
                    listView.Items.Insert(p, item);
                    listView.SelectedItem = item;
                    listView.Items.Refresh();
                    break;
                }

            }

        }
   
        private void LogMouseEventsUpdate(MouseEvent mEvent)
        {
           
            Record item = new Record
            {
                Id = count,
                EventMouse = mEvent,
                Type = Constants.MOUSE,
                Content = String.Format("{0} was triggered at ({1}, {2})", mEvent.Action, mEvent.Location.X, mEvent.Location.Y)
            };
            foreach (Record itemRecord in listView.Items)
            {
                if (itemRecord.Id == count)
                {

                    var itemListView = listView.SelectedItem as Record;
                    int p = listView.SelectedIndex;

                    listView.Items.RemoveAt(p);
                    listView.Items.Insert(p, item);
                    listView.SelectedItem = item;
                    listView.Items.Refresh();
                    break;
                }

            }


        }
        private void Config(object sender, RoutedEventArgs e)
        {
            var item = listView.SelectedItem as Record;
            switch (item.Type)
            {
                case Constants.MOUSE:

                    CreateManualClickWindow MouseWindow = new CreateManualClickWindow();
                    MouseWindow.mouseRecordEvent = item;

                    MouseWindow.ShowDialog();
                    if (MouseWindow.mouseEvents != null)
                    {
                        MouseWindow.mouseEvents.ForEach(me => LogMouseEventsUpdate(me));
                    }

                    break;
                case Constants.KEYBOARD:
                    CreateInsertKeyWindow keyboardwindow = new CreateInsertKeyWindow();
                    keyboardwindow.keyboardConfig = item;
                    keyboardwindow.ShowDialog();
                    if (keyboardwindow.keyboardEvents != null)
                    {
                        keyboardwindow.keyboardEvents.ForEach(me => LogKeyboardEventsUpdate(me));
                    }
                    break;
                case Constants.WAIT:
                    CreateWaitWindow window = new CreateWaitWindow(); 
                    window.waitEvent = item;  
                    

   window.ShowDialog();
                    Record record = window.waitEvent;
                    if (record != null)
                    {
                        LogWaitEventUpdate(record);
                    }


                    break;
                case Constants.WAITRandom:
                    CreateWaitWindow windowRandom = new CreateWaitWindow();
 
                windowRandom.waitEvent = item; 

                    windowRandom.ShowDialog();
                    Record recordRandom = windowRandom.waitEvent;
                    if (recordRandom != null)
                    {
                        LogWaitEventUpdate(recordRandom);
                    }

                    break;


                default:
                    break;
            }
        }

        private void OpenConfig(object sender, RoutedEventArgs e)
        {
            ConfigPage Window = new ConfigPage();
            Window.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.listView.Items.Refresh();

        }

        private void ExportButton(object sender, RoutedEventArgs e)
        {


            Microsoft.Win32.SaveFileDialog openFileDlg = new Microsoft.Win32.SaveFileDialog();
            openFileDlg.Filter = "xml documents(.xml)| *.xml";
            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true) { 

                AppExportUtils.Settings = new AppExportSettings();
            AppExportUtils.Settings.recordList = recordList ;

                AppExportUtils.SaveSettings(openFileDlg.FileName);
            }

        }

        private void ButtonImport(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            // Launch OpenFileDialog by calling ShowDialog method
            openFileDlg.Filter = "xml documents(.xml)| *.xml";

            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
               
                AppExportUtils.LoadSettings(openFileDlg.FileName);
                
                foreach(Record item in AppExportUtils.Settings.recordList)
                {
                    AddRecordItem(item);
                }
            }
        }

        private void ClearAll(object sender, RoutedEventArgs e)
        {
            this.recordList.Clear();
            this.listView.Items.Clear();
            
        }
    }
}
