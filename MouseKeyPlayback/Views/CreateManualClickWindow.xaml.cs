using MouseKeyboardLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static MouseKeyboardLibrary.MouseHook;
using static MouseKeyPlayback.MouseHook;

namespace MouseKeyPlayback.Views
{
	/// <summary>
	/// Interaction logic for CreateManualClickWindow.xaml
	/// </summary>
	public partial class CreateManualClickWindow : Window
	{
		private MouseHook mouseHook = new MouseHook();
		public Record mouseRecordEvent { get; set; }
		public List<MouseEvent> mouseEvents { get; set; }
		private double x;
		private double y;

		public CreateManualClickWindow()
		{
			InitializeComponent();

			mouseHook.Start();
			mouseHook.MouseMove += getMousePositionEvent;

			System.Windows.Application curApp = System.Windows.Application.Current;
			Window mainWindow = curApp.MainWindow;
			this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
			this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;
		}



        protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			mouseHook.Stop();
		}

		private void CbxMouseButton_Initialized(object sender, EventArgs e)
		{
			foreach (MouseKeys key in Enum.GetValues(typeof(MouseKeys)))
			{
				cbxMouseButton.Items.Add(key);
			}
		}

		private void CbxMouseAction_Initialized(object sender, EventArgs e)
		{
			foreach (MouseEventType action in Enum.GetValues(typeof(MouseEventType)))
			{
				cbxMouseAction.Items.Add(action);
			}
		}

		private void BtnOk_Click(object sender, RoutedEventArgs e)
		{
			int offset = 0;
			var mouseButton = (MouseKeys)cbxMouseButton.SelectedItem;
			var mouseAction = (MouseAction)cbxMouseAction.SelectedItem;

			if (mouseButton == MouseKeys.Left)
			{
				offset = 0;
			}
			else if(mouseButton == MouseKeys.Right)
			{
				offset = 3;
			}
			else if (mouseButton == MouseKeys.Middle)
			{
				offset = 6;
			}

			CursorPoint point = new CursorPoint(x, y);
			switch (mouseAction)
			{
				case MouseActions.Click:
					mouseEvents = new List<MouseEvent>
					{
						new MouseEvent
						{
							Location = point,
							Action = MouseEvents.LeftDown + offset
						},
						new MouseEvent
						{
							Location = point,
							Action = MouseEvents.LeftUp + offset
						}
					};
					break;
				case MouseActions.DoubleClick:
					mouseEvents = new List<MouseEvent>
					{
						new MouseEvent
						{
							Location = point,
							Action = MouseEvents.LeftDown + offset
						},
						new MouseEvent
						{
							Location = point,
							Action = MouseEvents.LeftUp + offset
						},
						new MouseEvent
						{
							Location = point,
							Action = MouseEvents.LeftDown + offset
						},
						new MouseEvent
						{
							Location = point,
							Action = MouseEvents.LeftUp + offset
						}
					};
					break;
				case MouseActions.Up:
					mouseEvents = new List<MouseEvent>
					{
						new MouseEvent
						{
							Location = point,
							Action = MouseEvents.LeftUp + offset
						}
					};
					break;
				case MouseActions.Down:
					mouseEvents = new List<MouseEvent>
					{
						new MouseEvent
						{
							Location = point,
							Action = MouseEvents.LeftDown + offset
						}
					};
					break;
			}
			this.Close();
		}

		private void BtnCancel_Click(object sender, RoutedEventArgs e)
		{
			this.Close();			
		}

		private bool getMousePosition(int mouseEvent)
		{
			if(!this.IsMouseOver && (MouseHook.MouseEvents)mouseEvent == MouseHook.MouseEvents.LeftDown)
			{
				System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
				x = point.X;
				y = point.Y;
				tbxX.Text = point.X.ToString();
				tbxY.Text = point.Y.ToString();
			}
			return false;
		}
		private void getMousePositionEvent(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (!this.IsMouseOver && (MouseHook.MouseEvents)e.Button == MouseHook.MouseEvents.LeftDown)
			{
				System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
				x = point.X;
				y = point.Y;
				tbxX.Text = point.X.ToString();
				tbxY.Text = point.Y.ToString();
			}
			
		}
		private void loadCbxMouseButton(MouseKeys value)
		{

			foreach (MouseKeys name in cbxMouseButton.Items)
			{
				if (name == value)
				{

					cbxMouseButton.SelectedItem = name;
				}
			}
		}
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (mouseRecordEvent != null)
			{
				var mouseButton = (MouseKeys)mouseRecordEvent.EventMouse.Action;
				var mouseAction = (MouseActions)mouseRecordEvent.EventMouse.Action ;


                switch (mouseRecordEvent.EventMouse.Action)
                {
					case MouseEvents.LeftDown:

						cbxMouseButton.SelectedItem = MouseKeys.Left;
						cbxMouseAction.SelectedItem = MouseActions.Down;
						Debug.WriteLine("LEFT BUTTON CLICK");
						break;
					case MouseEvents.LeftUp:
						cbxMouseButton.SelectedItem = MouseKeys.Left;
						cbxMouseAction.SelectedItem = MouseActions.Up;
						Debug.WriteLine("LEFT BUTTON CLICK");
							
                        break;
					case MouseEvents.RightUp:
						cbxMouseButton.SelectedItem = MouseKeys.Right;
						cbxMouseAction.SelectedItem = MouseActions.Up;
						Debug.WriteLine("RIGHT BUTTON CLICK");
						break;
					case MouseEvents.RightDown:
						cbxMouseButton.SelectedItem = MouseKeys.Right;
						cbxMouseAction.SelectedItem = MouseActions.Down;
						Debug.WriteLine("RIGHT BUTTON CLICK");

						break;
					case MouseEvents.MiddleUp:
						cbxMouseButton.SelectedItem = MouseKeys.Middle;
						cbxMouseAction.SelectedItem = MouseActions.Up;
						Debug.WriteLine("Middle BUTTON CLICK");
						break;
					case MouseEvents.MiddleDown:

						cbxMouseButton.SelectedItem = MouseKeys.Middle;
						cbxMouseAction.SelectedItem = MouseActions.Down;
						Debug.WriteLine("Middle BUTTON CLICK");

						break;
					

				


				}
				tbxX.Text = mouseRecordEvent.EventMouse.Location.X.ToString();
				tbxY.Text = mouseRecordEvent.EventMouse.Location.Y.ToString();


			}
		}
    }
}
