﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopBackgroundChanger.Library.Tests
{
    [TestClass]
    public class DesktopChangerTests
    {
        #region Properties

        private DesktopChanger DesktopChanger { get; set; } = null;

        private Mock<IDesktopAPI> MockDesktopAPI { get; set; } = null;

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

        #region Run

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Run_NullSettings_ThrowsArgumentNullException()
        {
            this.DesktopChanger.Run(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Run_InvalidImageLocationDirectory_ThrowsInvaidOperationException()
        {
            var settings = new ConfigSettings()
            {
                ImageLocationDirectory = "dir",
            };

            this.MockDesktopAPI.Setup(m => m.GetImagesFromDirectory(It.IsAny<string>()))
                               .Returns((IList<string>)null);

            this.DesktopChanger.Run(settings);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Run_NoImagesInImageDirectory_ThrowsInvaidOperationException()
        {
            var settings = new ConfigSettings()
            {
                ImageLocationDirectory = "dir",
            };

            var images = new List<string>();

            this.MockDesktopAPI.Setup(m => m.GetImagesFromDirectory(It.IsAny<string>()))
                               .Returns(images);

            this.DesktopChanger.Run(settings);
        }

        [TestMethod]
        public void Run_SetsImagesAtTime()
        {
            var settings = new ConfigSettings()
            {
                ImageLocationDirectory = "dir",
                Images = new List<ConfigSettings.Image>()
                {
                    new ConfigSettings.Image()
                    {
                        Name = "  image1.png",
                        Time = new TimeSpan(0, 0, 0),
                    },
                    new ConfigSettings.Image()
                    {
                        Name = "image2.PNG",
                        Time = new TimeSpan(1, 2, 0),
                    },
                    new ConfigSettings.Image()
                    {
                        Name = "image3.png",
                        Time = new TimeSpan(3, 6, 0),
                    },
                    new ConfigSettings.Image()
                    {
                        Name = "Image4.png  ",
                        Time = new TimeSpan(14, 40, 0),
                    },
                },
            };

            var images = new List<string>()
            {
                "image2.png",
                "IMAGE3.png",
                " image4.pNg",
                "image1.png  ",
            };

            this.MockDesktopAPI.Setup(m => m.GetImagesFromDirectory(It.IsAny<string>()))
                               .Returns(images);

            this.DesktopChanger.Run(settings);

            this.MockDesktopAPI.Verify(m => m.SetDesktopBackground(images[0]));
            this.MockDesktopAPI.Verify(m => m.WaitUntilTime(settings.Images[0].Time));

            this.MockDesktopAPI.Verify(m => m.SetDesktopBackground(images[1]));
            this.MockDesktopAPI.Verify(m => m.WaitUntilTime(settings.Images[1].Time));

            this.MockDesktopAPI.Verify(m => m.SetDesktopBackground(images[2]));
            this.MockDesktopAPI.Verify(m => m.WaitUntilTime(settings.Images[2].Time));

            this.MockDesktopAPI.Verify(m => m.SetDesktopBackground(images[3]));
            this.MockDesktopAPI.Verify(m => m.WaitUntilTime(settings.Images[3].Time));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Run_SettingsContainsImageNoFoundInImageDirectory_ThrowsInvalidOperationException()
        {
            var settings = new ConfigSettings()
            {
                ImageLocationDirectory = "dir",
                Images = new List<ConfigSettings.Image>()
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
                        Time = new TimeSpan(1, 2, 0),
                    },
                },
            };

            var images = new List<string>()
            {
                "image1.png",
                "image2.png",
            };

            this.MockDesktopAPI.Setup(m => m.GetImagesFromDirectory(It.IsAny<string>()))
                               .Returns(images);

            this.DesktopChanger.Run(settings);
        }

        #endregion

        #endregion

        #region Helper Methods

        private void Setup()
        {
            this.DesktopChanger = new DesktopChanger();

            this.MockDesktopAPI = new Mock<IDesktopAPI>();
            DesktopAPIFactory.DesktopAPI = this.MockDesktopAPI.Object;
        }

        private void Shutdown()
        {
            this.MockDesktopAPI = null;
            DesktopAPIFactory.DesktopAPI = null;

            this.DesktopChanger = null;
        }

        #endregion
    }
}
