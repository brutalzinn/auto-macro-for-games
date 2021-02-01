
using GlobalHotkeys;
using MouseKeyPlayback.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MouseKeyPlayback.Views
{
    /// <summary>
    /// Lógica interna para ConfigPage.xaml
    /// </summary>
    public partial class ConfigPage : Window
    {
        public ConfigPage()
        {
            InitializeComponent();
              RandomMinConfig.Text = ApplicationSettingsManager.Settings.minRamdomConfig.ToString();
            RandomMaxConfig.Text = ApplicationSettingsManager.Settings.maxRamdomConfig.ToString();
            TextBoxDefaultTimer.Text = ApplicationSettingsManager.Settings.DefaultTimer.ToString();
            CheckBoxRandomForeach.IsChecked = ApplicationSettingsManager.Settings.ForeachKeys;
            CheckBoxRandomBetween.IsChecked = ApplicationSettingsManager.Settings.BetweenKeys;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           ApplicationSettingsManager.Settings.minRamdomConfig = Convert.ToInt32(RandomMinConfig.Text);
            ApplicationSettingsManager.Settings.maxRamdomConfig = Convert.ToInt32(RandomMaxConfig.Text);
            ApplicationSettingsManager.Settings.DefaultTimer = Convert.ToInt32(TextBoxDefaultTimer.Text);
            ApplicationSettingsManager.Settings.ForeachKeys = CheckBoxRandomForeach.IsChecked.Value;
            ApplicationSettingsManager.Settings.BetweenKeys = CheckBoxRandomBetween.IsChecked.Value;
           
            ApplicationSettingsManager.SaveSettings();
             Globals.MainWindow.RegisterHotKeys();
            
            this.Close();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            KeyInfoHelper teste = new KeyInfoHelper();
        teste.ShowDialog();
            ApplicationSettingsManager.Settings.HotKeyStartRecord = teste.keysResult;

          
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            KeyInfoHelper teste = new KeyInfoHelper();
            teste.ShowDialog();
            ApplicationSettingsManager.Settings.HotKeyStopRecord = teste.keysResult;

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            KeyInfoHelper teste = new KeyInfoHelper();
            teste.ShowDialog();
            ApplicationSettingsManager.Settings.HotKeyPlay = teste.keysResult;

        }

        private void HotKeyStopMacro(object sender, RoutedEventArgs e)
        {
            KeyInfoHelperAdvaced teste = new KeyInfoHelperAdvaced();
            teste.ShowDialog();
         ApplicationSettingsManager.Settings.StopMacroControl.HotKeyStopKeys = teste.nonModifierKeys;
            ApplicationSettingsManager.Settings.StopMacroControl.HotKeyStopModifiers = teste.modifierKeys;
        }
    }
}
