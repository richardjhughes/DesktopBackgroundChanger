using DesktopBackgroundChanger.Library;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DesktopBackgroundChanger.UI.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo("logconfig.xml"));

            Logger.Log.Info(String.Empty);
            Logger.Log.Info("==============================");
            Logger.Log.Info("Started DesktopBackgroundChanger Console App.");

            try
            {
                var settings = new ConfigSettings();

                settings.LoadFromXML(File.ReadAllText("TestImages\\config.xml"));

                settings.ImageLocationDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestImages");

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