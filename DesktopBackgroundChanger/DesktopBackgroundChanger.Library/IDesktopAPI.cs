using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopBackgroundChanger.Library
{
    public interface IDesktopAPI
    {
        #region Methods

        IList<string> GetImagesFromDirectory(string directory);

        void SetDesktopBackground(string imagePath);

        void WaitInterval(int milliseconds);

        #endregion
    }
}
