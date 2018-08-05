using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DesktopBackgroundChanger.Library
{
    public class ConfigSettings
    {
        #region Structs

        public struct Image
        {
            public string Name;
            public TimeSpan Time;
        }

        #endregion

        #region Properties

        public string ImageLocationDirectory { get; set; }

        public List<Image> Images { get; set; } = new List<Image>();

        #endregion

        #region Public Methods

        public void LoadFromXML(string xml)
        {
            if (String.IsNullOrWhiteSpace(xml))
                throw new ArgumentNullException("xml");

            try
            {
                Logger.Log.Info("Loading XML:");
                Logger.Log.Info(xml);

                var doc = XDocument.Parse(xml);

                var desktopBackgroundSettings = doc.Element("DesktopBackgroundSettings");

                this.ImageLocationDirectory = desktopBackgroundSettings.Element("ImageLocationDirectory").Value;

                this.Images = (from e in desktopBackgroundSettings.Element("Images").Elements("Image")
                               select new Image()
                               {
                                   Name = e.Element("Name").Value,
                                   Time = TimeSpan.Parse(e.Element("Time").Value),
                               }).ToList();
            }
            catch (Exception ex)
            {
                Logger.Log.Fatal("Failed to load XML.", ex);

                throw new InvalidOperationException("Failed to load XML", ex);
            }
        }

        #endregion
    }
}
