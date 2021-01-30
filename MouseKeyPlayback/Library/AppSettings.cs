using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseKeyPlayback.Utils
{

    public static class ApplicationSettingsManager
    {
        private static AppSettings settings;

        public static AppSettings Settings {
            get {
                return settings;
            }
        }

        private const string SETTINGS_FILE = "settings.xml";

        public static void LoadSettings()
        {
            try {
                if (File.Exists(SETTINGS_FILE)) {
                    settings = XMLUtils.FromXML<AppSettings>(File.ReadAllText(SETTINGS_FILE));
                    return;
                }
            } catch (Exception) {
                //An error occured while loading the file.
                //Trying to delete the file.

                try {
                    File.Delete(SETTINGS_FILE);
                } catch (Exception) {
                    //Unable to delete file.
                    //Giving up on humanity.
                }
            }

            settings = new AppSettings();

        }

        public static void SaveSettings()
        {
            File.WriteAllText(SETTINGS_FILE, XMLUtils.ToXML(settings));
        }

       


    }

    [Serializable]
    public class AppSettings
    {
       
        /// <summary>
        /// Called to signal to subscribers that the theme was changed.
        /// </summary>
        public event EventHandler ColorSchemeChanged;
        protected virtual void OnColorSchemeChanged(EventArgs e)
        {
            EventHandler eh = ColorSchemeChanged;

            eh?.Invoke(this, e);
        }

    
        
        public int minRamdomConfig { get; set; }
        public int maxRamdomConfig { get; set; }
        public int DefaultTimer{ get; set; }
       public bool BetweenKeys { get; set; }
        public bool ForeachKeys { get; set; }

    }
}
