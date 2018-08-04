using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopBackgroundChanger.Library
{
    public static class DesktopAPIFactory
    {
        #region Properties

        public static IDesktopAPI DesktopAPI
        {
            get
            {
                if (DesktopAPIFactory._desktopAPI == null)
                {
                    DesktopAPIFactory._desktopAPI = new DesktopAPI();
                }

                return DesktopAPIFactory._desktopAPI;
            }
            set
            {
                DesktopAPIFactory._desktopAPI = value;
            }
        }
        private static IDesktopAPI _desktopAPI = null;

        #endregion
    }
}
