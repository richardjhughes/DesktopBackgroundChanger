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
        public void Run_SetsFirstImageAsDesktopBackground()
        {
            var settings = new ConfigSettings()
            {
                ImageLocationDirectory = "dir",
            };

            var images = new List<string>()
            {
                "image1",
                "image2",
                "image3",
            };

            this.MockDesktopAPI.Setup(m => m.GetImagesFromDirectory(It.IsAny<string>()))
                               .Returns(images);

            this.DesktopChanger.Run(settings);

            this.MockDesktopAPI.Verify(m => m.SetDesktopBackground(images[0]));
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
