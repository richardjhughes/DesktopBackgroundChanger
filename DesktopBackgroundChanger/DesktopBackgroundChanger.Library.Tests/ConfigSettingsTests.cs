using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopBackgroundChanger.Library.Tests
{
    [TestClass]
    public class ConfigSettingsTests
    {
        #region Properties

        private ConfigSettings ConfigSettings { get; set; } = null;

        #endregion

        #region Test Methods

        [TestInitialize]
        public void Init()
        {
            this.Setup();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.Shutdown();
        }

        #region LoadFromXML

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadFromXML_NullXML_ThrowsArgumentNullException()
        {
            this.ConfigSettings.LoadFromXML(null);
        }

        [TestMethod]
        public void LoadFromXML_LoadsSettings()
        {
            var expectedImageLocationDirectory = @"C:\test\directory";

            var images = new List<ConfigSettings.Image>()
            {
                new ConfigSettings.Image()
                {
                    Name = "image1.png",
                    Time = new TimeSpan(0, 0, 0),
                },
                new ConfigSettings.Image()
                {
                    Name = "image2.png",
                    Time = new TimeSpan(1, 2, 0),
                },
                new ConfigSettings.Image()
                {
                    Name = "image3.png",
                    Time = new TimeSpan(3, 6, 0),
                },
                new ConfigSettings.Image()
                {
                    Name = "image4.png",
                    Time = new TimeSpan(14, 40, 0),
                },
            };

            var xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <DesktopBackgroundSettings>
                          <ImageLocationDirectory>" + expectedImageLocationDirectory + @"</ImageLocationDirectory>
                          <Images>
                            <Image>
                              <Name>" + images[0].Name + @"</Name>
                              <Time>" + images[0].Time.ToString("c") + @"</Time>
                            </Image>
                            <Image>
                              <Name>" + images[1].Name + @"</Name>
                              <Time>" + images[1].Time.ToString("c") + @"</Time>
                            </Image>
                            <Image>
                              <Name>" + images[2].Name + @"</Name>
                              <Time>" + images[2].Time.ToString("c") + @"</Time>
                            </Image>
                            <Image>
                              <Name>" + images[3].Name + @"</Name>
                              <Time>" + images[3].Time.ToString("c") + @"</Time>
                            </Image>
                          </Images>
                        </DesktopBackgroundSettings>";

            this.ConfigSettings.LoadFromXML(xml);

            Assert.AreEqual(expectedImageLocationDirectory, this.ConfigSettings.ImageLocationDirectory, "ImageLocationDirectory");

            var expectedNumberOfImages = images.Count;

            Assert.AreEqual(expectedNumberOfImages, this.ConfigSettings.Images.Count, "Images.Count");

            for (int i = 0; i < images.Count; i++)
            {
                var expectedName = images[i].Name;
                var expectedTime = images[i].Time;

                Assert.AreEqual(expectedName, this.ConfigSettings.Images[i].Name, String.Format("Images[{0}].Name", i));
                Assert.AreEqual(expectedTime, this.ConfigSettings.Images[i].Time, String.Format("Images[{0}].Time", i));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LoadFromXML_LoadsSettings_InvalidXML_ThrowsInvalidOperationException()
        {
            var xml = @"abc";

            this.ConfigSettings.LoadFromXML(xml);
        }

        #endregion

        #endregion

        #region Helper Methods

        private void Setup()
        {
            this.ConfigSettings = new ConfigSettings();
        }

        private void Shutdown()
        {
            this.ConfigSettings = null;
        }

        #endregion
    }
}
