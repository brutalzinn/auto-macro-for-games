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

        private void getMousePositionEvent(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
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
			foreach (MouseActions action in Enum.GetValues(typeof(MouseActions)))
			{
				cbxMouseAction.Items.Add(action);
			}
		}

		private void BtnOk_Click(object sender, RoutedEventArgs e)
		{
			int offset = 0;
			var mouseButton = (MouseKeys)cbxMouseButton.SelectedItem;
			var mouseAction = (MouseActions)cbxMouseAction.SelectedItem;

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
							Action = MouseEventType.MouseDown + offset
						},
						new MouseEvent
						{
							Location = point,
							Action = MouseEventType.MouseUp + offset
						}
					};
					break;
				case MouseActions.DoubleClick:
					mouseEvents = new List<MouseEvent>
					{
						new MouseEvent
						{
							Location = point,
							Action = MouseEventType.MouseDown + offset
						},
						new MouseEvent
						{
							Location = point,
							Action = MouseEventType.MouseUp + offset
						},
						new MouseEvent
						{
							Location = point,
							Action = MouseEventType.MouseDown + offset
						},
						new MouseEvent
						{
							Location = point,
							Action = MouseEventType.MouseUp + offset
						}
					};
					break;
				case MouseActions.Up:
					mouseEvents = new List<MouseEvent>
					{
						new MouseEvent
						{
							Location = point,
							Action = MouseEventType.MouseUp + offset
						}
					};
					break;
				case MouseActions.Down:
					mouseEvents = new List<MouseEvent>
					{
						new MouseEvent
						{
							Location = point,
							Action = MouseEventType.MouseDown + offset
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
			if(!this.IsMouseOver && (MouseHook.MouseEventType)mouseEvent == MouseHook.MouseEventType.MouseDown)
			{
				System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
				x = point.X;
				y = point.Y;
				tbxX.Text = point.X.ToString();
				tbxY.Text = point.Y.ToString();
			}
			return false;
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
					case MouseHook.MouseEventType.MouseDown:

						cbxMouseButton.SelectedItem = MouseKeys.Left;
						cbxMouseAction.SelectedItem = MouseActions.Down;
						Debug.WriteLine("Mouse down");
						break;
					case MouseHook.MouseEventType.MouseUp:
						cbxMouseButton.SelectedItem = MouseKeys.Left;
						cbxMouseAction.SelectedItem = MouseActions.Up;
						Debug.WriteLine("Mouse up");
							
                        break;
				
					

				


				}
				tbxX.Text = mouseRecordEvent.EventMouse.Location.X.ToString();
				tbxY.Text = mouseRecordEvent.EventMouse.Location.Y.ToString();


			}
		}
    }
}
