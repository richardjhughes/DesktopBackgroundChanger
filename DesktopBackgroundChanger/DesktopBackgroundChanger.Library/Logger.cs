using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopBackgroundChanger.Library
{
    public static class Logger
    {
        #region Properties

        public static ILog Log
        {
            get
            {
                if (Logger._log == null)
                {
                    Logger._log = LogManager.GetLogger("DesktopBackgroundChanger");
                }

                return Logger._log;
            }
        }
        private static ILog _log = null;

        #endregion
    }
}
