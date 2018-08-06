using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;

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

            var images = settings.Images.OrderBy(i => i.Time.Ticks).ToList();
            images = images.DistinctBy(m => m.Time).ToList();

            var currentTime = api.GetTime();
            var time = new TimeSpan(currentTime.Hour, currentTime.Minute, currentTime.Second);

            var startingIndex = 0;

            do
            {
                startingIndex++;
            } while (time > images[startingIndex].Time);

            // we want the previous image
            startingIndex--;

            for (int i = startingIndex; i < images.Count; i++)
            {
                var image = images[i];

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

                var milliseconds = 0;

                if (i < images.Count - 1)
                {
                    milliseconds = (int)(images[i + 1].Time - images[i].Time).TotalMilliseconds;
                }
                else
                {
                    milliseconds = (int)(new TimeSpan(24, 0, 0) - images[0].Time - images[i].Time).TotalMilliseconds;
                }

                api.WaitInterval(milliseconds);
            }
        }

        #endregion
    }
}
