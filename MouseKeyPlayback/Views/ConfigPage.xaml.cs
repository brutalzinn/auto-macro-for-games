using BackendAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           ApplicationSettingsManager.Settings.minRamdomConfig = Convert.ToInt32(RandomMinConfig.Text);
            ApplicationSettingsManager.Settings.maxRamdomConfig = Convert.ToInt32(RandomMaxConfig.Text);
            ApplicationSettingsManager.SaveSettings();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
