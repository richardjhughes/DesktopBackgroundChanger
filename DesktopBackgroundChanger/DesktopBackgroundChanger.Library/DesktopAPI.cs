using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopBackgroundChanger.Library
{
    public class DesktopAPI : IDesktopAPI
    {
        #region Constants

        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        #endregion

        #region Imported Methods

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        #endregion

        #region Public Methods

        public IList<string> GetImagesFromDirectory(string directory)
        {
            try
            {
                var files = Directory.GetFiles(directory);
                return files;
            }
            catch (Exception ex)
            {
                Logger.Log.Fatal(ex.Message, ex);
                return null;
            }
        }

        public void SetDesktopBackground(string imagePath)
        {
            Logger.Log.InfoFormat("Setting desktop background image: '{0}'.", imagePath);

            var image = new Bitmap(imagePath);

            var directory = Path.GetDirectoryName(imagePath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imagePath);

            var backgroungFileName = Path.Combine(directory, String.Format("{0}_forbackground.bmp", fileNameWithoutExtension));

            Logger.Log.InfoFormat("Saving desktop background image as: '{0}'.", backgroungFileName);

            if (File.Exists(backgroungFileName))
            {
                File.Delete(backgroungFileName);
            }

            image.Save(backgroungFileName, System.Drawing.Imaging.ImageFormat.Bmp);

            RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);

            var wallpaperStyle = key.GetValue("WallpaperStyle");
            var tileWallpaper = key.GetValue("TileWallpaper");

            Logger.Log.InfoFormat("Old registry settings: 'Control Panel\\Desktop\\WallpaperStyle - {0}'.", wallpaperStyle);
            Logger.Log.InfoFormat("Old registry settings: 'Control Panel\\Desktop\\TileWallpaper - {0}'.", tileWallpaper);

            key.SetValue("WallpaperStyle", "1");
            key.SetValue("TileWallpaper", "1");

            SystemParametersInfo(SPI_SETDESKWALLPAPER,
                                 0,
                                 backgroungFileName,
                                 SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

            Logger.Log.Info("Desktop background set.");
        }

        public void WaitUntilTime(TimeSpan time)
        {
            Logger.Log.InfoFormat("Waiting for the time '{0}'.", time);

            var currentTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            var timeToWait = time - currentTime;

            Thread.Sleep((int)timeToWait.TotalMilliseconds);
        }

        #endregion
    }
}
