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

            foreach (var image in settings.Images)
            {
                var imageName = image.Name.Trim().ToLower();

                var imagePath = (from path in imagePaths
                                 where path.Trim().ToLower().EndsWith(imageName)
                                 select path).FirstOrDefault();

                if (String.IsNullOrWhiteSpace(imagePath))
                {
                    Logger.Log.ErrorFormat("Failed to find image: '{0}'.", imagePath);
                    throw new InvalidOperationException(String.Format("Failed to find image: '{0}'.", imagePath));
                }

                Logger.Log.InfoFormat("Setting desktop background image: '{0}'.", imagePath);

                api.SetDesktopBackground(imagePath);

                api.WaitUntilTime(image.Time);
            }
        }

        #endregion
    }
}
