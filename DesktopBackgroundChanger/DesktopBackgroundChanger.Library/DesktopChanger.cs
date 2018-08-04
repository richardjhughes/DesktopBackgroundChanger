using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopBackgroundChanger.Library
{
    public class DesktopChanger
    {
        #region Public Methods

        public void Run(ConfigSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            var api = DesktopAPIFactory.DesktopAPI;

            Logger.Log.InfoFormat("Getting images from: '{0}'.", settings.ImageLocationDirectory);

            var imagePaths = api.GetImagesFromDirectory(settings.ImageLocationDirectory);
            if (imagePaths == null)
                throw new InvalidOperationException(String.Format("Failed to load images from directory: '{0}'.", settings.ImageLocationDirectory));

            if (imagePaths.Count <= 0)
                throw new InvalidOperationException(String.Format("No images found in directory: '{0}'.", settings.ImageLocationDirectory));

            Logger.Log.InfoFormat("Setting desktop background image: '{0}'.", imagePaths[0]);

            api.SetDesktopBackground(imagePaths[0]);
        }

        #endregion
    }
}
