using DesktopBackgroundChanger.Library;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopBackgroundChanger.UI.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new System.IO.FileInfo("logconfig.xml"));

            Logger.Log.Info(String.Empty);
            Logger.Log.Info("==============================");
            Logger.Log.Info("Started DesktopBackgroundChanger Console App.");

            try
            {
                var settings = new ConfigSettings();

                settings.ImageLocationDirectory = @"D:\OneDrive\Pictures\Desktop Backgrounds\DesktopBackgroundChanger\";

                var changer = new DesktopChanger();
                changer.Run(settings);
            }
            catch (Exception ex)
            {
                Logger.Log.Fatal(ex.Message, ex);
            }

            Logger.Log.Info("Closed DesktopBackgroundChanger Console App.");
            System.Console.Read();
            Logger.Log.Info("Goodbye!");
            Logger.Log.Info("==============================");
        }
    }
}