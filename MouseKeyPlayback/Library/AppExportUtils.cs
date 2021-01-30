using MouseKeyPlayback;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseKeyPlayback.Utils
{

    public static class AppExportUtils
    {
        private static AppExportSettings settings;

        public static AppExportSettings Settings
        {
            get
            {
                return settings;
            }
            set
            {
                settings = value;
            }
        }

        private const string SETTINGS_FILE = "Record01.xml";

        public static void LoadSettings(string filepath)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    settings = XMLUtils.FromXML<AppExportSettings>(File.ReadAllText(filepath));
                    return;
                }
            }
            catch (Exception)
            {
                //An error occured while loading the file.
                //Trying to delete the file.

                try
                {
                    File.Delete(SETTINGS_FILE);
                }
                catch (Exception)
                {
                    //Unable to delete file.
                    //Giving up on humanity.
                }
            }

            settings = new AppExportSettings();

        }

        public static void SaveSettings(string path)
        {
            File.WriteAllText(path, XMLUtils.ToXML(settings));
        }




    }

    [Serializable]
    public class AppExportSettings
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



    public List<Record> recordList { get; set; } = new List<Record>();


    }
}
