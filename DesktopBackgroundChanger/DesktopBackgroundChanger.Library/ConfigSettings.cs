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
            public DateTime Time;
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

            var doc = XDocument.Parse(xml);

            this.ImageLocationDirectory = (from e in doc.Descendants("ImageLocationDirectory")
                                           select e).First().Value;
        }

        #endregion
    }
}
