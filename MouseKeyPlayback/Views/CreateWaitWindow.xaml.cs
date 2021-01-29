﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MouseKeyPlayback.Views
{
    /// <summary>
    /// Interaction logic for CreateWaitWindow.xaml
    /// </summary>
    public partial class CreateWaitWindow : Window
    {
		public Record waitEvent { get; set; }

        public CreateWaitWindow()
        {
            InitializeComponent();

			System.Windows.Application curApp = System.Windows.Application.Current;
			Window mainWindow = curApp.MainWindow;
			this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
			this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;
		}

		private void BtnOk_Click(object sender, RoutedEventArgs e)
		{
			waitEvent = new Record
			{
				WaitMs = Int32.Parse(tbxWait.Text),
				Type = Constants.WAIT
			};
			this.Close();
		}

		private void BtnCancel_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
          
			
        }

        private void HeaderControl_KeyDown(object sender, KeyEventArgs e)
        {
		
		}

        private void HeaderControl_Collapsed(object sender, RoutedEventArgs e)
        {
		
		}

        private void HeaderControl_TouchDown(object sender, TouchEventArgs e)
        {
				
        }

        private void HeaderControl_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
		
		}

        private void HeaderControl_LayoutUpdated(object sender, EventArgs e)
        {
			if (HeaderControl.IsExpanded)
			{
				this.Height = 262;
			}
			else
			{
				this.Height = 188;
			}
		}
    }
}
